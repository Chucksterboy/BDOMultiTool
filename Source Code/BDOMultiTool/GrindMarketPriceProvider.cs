using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BDOMultiTool;

internal sealed record GrindMarketPrice(
	long ItemId,
	int Enhancement,
	string Name,
	long Price,
	long? LowestListedPrice,
	long? BasePrice,
	long? LastSoldPrice,
	long? Stock,
	long? TradeCount,
	string Source,
	DateTimeOffset CapturedUtc);

internal sealed record GrindMarketPriceResponse(
	string Region,
	DateTimeOffset CapturedUtc,
	IReadOnlyList<GrindMarketPrice> Prices,
	IReadOnlyList<long> Missing,
	string Provider,
	string Message);

internal sealed class GrindMarketPriceProvider : IDisposable
{
	private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(12);

	private readonly HttpClient client;
	private readonly AppLogger logger;

	public GrindMarketPriceProvider(AppLogger logger)
	{
		this.logger = logger;
		client = new HttpClient
		{
			Timeout = RequestTimeout
		};
		client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("BDO-Multi-Tool", "1.0"));
		client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	public async Task<GrindMarketPriceResponse> GetPricesAsync(IEnumerable<long> itemIds, string region, CancellationToken cancellationToken)
	{
		string normalizedRegion = NormalizeRegion(region);
		long[] ids = itemIds.Where(id => id > 0).Distinct().OrderBy(id => id).ToArray();
		DateTimeOffset captured = DateTimeOffset.UtcNow;
		List<GrindMarketPrice> prices = new();
		string provider = "Arsha GetWorldMarketSubList";

		if (ids.Length == 0)
		{
			return new GrindMarketPriceResponse(normalizedRegion, captured, prices, Array.Empty<long>(), provider, "No market item IDs were supplied.");
		}

		try
		{
			prices.AddRange(await FetchArshaSubListResilientAsync(ids, normalizedRegion, captured, cancellationToken));
			logger.Info($"Arsha GetWorldMarketSubList {normalizedRegion.ToUpperInvariant()} batch returned {prices.Count.ToString(CultureInfo.InvariantCulture)}/{ids.Length.ToString(CultureInfo.InvariantCulture)} grind prices.");
		}
		catch (Exception ex) when (IsProviderFailure(ex))
		{
			logger.Warn($"Arsha GetWorldMarketSubList {normalizedRegion.ToUpperInvariant()} batch failed: {ex.Message}");
		}

		HashSet<long> resolved = prices.Select(price => price.ItemId).ToHashSet();
		long[] missingIds = ids.Except(resolved).ToArray();
		if (missingIds.Length > 0)
		{
			try
			{
				IReadOnlyList<GrindMarketPrice> codexPrices = await FetchBdoCodexPricesAsync(missingIds, captured, cancellationToken);
				foreach (GrindMarketPrice price in codexPrices)
				{
					if (resolved.Add(price.ItemId))
					{
						prices.Add(price);
					}
				}
				if (codexPrices.Count > 0)
				{
					provider = "Arsha GetWorldMarketSubList + BDO Codex";
					logger.Info($"BDO Codex EU fallback returned {codexPrices.Count.ToString(CultureInfo.InvariantCulture)}/{missingIds.Length.ToString(CultureInfo.InvariantCulture)} grind prices.");
				}
			}
			catch (Exception ex) when (IsProviderFailure(ex))
			{
				logger.Warn($"BDO Codex EU fallback failed: {ex.Message}");
			}
		}

		resolved = prices.Select(price => price.ItemId).ToHashSet();
		IReadOnlyList<long> missing = ids.Except(resolved).ToArray();
		string message = prices.Count == 0
			? "EU market prices could not be refreshed from Arsha or BDO Codex."
			: $"EU market prices refreshed: {prices.Count.ToString(CultureInfo.InvariantCulture)}/{ids.Length.ToString(CultureInfo.InvariantCulture)} items.";

		return new GrindMarketPriceResponse(normalizedRegion, captured, prices, missing, provider, message);
	}

