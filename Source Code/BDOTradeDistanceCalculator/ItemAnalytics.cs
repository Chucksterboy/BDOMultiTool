using System.Collections.Generic;

namespace BDOTradeDistanceCalculator;

internal sealed record ItemAnalytics(TrackedItem Item, long? CurrentPrice, long? MinimumPrice, long? MaximumPrice, double? AveragePrice, double? TrendPercent, IReadOnlyList<SalesWindow> Sales, IReadOnlyList<PricePoint> Points);
