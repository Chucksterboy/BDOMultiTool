using System.Runtime.CompilerServices;

namespace BDOMultiTool;

internal sealed record MarketSettings(string Region, int IntervalMinutes)
{
	public static MarketSettings Default { get; } = new MarketSettings("eu", 30);

	[CompilerGenerated]
	private MarketSettings(MarketSettings original)
	{
		Region = original.Region;
		IntervalMinutes = original.IntervalMinutes;
	}
}

