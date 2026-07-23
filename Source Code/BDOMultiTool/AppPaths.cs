using System;
using System.IO;

namespace BDOMultiTool;

internal sealed record AppPaths(string Root, string HtmlPath, string DatabasePath, string WebViewDataPath, string PortraitSettingsPath, string FontChangerSettingsPath, string LogPath)
{
	private const string CurrentAppDataFolderName = "BDO Multi-Tool";

	private static readonly string LegacyAppDataFolderName = string.Concat("BDO ", "Trade ", "Distance ", "Calculator");

	private const string HtmlFileName = "BDO_Multi_Tool.html";

	private static readonly string[] MigratedFiles =
	[
		"market-analytics.db",
		"coupons_cache.json",
		"coupon_settings.json",
		"events_cache.json",
		"events_cache.backup.json",
		"portrait-replacer-settings.json",
		"font-changer-settings.json",
		"app-behavior-settings.json",
		"grind-sessions.json",
		"grind-sessions.backup.json"
	];

	public string CouponsCachePath => Path.Combine(Root, "coupons_cache.json");

	public string CouponSettingsPath => Path.Combine(Root, "coupon_settings.json");

	public string CouponIconsPath => Path.Combine(Root, "data", "icons", "coupons");
	public string EventsCachePath => Path.Combine(Root, "events_cache.json");
	public string EventsBackupCachePath => Path.Combine(Root, "events_cache.backup.json");
	public string ThemeAssetsPath => Path.Combine(Root, "ThemeAssets");
	public string MasteryIconsPath => Path.Combine(Root, "Assets", "MasteryIcons");
	public string FontGuidePath => Path.Combine(Root, "Assets", "FontGuide");
	public string AppBehaviorSettingsPath => Path.Combine(Root, "app-behavior-settings.json");
	public string GrindSessionsPath => Path.Combine(Root, "grind-sessions.json");
	public string GrindSessionsBackupPath => Path.Combine(Root, "grind-sessions.backup.json");

	public static AppPaths Create()
	{
		string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
		string currentRoot = Path.Combine(localAppData, CurrentAppDataFolderName);
		MigrateLegacyData(Path.Combine(localAppData, LegacyAppDataFolderName), currentRoot);
		return CreateAt(currentRoot);
	}

	public static AppPaths CreateAt(string root)
	{
		return new AppPaths(root, Path.Combine(root, HtmlFileName), Path.Combine(root, "market-analytics.db"), Path.Combine(root, "webview-data"), Path.Combine(root, "portrait-replacer-settings.json"), Path.Combine(root, "font-changer-settings.json"), Path.Combine(root, "logs", "bdo-multi-tool.log"));
	}

	public void EnsureDirectories()
	{
		Directory.CreateDirectory(Root);
		Directory.CreateDirectory(WebViewDataPath);
		Directory.CreateDirectory(CouponIconsPath);
		Directory.CreateDirectory(ThemeAssetsPath);
		Directory.CreateDirectory(MasteryIconsPath);
		Directory.CreateDirectory(FontGuidePath);
		string? logDirectory = Path.GetDirectoryName(LogPath);
		if (!string.IsNullOrWhiteSpace(logDirectory))
		{
			Directory.CreateDirectory(logDirectory);
		}
	}

	private static void MigrateLegacyData(string legacyRoot, string currentRoot)
	{
		if (!Directory.Exists(legacyRoot) || string.Equals(Path.GetFullPath(legacyRoot), Path.GetFullPath(currentRoot), StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		Directory.CreateDirectory(currentRoot);
		foreach (string fileName in MigratedFiles)
		{
			CopyFileIfMissing(Path.Combine(legacyRoot, fileName), Path.Combine(currentRoot, fileName));
		}

		CopyFileIfMissing(Path.Combine(legacyRoot, "logs", "market-analytics.log"), Path.Combine(currentRoot, "logs", "bdo-multi-tool.log"));
		CopyDirectoryIfMissing(Path.Combine(legacyRoot, "data"), Path.Combine(currentRoot, "data"));
		CopyDirectoryIfMissing(Path.Combine(legacyRoot, "Assets"), Path.Combine(currentRoot, "Assets"));
		CopyDirectoryIfMissing(Path.Combine(legacyRoot, "ThemeAssets"), Path.Combine(currentRoot, "ThemeAssets"));
	}

	private static void CopyDirectoryIfMissing(string sourceDirectory, string targetDirectory)
	{
		if (!Directory.Exists(sourceDirectory))
		{
			return;
		}

		foreach (string sourcePath in Directory.EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories))
		{
			string relativePath = Path.GetRelativePath(sourceDirectory, sourcePath);
			CopyFileIfMissing(sourcePath, Path.Combine(targetDirectory, relativePath));
		}
	}

	private static void CopyFileIfMissing(string sourcePath, string targetPath)
	{
		if (!File.Exists(sourcePath) || File.Exists(targetPath))
		{
			return;
		}

		string? targetDirectory = Path.GetDirectoryName(targetPath);
		if (!string.IsNullOrWhiteSpace(targetDirectory))
		{
			Directory.CreateDirectory(targetDirectory);
		}

		try
		{
			File.Copy(sourcePath, targetPath);
		}
		catch (IOException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
	}
}
