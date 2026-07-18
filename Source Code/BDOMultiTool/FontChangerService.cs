using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BDOMultiTool;

internal sealed class FontChangerService
{
	private sealed record FontLayout(string FontFolder, string PearlPath, string BackupFolder);

	public const string InstalledFontName = "pearl.ttf";

	public const string BackupFolderName = "font_BDOMultiToolBackups";

	private const string PreviewText = "Black Desert Online 1234567890";

	private readonly AppPaths paths;

	private readonly string presetFolder;

	private readonly object galleryCacheSync = new();

	private string galleryFingerprint = string.Empty;

	private object? cachedGallery;

	private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		PropertyNameCaseInsensitive = true,
		WriteIndented = true
	};

	public static string DefaultBdoFolder => Path.Combine(Path.GetPathRoot(Environment.SystemDirectory) ?? "C:\\", "Pearlabyss", "BlackDesert");

	public FontChangerService(AppPaths paths)
	{
		this.paths = paths;
		presetFolder = Path.Combine(AppContext.BaseDirectory, "Assets", "Fonts");
		if (Directory.Exists(DefaultBdoFolder))
		{
			GetLayout(DefaultBdoFolder, createFontFolder: true);
		}
	}

	public async Task<FontChangerSettings> GetSettingsAsync(CancellationToken cancellationToken)
	{
		if (!File.Exists(paths.FontChangerSettingsPath))
		{
			return EnsureFontFolders(FontChangerSettings.Default);
		}
		try
		{
			return EnsureFontFolders(JsonSerializer.Deserialize<FontChangerSettings>(await File.ReadAllTextAsync(paths.FontChangerSettingsPath, cancellationToken), JsonOptions) ?? FontChangerSettings.Default);
		}
		catch (JsonException)
		{
			return EnsureFontFolders(FontChangerSettings.Default);
		}
	}

	public async Task<FontChangerSettings> SaveBdoFolderAsync(string folderPath, CancellationToken cancellationToken)
	{
		string bdoFolder = ValidateBdoFolder(folderPath);
		GetLayout(bdoFolder, createFontFolder: true);
		FontChangerSettings settings = new FontChangerSettings(bdoFolder);
		await File.WriteAllTextAsync(paths.FontChangerSettingsPath, JsonSerializer.Serialize(settings, JsonOptions), cancellationToken);
		return settings;
	}

	private static FontChangerSettings EnsureFontFolders(FontChangerSettings settings)
	{
		if (Directory.Exists(settings.BdoFolder))
		{
			GetLayout(settings.BdoFolder, createFontFolder: true);
		}
		return settings;
	}

	public object GetPresetGallery()
	{
		if (!Directory.Exists(presetFolder))
		{
			return new
			{
				presets = Array.Empty<object>()
			};
		}
		string[] files = Directory.EnumerateFiles(presetFolder, "*.ttf", SearchOption.TopDirectoryOnly).OrderBy<string, string>(Path.GetFileName, StringComparer.OrdinalIgnoreCase).ToArray();
		string fingerprint = string.Join("|", files.Select(path => $"{Path.GetFileName(path)}:{new FileInfo(path).Length}:{File.GetLastWriteTimeUtc(path).Ticks}"));
		lock (galleryCacheSync)
		{
			if (cachedGallery is not null && string.Equals(galleryFingerprint, fingerprint, StringComparison.Ordinal))
			{
				return cachedGallery;
			}
		}

		List<object> list = new List<object>();
		foreach (string item in files)
		{
			try
			{
				string fileName = Path.GetFileName(item);
				list.Add(new
				{
					id = fileName,
					name = ReadFamilyName(item),
					description = Path.GetFileNameWithoutExtension(item),
					previewText = "Black Desert Online 1234567890",
					previewDataUrl = RenderPreviewDataUrl(item)
				});
			}
			catch (InvalidDataException)
			{
			}
		}
		object gallery = new
		{
			presets = list
		};
		lock (galleryCacheSync)
		{
			galleryFingerprint = fingerprint;
			cachedGallery = gallery;
		}
		return gallery;
	}

	public object DescribeCustomFont(string filePath)
	{
		string text = ValidateTrueTypeFont(filePath);
		return new
		{
			path = text,
			fileName = Path.GetFileName(text),
			familyName = ReadFamilyName(text),
			previewText = "Black Desert Online 1234567890",
			previewDataUrl = RenderPreviewDataUrl(text)
		};
	}

	public async Task<object> ApplyPresetAsync(string bdoFolder, string presetId, CancellationToken cancellationToken)
	{
		string presetPath = GetPresetPath(presetId);
		return await ApplyFontAsync(bdoFolder, presetPath, ReadFamilyName(presetPath), cancellationToken);
	}

	public Task<object> ApplyCustomAsync(string bdoFolder, string customFontPath, CancellationToken cancellationToken)
	{
		return ApplyFontAsync(bdoFolder, ValidateTrueTypeFont(customFontPath), ReadFamilyName(customFontPath), cancellationToken);
	}

	public async Task<object> RestoreLastBackupAsync(string bdoFolder, CancellationToken cancellationToken)
	{
		FontLayout layout = GetLayout(ValidateBdoFolder(bdoFolder), createFontFolder: true);
		if (!Directory.Exists(layout.BackupFolder))
		{
			throw new InvalidOperationException("No font backup folder exists yet.");
		}
		string latest = Directory.EnumerateFiles(layout.BackupFolder, "pearl_*.ttf", SearchOption.TopDirectoryOnly).OrderByDescending(File.GetLastWriteTimeUtc).FirstOrDefault();
		if (latest == null)
		{
			throw new InvalidOperationException("No pearl.ttf backup is available to restore.");
		}
		string temporaryPath = CreateTemporaryPath(layout.FontFolder);
		try
		{
			ValidateTrueTypeFont(latest);
			await CopyFileAsync(latest, temporaryPath, cancellationToken);
			ValidateTrueTypeFont(temporaryPath);
			if (File.Exists(layout.PearlPath))
			{
				await BackupCurrentAsync(layout, "before-restore", cancellationToken);
				File.Replace(temporaryPath, layout.PearlPath, null, ignoreMetadataErrors: true);
			}
			else
			{
				File.Move(temporaryPath, layout.PearlPath);
			}
			return new
			{
				restored = true,
				restoredFrom = latest,
				path = layout.PearlPath,
				message = "Font restored. Restart BDO for the change to appear."
			};
		}
		catch (UnauthorizedAccessException ex)
		{
			throw CreateAccessException(ex);
		}
		catch (IOException ex2) when (IsLikelyLocked(ex2))
		{
			throw CreateLockedException(ex2);
		}
		finally
		{
			TryDelete(temporaryPath);
		}
	}

	public async Task<object> RemoveCustomFontAsync(string bdoFolder, CancellationToken cancellationToken)
	{
		FontLayout layout = GetLayout(ValidateBdoFolder(bdoFolder), createFontFolder: false);
		if (!File.Exists(layout.PearlPath))
		{
			return new
			{
				removed = false,
				message = "No custom pearl.ttf is installed. BDO is already using its default font."
			};
		}
		try
		{
			string backupPath = await BackupCurrentAsync(layout, "before-remove", cancellationToken);
			File.Delete(layout.PearlPath);
			return new
			{
				removed = true,
				backupPath = backupPath,
				message = "Custom font removed. Restart BDO to use the default font again."
			};
		}
		catch (UnauthorizedAccessException ex)
		{
			throw CreateAccessException(ex);
		}
		catch (IOException ex2) when (IsLikelyLocked(ex2))
		{
			throw CreateLockedException(ex2);
		}
	}

	public object OpenFontFolder(string bdoFolder)
	{
		FontLayout layout = GetLayout(ValidateBdoFolder(bdoFolder), createFontFolder: true);
		Process.Start(new ProcessStartInfo
		{
			FileName = layout.FontFolder,
			UseShellExecute = true
		});
		return new
		{
			opened = true,
			path = layout.FontFolder
		};
	}

	private async Task<object> ApplyFontAsync(string bdoFolder, string sourceFontPath, string displayName, CancellationToken cancellationToken)
	{
		FontLayout layout = GetLayout(ValidateBdoFolder(bdoFolder), createFontFolder: true);
		string source = ValidateTrueTypeFont(sourceFontPath);
		string temporaryPath = CreateTemporaryPath(layout.FontFolder);
		string backupPath = null;
		try
		{
			await CopyFileAsync(source, temporaryPath, cancellationToken);
			ValidateTrueTypeFont(temporaryPath);
			if (File.Exists(layout.PearlPath))
			{
				backupPath = await BackupCurrentAsync(layout, null, cancellationToken);
				File.Replace(temporaryPath, layout.PearlPath, null, ignoreMetadataErrors: true);
			}
			else
			{
				File.Move(temporaryPath, layout.PearlPath);
			}
			return new
			{
				installed = true,
				fontName = displayName,
				path = layout.PearlPath,
				backupPath = backupPath,
				message = "Font installed. Restart BDO for the change to appear."
			};
		}
		catch (UnauthorizedAccessException ex)
		{
			throw CreateAccessException(ex);
		}
		catch (IOException ex2) when (IsLikelyLocked(ex2))
		{
			throw CreateLockedException(ex2);
		}
		finally
		{
			TryDelete(temporaryPath);
		}
	}

	private async Task<string> BackupCurrentAsync(FontLayout layout, string? suffix, CancellationToken cancellationToken)
	{
		Directory.CreateDirectory(layout.BackupFolder);
		string value = (string.IsNullOrWhiteSpace(suffix) ? "" : ("_" + suffix));
		string backupPath = Path.Combine(layout.BackupFolder, $"pearl_{DateTime.Now:yyyy-MM-dd_HHmmssfff}{value}.ttf");
		await CopyFileAsync(layout.PearlPath, backupPath, cancellationToken);
		return backupPath;
	}

	private string GetPresetPath(string presetId)
	{
		string fileName = Path.GetFileName(presetId);
		if (!string.Equals(fileName, presetId, StringComparison.Ordinal) || !string.Equals(Path.GetExtension(fileName), ".ttf", StringComparison.OrdinalIgnoreCase))
		{
			throw new InvalidDataException("Select a valid preset font.");
		}
		return ValidateTrueTypeFont(Path.Combine(presetFolder, fileName));
	}

	private static string ValidateBdoFolder(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new DirectoryNotFoundException("Select the main Black Desert Online folder.");
		}
		string fullPath = Path.GetFullPath(value.Trim());
		if (!Directory.Exists(fullPath))
		{
			throw new DirectoryNotFoundException("The selected BDO folder does not exist.");
		}
		if (!new string[5]
		{
			Path.Combine(fullPath, "bin64"),
			Path.Combine(fullPath, "Paz"),
			Path.Combine(fullPath, "prestringtable"),
			Path.Combine(fullPath, "BlackDesertLauncher.exe"),
			Path.Combine(fullPath, "BlackDesert64.exe")
		}.Any((string path) => Directory.Exists(path) || File.Exists(path)))
		{
			throw new InvalidDataException("This folder does not look like a Black Desert Online installation. Select the main BDO folder.");
		}
		return fullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
	}

	private static FontLayout GetLayout(string bdoFolder, bool createFontFolder)
	{
		string path = Path.Combine(bdoFolder, "prestringtable");
		string text = Path.Combine(path, "font");
		if (createFontFolder)
		{
			Directory.CreateDirectory(text);
		}
		string backupFolder = Path.Combine(path, "font_BDOMultiToolBackups");
		return new FontLayout(text, Path.Combine(text, "pearl.ttf"), backupFolder);
	}

	private static string ValidateTrueTypeFont(string value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new InvalidDataException("Select a TrueType .ttf font.");
		}
		string fullPath = Path.GetFullPath(value.Trim());
		if (!File.Exists(fullPath) || !string.Equals(Path.GetExtension(fullPath), ".ttf", StringComparison.OrdinalIgnoreCase))
		{
			throw new InvalidDataException("Select an existing TrueType .ttf font file.");
		}
		try
		{
			UseFontCollection(fullPath, delegate(PrivateFontCollection collection)
			{
				if (collection.Families.Length == 0)
				{
					throw new InvalidDataException("The selected file does not contain a readable TrueType font.");
				}
				return true;
			});
			return fullPath;
		}
		catch (ArgumentException innerException)
		{
			throw new InvalidDataException("The selected file is not a readable TrueType font.", innerException);
		}
	}

	private static string ReadFamilyName(string path)
	{
		return UseFontCollection(ValidateTrueTypeFont(path), (PrivateFontCollection collection) => collection.Families[0].Name);
	}

	private static string RenderPreviewDataUrl(string fontPath)
	{
		return UseFontCollection(ValidateTrueTypeFont(fontPath), delegate(PrivateFontCollection collection)
		{
			using Bitmap bitmap = new Bitmap(920, 150, PixelFormat.Format32bppArgb);
			using Graphics graphics = Graphics.FromImage(bitmap);
			graphics.Clear(Color.Transparent);
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			FontFamily family = collection.Families[0];
			FontStyle style = ResolveFontStyle(family);
			using Font font = new Font(family, 31f, style, GraphicsUnit.Pixel);
			using SolidBrush brush = new SolidBrush(Color.White);
			graphics.DrawString("Black Desert Online 1234567890", font, brush, new RectangleF(12f, 30f, 896f, 100f));
			using MemoryStream memoryStream = new MemoryStream();
			bitmap.Save(memoryStream, ImageFormat.Png);
			return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
		});
	}

	private static T UseFontCollection<T>(string path, Func<PrivateFontCollection, T> action)
	{
		byte[] array = File.ReadAllBytes(path);
		nint num = Marshal.AllocCoTaskMem(array.Length);
		try
		{
			Marshal.Copy(array, 0, num, array.Length);
			using PrivateFontCollection privateFontCollection = new PrivateFontCollection();
			privateFontCollection.AddMemoryFont(num, array.Length);
			return action(privateFontCollection);
		}
		finally
		{
			Marshal.FreeCoTaskMem(num);
		}
	}

	private static FontStyle ResolveFontStyle(FontFamily family)
	{
		FontStyle[] array = new FontStyle[4]
		{
			FontStyle.Regular,
			FontStyle.Bold,
			FontStyle.Italic,
			FontStyle.Bold | FontStyle.Italic
		};
		foreach (FontStyle fontStyle in array)
		{
			if (family.IsStyleAvailable(fontStyle))
			{
				return fontStyle;
			}
		}
		return FontStyle.Regular;
	}

	private static async Task CopyFileAsync(string source, string destination, CancellationToken cancellationToken)
	{
		await using FileStream input = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read);
		await using FileStream output = new FileStream(destination, FileMode.CreateNew, FileAccess.Write, FileShare.None);
		await input.CopyToAsync(output, cancellationToken);
		await output.FlushAsync(cancellationToken);
	}

	private static string CreateTemporaryPath(string fontFolder)
	{
		return Path.Combine(fontFolder, $".pearl-{Guid.NewGuid():N}.ttf");
	}

	private static IOException CreateLockedException(IOException ex)
	{
		return new IOException("The BDO font file is in use. Close Black Desert Online completely, then try again.", ex);
	}

	private static IOException CreateAccessException(Exception ex)
	{
		return new IOException("The font could not be changed. Close Black Desert Online and make sure the BDO folder is writable.", ex);
	}

	private static bool IsLikelyLocked(IOException ex)
	{
		int num = ex.HResult & 0xFFFF;
		if ((uint)(num - 32) <= 1u)
		{
			return true;
		}
		return false;
	}

	private static void TryDelete(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
		catch
		{
		}
	}
}

