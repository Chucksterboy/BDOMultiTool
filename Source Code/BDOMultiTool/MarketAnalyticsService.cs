using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BDOMultiTool;

internal sealed class MarketAnalyticsService : IDisposable
{
	public const int DefaultOutfitsPerScan = 100;

	public static readonly TimeSpan DefaultCollectorInterval = TimeSpan.FromHours(24);

	private static readonly TimeSpan MarketSampleRetention = TimeSpan.FromDays(180);

	private readonly MarketDatabase database;

	private readonly IMarketDataProvider provider;

	private readonly AppLogger logger;

	private readonly SemaphoreSlim updateLock = new SemaphoreSlim(1, 1);

	private readonly SemaphoreSlim outfitLock = new SemaphoreSlim(1, 1);

	private readonly Semaphore processUpdateLock = new Semaphore(1, 1, "Local\\BDOMultiTool-MarketUpdate");

	private readonly CancellationTokenSource shutdown = new CancellationTokenSource();

	private readonly object reportCacheSync = new();

	private readonly Dictionary<string, (OutfitReport Report, DateTimeOffset CachedUtc)> reportCache = new(StringComparer.OrdinalIgnoreCase);

	private Timer? timer;

	private MarketSettings settings = MarketSettings.Default;

	private bool foregroundUpdatesEnabled;

	public MarketSettings Settings => settings;

	public string ProviderName => provider.Name;

	public event EventHandler? DataChanged;

	public event EventHandler<string>? StatusChanged;

	public MarketAnalyticsService(MarketDatabase database, IMarketDataProvider provider, AppLogger logger)
	{
		this.database = database;
		this.provider = provider;
		this.logger = logger;
	}

	public async Task InitializeAsync(CancellationToken cancellationToken, bool startForegroundUpdates = true)
	{
		await database.InitializeAsync(cancellationToken);
		MarketSettings storedSettings = await database.GetSettingsAsync(cancellationToken);
		settings = storedSettings with { Region = "eu" };
		if (!string.Equals(storedSettings.Region, "eu", StringComparison.OrdinalIgnoreCase))
		{
			await database.SaveSettingsAsync(settings, cancellationToken);
		}
		foregroundUpdatesEnabled = startForegroundUpdates;
		if (!startForegroundUpdates)
		{
			return;
		}
		ResetTimer();
		_ = Task.Run(async delegate
		{
			try
			{
				await RefreshDueMarketSamplesAsync(DefaultCollectorInterval, "startup catch-up", shutdown.Token);
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception exception)
			{
				logger.Error("Startup market catch-up failed.", exception);
			}
		});
	}

	public Task<IReadOnlyList<TrackedItem>> GetTrackedItemsAsync(CancellationToken cancellationToken)
	{
		return database.GetTrackedItemsAsync(settings.Region, cancellationToken);
	}

	public Task<IReadOnlyList<TrackedItem>> GetTrackedItemsAsync(string region, CancellationToken cancellationToken)
	{
		return database.GetTrackedItemsAsync(NormalizeRegion(region), cancellationToken);
	}

	public Task<IReadOnlyList<MarketItem>> SearchAsync(string query, CancellationToken cancellationToken)
	{
		return provider.SearchAsync(query, settings.Region, cancellationToken);
	}

	public Task<IReadOnlyList<MarketItem>> GetVariantsAsync(long itemId, CancellationToken cancellationToken)
	{
		return provider.GetVariantsAsync(itemId, settings.Region, cancellationToken);
	}

	public async Task AddTrackedItemAsync(MarketItem item, CancellationToken cancellationToken)
	{
		await database.AddTrackedItemAsync(item, settings.Region, cancellationToken);
		await RefreshItemAsync(item.ItemId, item.Enhancement, cancellationToken);
		this.DataChanged?.Invoke(this, EventArgs.Empty);
	}

	public async Task RemoveTrackedItemAsync(long itemId, int enhancement, CancellationToken cancellationToken)
	{
		await database.RemoveTrackedItemAsync(itemId, enhancement, settings.Region, cancellationToken);
		this.DataChanged?.Invoke(this, EventArgs.Empty);
	}

	public Task<ItemAnalytics?> GetAnalyticsAsync(long itemId, int enhancement, int days, CancellationToken cancellationToken)
	{
		return database.GetAnalyticsAsync(itemId, enhancement, settings.Region, days, cancellationToken);
	}

