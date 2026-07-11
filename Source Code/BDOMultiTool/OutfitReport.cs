using System;
using System.Collections.Generic;

namespace BDOMultiTool;

internal sealed record OutfitReport(int CatalogCount, int DetailedCount, double CoveragePercent, DateTimeOffset? LastCatalogSyncUtc, IReadOnlyList<OutfitOpportunity> Opportunities);

