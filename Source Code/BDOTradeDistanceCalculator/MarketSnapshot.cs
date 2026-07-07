using System.Collections.Generic;

namespace BDOTradeDistanceCalculator;

internal sealed record MarketSnapshot(long Price, long Stock, long TradeCount, long PreorderCount, long OrderBookMin, long OrderBookMax, double OrderBookAverage, IReadOnlyList<ProviderHistoryPoint> History);
