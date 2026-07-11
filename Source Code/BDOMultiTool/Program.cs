using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace BDOMultiTool;

internal static class Program
{
	private const string ResourceName = "BDOMultiTool.Resources.BDO_Multi_Tool.html";

	private const string SingleInstanceMutexName = "Local\\BDOMultiTool.SingleInstance";

	private const string SingleInstancePipeName = "BDOMultiTool.SingleInstance.Restore";

	[STAThread]
	private static void Main(string[] args)
	{
		ApplicationConfiguration.Initialize();
		Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
		if (args.Any(a => string.Equals(a, "--coupon-smoke-test", StringComparison.OrdinalIgnoreCase)))
		{
			string root = Path.Combine(Path.GetTempPath(), $"bdo-coupon-smoke-{Guid.NewGuid():N}");
			AppPaths testPaths = AppPaths.CreateAt(root);
			testPaths.EnsureDirectories();
			using AppLogger testLogger = new AppLogger(testPaths.LogPath);
			using CouponService service = new CouponService(testPaths, testLogger);
			JsonElement dashboard = JsonSerializer.SerializeToElement(service.InitializeAsync(CancellationToken.None).GetAwaiter().GetResult());
			JsonElement refresh = JsonSerializer.SerializeToElement(service.RefreshAsync(CancellationToken.None).GetAwaiter().GetResult());
			int result = dashboard.GetProperty("coupons").GetArrayLength() >= 3
				&& dashboard.GetProperty("availableCount").GetInt32() >= 1
				&& File.Exists(testPaths.CouponsCachePath)
				&& File.Exists(testPaths.CouponSettingsPath)
				&& refresh.TryGetProperty("lastAttempt", out _)
				&& refresh.TryGetProperty("refreshDebug", out _)
				&& refresh.GetProperty("status").GetString() == "LIVE"
				&& refresh.GetProperty("coupons").GetArrayLength() >= 1 ? 0 : 41;
			try { Directory.Delete(root, true); } catch { }
			Environment.Exit(result);
			return;
		}
		if (args.Any((string a) => string.Equals(a, "--portrait-smoke-test", StringComparison.OrdinalIgnoreCase)))
		{
			string text = Path.Combine(Path.GetTempPath(), $"bdo-portrait-app-smoke-{Guid.NewGuid():N}");
			AppPaths appPaths = AppPaths.CreateAt(text);
			appPaths.EnsureDirectories();
			using AppLogger logger = new AppLogger(appPaths.LogPath);
			int result = RunPortraitSmokeTestAsync(appPaths, logger).GetAwaiter().GetResult();
			try
			{
				if (result == 0)
				{
					Directory.Delete(text, recursive: true);
				}
			}
			catch
			{
			}
			Environment.Exit(result);
			return;
		}
		if (args.Any((string a) => string.Equals(a, "--font-smoke-test", StringComparison.OrdinalIgnoreCase)))
		{
			string text2 = Path.Combine(Path.GetTempPath(), $"bdo-font-app-smoke-{Guid.NewGuid():N}");
			AppPaths appPaths2 = AppPaths.CreateAt(text2);
			appPaths2.EnsureDirectories();
			using AppLogger logger2 = new AppLogger(appPaths2.LogPath);
			int result2 = RunFontSmokeTestAsync(appPaths2, logger2).GetAwaiter().GetResult();
			try
			{
				if (result2 == 0)
				{
					Directory.Delete(text2, recursive: true);
				}
			}
			catch
			{
			}
			Environment.Exit(result2);
			return;
		}
		AppPaths appPaths3 = AppPaths.Create();
		appPaths3.EnsureDirectories();
		PrepareUiFiles(appPaths3);
		using AppLogger logger3 = new AppLogger(appPaths3.LogPath);
		Application.ThreadException += (_, e) => logger3.Error("Unhandled UI exception.", e.Exception);
		AppDomain.CurrentDomain.UnhandledException += (_, e) =>
		{
			if (e.ExceptionObject is Exception exception)
			{
				logger3.Error("Unhandled app exception.", exception);
			}
		};
		if (args.Any((string a) => string.Equals(a, "--smoke-test", StringComparison.OrdinalIgnoreCase)))
		{
			Environment.Exit(RunSmokeTestAsync(appPaths3, logger3).GetAwaiter().GetResult());
		}
		else if (args.Any((string a) => string.Equals(a, "--market-scheduled-update", StringComparison.OrdinalIgnoreCase)))
		{
			Environment.Exit(RunScheduledMarketUpdateAsync(appPaths3, logger3).GetAwaiter().GetResult());
		}
		else
		{
			using Mutex singleInstanceMutex = new Mutex(initiallyOwned: true, SingleInstanceMutexName, out bool ownsSingleInstance);
			if (!ownsSingleInstance)
			{
				SendRestoreRequestToExistingInstance();
				return;
			}

			using CancellationTokenSource singleInstanceServer = new CancellationTokenSource();
			using CalculatorForm form = new CalculatorForm(appPaths3, logger3);
			_ = RunSingleInstanceServerAsync(form, singleInstanceServer.Token);
			Application.Run(form);
			singleInstanceServer.Cancel();
		}
	}

