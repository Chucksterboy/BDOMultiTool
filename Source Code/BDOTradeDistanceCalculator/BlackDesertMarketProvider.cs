using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BDOTradeDistanceCalculator;

internal sealed class BlackDesertMarketProvider : IMarketDataProvider, IDisposable
{
	private const string BaseUrl = "https://api.blackdesertmarket.com";

	private readonly HttpClient client;

	private readonly AppLogger logger;

	public string Name => "Black Desert Market API";

	public BlackDesertMarketProvider(AppLogger logger)
	{
		this.logger = logger;
		client = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(25.0)
		};
		client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("BDO-Trade-Distance-Calculator", "1.0"));
		client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	}

	public async Task<IReadOnlyList<MarketItem>> SearchAsync(string query, string region, CancellationToken cancellationToken)
	{
		if (string.IsNullOrWhiteSpace(query))
		{
			return Array.Empty<MarketItem>();
		}
		string url = $"{"https://api.blackdesertmarket.com"}/search/{Uri.EscapeDataString(query.Trim())}?region={ValidateRegion(region)}&language=en-US";
		using JsonDocument jsonDocument = await GetJsonWithRetryAsync(url, cancellationToken);
		return ParseItems(jsonDocument.RootElement.GetProperty("data"), includeEnhancement: false);
	}

	public async Task<IReadOnlyList<MarketItem>> GetVariantsAsync(long itemId, string region, CancellationToken cancellationToken)
	{
		string url = $"{"https://api.blackdesertmarket.com"}/item/{itemId}?region={ValidateRegion(region)}&language=en-US";
		using JsonDocument jsonDocument = await GetJsonWithRetryAsync(url, cancellationToken);
		return ParseItems(jsonDocument.RootElement.GetProperty("data"), includeEnhancement: true);
	}

	public async Task<IReadOnlyList<MarketItem>> GetCategoryAsync(int mainCategory, int subCategory, string region, CancellationToken cancellationToken)
	{
		string url = $"{"https://api.blackdesertmarket.com"}/list/{mainCategory}/{subCategory}?region={ValidateRegion(region)}&language=en-US";
		using JsonDocument jsonDocument = await GetJsonWithRetryAsync(url, cancellationToken);
		return (from item in ParseItems(jsonDocument.RootElement.GetProperty("data"), includeEnhancement: false)
			select item with
			{
				MainCategory = mainCategory,
				SubCategory = subCategory
			}).ToArray();
	}

	public async Task<MarketSnapshot> GetSnapshotAsync(long itemId, int enhancement, string region, CancellationToken cancellationToken)
	{
		string url = $"{"https://api.blackdesertmarket.com"}/item/{itemId}/{enhancement}?region={ValidateRegion(region)}&language=en-US";
		using JsonDocument jsonDocument = await GetJsonWithRetryAsync(url, cancellationToken);
		JsonElement property = jsonDocument.RootElement.GetProperty("data");
		JsonElement[] source = property.GetProperty("availability").EnumerateArray().ToArray();
		var array = (from x in source
			where GetInt64(x, "sellCount") > 0 || GetInt64(x, "buyCount") > 0
			select new
			{
				Price = GetInt64(x, "onePrice"),
				Weight = Math.Max(1L, GetInt64(x, "sellCount") + GetInt64(x, "buyCount"))
			}).ToArray();
		long preorderCount = source.Sum((JsonElement x) => GetInt64(x, "buyCount"));
		long @int = GetInt64(property, "basePrice");
		long orderBookMin = ((array.Length == 0) ? @int : array.Min(x => x.Price));
		long orderBookMax = ((array.Length == 0) ? @int : array.Max(x => x.Price));
		double num = array.Sum(x => (double)x.Weight);
		double orderBookAverage = ((num <= 0.0) ? ((double)@int) : (array.Sum(x => (double)x.Price * (double)x.Weight) / num));
		List<ProviderHistoryPoint> list = new List<ProviderHistoryPoint>();
		if (property.TryGetProperty("history", out var value))
		{
			foreach (JsonElement item in value.EnumerateArray())
			{
				if (DateTimeOffset.TryParseExact(item.GetProperty("date").GetString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var result))
				{
					list.Add(new ProviderHistoryPoint(result.AddHours(12.0), GetInt64(item, "onePrice")));
				}
			}
		}
		return new MarketSnapshot(@int, GetInt64(property, "sellCount"), 0L, preorderCount, orderBookMin, orderBookMax, orderBookAverage, list);
	}

	private async Task<JsonDocument> GetJsonWithRetryAsync(string url, CancellationToken cancellationToken)
	{
		Exception lastError = null;
		for (int attempt = 1; attempt <= 3; attempt++)
		{
			try
			{
				using HttpResponseMessage response = await client.GetAsync(url, cancellationToken);
				string json = await response.Content.ReadAsStringAsync(cancellationToken);
				if (!response.IsSuccessStatusCode)
				{
					throw new HttpRequestException($"Market provider returned {response.StatusCode} {response.ReasonPhrase}.");
				}
				JsonDocument jsonDocument = JsonDocument.Parse(json);
				if (!jsonDocument.RootElement.TryGetProperty("code", out var value) || !string.Equals(value.GetString(), "SUCCESS", StringComparison.OrdinalIgnoreCase))
				{
					jsonDocument.Dispose();
					throw new InvalidDataException("Market provider returned an unsuccessful response.");
				}
				return jsonDocument;
			}
			catch (Exception ex) when (((ex is HttpRequestException || ex is TaskCanceledException || ex is JsonException || ex is InvalidDataException) ? 1 : 0) != 0)
			{
				lastError = ex;
				logger.Warn($"Provider request attempt {attempt} failed: {ex.Message}");
				if (attempt < 3)
				{
					await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2.0, attempt)), cancellationToken);
				}
			}
		}
		throw new HttpRequestException("The market provider could not be reached after three attempts.", lastError);
	}

	private static IReadOnlyList<MarketItem> ParseItems(JsonElement data, bool includeEnhancement)
	{
		List<MarketItem> list = new List<MarketItem>();
		foreach (JsonElement item in data.EnumerateArray())
		{
			list.Add(new MarketItem(GetInt64(item, "id"), (int)(includeEnhancement ? GetInt64(item, "enhancement") : 0), item.GetProperty("name").GetString() ?? "Unknown item", (int)GetInt64(item, "grade"), GetInt64(item, "basePrice"), GetInt64(item, "count"), GetInt64(item, "tradeCount"), (int)GetInt64(item, "mainCategory"), (int)GetInt64(item, "subCategory")));
		}
		return list;
	}

	private static long GetInt64(JsonElement element, string propertyName)
	{
		if (!element.TryGetProperty(propertyName, out var value) || !value.TryGetInt64(out var value2))
		{
			return 0L;
		}
		return value2;
	}

	private static string ValidateRegion(string region)
	{
		if (!string.Equals(region, "eu", StringComparison.OrdinalIgnoreCase))
		{
			return "na";
		}
		return "eu";
	}

	public void Dispose()
	{
		client.Dispose();
	}
}
