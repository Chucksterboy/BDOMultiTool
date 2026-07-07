using System;

namespace BDOTradeDistanceCalculator;

internal sealed record TrackedItem(long ItemId, int Enhancement, string Region, string Name, int Grade, int MainCategory, int SubCategory, long? LastPrice, long? LastStock, long? LastTradeCount, DateTimeOffset? LastUpdatedUtc);
