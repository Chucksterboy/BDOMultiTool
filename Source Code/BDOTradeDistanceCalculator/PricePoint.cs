using System;

namespace BDOTradeDistanceCalculator;

internal sealed record PricePoint(DateTimeOffset Timestamp, long Price, long? Stock, long? TradeCount);