	public Task<ItemAnalytics?> GetAnalyticsAsync(long itemId, int enhancement, string region, int days, CancellationToken cancellationToken)
	{
		return database.GetAnalyticsAsync(itemId, enhancement, NormalizeRegion(region), days, cancellationToken);
	}

	public Task<OutfitReport> GetOutfitReportAsync(CancellationToken cancellationToken)
	{
		return GetOutfitReportAsync(settings.Region, cancellationToken);
	}

	public async Task<OutfitReport> GetOutfitReportAsync(string region, CancellationToken cancellationToken)
	{
		region = NormalizeRegion(region);
		lock (reportCacheSync)
		{
			if (reportCache.TryGetValue(region, out var cached) && DateTimeOffset.UtcNow - cached.CachedUtc < TimeSpan.FromMinutes(2))
			{
				return cached.Report;
			}
		}

		OutfitReport report = await database.GetOutfitReportAsync(region, cancellationToken);
		CacheOutfitReport(region, report);
		return report;
	}

	private void CacheOutfitReport(string region, OutfitReport report)
	{
		lock (reportCacheSync)
		{
			reportCache[NormalizeRegion(region)] = (report, DateTimeOffset.UtcNow);
		}
	}

	private void InvalidateOutfitReport(string region)
	{
		lock (reportCacheSync)
		{
			reportCache.Remove(NormalizeRegion(region));
		}
	}

	public async Task SaveSettingsAsync(string region, int intervalMinutes, CancellationToken cancellationToken)
	{
		string previousRegion = settings.Region;
		string normalizedRegion = NormalizeRegion(region);
		settings = new MarketSettings(normalizedRegion, Math.Clamp(intervalMinutes, 5, 1440));
		await database.SaveSettingsAsync(settings, cancellationToken);
		if (foregroundUpdatesEnabled)
		{
			ResetTimer();
		}
		if (!string.Equals(previousRegion, settings.Region, StringComparison.OrdinalIgnoreCase))
		{
			await SyncOutfitCatalogAsync(settings.Region, cancellationToken);
		}
		this.DataChanged?.Invoke(this, EventArgs.Empty);
	}

	private static string NormalizeRegion(string region)
	{
		return "eu";
	}

	public async Task RefreshAllAsync(CancellationToken cancellationToken)
	{
		if (!(await updateLock.WaitAsync(0, cancellationToken)))
		{
			this.StatusChanged?.Invoke(this, "An update is already running.");
			return;
		}
		bool ownsProcessLock = false;
		try
		{
			ownsProcessLock = processUpdateLock.WaitOne(0);
			if (!ownsProcessLock)
			{
				this.StatusChanged?.Invoke(this, "A background market update is already running.");
				return;
			}
			await RefreshRegionAsync("eu", DefaultOutfitsPerScan, cancellationToken);
			this.DataChanged?.Invoke(this, EventArgs.Empty);
		}
		finally
		{
			if (ownsProcessLock)
			{
				processUpdateLock.Release();
			}
			updateLock.Release();
		}
	}

	public Task ExportCsvAsync(string path, CancellationToken cancellationToken)
	{
		return database.ExportCsvAsync(settings.Region, path, cancellationToken);
	}

	public async Task RefreshOutfitsAsync(int batchSize, CancellationToken cancellationToken)
	{
		if (!(await outfitLock.WaitAsync(0, cancellationToken)))
		{
			return;
		}
		try
		{
			await RefreshOutfitsForRegionAsync(settings.Region, batchSize, cancellationToken);
			this.DataChanged?.Invoke(this, EventArgs.Empty);
		}
		finally
		{
			outfitLock.Release();
		}
	}