	private async Task<IReadOnlyList<GrindMarketPrice>> FetchArshaSubListBatchAsync(long[] itemIds, string region, DateTimeOffset captured, CancellationToken cancellationToken)
	{
		for (int attempt = 1; ; attempt++)
		{
			using HttpRequestMessage request = new(HttpMethod.Post, $"https://api.arsha.io/v2/{region}/GetWorldMarketSubList?lang=en");
			request.Content = new StringContent(JsonSerializer.Serialize(itemIds), Encoding.UTF8, "application/json");
			try
			{
				using JsonDocument document = await SendJsonAsync(request, cancellationToken);
				List<GrindMarketPrice> prices = new();
				foreach (long itemId in itemIds)
				{
					MarketSubListEntry entry = ParseSubList(document.RootElement, itemId).FirstOrDefault();
					if (entry.ItemId > 0)
					{
						prices.Add(ToPrice(entry, null, "arsha-sublist-cache", captured));
					}
				}
				return prices;
			}
			catch (Exception ex) when (attempt == 1 && IsEndpointUnavailable(ex))
			{
				logger.Warn($"Arsha GetWorldMarketSubList {region.ToUpperInvariant()} retrying after upstream failure: {ex.Message}");
				await Task.Delay(400, cancellationToken);
			}
		}
	}

	private async Task<IReadOnlyList<GrindMarketPrice>> FetchArshaSubListResilientAsync(long[] itemIds, string region, DateTimeOffset captured, CancellationToken cancellationToken)
	{
		try
		{
			return await FetchArshaSubListBatchAsync(itemIds, region, captured, cancellationToken);
		}
		catch (Exception ex) when (IsProviderFailure(ex))
		{
			if (!CanSplitArshaFailure(ex, region) || itemIds.Length <= 1)
			{
				if (itemIds.Length == 1)
				{
					logger.Warn($"Arsha GetWorldMarketSubList {region.ToUpperInvariant()} skipped item {itemIds[0].ToString(CultureInfo.InvariantCulture)}: {ex.Message}");
					return Array.Empty<GrindMarketPrice>();
				}
				throw;
			}

			int middle = itemIds.Length / 2;
			long[] leftIds = itemIds.Take(middle).ToArray();
			long[] rightIds = itemIds.Skip(middle).ToArray();
			IReadOnlyList<GrindMarketPrice> left = await FetchArshaSubListResilientAsync(leftIds, region, captured, cancellationToken);
			IReadOnlyList<GrindMarketPrice> right = await FetchArshaSubListResilientAsync(rightIds, region, captured, cancellationToken);
			return left.Concat(right).ToArray();
		}
	}

	private async Task<GrindMarketPrice> FetchArshaSubListSingleAsync(long itemId, string region, DateTimeOffset captured, CancellationToken cancellationToken)
	{
		foreach (Func<CancellationToken, Task<JsonDocument>> fetch in new Func<CancellationToken, Task<JsonDocument>>[]
		{
			token => FetchArshaSubListV2GetAsync(itemId, region, token),
			token => FetchArshaSubListV2PostAsync(itemId, region, token),
			token => FetchArshaItemV1GetAsync(itemId, region, token)
		})
		{
			try
			{
				using JsonDocument document = await fetch(cancellationToken);
				MarketSubListEntry entry = ParseSubList(document.RootElement, itemId).FirstOrDefault();
				if (entry.ItemId > 0)
				{
					return ToPrice(entry, null, "arsha-sublist-cache", captured);
				}
			}
			catch (Exception ex) when (IsProviderFailure(ex))
			{
				logger.Warn($"Arsha sublist fallback failed for {itemId}: {ex.Message}");
			}
		}

		throw new InvalidDataException($"Arsha GetWorldMarketSubList returned no usable item data for {itemId}.");
	}

	private async Task<IReadOnlyList<GrindMarketPrice>> FetchBdoCodexPricesAsync(long[] itemIds, DateTimeOffset captured, CancellationToken cancellationToken)
	{
		using SemaphoreSlim gate = new(6);
		Task<GrindMarketPrice?>[] tasks = itemIds.Select(async itemId =>
		{
			await gate.WaitAsync(cancellationToken);
			try
			{
				return await FetchBdoCodexPriceAsync(itemId, captured, cancellationToken);
			}
			catch (Exception ex) when (IsProviderFailure(ex))
			{
				logger.Warn($"BDO Codex EU fallback failed for {itemId.ToString(CultureInfo.InvariantCulture)}: {ex.Message}");
				return null;
			}
			finally
			{
				gate.Release();
			}
		}).ToArray();

		GrindMarketPrice?[] prices = await Task.WhenAll(tasks);
		return prices.Where(price => price is not null).Select(price => price!).ToArray();
	}

