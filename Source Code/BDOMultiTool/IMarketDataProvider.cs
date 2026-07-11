using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BDOMultiTool;

internal interface IMarketDataProvider
{
	string Name { get; }

	Task<IReadOnlyList<MarketItem>> SearchAsync(string query, string region, CancellationToken cancellationToken);

	Task<IReadOnlyList<MarketItem>> GetVariantsAsync(long itemId, string region, CancellationToken cancellationToken);

	Task<IReadOnlyList<MarketItem>> GetCategoryAsync(int mainCategory, int subCategory, string region, CancellationToken cancellationToken);

	Task<MarketSnapshot> GetSnapshotAsync(long itemId, int enhancement, string region, CancellationToken cancellationToken);
}