	public async Task RefreshDueMarketSamplesAsync(TimeSpan maximumAge, string reason, CancellationToken cancellationToken)
	{
		if (!(await updateLock.WaitAsync(0, cancellationToken)))
		{
			this.StatusChanged?.Invoke(this, "A market sample check is already running.");
			return;
		}
		bool ownsProcessLock = false;
		try
		{
			ownsProcessLock = processUpdateLock.WaitOne(0);
			if (!ownsProcessLock)
			{
				this.StatusChanged?.Invoke(this, "A background market update is already running.");
				return;
			}
			bool refreshedAny = false;
			foreach (string region in new[] { "eu" })
			{
				DateTimeOffset? latest = await database.GetLatestMarketSampleUtcAsync(region, cancellationToken);
				bool isDue = !latest.HasValue || DateTimeOffset.UtcNow - latest.Value >= maximumAge;
				if (!isDue)
				{
					this.StatusChanged?.Invoke(this, $"{region.ToUpperInvariant()} market samples are fresh. Last sample: {latest.Value.LocalDateTime:g}.");
					continue;
				}
				this.StatusChanged?.Invoke(this, $"{region.ToUpperInvariant()} market samples are due ({reason}). Collecting now...");
				await RefreshRegionAsync(region, DefaultOutfitsPerScan, cancellationToken);
				refreshedAny = true;
			}
			if (!refreshedAny)
			{
				this.StatusChanged?.Invoke(this, "EU market samples are already up to date.");
			}
			else
			{
				int pruned = await database.PruneOldMarketSamplesAsync(MarketSampleRetention, cancellationToken);
				if (pruned > 0)
				{
					logger.Info($"Market sample retention removed {pruned:N0} old row(s).");
				}
			}
			this.DataChanged?.Invoke(this, EventArgs.Empty);
		}
		finally
		{
			if (ownsProcessLock)
			{
				processUpdateLock.Release();
			}
			updateLock.Release();
		}
	}

	private async Task RefreshOutfitsForAllRegionsAsync(int batchSize, CancellationToken cancellationToken)
	{
		int failures = 0;
		foreach (string region in new[] { "eu" })
		{
			try
			{
				await RefreshOutfitsForRegionAsync(region, Math.Max(1, batchSize / 2), cancellationToken);
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				throw;
			}
			catch (Exception exception)
			{
				failures++;
				logger.Error($"Scheduled {region.ToUpperInvariant()} outfit scan failed.", exception);
				this.StatusChanged?.Invoke(this, $"{region.ToUpperInvariant()} outfit scan failed. The other region will continue normally.");
			}
		}
		if (failures == 0)
		{
			this.StatusChanged?.Invoke(this, "EU outfit samples are up to date.");
		}
	}

	private async Task RefreshRegionAsync(string region, int outfitBatchSize, CancellationToken cancellationToken)
	{
		region = NormalizeRegion(region);
		IReadOnlyList<TrackedItem> trackedItems = await database.GetTrackedItemsAsync(region, cancellationToken);
		this.StatusChanged?.Invoke(this, trackedItems.Count == 0 ? $"Updating {region.ToUpperInvariant()} outfit samples..." : $"Updating {trackedItems.Count} tracked {region.ToUpperInvariant()} item sample(s)...");
		IEnumerable<IGrouping<long, TrackedItem>> groups = from x in trackedItems
			group x by x.ItemId;
		int completed = 0;
		int failures = 0;
		foreach (IGrouping<long, TrackedItem> group in groups)
		{
			IReadOnlyList<MarketItem> variants;
			try
			{
				variants = await provider.GetVariantsAsync(group.Key, region, cancellationToken);
			}
			catch (Exception exception)
			{
				failures += group.Count();
				logger.Error($"Could not retrieve {region.ToUpperInvariant()} variants for item {group.Key}.", exception);
				continue;
			}
			foreach (TrackedItem tracked in group)
			{
				try
				{
					MarketItem variant = variants.FirstOrDefault((MarketItem x) => x.Enhancement == tracked.Enhancement) ?? throw new InvalidDataException("The tracked enhancement is no longer available.");
					MarketSnapshot snapshot = await provider.GetSnapshotAsync(tracked.ItemId, tracked.Enhancement, region, cancellationToken);
					await database.SaveSnapshotAsync(tracked, variant, snapshot, cancellationToken);
					completed++;
				}
				catch (Exception exception2)
				{
					failures++;
					logger.Error($"Could not update {region.ToUpperInvariant()} item {tracked.ItemId} enhancement {tracked.Enhancement}.", exception2);
				}
			}
		}
		if (trackedItems.Count > 0)
		{
			this.StatusChanged?.Invoke(this, failures == 0 ? $"{region.ToUpperInvariant()} tracked item samples refreshed." : $"{region.ToUpperInvariant()} tracked samples: {completed} refreshed, {failures} failed.");
		}
		await RefreshOutfitsForRegionAsync(region, outfitBatchSize, cancellationToken);
	}