	private static async Task<int> RunScheduledMarketUpdateAsync(AppPaths paths, AppLogger logger)
	{
		try
		{
			MarketDatabase database = new MarketDatabase(paths.DatabasePath);
			using BlackDesertMarketProvider provider = new BlackDesertMarketProvider(logger);
			using MarketAnalyticsService service = new MarketAnalyticsService(database, provider, logger);
			await service.InitializeAsync(CancellationToken.None, startForegroundUpdates: false);
			await service.RefreshDueMarketSamplesAsync(MarketAnalyticsService.DefaultCollectorInterval, "Windows scheduled task", CancellationToken.None);
			return 0;
		}
		catch (Exception exception)
		{
			logger.Error("Scheduled market collector failed.", exception);
			return 1;
		}
	}

	private static void SendRestoreRequestToExistingInstance()
	{
		try
		{
			using NamedPipeClientStream client = new NamedPipeClientStream(".", SingleInstancePipeName, PipeDirection.Out);
			client.Connect(750);
			using StreamWriter writer = new StreamWriter(client);
			writer.WriteLine("restore");
			writer.Flush();
		}
		catch
		{
		}
	}

	private static async Task RunSingleInstanceServerAsync(CalculatorForm form, CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			try
			{
				using NamedPipeServerStream server = new NamedPipeServerStream(
					SingleInstancePipeName,
					PipeDirection.In,
					1,
					PipeTransmissionMode.Byte,
					PipeOptions.Asynchronous);
				await server.WaitForConnectionAsync(cancellationToken);
				form.RestoreFromExternalLaunch();
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch
			{
				await Task.Delay(250, cancellationToken).ContinueWith(_ => { }, TaskScheduler.Default);
			}
		}
	}

	private static async Task<int> RunSmokeTestAsync(AppPaths paths, AppLogger logger)
	{
		if (!File.Exists(paths.HtmlPath))
		{
			return 2;
		}
		string testDatabasePath = Path.Combine(Path.GetTempPath(), $"bdo-market-smoke-{Guid.NewGuid():N}.db");
		try
		{
			MarketDatabase database = new MarketDatabase(testDatabasePath);
			await database.InitializeAsync(CancellationToken.None);
			await database.SaveSettingsAsync(MarketSettings.Default, CancellationToken.None);
			using BlackDesertMarketProvider provider = new BlackDesertMarketProvider(logger);
			IReadOnlyList<MarketItem> source = await provider.SearchAsync("Black Stone Powder", "na", CancellationToken.None);
			MarketItem marketItem = source.FirstOrDefault((MarketItem x) => x.ItemId == 4901) ?? source.FirstOrDefault();
			if ((object)marketItem == null)
			{
				return 3;
			}
			MarketItem variant = (await provider.GetVariantsAsync(marketItem.ItemId, "na", CancellationToken.None)).First();
			await database.AddTrackedItemAsync(variant, "na", CancellationToken.None);
			TrackedItem tracked = (await database.GetTrackedItemsAsync("na", CancellationToken.None)).Single();
			await database.SaveSnapshotAsync(tracked, variant, await provider.GetSnapshotAsync(variant.ItemId, variant.Enhancement, "na", CancellationToken.None), CancellationToken.None);
			if (!((await database.GetAnalyticsAsync(variant.ItemId, variant.Enhancement, "na", 30, CancellationToken.None))?.CurrentPrice).HasValue)
			{
				return 4;
			}
			MarketItem[] outfits = (from x in (await provider.GetCategoryAsync(55, 1, "eu", CancellationToken.None)).Concat(await provider.GetCategoryAsync(55, 2, "eu", CancellationToken.None))
				group x by x.ItemId into x
				select x.First()).ToArray();
			if (outfits.Length < 1000)
			{
				return 5;
			}
			await database.SyncOutfitCatalogAsync(outfits.Take(3).ToArray(), "eu", CancellationToken.None);
			MarketItem outfit = outfits[0];
			MarketItem outfitVariant = (await provider.GetVariantsAsync(outfit.ItemId, "eu", CancellationToken.None)).First();
			await database.SaveOutfitDetailAsync(outfit, outfitVariant, await provider.GetSnapshotAsync(outfitVariant.ItemId, outfitVariant.Enhancement, "eu", CancellationToken.None), "eu", CancellationToken.None);
			OutfitReport outfitReport = await database.GetOutfitReportAsync("eu", CancellationToken.None);
			return (outfitReport.CatalogCount != 3 || outfitReport.DetailedCount != 1) ? 6 : 0;
		}
		catch (Exception exception)
		{
			logger.Error("Smoke test failed.", exception);
			return 1;
		}
		finally
		{
			SqliteCleanup(testDatabasePath);
		}
	}