	private async Task<GrindMarketPrice?> FetchBdoCodexPriceAsync(long itemId, DateTimeOffset captured, CancellationToken cancellationToken)
	{
		using HttpRequestMessage request = new(HttpMethod.Get, $"https://bdocodex.com/us/item/{itemId.ToString(CultureInfo.InvariantCulture)}/");
		request.Headers.Accept.Clear();
		request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
		request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36");
		using HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
		string html = await response.Content.ReadAsStringAsync(cancellationToken);
		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException($"BDO Codex returned {(int)response.StatusCode} {response.ReasonPhrase}.");
		}

		Match match = Regex.Match(html, "real_item_prices=(\\{.*?\\});</script>", RegexOptions.Singleline);
		if (!match.Success)
		{
			return ToCodexVendorSellPrice(itemId, html, captured);
		}

		using JsonDocument document = JsonDocument.Parse(match.Groups[1].Value);
		if (!document.RootElement.TryGetProperty("prices", out JsonElement prices)
			|| !prices.TryGetProperty("EU", out JsonElement euPrices)
			|| euPrices.ValueKind != JsonValueKind.Array
			|| euPrices.GetArrayLength() == 0)
		{
			return ToCodexVendorSellPrice(itemId, html, captured);
		}

		JsonElement first = euPrices[0];
		if (first.ValueKind != JsonValueKind.Array || first.GetArrayLength() == 0)
		{
			return ToCodexVendorSellPrice(itemId, html, captured);
		}

		long price = ReadCodexLong(first[0]);
		long? stock = first.GetArrayLength() > 1 ? ReadCodexLong(first[1]) : null;
		if (price <= 0)
		{
			return ToCodexVendorSellPrice(itemId, html, captured);
		}

