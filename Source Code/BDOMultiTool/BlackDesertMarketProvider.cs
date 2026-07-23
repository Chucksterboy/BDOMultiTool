using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BDOMultiTool;

internal sealed class BlackDesertMarketProvider : IMarketDataProvider, IDisposable
{
	private const string BaseUrl = "https://api.blackdesertmarket.com";
	private const int MaxAttempts = 2;
	private const long MaxResponseBytes = 8 * 1024 * 1024;
	private const int CircuitFailureThreshold = 3;
	private static readonly TimeSpan CircuitBreakDuration = TimeSpan.FromMinutes(10);

	private readonly HttpClient client;

	private readonly AppLogger logger;
	private readonly object circuitSync = new();
	private int consecutiveFailures;
	private DateTimeOffset circuitOpenUntilUtc;

	public string Name => "Black Desert Market API";

	public BlackDesertMarketProvider(AppLogger logger)
	{
		this.logger = logger;
		client = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(25.0),
			MaxResponseContentBufferSize = MaxResponseBytes
		};
		client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("BDO-Multi-Tool", AppVersion.Current.TrimStart('v', 'V')));
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
		var sellOrders = (from x in source
			where GetInt64(x, "sellCount") > 0
			select new
			{
				Price = GetInt64(x, "onePrice"),
				Weight = Math.Max(1L, GetInt64(x, "sellCount"))
			}).ToArray();
		long preorderCount = source.Sum((JsonElement x) => GetInt64(x, "buyCount"));
		long @int = GetInt64(property, "basePrice");
		long orderBookMin = ((sellOrders.Length == 0) ? @int : sellOrders.Min(x => x.Price));
		long orderBookMax = ((sellOrders.Length == 0) ? @int : sellOrders.Max(x => x.Price));
		double num = sellOrders.Sum(x => (double)x.Weight);
		double orderBookAverage = ((num <= 0.0) ? ((double)@int) : (sellOrders.Sum(x => (double)x.Price * (double)x.Weight) / num));
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
		ThrowIfCircuitOpen();
		Exception? lastError = null;
		for (int attempt = 1; attempt <= MaxAttempts; attempt++)
		{
			try
			{
				using HttpResponseMessage response = await client.GetAsync(url, cancellationToken);
				string json = await response.Content.ReadAsStringAsync(cancellationToken);
				if (!response.IsSuccessStatusCode)
				{
					throw new HttpRequestException($"Market provider returned {response.StatusCode} {response.ReasonPhrase}.", null, response.StatusCode);
				}
				JsonDocument jsonDocument = JsonDocument.Parse(json);
				if (!jsonDocument.RootElement.TryGetProperty("code", out var value) || !string.Equals(value.GetString(), "SUCCESS", StringComparison.OrdinalIgnoreCase))
				{
					jsonDocument.Dispose();
					throw new InvalidDataException("Market provider returned an unsuccessful response.");
				}
				RegisterSuccess();
				return jsonDocument;
			}
			catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or InvalidDataException)
			{
				if (ex is OperationCanceledException && cancellationToken.IsCancellationRequested)
				{
					throw;
				}
				lastError = ex;
				bool shouldRetry = ex switch
				{
					HttpRequestException { StatusCode: HttpStatusCode.TooManyRequests } => true,
					HttpRequestException { StatusCode: >= HttpStatusCode.InternalServerError } => true,
					HttpRequestException { StatusCode: null } => true,
					TaskCanceledException => true,
					JsonException => true,
					InvalidDataException => true,
					_ => false
				};
				if (!shouldRetry || attempt >= MaxAttempts)
				{
					break;
				}
				if (attempt < MaxAttempts)
				{
					await Task.Delay(TimeSpan.FromSeconds(attempt), cancellationToken);
				}
			}
		}

		RegisterFailure(lastError);
		throw new HttpRequestException("The market provider is temporarily unavailable. Cached data will continue to be used.", lastError);
	}

	private void ThrowIfCircuitOpen()
	{
		lock (circuitSync)
		{
			if (circuitOpenUntilUtc > DateTimeOffset.UtcNow)
			{
				throw new HttpRequestException($"Market provider circuit is paused until {circuitOpenUntilUtc.LocalDateTime:t}.");
			}
			if (circuitOpenUntilUtc != default)
			{
				circuitOpenUntilUtc = default;
				consecutiveFailures = 0;
			}
		}
	}

	private void RegisterSuccess()
	{
		lock (circuitSync)
		{
			consecutiveFailures = 0;
			circuitOpenUntilUtc = default;
		}
	}

	private void RegisterFailure(Exception? error)
	{
		lock (circuitSync)
		{
			consecutiveFailures++;
			if (consecutiveFailures < CircuitFailureThreshold)
			{
				return;
			}

			circuitOpenUntilUtc = DateTimeOffset.UtcNow.Add(CircuitBreakDuration);
			logger.Warn($"Market provider circuit opened for {CircuitBreakDuration.TotalMinutes:N0} minutes after {consecutiveFailures} failed requests. Last error: {error?.Message}");
		}
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
		return "eu";
	}

	public void Dispose()
	{
		client.Dispose();
	}
}