	private static void SqliteCleanup(string path)
	{
		SqliteConnection.ClearAllPools();
		string[] array = new string[3]
		{
			path,
			path + "-wal",
			path + "-shm"
		};
		foreach (string path2 in array)
		{
			if (File.Exists(path2))
			{
				try
				{
					File.Delete(path2);
				}
				catch (IOException)
				{
				}
			}
		}
	}

	private static async Task<int> RunPortraitSmokeTestAsync(AppPaths paths, AppLogger logger)
	{
		string root = Path.Combine(Path.GetTempPath(), $"bdo-portrait-smoke-{Guid.NewGuid():N}");
		string faceTexture = Path.Combine(root, "FaceTexture");
		Directory.CreateDirectory(faceTexture);
		string oldPath = Path.Combine(faceTexture, "portrait.bmp");
		string text = Path.Combine(root, "replacement.png");
		try
		{
			using (Bitmap bitmap = new Bitmap(624, 804))
			{
				using Graphics graphics = Graphics.FromImage(bitmap);
				graphics.Clear(Color.DarkRed);
				bitmap.Save(oldPath, ImageFormat.Bmp);
			}
			using (Bitmap bitmap2 = new Bitmap(1200, 700))
			{
				using Graphics graphics2 = Graphics.FromImage(bitmap2);
				graphics2.Clear(Color.CornflowerBlue);
				bitmap2.Save(text, ImageFormat.Png);
			}
			PortraitReplacerService service = new PortraitReplacerService(paths);
			await service.ReplaceAsync(faceTexture, oldPath, text, "crop", 50.0, 50.0, 1.0, CancellationToken.None);
			using (Image image = Image.FromFile(oldPath))
			{
				if (image.Width != 624 || image.Height != 804)
				{
					return 21;
				}
			}
			if (Directory.GetFiles(Path.Combine(faceTexture, "_BDOMultiToolBackups"), "portrait_*.bmp").Length != 1)
			{
				return 22;
			}
			await service.RestoreLastBackupAsync(faceTexture, oldPath, CancellationToken.None);
			return (!File.Exists(oldPath)) ? 23 : 0;
		}
		catch (Exception exception)
		{
			logger.Error("Portrait replacer smoke test failed.", exception);
			return 20;
		}
		finally
		{
			try
			{
				Directory.Delete(root, recursive: true);
			}
			catch
			{
			}
		}
	}

