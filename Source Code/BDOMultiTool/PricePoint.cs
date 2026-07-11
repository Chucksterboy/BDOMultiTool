using System;

namespace BDOMultiTool;

internal sealed record PricePoint(DateTimeOffset Timestamp, long Price, long? Stock, long? TradeCount);

