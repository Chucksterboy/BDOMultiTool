using System;

namespace BDOTradeDistanceCalculator;

internal sealed record ProviderHistoryPoint(DateTimeOffset Timestamp, long Price);