	private static async Task<int> RunFontSmokeTestAsync(AppPaths paths, AppLogger logger)
	{
		string root = Path.Combine(Path.GetTempPath(), $"bdo-font-smoke-{Guid.NewGuid():N}");
		Directory.CreateDirectory(Path.Combine(root, "bin64"));
		try
		{
			FontChangerService service = new FontChangerService(paths);
			await service.SaveBdoFolderAsync(root, CancellationToken.None);
			int arrayLength = JsonSerializer.SerializeToElement(service.GetPresetGallery()).GetProperty("presets").GetArrayLength();
			if (arrayLength == 0)
			{
				return 31;
			}
			string[] presetFiles = (from fileName in Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "Assets", "Fonts"), "*.ttf", SearchOption.TopDirectoryOnly).Select(Path.GetFileName)
				where !string.IsNullOrWhiteSpace(fileName)
				select fileName).Cast<string>().OrderBy<string, string>((string fileName) => fileName, StringComparer.OrdinalIgnoreCase).Take(2)
				.ToArray();
			if (presetFiles.Length < 2)
			{
				return 31;
			}
			if (arrayLength != Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "Assets", "Fonts"), "*.ttf", SearchOption.TopDirectoryOnly).Length)
			{
				return 31;
			}
			await service.ApplyPresetAsync(root, presetFiles[0], CancellationToken.None);
			string pearlPath = Path.Combine(root, "prestringtable", "font", "pearl.ttf");
			if (!File.Exists(pearlPath))
			{
				return 32;
			}
			await service.ApplyPresetAsync(root, presetFiles[1], CancellationToken.None);
			if (Directory.GetFiles(Path.Combine(root, "prestringtable", "font_BDOMultiToolBackups"), "pearl_*.ttf").Length != 1)
			{
				return 33;
			}
			await service.RestoreLastBackupAsync(root, CancellationToken.None);
			await service.RemoveCustomFontAsync(root, CancellationToken.None);
			return File.Exists(pearlPath) ? 34 : 0;
		}
		catch (Exception exception)
		{
			logger.Error("Font changer smoke test failed.", exception);
			return 30;
		}
		finally
		{
			try
			{
				Directory.Delete(root, recursive: true);
			}
			catch
			{
			}
		}
	}

	private static void ExtractResource(string resourceName, string targetPath)
	{
		using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException("The embedded resource is missing: " + resourceName);
		using MemoryStream memoryStream = new MemoryStream();
		stream.CopyTo(memoryStream);
		byte[] array = memoryStream.ToArray();
		string targetDirectory = Path.GetDirectoryName(targetPath);
		if (!string.IsNullOrWhiteSpace(targetDirectory))
		{
			Directory.CreateDirectory(targetDirectory);
		}
		if (!File.Exists(targetPath) || new FileInfo(targetPath).Length != array.Length)
		{
			File.WriteAllBytes(targetPath, array);
		}
	}

	private static void PrepareUiFiles(AppPaths paths)
	{
		string baseDirectory = AppContext.BaseDirectory;
		string htmlSource = Path.Combine(baseDirectory, "BDOMultiTool.Resources.BDO_Multi_Tool.html");
		if (File.Exists(htmlSource))
		{
			CopyFileIfChanged(htmlSource, paths.HtmlPath);
		}
		else
		{
			ExtractResource(ResourceName, paths.HtmlPath);
		}

		CopyFileIfChanged(Path.Combine(baseDirectory, "gold-coins.png"), Path.Combine(paths.Root, "gold-coins.png"));
		CopyDirectoryIfPresent(Path.Combine(baseDirectory, "Assets"), Path.Combine(paths.Root, "Assets"));
		CopyDirectoryIfPresent(Path.Combine(baseDirectory, "ThemeAssets"), paths.ThemeAssetsPath);
	}

	private static void CopyDirectoryIfPresent(string sourceDirectory, string targetDirectory)
	{
		if (!Directory.Exists(sourceDirectory))
		{
			return;
		}

		foreach (string sourcePath in Directory.EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories))
		{
			string relativePath = Path.GetRelativePath(sourceDirectory, sourcePath);
			CopyFileIfChanged(sourcePath, Path.Combine(targetDirectory, relativePath));
		}
	}

	private static void CopyFileIfChanged(string sourcePath, string targetPath)
	{
		if (!File.Exists(sourcePath))
		{
			return;
		}

		string targetDirectory = Path.GetDirectoryName(targetPath);
		if (!string.IsNullOrWhiteSpace(targetDirectory))
		{
			Directory.CreateDirectory(targetDirectory);
		}

		FileInfo sourceInfo = new FileInfo(sourcePath);
		FileInfo targetInfo = new FileInfo(targetPath);
		if (targetInfo.Exists
			&& targetInfo.Length == sourceInfo.Length
			&& targetInfo.LastWriteTimeUtc >= sourceInfo.LastWriteTimeUtc)
		{
			return;
		}

		File.Copy(sourcePath, targetPath, overwrite: true);
	}
}