		string name = ParseCodexItemName(html);
		return new GrindMarketPrice(
			itemId,
			0,
			name,
			price,
			null,
			price,
			null,
			stock,
			null,
			"bdocodex-eu-market",
			captured);
	}

	private static GrindMarketPrice? ToCodexVendorSellPrice(long itemId, string html, DateTimeOffset captured)
	{
		long price = ParseCodexSellPrice(html);
		if (price <= 0)
		{
			return null;
		}

		return new GrindMarketPrice(
			itemId,
			0,
			ParseCodexItemName(html),
			price,
			null,
			null,
			null,
			null,
			null,
			"bdocodex-vendor-sell",
			captured);
	}

	private async Task<GrindMarketPrice> FetchPearlAbyssPriceAsync(long itemId, string region, DateTimeOffset captured, CancellationToken cancellationToken)
	{
		string host = "https://eu-trade.naeu.playblackdesert.com";
		using HttpRequestMessage request = new(HttpMethod.Post, $"{host}/Home/GetWorldMarketSubList");
		request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		request.Headers.Referrer = new Uri($"{host}/");
		request.Headers.TryAddWithoutValidation("Origin", host);
		request.Headers.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
		request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
		{
			["mainKey"] = itemId.ToString(CultureInfo.InvariantCulture)
		});
		using JsonDocument document = await SendJsonAsync(request, cancellationToken);
		JsonElement root = document.RootElement;
		if (IsUnauthorized(root))
		{
			throw new UnauthorizedAccessException(GetString(root, "resultMsg") ?? "Pearl Abyss market endpoint denied access.");
		}

		MarketSubListEntry entry = ParseSubList(root, itemId).FirstOrDefault();
		if (entry.ItemId <= 0)
		{
			throw new InvalidDataException("Pearl Abyss market endpoint returned no usable item data.");
		}

		long? lowestOrder = await TryFetchPearlAbyssLowestOrderAsync(itemId, entry.Enhancement, host, cancellationToken);
		return ToPrice(entry, lowestOrder, lowestOrder.HasValue ? "pa-lowest-listing" : "pa-sublist", captured);
	}

	private async Task<GrindMarketPrice> FetchArshaPriceAsync(long itemId, string region, DateTimeOffset captured, CancellationToken cancellationToken)
	{
		List<Exception> failures = new();
		foreach (Func<CancellationToken, Task<JsonDocument>> fetch in new Func<CancellationToken, Task<JsonDocument>>[]
		{
			token => FetchArshaItemV2GetAsync(itemId, region, token),
			token => FetchArshaItemV1GetAsync(itemId, region, token)
		})
		{
			try
			{
				using JsonDocument document = await fetch(cancellationToken);
				MarketSubListEntry entry = ParseSubList(document.RootElement, itemId).FirstOrDefault();
				if (entry.ItemId > 0)
				{
					return ToPrice(entry, null, "arsha-item", captured);
				}
				failures.Add(new InvalidDataException("Arsha returned no usable item data."));
			}
			catch (Exception ex) when (IsProviderFailure(ex))
			{
				failures.Add(ex);
			}
		}

		throw new InvalidDataException($"Arsha item lookup failed for {itemId}: {string.Join(" | ", failures.Select(error => error.Message).Take(3))}");
	}

	private async Task<JsonDocument> FetchArshaItemV2PostAsync(long itemId, string region, CancellationToken cancellationToken)
	{
		using HttpRequestMessage request = new(HttpMethod.Post, $"https://api.arsha.io/v2/{region}/item?lang=en");
		request.Content = new StringContent($"[{itemId.ToString(CultureInfo.InvariantCulture)}]", Encoding.UTF8, "application/json");
		return await SendJsonAsync(request, cancellationToken);
	}

	private async Task<JsonDocument> FetchArshaSubListV2PostAsync(long itemId, string region, CancellationToken cancellationToken)
	{
		using HttpRequestMessage request = new(HttpMethod.Post, $"https://api.arsha.io/v2/{region}/GetWorldMarketSubList?lang=en");
		request.Content = new StringContent($"[{itemId.ToString(CultureInfo.InvariantCulture)}]", Encoding.UTF8, "application/json");
		return await SendJsonAsync(request, cancellationToken);
	}

	private async Task<JsonDocument> FetchArshaSubListV2GetAsync(long itemId, string region, CancellationToken cancellationToken)
	{
		using HttpRequestMessage request = new(HttpMethod.Get, $"https://api.arsha.io/v2/{region}/GetWorldMarketSubList?id={itemId}&lang=en");
		return await SendJsonAsync(request, cancellationToken);
	}

	private async Task<JsonDocument> FetchArshaItemV2GetAsync(long itemId, string region, CancellationToken cancellationToken)
	{
		using HttpRequestMessage request = new(HttpMethod.Get, $"https://api.arsha.io/v2/{region}/item?id={itemId}&lang=en");
		return await SendJsonAsync(request, cancellationToken);
	}

	private async Task<JsonDocument> FetchArshaItemV1GetAsync(long itemId, string region, CancellationToken cancellationToken)
	{
		using HttpRequestMessage request = new(HttpMethod.Get, $"https://api.arsha.io/v1/{region}/item?id={itemId}&lang=en");
		return await SendJsonAsync(request, cancellationToken);
	}

	private async Task<JsonDocument> SendJsonAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		using HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
		string text = await response.Content.ReadAsStringAsync(cancellationToken);
		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException($"Market provider returned {(int)response.StatusCode} {response.ReasonPhrase}: {Truncate(text)}");
		}
		return JsonDocument.Parse(text);
	}

	private async Task<long?> TryFetchPearlAbyssLowestOrderAsync(long itemId, int enhancement, string host, CancellationToken cancellationToken)
	{
		try
		{
			using HttpRequestMessage request = new(HttpMethod.Post, $"{host}/Home/GetItemSellBuyInfo");
			request.Headers.Referrer = new Uri($"{host}/");
			request.Headers.TryAddWithoutValidation("Origin", host);
			request.Headers.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
			request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				["keyType"] = "0",
				["mainKey"] = itemId.ToString(CultureInfo.InvariantCulture),
				["subKey"] = enhancement.ToString(CultureInfo.InvariantCulture),
				["isUp"] = "false"
			});
			using JsonDocument document = await SendJsonAsync(request, cancellationToken);
			if (IsUnauthorized(document.RootElement))
			{
				return null;
			}
			return ParseLowestListedPrice(document.RootElement);
		}
		catch (Exception ex) when (IsProviderFailure(ex))
		{
			logger.Warn($"Pearl Abyss order lookup failed for {itemId}: {ex.Message}");
			return null;
		}
	}

	private async Task<long?> TryFetchArshaLowestOrderAsync(long itemId, int enhancement, string region, CancellationToken cancellationToken)
	{
		try
		{
			using HttpRequestMessage request = new(HttpMethod.Get, $"https://api.arsha.io/v2/{region}/GetItemSellBuyInfo?id={itemId}&sid={enhancement}&lang=en");
			using JsonDocument document = await SendJsonAsync(request, cancellationToken);
			return ParseLowestListedPrice(document.RootElement);
		}
		catch (Exception ex) when (IsProviderFailure(ex))
		{
			logger.Warn($"Arsha v2 order lookup failed for {itemId}: {ex.Message}");
			return null;
		}
	}

	private static GrindMarketPrice ToPrice(MarketSubListEntry entry, long? lowestOrder, string source, DateTimeOffset captured)
	{
		long? lowestListed = lowestOrder ?? entry.LowestListedPrice;
		long price = lowestListed
			?? entry.BasePrice
			?? entry.LastSoldPrice
			?? 0L;
		return new GrindMarketPrice(
			entry.ItemId,
			entry.Enhancement,
			entry.Name,
			Math.Max(0, price),
			lowestListed,
			entry.BasePrice,
			entry.LastSoldPrice,
			entry.Stock,
			entry.TradeCount,
			source,
			captured);
	}

	private static IReadOnlyList<MarketSubListEntry> ParseSubList(JsonElement root, long requestedItemId)
	{
		List<MarketSubListEntry> entries = new();
		foreach (JsonElement item in EnumerateSubListItems(root))
		{
			MarketSubListEntry entry = ParseSubListItem(item, requestedItemId);
			if (entry.ItemId > 0)
			{
				entries.Add(entry);
			}
		}

		if (entries.Count == 0 && root.ValueKind == JsonValueKind.Object && root.TryGetProperty("resultMsg", out JsonElement resultMsg))
		{
			string raw = resultMsg.GetString() ?? string.Empty;
			foreach (string row in raw.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
			{
				string[] parts = row.Split('-', StringSplitOptions.TrimEntries);
				if (parts.Length < 9 || !long.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out long itemId))
				{
					continue;
				}
				int enhancement = TryParseInt(parts.ElementAtOrDefault(1)) ?? 0;
				long? basePrice = TryParseLong(parts.ElementAtOrDefault(3));
				long? stock = TryParseLong(parts.ElementAtOrDefault(4));
				long? tradeCount = TryParseLong(parts.ElementAtOrDefault(5));
				long? lastSold = TryParseLong(parts.ElementAtOrDefault(8));
				entries.Add(new MarketSubListEntry(itemId, enhancement, "", null, basePrice, lastSold, stock, tradeCount));
			}
		}

		return entries
			.Where(entry => entry.ItemId == requestedItemId)
			.OrderBy(entry => entry.Enhancement == 0 ? 0 : 1)
			.ThenBy(entry => entry.Enhancement)
			.ToArray();
	}

	private static IEnumerable<JsonElement> EnumerateSubListItems(JsonElement root)
	{
		if (root.ValueKind == JsonValueKind.Array)
		{
			foreach (JsonElement item in root.EnumerateArray())
			{
				if (item.ValueKind == JsonValueKind.Array)
				{
					foreach (JsonElement nestedItem in item.EnumerateArray())
					{
						yield return nestedItem;
					}
				}
				else
				{
					yield return item;
				}
			}
			yield break;
		}

		if (root.ValueKind != JsonValueKind.Object)
		{
			yield break;
		}

		if (root.TryGetProperty("resultMsg", out JsonElement resultMsg) && resultMsg.ValueKind == JsonValueKind.String)
		{
			string raw = resultMsg.GetString() ?? string.Empty;
			string trimmed = raw.TrimStart();
			if (trimmed.StartsWith("[", StringComparison.Ordinal) || trimmed.StartsWith("{", StringComparison.Ordinal))
			{
				using JsonDocument nested = JsonDocument.Parse(raw);
				foreach (JsonElement item in EnumerateSubListItems(nested.RootElement))
				{
					yield return item;
				}
				yield break;
			}
		}

		string[] arrayNames = ["detailList", "data", "items", "list", "result", "results"];
		foreach (string name in arrayNames)
		{
			if (root.TryGetProperty(name, out JsonElement array) && array.ValueKind == JsonValueKind.Array)
			{
				foreach (JsonElement item in array.EnumerateArray())
				{
					yield return item;
				}
				yield break;
			}
		}

		if (GetLong(root, "id", "itemId", "mainKey", "main_key").HasValue)
		{
			yield return root;
		}
	}

	private static MarketSubListEntry ParseSubListItem(JsonElement item, long requestedItemId)
	{
		long itemId = GetLong(item, "id", "itemId", "mainKey", "main_key") ?? requestedItemId;
		int enhancement = (int)(GetLong(item, "sid", "subId", "subKey", "sub_key", "enhancement", "minEnhance") ?? 0);
		string name = GetString(item, "name", "itemName") ?? string.Empty;
		long? stock = GetLong(item, "amountListed", "currentStock", "stock", "count", "listedCount", "sellCount");
		long? lowest = GetLong(item, "lowestListedPrice", "lowestPrice", "minListedPrice", "pricePerOne");
		long? basePrice = GetLong(item, "basePrice", "currentPrice", "price", "minPrice");
		long? lastSoldPrice = GetLong(item, "lastSoldPrice", "lastPrice");
		long? tradeCount = GetLong(item, "totalTrades", "tradeCount");
		return new MarketSubListEntry(itemId, enhancement, name, lowest, basePrice, lastSoldPrice, stock, tradeCount);
	}

	private static bool IsOnlyRangePrice(JsonElement item, long? value)
	{
		if (!value.HasValue)
		{
			return false;
		}
		return item.TryGetProperty("priceMin", out _) || item.TryGetProperty("minPrice", out _);
	}

	private static long? ParseLowestListedPrice(JsonElement root)
	{
		List<(long Price, long SellCount)> orders = new();
		foreach (JsonElement item in EnumerateOrderItems(root))
		{
			long? price = GetLong(item, "pricePerOne", "price", "onePrice", "sellPrice");
			long sellCount = GetLong(item, "sellCount", "count", "sellOrderCount", "stock") ?? 0L;
			if (price.HasValue && price.Value > 0 && sellCount > 0)
			{
				orders.Add((price.Value, sellCount));
			}
		}

		if (orders.Count > 0)
		{
			return orders.Min(order => order.Price);
		}

		if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("resultMsg", out JsonElement resultMsg))
		{
			string raw = resultMsg.GetString() ?? string.Empty;
			foreach (string row in raw.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
			{
				string[] parts = row.Split('-', StringSplitOptions.TrimEntries);
				if (parts.Length < 2)
				{
					continue;
				}
				long? price = TryParseLong(parts[0]);
				long sellCount = TryParseLong(parts.ElementAtOrDefault(1)) ?? 0L;
				if (price.HasValue && price.Value > 0 && sellCount > 0)
				{
					orders.Add((price.Value, sellCount));
				}
			}
		}

		return orders.Count > 0 ? orders.Min(order => order.Price) : null;
	}

	private static IEnumerable<JsonElement> EnumerateOrderItems(JsonElement root)
	{
		if (root.ValueKind == JsonValueKind.Array)
		{
			foreach (JsonElement item in root.EnumerateArray())
			{
				yield return item;
			}
			yield break;
		}

		if (root.ValueKind != JsonValueKind.Object)
		{
			yield break;
		}

		string[] arrayNames = ["marketConditionList", "orders", "sellOrders", "data", "items", "priceList"];
		foreach (string name in arrayNames)
		{
			if (root.TryGetProperty(name, out JsonElement array) && array.ValueKind == JsonValueKind.Array)
			{
				foreach (JsonElement item in array.EnumerateArray())
				{
					yield return item;
				}
				yield break;
			}
		}
	}

	private static bool IsUnauthorized(JsonElement root)
	{
		string message = GetString(root, "resultMsg", "message") ?? string.Empty;
		long code = GetLong(root, "resultCode", "status", "code") ?? 0;
		return code == 2000
			|| message.Contains("UNAUTHORIZED", StringComparison.OrdinalIgnoreCase)
			|| message.Contains("denied", StringComparison.OrdinalIgnoreCase);
	}

	private static bool IsPearlAbyssAccessFailure(Exception ex)
	{
		return ex is UnauthorizedAccessException
			|| ex.Message.Contains("UNAUTHORIZED", StringComparison.OrdinalIgnoreCase)
			|| ex.Message.Contains("Pearlabyss/Index", StringComparison.OrdinalIgnoreCase);
	}

	private static bool IsEndpointUnavailable(Exception ex)
	{
		return ex is TaskCanceledException
			|| ex.Message.Contains("503", StringComparison.OrdinalIgnoreCase)
			|| ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase)
			|| ex.Message.Contains("could not be reached", StringComparison.OrdinalIgnoreCase)
			|| ex.Message.Contains("connect error", StringComparison.OrdinalIgnoreCase);
	}

	private static bool CanSplitArshaFailure(Exception ex, string region)
	{
		return false;
	}

	private static bool IsProviderFailure(Exception ex)
	{
		return ex is HttpRequestException or TaskCanceledException or JsonException or InvalidDataException or UnauthorizedAccessException;
	}

	private static long? GetLong(JsonElement element, params string[] propertyNames)
	{
		if (element.ValueKind != JsonValueKind.Object)
		{
			return null;
		}

		foreach (string propertyName in propertyNames)
		{
			if (!element.TryGetProperty(propertyName, out JsonElement value))
			{
				continue;
			}
			if (value.ValueKind == JsonValueKind.Number && value.TryGetInt64(out long number))
			{
				return number;
			}
			if (value.ValueKind == JsonValueKind.String && long.TryParse(value.GetString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out number))
			{
				return number;
			}
		}
		return null;
	}

	private static string? GetString(JsonElement element, params string[] propertyNames)
	{
		if (element.ValueKind != JsonValueKind.Object)
		{
			return null;
		}

		foreach (string propertyName in propertyNames)
		{
			if (element.TryGetProperty(propertyName, out JsonElement value))
			{
				return value.ValueKind == JsonValueKind.String ? value.GetString() : value.ToString();
			}
		}
		return null;
	}

	private static int? TryParseInt(string? value)
	{
		return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed) ? parsed : null;
	}

	private static long ReadCodexLong(JsonElement value)
	{
		if (value.ValueKind == JsonValueKind.Number && value.TryGetInt64(out long number))
		{
			return number;
		}
		if (value.ValueKind == JsonValueKind.String)
		{
			string text = (value.GetString() ?? string.Empty).Replace(",", "", StringComparison.Ordinal).Trim();
			return long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out number) ? number : 0L;
		}
		return 0L;
	}

	private static string ParseCodexItemName(string html)
	{
		foreach (string pattern in new[]
		{
			"<meta\\s+property=\"og:title\"\\s+content=\"([^\"]+)\"",
			"<title>(.*?)\\s+-\\s+BDO Codex</title>"
		})
		{
			Match match = Regex.Match(html, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			if (!match.Success)
			{
				continue;
			}

			string name = System.Net.WebUtility.HtmlDecode(match.Groups[1].Value).Trim();
			if (!string.IsNullOrWhiteSpace(name))
			{
				return name;
			}
		}
		return string.Empty;
	}

	private static long ParseCodexSellPrice(string html)
	{
		Match match = Regex.Match(html, "Sell price:\\s*([\\d,]+)", RegexOptions.IgnoreCase);
		if (!match.Success)
		{
			return 0L;
		}

		return long.TryParse(match.Groups[1].Value.Replace(",", "", StringComparison.Ordinal), NumberStyles.Integer, CultureInfo.InvariantCulture, out long price)
			? price
			: 0L;
	}

	private static long? TryParseLong(string? value)
	{
		return long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out long parsed) ? parsed : null;
	}

	private static string NormalizeRegion(string region)
	{
		return "eu";
	}

	private static string Truncate(string value)
	{
		return value.Length <= 180 ? value : value[..180] + "...";
	}

	public void Dispose()
	{
		client.Dispose();
	}

	private readonly record struct MarketSubListEntry(
		long ItemId,
		int Enhancement,
		string Name,
		long? LowestListedPrice,
		long? BasePrice,
		long? LastSoldPrice,
		long? Stock,
		long? TradeCount);
}
