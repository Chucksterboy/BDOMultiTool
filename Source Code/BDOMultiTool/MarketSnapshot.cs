using System.Collections.Generic;

namespace BDOMultiTool;

internal sealed record MarketSnapshot(long Price, long Stock, long TradeCount, long PreorderCount, long OrderBookMin, long OrderBookMax, double OrderBookAverage, IReadOnlyList<ProviderHistoryPoint> History);

