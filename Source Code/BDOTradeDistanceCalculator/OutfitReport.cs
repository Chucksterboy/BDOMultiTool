using System;
using System.Collections.Generic;

namespace BDOTradeDistanceCalculator;

internal sealed record OutfitReport(int CatalogCount, int DetailedCount, double CoveragePercent, DateTimeOffset? LastCatalogSyncUtc, IReadOnlyList<OutfitOpportunity> Opportunities);
