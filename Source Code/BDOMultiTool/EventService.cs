using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BDOMultiTool;

internal sealed class EventService : IDisposable
{
	internal const string OfficialEventsUrl = "https://www.naeu.playblackdesert.com/en-US/News/Notice?boardType=3&progressType=1";
	private static readonly Uri SiteRoot = new("https://www.naeu.playblackdesert.com");
	private readonly AppPaths paths;
	private readonly AppLogger logger;
	private readonly HttpClient http;

	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		PropertyNameCaseInsensitive = true,
		WriteIndented = true
	};

	public EventService(AppPaths paths, AppLogger logger)
	{
		this.paths = paths;
		this.logger = logger;
		http = new HttpClient { Timeout = TimeSpan.FromSeconds(25) };
		http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126 Safari/537.36 BDO-Multi-Tool");
		http.DefaultRequestHeaders.Referrer = new Uri("https://www.naeu.playblackdesert.com/en-US/News/Notice");
	}

	public async Task<object> InitializeAsync(CancellationToken cancellationToken)
	{
		EventCache? cache = await ReadJsonAsync<EventCache>(paths.EventsCachePath, cancellationToken);
		if (HasEvents(cache))
			return BuildDashboard(cache!, "CACHED", null);

		EventCache? backup = await ReadJsonAsync<EventCache>(paths.EventsBackupCachePath, cancellationToken);
		if (HasEvents(backup))
			return BuildDashboard(backup!, "CACHED", "Showing saved Events data while the official page refreshes.");

		return await RefreshAsync(cancellationToken);
	}

	public async Task<object> RefreshAsync(CancellationToken cancellationToken)
	{
		DateTimeOffset attemptTime = DateTimeOffset.UtcNow;
		try
		{
			logger.Info("Events refresh started.");
			logger.Info("Events official source URL: " + OfficialEventsUrl);
			string html = await GetStringAsync(OfficialEventsUrl, cancellationToken);
			return await BuildAndCacheDashboardAsync(html, attemptTime, enrichDetails: true, cancellationToken);
		}
		catch (Exception ex)
		{
			logger.Warn("Events refresh failed reason: " + ex.Message);
			EventCache cache = await ReadJsonAsync<EventCache>(paths.EventsCachePath, cancellationToken)
				?? new EventCache(attemptTime, OfficialEventsUrl, [], "No official events could be loaded yet.");
			if (!HasEvents(cache))
			{
				EventCache? backup = await ReadJsonAsync<EventCache>(paths.EventsBackupCachePath, cancellationToken);
				if (HasEvents(backup))
					cache = backup!;
			}
			return BuildDashboard(cache, "CACHED", "Could not refresh official events. Showing cached data. " + ex.Message, attemptTime);
		}
	}

	public Task<object> RefreshFromRenderedHtmlAsync(string html, CancellationToken cancellationToken)
	{
		return BuildAndCacheDashboardAsync(html, DateTimeOffset.UtcNow, enrichDetails: false, cancellationToken);
	}

	private async Task<object> BuildAndCacheDashboardAsync(string html, DateTimeOffset attemptTime, bool enrichDetails, CancellationToken cancellationToken)
	{
		List<EventEntry> events = ParseList(html);
		if (events.Count == 0)
			throw new InvalidDataException(GetEmptyEventsReason(html));

		if (enrichDetails)
		{
			using SemaphoreSlim concurrency = new(4, 4);
			Task<(EventEntry Entry, bool Success)>[] enrichmentTasks = events.Take(18).Select(async entry =>
			{
				await concurrency.WaitAsync(cancellationToken);
				try
				{
					using CancellationTokenSource detailTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
					detailTimeout.CancelAfter(TimeSpan.FromSeconds(12));
					return (await EnrichAsync(entry, detailTimeout.Token), true);
				}
				catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
				{
					throw;
				}
				catch
				{
					return (entry, false);
				}
				finally
				{
					concurrency.Release();
				}
			}).ToArray();

			(EventEntry Entry, bool Success)[] enrichmentResults = await Task.WhenAll(enrichmentTasks);
			foreach ((EventEntry entry, bool success) in enrichmentResults)
			{
				if (!success)
				{
					continue;
				}
				int index = events.FindIndex(x => x.Id == entry.Id);
				if (index >= 0)
				{
					events[index] = entry;
				}
			}
			int failedDetails = enrichmentResults.Count(result => !result.Success);
			if (failedDetails > 0)
			{
				logger.Warn($"Event refresh used list data for {failedDetails} detail page(s) that did not respond within the refresh budget.");
			}
		}

		events = events
			.OrderBy(x => x.RemainingHours ?? int.MaxValue)
			.ThenBy(x => x.Title, StringComparer.OrdinalIgnoreCase)
			.ToList();

		EventCache cache = new(attemptTime, OfficialEventsUrl, events, null);
		await WriteJsonAsync(paths.EventsCachePath, cache, cancellationToken);
		await WriteJsonAsync(paths.EventsBackupCachePath, cache, cancellationToken);
		logger.Info($"Events parsed: {events.Count}.");
		logger.Info("Events cache updated: yes.");
		return BuildDashboard(cache, "LIVE", null, attemptTime);
	}

	private async Task<string> GetStringAsync(string url, CancellationToken cancellationToken)
	{
		using HttpRequestMessage request = new(HttpMethod.Get, url);
		using HttpResponseMessage response = await http.SendAsync(request, cancellationToken);
		string html = await response.Content.ReadAsStringAsync(cancellationToken);
		if (!response.IsSuccessStatusCode)
			throw new InvalidDataException($"Official events source returned HTTP {(int)response.StatusCode}.");
		return html;
	}

	private static string GetEmptyEventsReason(string html)
	{
		if (html.Contains("_Incapsula_Resource", StringComparison.OrdinalIgnoreCase)
			|| html.Contains("Incapsula incident", StringComparison.OrdinalIgnoreCase)
			|| html.Contains("Request unsuccessful", StringComparison.OrdinalIgnoreCase))
			return "The official BDO events page blocked the app's background request.";

		if (html.Contains("Under Maintenance", StringComparison.OrdinalIgnoreCase)
			|| html.Contains("Website Maintenance", StringComparison.OrdinalIgnoreCase))
			return "The official BDO events page is currently under maintenance.";

		return "The official BDO events page loaded, but no event cards could be parsed.";
	}

	private static bool HasEvents(EventCache? cache) => cache?.Events is { Count: > 0 };

	private async Task<EventEntry> EnrichAsync(EventEntry entry, CancellationToken cancellationToken)
	{
		string detailHtml = await GetStringAsync(entry.Url, cancellationToken);
		string summary = FirstNonBlank(
			ReadMeta(detailHtml, "description"),
			ReadMeta(detailHtml, "og:description"),
			entry.Summary);
		string image = FirstNonBlank(ReadMeta(detailHtml, "og:image"), entry.ImageUrl);
		DateTimeOffset? published = ParsePublishedDate(detailHtml) ?? entry.PublishedUtc;
		DateTimeOffset? end = entry.EndUtc;
		DateTimeOffset? start = FindLikelyStartDate(detailHtml) ?? published ?? entry.StartUtc;

		if (end == null && entry.RemainingHours.HasValue)
			end = DateTimeOffset.UtcNow.AddHours(Math.Max(1, entry.RemainingHours.Value));

		double progress = EstimateProgress(start, end, entry.RemainingHours);
		return entry with
		{
			ImageUrl = image,
			Summary = Truncate(StripTags(summary), 220),
			PublishedUtc = published,
			StartUtc = start,
			EndUtc = end,
			Progress = progress,
			DateRangeText = FormatDateRange(start, end, entry.TimeLeftText)
		};
	}

	private static List<EventEntry> ParseList(string html)
	{
		Match listMatch = Regex.Match(html, "<div\\s+class=\"event_list\">(?<list>[\\s\\S]*?)</ul>", RegexOptions.IgnoreCase);
		string listHtml = listMatch.Success ? listMatch.Groups["list"].Value : html;
		Regex itemRegex = new("<li>\\s*(?<item>[\\s\\S]*?)</li>", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));

		List<EventEntry> entries = [];
		HashSet<string> seen = new(StringComparer.OrdinalIgnoreCase);
		foreach (Match match in itemRegex.Matches(listHtml))
		{
			AddParsedEvent(entries, seen, match.Groups["item"].Value);
		}

		if (entries.Count == 0)
		{
			Regex anchorRegex = new(
				"<a\\b(?=[^>]*groupContentNo=)(?<attrs>[^>]*)>(?<item>[\\s\\S]*?)</a>",
				RegexOptions.IgnoreCase,
				TimeSpan.FromSeconds(2));
			foreach (Match match in anchorRegex.Matches(listHtml))
			{
				AddParsedEvent(entries, seen, "<a " + match.Groups["attrs"].Value + ">" + match.Groups["item"].Value + "</a>");
			}
		}

		if (entries.Count == 0 && html.Contains("event_list", StringComparison.OrdinalIgnoreCase))
			throw new InvalidDataException("Official event cards were found, but none could be parsed.");

		return entries;
	}

	private static void AddParsedEvent(List<EventEntry> entries, HashSet<string> seen, string itemHtml)
	{
		string url = FirstNonBlank(
			ReadGroup(itemHtml, "<a\\b[^>]+href=\"(?<value>[^\"]*groupContentNo=(?<id>\\d+)[^\"]*)\""),
			ReadGroup(itemHtml, "<a\\b[^>]+href='(?<value>[^']*groupContentNo=(?<id>\\d+)[^']*)'"));
		string id = FirstNonBlank(
			ReadGroup(itemHtml, "<a\\b[^>]+href=\"[^\"]*groupContentNo=(?<value>\\d+)[^\"]*\""),
			ReadGroup(itemHtml, "<a\\b[^>]+href='[^']*groupContentNo=(?<value>\\d+)[^']*'"));
		if (string.IsNullOrWhiteSpace(id) || !seen.Add(id))
			return;

		string title = Clean(FirstNonBlank(
			ReadGroup(itemHtml, "<strong[^>]*class=\"[^\"]*title[^\"]*\"[^>]*>\\s*(?:<em[^>]*>)?(?<value>[\\s\\S]*?)(?:</em>)?\\s*</strong>"),
			ReadGroup(itemHtml, "<[^>]+class=\"[^\"]*(?:title|subject|name)[^\"]*\"[^>]*>(?<value>[\\s\\S]*?)</[^>]+>"),
			ReadGroup(itemHtml, "<img[^>]+alt=\"(?<value>[^\"]+)\"")));
		if (string.IsNullOrWhiteSpace(title))
			title = Truncate(Clean(itemHtml), 120);
		if (string.IsNullOrWhiteSpace(title))
			return;

		string image = ReadGroup(itemHtml, "<img[^>]+src=\"(?<value>[^\"]+)\"");
		Match countMatch = Regex.Match(
			itemHtml,
			"<span[^>]*class=\"[^\"]*count[^\"]*\"[^>]*>[\\s\\S]*?<em[^>]*>\\s*(?<count>\\d+)\\s*</em>\\s*(?<unit>[^<]+)",
			RegexOptions.IgnoreCase,
			TimeSpan.FromSeconds(2));
		if (!countMatch.Success)
		{
			countMatch = Regex.Match(
				Clean(itemHtml),
				"(?<count>\\d+)\\s*(?<unit>days?|hours?|minutes?)\\s*(?:left|remaining)?",
				RegexOptions.IgnoreCase,
				TimeSpan.FromSeconds(2));
		}

		int remainingValue = countMatch.Success && int.TryParse(countMatch.Groups["count"].Value, out int parsed) ? parsed : 0;
		string rawUnit = countMatch.Success ? Clean(countMatch.Groups["unit"].Value) : "";
		int? remainingHours = countMatch.Success ? ToRemainingHours(remainingValue, rawUnit) : null;
		string timeLeft = countMatch.Success ? $"{remainingValue} {rawUnit}".Trim() : "Official event";
		string status = remainingHours.HasValue && remainingHours.Value <= 72 ? "endingSoon" : "active";
		DateTimeOffset? end = remainingHours.HasValue ? DateTimeOffset.UtcNow.AddHours(Math.Max(1, remainingHours.Value)) : null;

		entries.Add(new EventEntry(
			id,
			title,
			InferCategory(title),
			NormalizeUrl(url),
			NormalizeUrl(image),
			"",
			null,
			null,
			end,
			timeLeft,
			remainingHours,
			status,
			EstimateProgress(null, end, remainingHours),
			FormatDateRange(null, end, timeLeft),
			"Official BDO"));
	}

	private static string ReadGroup(string html, string pattern)
	{
		Match match = Regex.Match(html, pattern, RegexOptions.IgnoreCase, TimeSpan.FromSeconds(2));
		return match.Success ? match.Groups["value"].Value : "";
	}

	private static int? ToRemainingHours(int value, string unit)
	{
		string normalized = unit.ToLowerInvariant();
		if (normalized.Contains("hour"))
			return value;
		if (normalized.Contains("day"))
			return value * 24;
		if (normalized.Contains("minute"))
			return Math.Max(1, (int)Math.Ceiling(value / 60d));
		return null;
	}

	private static string InferCategory(string title)
	{
		string value = title.ToLowerInvariant();
		if (value.Contains("coupon") || value.Contains("login") || value.Contains("gift"))
			return "Login Rewards";
		if (value.Contains("hot time") || value.Contains("exp") || value.Contains("combat") || value.Contains("solare"))
			return "Combat";
		if (value.Contains("life") || value.Contains("fishing") || value.Contains("gather") || value.Contains("cooking"))
			return "Life";
		if (value.Contains("sale") || value.Contains("package") || value.Contains("pearl"))
			return "Rewards";
		if (value.Contains("guild") || value.Contains("war"))
			return "Guild";
		if (value.Contains("season"))
			return "Season";
		return "Adventure";
	}

	private static DateTimeOffset? ParsePublishedDate(string html)
	{
		Match match = Regex.Match(html, "<span\\s+class=\"date\">(?<date>.*?)</span>", RegexOptions.IgnoreCase);
		if (!match.Success)
			return null;

		string text = Clean(match.Groups["date"].Value).Replace(" (UTC)", " +00:00", StringComparison.OrdinalIgnoreCase);
		return DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTimeOffset date)
			? date.ToUniversalTime()
			: null;
	}

	private static DateTimeOffset? FindLikelyStartDate(string html)
	{
		string text = StripTags(html);
		Match match = Regex.Match(text, "([A-Z][a-z]{2,8}\\.?\\s+\\d{1,2},\\s+20\\d{2})", RegexOptions.IgnoreCase);
		if (!match.Success)
			return null;
		return DateTimeOffset.TryParse(match.Groups[1].Value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTimeOffset date)
			? date.ToUniversalTime()
			: null;
	}

	private static string ReadMeta(string html, string name)
	{
		Match match = Regex.Match(
			html,
			$"<meta[^>]+(?:name|property)=\"{Regex.Escape(name)}\"[^>]+content=\"(?<content>[^\"]*)\"[^>]*>",
			RegexOptions.IgnoreCase);
		return match.Success ? Clean(match.Groups["content"].Value) : "";
	}

	private static object BuildDashboard(EventCache cache, string status, string? message, DateTimeOffset? lastAttempt = null)
	{
		bool isStale = DateTimeOffset.UtcNow - cache.LastRefreshed > TimeSpan.FromHours(3);
		int cacheAgeMinutes = Math.Max(0, (int)Math.Round((DateTimeOffset.UtcNow - cache.LastRefreshed).TotalMinutes));
		var events = cache.Events.Select(x => new
		{
			x.Id,
			x.Title,
			x.Category,
			x.Url,
			x.ImageUrl,
			x.Summary,
			x.PublishedUtc,
			x.StartUtc,
			x.EndUtc,
			x.TimeLeftText,
			x.RemainingHours,
			x.Status,
			x.Progress,
			x.DateRangeText,
			x.Source
		}).ToArray();

		return new
		{
			status,
			message,
			sourceUrl = cache.SourceUrl,
			lastRefreshed = cache.LastRefreshed,
			lastAttempt,
			isStale,
			cacheAgeMinutes,
			events,
			activeCount = events.Count(x => x.Status == "active"),
			endingSoonCount = events.Count(x => x.Status == "endingSoon"),
			totalCount = events.Length
		};
	}

	private static double EstimateProgress(DateTimeOffset? start, DateTimeOffset? end, int? remainingHours)
	{
		if (start.HasValue && end.HasValue && end > start)
		{
			double total = (end.Value - start.Value).TotalSeconds;
			double elapsed = (DateTimeOffset.UtcNow - start.Value).TotalSeconds;
			return Math.Clamp(elapsed / total, 0.04, 0.98);
		}

		if (!remainingHours.HasValue)
			return 0.52;

		double days = Math.Max(1, remainingHours.Value / 24d);
		double assumedTotal = days <= 3 ? 7 : days <= 14 ? 21 : 35;
		return Math.Clamp((assumedTotal - days) / assumedTotal, 0.08, 0.92);
	}

	private static string FormatDateRange(DateTimeOffset? start, DateTimeOffset? end, string fallback)
	{
		if (start.HasValue && end.HasValue)
			return $"{start.Value:MMM d} - {end.Value:MMM d}";
		if (end.HasValue)
			return $"Ends around {end.Value:MMM d}";
		return fallback;
	}

	private static string NormalizeUrl(string value)
	{
		string cleaned = WebUtility.HtmlDecode(value ?? "").Trim();
		if (string.IsNullOrWhiteSpace(cleaned))
			return "";
		if (Uri.TryCreate(cleaned, UriKind.Absolute, out Uri? absolute))
			return absolute.AbsoluteUri;
		return new Uri(SiteRoot, cleaned).AbsoluteUri;
	}

	private static string FirstNonBlank(params string[] values) => values.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)) ?? "";

	private static string Clean(string value) => WebUtility.HtmlDecode(StripTags(value)).Replace('\u00a0', ' ').Trim();

	private static string StripTags(string value)
	{
		string text = Regex.Replace(value ?? "", "<[^>]+>", " ");
		return Regex.Replace(text, "\\s+", " ").Trim();
	}

	private static string Truncate(string value, int max)
	{
		if (string.IsNullOrWhiteSpace(value) || value.Length <= max)
			return value;
		return value[..Math.Max(0, max - 1)].TrimEnd() + "...";
	}

	private static async Task<T?> ReadJsonAsync<T>(string path, CancellationToken cancellationToken)
	{
		if (!File.Exists(path))
			return default;
		await using FileStream stream = File.OpenRead(path);
		return await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions, cancellationToken);
	}

	private static async Task WriteJsonAsync<T>(string path, T value, CancellationToken cancellationToken)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(path)!);
		await using FileStream stream = File.Create(path);
		await JsonSerializer.SerializeAsync(stream, value, JsonOptions, cancellationToken);
	}

	public void Dispose() => http.Dispose();

	private sealed record EventCache(DateTimeOffset LastRefreshed, string SourceUrl, IReadOnlyList<EventEntry> Events, string? Error);

	private sealed record EventEntry(
		string Id,
		string Title,
		string Category,
		string Url,
		string ImageUrl,
		string Summary,
		DateTimeOffset? PublishedUtc,
		DateTimeOffset? StartUtc,
		DateTimeOffset? EndUtc,
		string TimeLeftText,
		int? RemainingHours,
		string Status,
		double Progress,
		string DateRangeText,
		string Source);
}

