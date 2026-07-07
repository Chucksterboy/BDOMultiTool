namespace BDOTradeDistanceCalculator;

internal sealed record MarketItem(long ItemId, int Enhancement, string Name, int Grade, long CurrentPrice, long Stock, long TradeCount, int MainCategory, int SubCategory);
