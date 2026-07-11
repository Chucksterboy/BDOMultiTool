using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BDOMultiTool;

internal sealed class CouponService : IDisposable
{
	private const string SourceUrl = "https://api.bdoalerts.net/api/coupons";
	private const string OfficialSourceUrl = "https://www.naeu.playblackdesert.com/en-US/News/Detail?groupContentNo=5676";
	private readonly AppPaths paths;
	private readonly AppLogger logger;
	private readonly HttpClient http;
	private static readonly JsonSerializerOptions JsonOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		PropertyNameCaseInsensitive = true,
		WriteIndented = true
	};

	public CouponService(AppPaths paths, AppLogger logger)
	{
		this.paths = paths;
		this.logger = logger;
		http = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
		http.DefaultRequestHeaders.UserAgent.ParseAdd("BDO-Multi-Tool/2.7 (+local read-only coupon tracker)");
		http.DefaultRequestHeaders.Referrer = new Uri("https://bdoalerts.net/coupons/");
		http.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://bdoalerts.net");
	}

	public async Task<object> InitializeAsync(CancellationToken cancellationToken)
	{
		await EnsureSeedCacheAsync(cancellationToken);
		CouponCache? cache = await ReadJsonAsync<CouponCache>(paths.CouponsCachePath, cancellationToken);
		if (cache != null)
			await CacheIconsAsync(cache.Coupons, cancellationToken);
		return await BuildDashboardAsync("CACHED", null, cancellationToken);
	}

	public async Task<object> SaveSettingsAsync(CouponSettings settings, CancellationToken cancellationToken)
	{
		await WriteJsonAsync(paths.CouponSettingsPath, settings, cancellationToken);
		return await BuildDashboardAsync("CACHED", null, cancellationToken);
	}

	public async Task<object> RefreshAsync(CancellationToken cancellationToken)
	{
		DateTimeOffset attemptTime = DateTimeOffset.UtcNow;
		logger.Info("Coupons refresh started.");
		logger.Info($"Coupons official source URL: {OfficialSourceUrl}");
		logger.Info($"Coupons BDO Alerts source URL: {SourceUrl}");
		bool cacheUpdated = false;
		try
		{
			List<CouponEntry> officialCoupons = [];
			string? officialFailure = null;
			int officialLength = 0;
			try
			{
				using HttpRequestMessage officialRequest = new(HttpMethod.Get, OfficialSourceUrl);
				officialRequest.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126 Safari/537.36");
				officialRequest.Headers.Referrer = new Uri("https://www.naeu.playblackdesert.com/");
				using HttpResponseMessage officialResponse = await http.SendAsync(officialRequest, cancellationToken);
				string officialHtml = await officialResponse.Content.ReadAsStringAsync(cancellationToken);
				officialLength = officialHtml.Length;
				if (officialResponse.IsSuccessStatusCode)
				{
					officialCoupons = ParseOfficialCouponPage(officialHtml);
					logger.Info($"Official coupons parsed: {officialCoupons.Count}.");
				}
				else
				{
					officialFailure = $"Official BDO source returned HTTP {(int)officialResponse.StatusCode}.";
					logger.Warn(officialFailure);
				}
			}
			catch (Exception ex)
			{
				officialFailure = "Official BDO source could not be read: " + ex.Message;
				logger.Warn(officialFailure);
			}

			using HttpResponseMessage response = await http.GetAsync(SourceUrl, cancellationToken);
			string html = await response.Content.ReadAsStringAsync(cancellationToken);
			int statusCode = (int)response.StatusCode;
			logger.Info($"Coupons HTTP status: {statusCode} {response.StatusCode}");
			if (!response.IsSuccessStatusCode)
			{
				string failure = statusCode == 403
					? "Live refresh blocked by BDO Alerts: HTTP 403. Showing cached data."
					: $"Live coupon refresh failed: HTTP {statusCode}. Showing cached data.";
				logger.Info("Coupons parsing succeeded: no (HTTP request was rejected).");
				logger.Info("Coupons parsed: 0.");
				logger.Info("Coupons cache updated: no.");
				logger.Warn("Coupons refresh failed reason: " + failure);
				return await BuildDashboardAsync("CACHED", failure, cancellationToken, attemptTime,
					new CouponRefreshDebug(SourceUrl, statusCode, 0, 0, false, false, failure));
			}
			logger.Info($"Coupons raw response length: {html.Length} characters.");
			List<CouponEntry> coupons = ParseBdoAlertsResponse(html);
			coupons = MergeCouponSources(officialCoupons, coupons);
			logger.Info($"Coupons parsed: {coupons.Count}.");
			logger.Info($"Coupons parsing succeeded: {(coupons.Count > 0 ? "yes" : "no")}.");
			if (coupons.Count == 0)
				throw new InvalidDataException("No coupon entries could be read from the public page.");

			int icons = await CacheIconsAsync(coupons, cancellationToken);
			CouponCache cache = new(DateTimeOffset.UtcNow, "BDO Alerts", coupons, null);
			await WriteJsonAsync(paths.CouponsCachePath, cache, cancellationToken);
			cacheUpdated = true;
			logger.Info("Coupons cache updated: yes.");
			LogSummary(coupons, icons, "LIVE");
			return await BuildDashboardAsync("LIVE", null, cancellationToken, attemptTime,
				new CouponRefreshDebug($"{OfficialSourceUrl} + {SourceUrl}", statusCode, html.Length + officialLength, coupons.Count, true, true, officialFailure));
		}
		catch (Exception ex)
		{
			logger.Info($"Coupons cache updated: {(cacheUpdated ? "yes" : "no")}.");
			logger.Warn("Coupons refresh failed reason: " + ex.Message);
			await EnsureSeedCacheAsync(cancellationToken);
			string failure = "Could not refresh coupons. Showing cached data. " + ex.Message;
			return await BuildDashboardAsync("CACHED", failure, cancellationToken, attemptTime,
				new CouponRefreshDebug(SourceUrl, null, 0, 0, false, cacheUpdated, ex.Message));
		}
	}

	public async Task<IReadOnlyList<CouponEntry>> GetCouponsAsync(CancellationToken cancellationToken)
	{
		await EnsureSeedCacheAsync(cancellationToken);
		return (await ReadJsonAsync<CouponCache>(paths.CouponsCachePath, cancellationToken))?.Coupons ?? [];
	}

	private async Task<object> BuildDashboardAsync(string status, string? error, CancellationToken cancellationToken,
		DateTimeOffset? lastAttempt = null, CouponRefreshDebug? refreshDebug = null)
	{
		CouponCache cache = await ReadJsonAsync<CouponCache>(paths.CouponsCachePath, cancellationToken)
			?? new CouponCache(DateTimeOffset.UtcNow, "Manual", [], error);
		CouponSettings settings = await ReadJsonAsync<CouponSettings>(paths.CouponSettingsPath, cancellationToken)
			?? new CouponSettings(true, true, "", "all");
		bool isStale = DateTimeOffset.UtcNow - cache.LastRefreshed > TimeSpan.FromHours(6);
		int cacheAgeMinutes = Math.Max(0, (int)Math.Round((DateTimeOffset.UtcNow - cache.LastRefreshed).TotalMinutes));
		var coupons = cache.Coupons.Where(c => !IsLikelyNonCouponToken(c.Code)).Select(c => new
		{
			c.Code,
			addedUtc = c.AddedUtc,
			expiryUtc = c.ExpiryUtc,
			addedText = c.AddedUtc.HasValue ? FormatRelativeDate(c.AddedUtc) : c.AddedText,
			expiryText = c.ExpiryUtc.HasValue ? FormatExpiry(c.ExpiryUtc, c.IsExpired) : c.ExpiryText,
			c.IsExpired,
			c.Source,
			rewards = c.Rewards.Select(r => new
			{
				r.ItemName,
				r.Quantity,
				icon = ReadIconDataUrl(r.IconFileName)
			})
		}).ToArray();
		return new
		{
			status,
			message = error,
			sourceUrl = SourceUrl,
			lastRefreshed = cache.LastRefreshed,
			lastAttempt,
			isStale,
			cacheAgeMinutes,
			refreshDebug,
			settings,
			coupons,
			availableCount = coupons.Count(x => !x.IsExpired),
			expiredCount = coupons.Count(x => x.IsExpired),
			totalCount = coupons.Length
		};
	}

	private async Task EnsureSeedCacheAsync(CancellationToken cancellationToken)
	{
		if (!File.Exists(paths.CouponsCachePath))
		{
			CouponCache seed = new(DateTimeOffset.UtcNow, "Cached", SeedCoupons(), "Seed cache created from the last publicly verified coupon listing.");
			await WriteJsonAsync(paths.CouponsCachePath, seed, cancellationToken);
			logger.Info($"Coupon seed cache created with {seed.Coupons.Count} entries.");
		}
		if (!File.Exists(paths.CouponSettingsPath))
			await WriteJsonAsync(paths.CouponSettingsPath, new CouponSettings(true, true, "", "all"), cancellationToken);
	}

	private static List<CouponEntry> SeedCoupons() =>
	[
		new("BDAYWAKAPARTYNOW", null, "2 days ago", null, "5 days", false,
			[new("Resplendent Oasis Box", 1, "https://assets.garmoth.com/img/new_icon/03_etc/01000306.webp", "01000306.webp")], "Garmoth"),
		new("THEDESERTTHNXYOU", null, "6 months ago", null, "9 days", false,
			[new("Cron Stone", 10000, "https://assets.garmoth.com/img/new_icon/03_etc/00016080.webp", "00016080.webp")], "Garmoth"),
		new("THNX4BEINGWITHUS", null, "6 months ago", null, "9 days", false,
			[
				new("Cron Stone", 20000, "https://assets.garmoth.com/img/new_icon/03_etc/00016080.webp", "00016080.webp"),
				new("Choose Your Transcendent Hammer Box", 5, "https://assets.garmoth.com/img/new_icon/09_cash/00046991.webp", "00046991.webp"),
				new("Advice of Valks (+400)", 1, "https://assets.garmoth.com/img/new_icon/03_etc/15_advice/00000400_11.webp", "00000400_11.webp"),
				new("Advice of Valks (+350)", 1, "https://assets.garmoth.com/img/new_icon/03_etc/15_advice/00000350_11.webp", "00000350_11.webp"),
				new("Advice of Valks (+300)", 1, "https://assets.garmoth.com/img/new_icon/03_etc/15_advice/00000300_11.webp", "00000300_11.webp"),
				new("Advice of Valks (+250)", 1, "https://assets.garmoth.com/img/new_icon/03_etc/15_advice/00000250_11.webp", "00000250_11.webp"),
				new("Weapon Exchange Coupon Box", 1, "https://assets.garmoth.com/img/new_icon/09_cash/00290007.webp", "00290007.webp"),
				new("J's Special Scroll", 20, "https://assets.garmoth.com/img/new_icon/03_etc/08_potion/00000771.webp", "00000771.webp")
			], "Garmoth"),
		new("OFFTOBATTLE", null, "15 days ago", null, "Expired 2 days ago", true,
			[new("Perfume of Courage", 3, "", ""), new("Perfume of Deep Sea", 3, "", ""), new("Tough Whale Tendon Elixir", 3, "", "")], "Garmoth"),
		new("POTIMATOUBDAY", null, "1 month ago", null, "Expired 1 month ago", true,
			[new("Resplendent Oasis Box", 1, "https://assets.garmoth.com/img/new_icon/03_etc/01000306.webp", "01000306.webp")], "Garmoth"),
		new("BLADENZBDAY", null, "1 month ago", null, "Expired 1 month ago", true,
			[new("Resplendent Oasis Box", 1, "https://assets.garmoth.com/img/new_icon/03_etc/01000306.webp", "01000306.webp")], "Garmoth")
	];

	private static List<CouponEntry> ParseBdoAlertsResponse(string json)
	{
		List<CouponEntry> result = [];
		using JsonDocument document = JsonDocument.Parse(json);
		if (!document.RootElement.TryGetProperty("coupons", out JsonElement coupons)
			|| coupons.ValueKind != JsonValueKind.Array)
			return result;
		foreach (JsonElement coupon in coupons.EnumerateArray())
		{
			string code = coupon.TryGetProperty("code", out JsonElement codeValue)
				? (codeValue.GetString() ?? "").Trim().ToUpperInvariant() : "";
			if (string.IsNullOrWhiteSpace(code))
				continue;
			bool expired = coupon.TryGetProperty("is_expired", out JsonElement expiredValue) && expiredValue.GetBoolean();
			DateTimeOffset? created = ReadDate(coupon, "created_at");
			DateTimeOffset? expiry = ReadDate(coupon, "expiry_date");
			List<CouponReward> rewards = [];
			if (coupon.TryGetProperty("rewards_structured", out JsonElement structured)
				&& structured.TryGetProperty("items", out JsonElement items)
				&& items.ValueKind == JsonValueKind.Array)
			{
				foreach (JsonElement item in items.EnumerateArray())
				{
					string name = item.TryGetProperty("name", out JsonElement nameValue)
						? (nameValue.GetString() ?? "Unknown reward").Trim() : "Unknown reward";
					int quantity = item.TryGetProperty("quantity", out JsonElement quantityValue)
						&& quantityValue.TryGetInt32(out int parsedQuantity) ? Math.Max(1, parsedQuantity) : 1;
					(string iconUrl, string iconFileName) = ResolveKnownIcon(name);
					rewards.Add(new CouponReward(name, quantity, iconUrl, iconFileName));
				}
			}
			if (rewards.Count == 0)
				rewards.Add(new CouponReward("Reward details available on BDO Alerts", 1, "", ""));
			result.Add(new CouponEntry(code, created, FormatRelativeDate(created), expiry, FormatExpiry(expiry, expired), expired, rewards, "BDO Alerts"));
		}
		return result;
	}

	private static List<CouponEntry> ParseOfficialCouponPage(string html)
	{
		List<CouponEntry> result = [];
		HashSet<string> seen = new(StringComparer.OrdinalIgnoreCase);
		string text = WebUtility.HtmlDecode(Regex.Replace(html, "<[^>]+>", " "));
		foreach (Match match in Regex.Matches(text, @"\b[A-Z0-9]{12,20}\b"))
		{
			string code = match.Value.Trim().ToUpperInvariant();
			if (!seen.Add(code) || IsLikelyNonCouponToken(code))
				continue;
			result.Add(new CouponEntry(code, null, "Official source", null, "No expiry listed", false,
				[new CouponReward("Official BDO coupon reward", 1, "", "")], "Official BDO"));
		}
		return result;
	}

	private static bool IsLikelyNonCouponToken(string value)
	{
		if (Regex.IsMatch(value, @"^\d+$"))
			return true;
		string[] blocked =
		[
			"BLACKDESERT", "PEARLABYSS", "WINDOWS", "JAVASCRIPT", "COMMUNITY",
			"ANNOUNCEMENT", "ADVENTURER", "UPDATEHISTORY", "DOWNLOADGAME"
		];
		return blocked.Any(value.Contains);
	}

	private static List<CouponEntry> MergeCouponSources(IEnumerable<CouponEntry> officialCoupons, IEnumerable<CouponEntry> alertCoupons)
	{
		Dictionary<string, CouponEntry> merged = new(StringComparer.OrdinalIgnoreCase);
		foreach (CouponEntry coupon in officialCoupons)
			merged[coupon.Code] = coupon;
		foreach (CouponEntry coupon in alertCoupons)
		{
			if (merged.TryGetValue(coupon.Code, out CouponEntry? official))
			{
				merged[coupon.Code] = coupon with
				{
					Source = official.Source + " + " + coupon.Source
				};
			}
			else
			{
				merged[coupon.Code] = coupon;
			}
		}
		return merged.Values.ToList();
	}

	private static DateTimeOffset? ReadDate(JsonElement element, string property)
	{
		if (!element.TryGetProperty(property, out JsonElement value)
			|| value.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
			return null;
		return DateTimeOffset.TryParse(value.GetString(), out DateTimeOffset parsed) ? parsed : null;
	}

	private static string FormatRelativeDate(DateTimeOffset? date)
	{
		if (!date.HasValue)
			return "Unknown";
		TimeSpan age = DateTimeOffset.UtcNow - date.Value;
		if (age.TotalDays < 1)
			return age.TotalHours < 1 ? "Just now" : $"{Math.Max(1, (int)age.TotalHours)} hours ago";
		if (age.TotalDays < 60)
			return $"{Math.Max(1, (int)age.TotalDays)} days ago";
		return $"{Math.Max(1, (int)(age.TotalDays / 30))} months ago";
	}

	private static string FormatExpiry(DateTimeOffset? expiry, bool expired)
	{
		if (!expiry.HasValue)
			return expired ? "Expired" : "No expiry listed";
		TimeSpan remaining = expiry.Value - DateTimeOffset.UtcNow;
		if (remaining <= TimeSpan.Zero)
			return $"Expired {Math.Max(1, (int)Math.Abs(remaining.TotalDays))} days ago";
		return remaining.TotalDays < 1 ? "Expires today" : $"{Math.Max(1, (int)Math.Ceiling(remaining.TotalDays))} days";
	}

	private static (string Url, string FileName) ResolveKnownIcon(string itemName)
	{
		string key = itemName.ToLowerInvariant();
		if (key.Contains("cron stone")) return ("https://assets.garmoth.com/img/new_icon/03_etc/00016080.webp", "00016080.webp");
		if (key.Contains("resplendent oasis box")) return ("https://assets.garmoth.com/img/new_icon/03_etc/01000306.webp", "01000306.webp");
		if (key.Contains("transcendent hammer")) return ("https://assets.garmoth.com/img/new_icon/09_cash/00046991.webp", "00046991.webp");
		if (key.Contains("+400")) return ("https://assets.garmoth.com/img/new_icon/03_etc/15_advice/00000400_11.webp", "00000400_11.webp");
		if (key.Contains("+350")) return ("https://assets.garmoth.com/img/new_icon/03_etc/15_advice/00000350_11.webp", "00000350_11.webp");
		if (key.Contains("+300")) return ("https://assets.garmoth.com/img/new_icon/03_etc/15_advice/00000300_11.webp", "00000300_11.webp");
		if (key.Contains("+250")) return ("https://assets.garmoth.com/img/new_icon/03_etc/15_advice/00000250_11.webp", "00000250_11.webp");
		if (key.Contains("weapon exchange coupon")) return ("https://assets.garmoth.com/img/new_icon/09_cash/00290007.webp", "00290007.webp");
		if (key.Contains("j's special scroll")) return ("https://assets.garmoth.com/img/new_icon/03_etc/08_potion/00000771.webp", "00000771.webp");
		return ("", "");
	}

	private async Task<int> CacheIconsAsync(IEnumerable<CouponEntry> coupons, CancellationToken cancellationToken)
	{
		int count = 0;
		foreach (CouponReward reward in coupons.SelectMany(c => c.Rewards))
		{
			if (string.IsNullOrWhiteSpace(reward.IconUrl) || string.IsNullOrWhiteSpace(reward.IconFileName))
				continue;
			if (!Uri.TryCreate(reward.IconUrl, UriKind.Absolute, out Uri? uri) || uri.Scheme != Uri.UriSchemeHttps || !uri.Host.Equals("assets.garmoth.com", StringComparison.OrdinalIgnoreCase))
				continue;
			string target = Path.Combine(paths.CouponIconsPath, Path.GetFileName(reward.IconFileName));
			if (File.Exists(target))
			{
				count++;
				continue;
			}
			try
			{
				using HttpRequestMessage request = new(HttpMethod.Get, uri);
				request.Headers.Referrer = new Uri("https://garmoth.com/coupons");
				request.Headers.Accept.ParseAdd("image/avif,image/webp,image/png,image/*");
				using HttpResponseMessage response = await http.SendAsync(request, cancellationToken);
				response.EnsureSuccessStatusCode();
				byte[] bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
				if (bytes.Length is > 0 and < 2_000_000)
				{
					await File.WriteAllBytesAsync(target, bytes, cancellationToken);
					count++;
				}
			}
			catch (Exception ex)
			{
				logger.Warn($"Coupon icon could not be cached ({uri}): {ex.Message}");
			}
		}
		return count;
	}

	private string ReadIconDataUrl(string fileName)
	{
		if (!string.IsNullOrWhiteSpace(fileName))
		{
			string path = Path.Combine(paths.CouponIconsPath, Path.GetFileName(fileName));
			if (File.Exists(path))
			{
				string extension = Path.GetExtension(path).ToLowerInvariant();
				string mime = extension == ".png" ? "image/png" : extension is ".jpg" or ".jpeg" ? "image/jpeg" : "image/webp";
				return $"data:{mime};base64,{Convert.ToBase64String(File.ReadAllBytes(path))}";
			}
		}
		return "data:image/svg+xml;charset=utf-8," + Uri.EscapeDataString("<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 48 48'><defs><linearGradient id='g' x2='1' y2='1'><stop stop-color='#e2ae43'/><stop offset='1' stop-color='#6f2b16'/></linearGradient></defs><rect x='3' y='3' width='42' height='42' rx='7' fill='#24080a' stroke='#b97729'/><path d='M14 17h20v19H14zM12 13h24v7H12zm12 0v23M18 13c-5-5 5-9 6 0m6 0c5-5-5-9-6 0' fill='none' stroke='url(#g)' stroke-width='2'/></svg>");
	}

	private void LogSummary(IReadOnlyCollection<CouponEntry> coupons, int icons, string source)
	{
		logger.Info($"Coupons source: {source}; found: {coupons.Count}; available: {coupons.Count(c => !c.IsExpired)}; expired: {coupons.Count(c => c.IsExpired)}; icons loaded: {icons}; refreshed: {DateTimeOffset.Now:O}");
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
		string temporary = path + ".tmp";
		await using (FileStream stream = File.Create(temporary))
			await JsonSerializer.SerializeAsync(stream, value, JsonOptions, cancellationToken);
		File.Move(temporary, path, true);
	}

	public void Dispose() => http.Dispose();
}

internal sealed record CouponSettings(bool ShowAvailableOnly, bool ShowExpired, string Search, string Status);
internal sealed record CouponReward(string ItemName, int Quantity, string IconUrl, string IconFileName);
internal sealed record CouponEntry(string Code, DateTimeOffset? AddedUtc, string AddedText, DateTimeOffset? ExpiryUtc, string ExpiryText, bool IsExpired, List<CouponReward> Rewards, string Source);
internal sealed record CouponCache(DateTimeOffset LastRefreshed, string Source, List<CouponEntry> Coupons, string? LastError);
internal sealed record CouponRefreshDebug(string SourceUrl, int? HttpStatus, int RawResponseLength, int CouponsParsed, bool ParsingSucceeded, bool CacheUpdated, string? FailureReason);

