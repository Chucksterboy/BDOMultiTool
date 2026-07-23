using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
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
	private const long MaxResponseBytes = 8 * 1024 * 1024;

	private readonly HttpClient client;
	private readonly AppLogger logger;

	public GrindMarketPriceProvider(AppLogger logger)
	{
		this.logger = logger;
		client = new HttpClient
		{
			Timeout = RequestTimeout,
			MaxResponseContentBufferSize = MaxResponseBytes
		};
		client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("BDO-Multi-Tool", AppVersion.Current.TrimStart('v', 'V')));
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
		catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
		{
			throw;
		}
		catch (Exception ex) when (IsProviderFailure(ex))
		{
			logger.Warn($"Arsha GetWorldMarketSubList {normalizedRegion.ToUpperInvariant()} batch failed: {ex.Message}");
		}

		HashSet<long> resolved = prices.Select(price => price.ItemId).ToHashSet();
		IReadOnlyList<long> missing = ids.Except(resolved).ToArray();
		string message = prices.Count == 0
			? "EU market prices could not be refreshed from Arsha. Cached prices remain available."
			: $"EU market prices refreshed: {prices.Count.ToString(CultureInfo.InvariantCulture)}/{ids.Length.ToString(CultureInfo.InvariantCulture)} items.";

		return new GrindMarketPriceResponse(normalizedRegion, captured, prices, missing, provider, message);
	}

	private async Task<IReadOnlyList<GrindMarketPrice>> FetchArshaSubListBatchAsync(long[] itemIds, string region, DateTimeOffset captured, CancellationToken cancellationToken)
	{
		for (int attempt = 1; attempt <= 2; attempt++)
		{
			try
			{
				using HttpRequestMessage request = new(HttpMethod.Post, $"https://api.arsha.io/v2/{region}/GetWorldMarketSubList?lang=en");
				request.Content = new StringContent(JsonSerializer.Serialize(itemIds), Encoding.UTF8, "application/json");
				using JsonDocument document = await SendJsonAsync(request, cancellationToken);
				List<GrindMarketPrice> prices = new();
				Dictionary<long, MarketSubListEntry> entries = ParseSubList(document.RootElement, 0)
					.Where(entry => entry.ItemId > 0)
					.GroupBy(entry => entry.ItemId)
					.ToDictionary(group => group.Key, group => group.First());
				foreach (long itemId in itemIds)
				{
					if (entries.TryGetValue(itemId, out MarketSubListEntry entry))
					{
						prices.Add(ToPrice(entry, null, "arsha-sublist-cache", captured));
					}
				}
				return prices;
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				throw;
			}
			catch (Exception ex) when (attempt < 2 && IsEndpointUnavailable(ex))
			{
				logger.Warn($"Arsha GetWorldMarketSubList {region.ToUpperInvariant()} retrying after upstream failure: {ex.Message}");
				await Task.Delay(400, cancellationToken);
			}
		}

		throw new InvalidDataException("Arsha GetWorldMarketSubList did not return a usable response.");
	}

	private async Task<IReadOnlyList<GrindMarketPrice>> FetchArshaSubListResilientAsync(long[] itemIds, string region, DateTimeOffset captured, CancellationToken cancellationToken)
	{
		try
		{
			return await FetchArshaSubListBatchAsync(itemIds, region, captured, cancellationToken);
		}
		catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
		{
			throw;
		}
		catch (Exception ex) when (IsProviderFailure(ex))
		{
			if (!CanSplitArshaFailure(ex) || itemIds.Length <= 1)
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

	private async Task<JsonDocument> SendJsonAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		using HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
		string text = await response.Content.ReadAsStringAsync(cancellationToken);
		if (!response.IsSuccessStatusCode)
		{
			throw new HttpRequestException(
				$"Market provider returned {(int)response.StatusCode} {response.ReasonPhrase}: {Truncate(text)}",
				null,
				response.StatusCode);
		}
		return JsonDocument.Parse(text);
	}

	private static GrindMarketPrice ToPrice(MarketSubListEntry entry, long? lowestOrder, string source, DateTimeOffset captured)
	{
		long? lowestListed = lowestOrder
			?? (entry.Stock.GetValueOrDefault() > 0 ? entry.LowestListedPrice : null);
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
			.Where(entry => requestedItemId <= 0 || entry.ItemId == requestedItemId)
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

	private static bool IsEndpointUnavailable(Exception ex)
	{
		return ex is TaskCanceledException
			|| ex is HttpRequestException { StatusCode: System.Net.HttpStatusCode.TooManyRequests }
			|| ex is HttpRequestException { StatusCode: >= System.Net.HttpStatusCode.InternalServerError }
			|| ex.Message.Contains("503", StringComparison.OrdinalIgnoreCase)
			|| ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase)
			|| ex.Message.Contains("could not be reached", StringComparison.OrdinalIgnoreCase)
			|| ex.Message.Contains("connect error", StringComparison.OrdinalIgnoreCase);
	}

	private static bool CanSplitArshaFailure(Exception ex)
	{
		return ex is InvalidDataException
			|| ex is HttpRequestException
			{
				StatusCode: System.Net.HttpStatusCode.BadRequest
					or System.Net.HttpStatusCode.RequestEntityTooLarge
					or System.Net.HttpStatusCode.RequestUriTooLong
					or System.Net.HttpStatusCode.UnprocessableEntity
			};
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
