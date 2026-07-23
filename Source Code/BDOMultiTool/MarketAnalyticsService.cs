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

	private readonly Semaphore processUpdateLock = new Semaphore(1, 1, "Local\\BDOMultiTool-MarketUpdate");

	private readonly CancellationTokenSource shutdown = new CancellationTokenSource();

	private readonly object reportCacheSync = new();

	private readonly Dictionary<string, (OutfitReport Report, DateTimeOffset CachedUtc)> reportCache = new(StringComparer.OrdinalIgnoreCase);

	private CancellationTokenSource? timerCancellation;

	private Task timerTask = Task.CompletedTask;

	private Task startupCatchUpTask = Task.CompletedTask;

	private MarketSettings settings = MarketSettings.Default;

	private int disposeRequested;

	private int resourcesDisposed;

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
		settings = new MarketSettings("eu", 1440);
		if (!string.Equals(storedSettings.Region, "eu", StringComparison.OrdinalIgnoreCase)
			|| storedSettings.IntervalMinutes != settings.IntervalMinutes)
		{
			await database.SaveSettingsAsync(settings, cancellationToken);
		}
		if (!startForegroundUpdates)
		{
			return;
		}
		ResetTimer();
		startupCatchUpTask = Task.Run(async delegate
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

	private static string NormalizeRegion(string region)
	{
		return "eu";
	}

	public Task ExportCsvAsync(string path, CancellationToken cancellationToken)
	{
		return database.ExportCsvAsync(settings.Region, path, cancellationToken);
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
				bool isDue = await database.IsMarketRefreshDueAsync(region, maximumAge, cancellationToken);
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
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				throw;
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
				catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
				{
					throw;
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
		timerCancellation?.Cancel();
		timerCancellation?.Dispose();
		timerCancellation = CancellationTokenSource.CreateLinkedTokenSource(shutdown.Token);
		timerTask = RunTimerAsync(TimeSpan.FromMinutes(settings.IntervalMinutes), timerCancellation.Token);
	}

	private async Task RunTimerAsync(TimeSpan interval, CancellationToken cancellationToken)
	{
		using PeriodicTimer periodicTimer = new(interval);
		try
		{
			while (await periodicTimer.WaitForNextTickAsync(cancellationToken))
			{
				try
				{
					await RefreshDueMarketSamplesAsync(DefaultCollectorInterval, "scheduled check", cancellationToken);
				}
				catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
				{
					break;
				}
				catch (Exception exception)
				{
					logger.Error("Scheduled market update failed.", exception);
					this.StatusChanged?.Invoke(this, "Scheduled update failed. The app will retry at the next interval.");
				}
			}
		}
		catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
		{
		}
	}

	public void Dispose()
	{
		if (Interlocked.Exchange(ref disposeRequested, 1) != 0)
		{
			return;
		}
		shutdown.Cancel();
		timerCancellation?.Cancel();
		Task pendingTasks = Task.WhenAll(timerTask, startupCatchUpTask);
		try
		{
			pendingTasks.Wait(TimeSpan.FromSeconds(2));
		}
		catch (AggregateException exception)
		{
			if (exception.Flatten().InnerExceptions.Any(error => error is not OperationCanceledException))
			{
				logger.Warn("A market background task ended with an error during shutdown.");
			}
		}
		if (pendingTasks.IsCompleted)
		{
			DisposeResources();
			return;
		}

		logger.Warn("Market background work is still stopping; resource cleanup was deferred.");
		_ = pendingTasks.ContinueWith(
			task =>
			{
				_ = task.Exception;
				DisposeResources();
			},
			CancellationToken.None,
			TaskContinuationOptions.ExecuteSynchronously,
			TaskScheduler.Default);
	}

	private void DisposeResources()
	{
		if (Interlocked.Exchange(ref resourcesDisposed, 1) != 0)
		{
			return;
		}
		timerCancellation?.Dispose();
		updateLock.Dispose();
		shutdown.Dispose();
		processUpdateLock.Dispose();
		if (provider is IDisposable disposableProvider)
		{
			disposableProvider.Dispose();
		}
	}
}