	private async Task RefreshOutfitsForRegionAsync(string region, int batchSize, CancellationToken cancellationToken)
	{
		this.StatusChanged?.Invoke(this, $"Syncing {region.ToUpperInvariant()} outfit samples...");
		await SyncOutfitCatalogAsync(region, cancellationToken);
		IReadOnlyList<MarketItem> readOnlyList = await database.GetOutfitsDueAsync(region, Math.Clamp(batchSize, 1, DefaultOutfitsPerScan), cancellationToken);
		int completed = 0;
		int failures = 0;
		int consecutiveFailures = 0;
		foreach (MarketItem item in readOnlyList)
		{
			try
			{
				IReadOnlyList<MarketItem> source = await provider.GetVariantsAsync(item.ItemId, region, cancellationToken);
				MarketItem variant = source.FirstOrDefault((MarketItem x) => x.Enhancement == 0) ?? source.First();
				MarketSnapshot snapshot = await provider.GetSnapshotAsync(item.ItemId, variant.Enhancement, region, cancellationToken);
				await database.SaveOutfitDetailAsync(item, variant, snapshot, region, cancellationToken);
				completed++;
				consecutiveFailures = 0;
				await Task.Delay(100, cancellationToken);
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				throw;
			}
			catch (Exception exception)
			{
				failures++;
				consecutiveFailures++;
				if (consecutiveFailures >= 3)
				{
					logger.Warn($"{region.ToUpperInvariant()} outfit scan paused after {consecutiveFailures} consecutive provider failures. Cached data remains available. Last error: {exception.Message}");
					break;
				}
			}
		}
		OutfitReport outfitReport = await database.GetOutfitReportAsync(region, cancellationToken);
		CacheOutfitReport(region, outfitReport);
		this.StatusChanged?.Invoke(this, $"{region.ToUpperInvariant()} outfit scan updated {completed} item(s), {failures} failed. Coverage: {outfitReport.DetailedCount:N0}/{outfitReport.CatalogCount:N0}.");
	}

	private async Task SyncOutfitCatalogAsync(string region, CancellationToken cancellationToken)
	{
		MarketItem[] catalog = (from x in (await provider.GetCategoryAsync(55, 1, region, cancellationToken)).Concat(await provider.GetCategoryAsync(55, 2, region, cancellationToken))
			group x by x.ItemId into x
			select x.First()).ToArray();
		await database.SyncOutfitCatalogAsync(catalog, region, cancellationToken);
		InvalidateOutfitReport(region);
		this.StatusChanged?.Invoke(this, $"{catalog.Length:N0} outfits loaded for {region.ToUpperInvariant()}.");
		this.DataChanged?.Invoke(this, EventArgs.Empty);
	}

	private async Task RefreshItemAsync(long itemId, int enhancement, CancellationToken cancellationToken)
	{
		TrackedItem tracked = (await database.GetTrackedItemsAsync(settings.Region, cancellationToken)).First((TrackedItem x) => x.ItemId == itemId && x.Enhancement == enhancement);
		MarketItem variant = (await provider.GetVariantsAsync(itemId, settings.Region, cancellationToken)).FirstOrDefault((MarketItem x) => x.Enhancement == enhancement) ?? throw new InvalidDataException("The selected enhancement is not available.");
		MarketSnapshot snapshot = await provider.GetSnapshotAsync(itemId, enhancement, settings.Region, cancellationToken);
		await database.SaveSnapshotAsync(tracked, variant, snapshot, cancellationToken);
	}

	private void ResetTimer()
	{
		timer?.Dispose();
		timer = new Timer(async delegate
		{
			try
			{
				await RefreshDueMarketSamplesAsync(DefaultCollectorInterval, "scheduled check", shutdown.Token);
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception exception)
			{
				logger.Error("Scheduled market update failed.", exception);
				this.StatusChanged?.Invoke(this, "Scheduled update failed. The app will retry at the next interval.");
			}
		}, null, TimeSpan.FromMinutes(settings.IntervalMinutes), TimeSpan.FromMinutes(settings.IntervalMinutes));
	}

	public void Dispose()
	{
		shutdown.Cancel();
		timer?.Dispose();
		shutdown.Dispose();
		processUpdateLock.Dispose();
	}
}
