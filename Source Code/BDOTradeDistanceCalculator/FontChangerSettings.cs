namespace BDOTradeDistanceCalculator;

internal sealed record FontChangerSettings(string BdoFolder)
{
	public static FontChangerSettings Default => new FontChangerSettings(FontChangerService.DefaultBdoFolder);
}
