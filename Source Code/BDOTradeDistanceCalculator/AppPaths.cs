using System;
using System.IO;

namespace BDOTradeDistanceCalculator;

internal sealed record AppPaths(string Root, string HtmlPath, string DatabasePath, string WebViewDataPath, string PortraitSettingsPath, string FontChangerSettingsPath, string LogPath)
{
	public string CouponsCachePath => Path.Combine(Root, "coupons_cache.json");

	public string CouponSettingsPath => Path.Combine(Root, "coupon_settings.json");

	public string CouponIconsPath => Path.Combine(Root, "data", "icons", "coupons");
	public string EventsCachePath => Path.Combine(Root, "events_cache.json");
	public string ThemeAssetsPath => Path.Combine(Root, "ThemeAssets");
	public string MasteryIconsPath => Path.Combine(Root, "Assets", "MasteryIcons");
	public string FontGuidePath => Path.Combine(Root, "Assets", "FontGuide");
	public string AppBehaviorSettingsPath => Path.Combine(Root, "app-behavior-settings.json");

	public static AppPaths Create()
	{
		return CreateAt(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BDO Trade Distance Calculator"));
	}

	public static AppPaths CreateAt(string root)
	{
		return new AppPaths(root, Path.Combine(root, "BDO_Trade_Distance_Calculator.html"), Path.Combine(root, "market-analytics.db"), Path.Combine(root, "webview-data"), Path.Combine(root, "portrait-replacer-settings.json"), Path.Combine(root, "font-changer-settings.json"), Path.Combine(root, "logs", "market-analytics.log"));
	}

	public void EnsureDirectories()
	{
		Directory.CreateDirectory(Root);
		Directory.CreateDirectory(WebViewDataPath);
		Directory.CreateDirectory(CouponIconsPath);
		Directory.CreateDirectory(ThemeAssetsPath);
		Directory.CreateDirectory(MasteryIconsPath);
		Directory.CreateDirectory(FontGuidePath);
		Directory.CreateDirectory(Path.GetDirectoryName(LogPath));
	}
}
