using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BDOMultiTool;

internal sealed class PortraitReplacerService
{
	public const int OutputWidth = 624;

	public const int OutputHeight = 804;

	public const string BackupFolderName = "_BDOMultiToolBackups";

	private readonly AppPaths paths;

	private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		PropertyNameCaseInsensitive = true,
		WriteIndented = true
	};

	public static string DefaultFaceTextureFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Black Desert", "FaceTexture");

	public PortraitReplacerService(AppPaths paths)
	{
		this.paths = paths;
	}

	public async Task<PortraitSettings> GetSettingsAsync(CancellationToken cancellationToken)
	{
		if (!File.Exists(paths.PortraitSettingsPath))
		{
			return PortraitSettings.Default;
		}
		try
		{
			return JsonSerializer.Deserialize<PortraitSettings>(await File.ReadAllTextAsync(paths.PortraitSettingsPath, cancellationToken), JsonOptions) ?? PortraitSettings.Default;
		}
		catch (JsonException)
		{
			return PortraitSettings.Default;
		}
	}

	public async Task<PortraitSettings> SaveFaceTextureFolderAsync(string folderPath, CancellationToken cancellationToken)
	{
		string faceTextureFolder = ValidateFolder(folderPath);
		PortraitSettings settings = new PortraitSettings(faceTextureFolder);
		await File.WriteAllTextAsync(paths.PortraitSettingsPath, JsonSerializer.Serialize(settings, JsonOptions), cancellationToken);
		return settings;
	}

	public object DescribeImage(string filePath, bool renderFinal, string cropMode = "crop", double cropX = 50.0, double cropY = 50.0, double zoom = 1.0)
	{
		string path = ValidateImageFile(filePath, requireBitmap: false);
		using Image image = LoadUnlockedImage(path);
		using Bitmap image2 = (renderFinal ? CreateOutputBitmap(image, ParseCropMode(cropMode), cropX, cropY, zoom) : CreatePreviewBitmap(image, 520, 520));
		return new
		{
			path = path,
			fileName = Path.GetFileName(path),
			width = image.Width,
			height = image.Height,
			previewDataUrl = ToPngDataUrl(image2)
		};
	}

	public async Task<object> ReplaceAsync(string faceTextureFolder, string oldImagePath, string newImagePath, string cropMode, double cropX, double cropY, double zoom, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		string text = ValidateFolder(faceTextureFolder);
		string oldPath = ValidateOldImage(text, oldImagePath);
		string path = ValidateImageFile(newImagePath, requireBitmap: false);
		PortraitCropMode mode = ParseCropMode(cropMode);
		string backupFolder = Path.Combine(text, "_BDOMultiToolBackups");
		Directory.CreateDirectory(backupFolder);
		string temporaryPath = Path.Combine(text, $".bdo-portrait-{Guid.NewGuid():N}.bmp");
		string backupPath = CreateBackupPath(backupFolder, oldPath, DateTime.Now);
		try
		{
			using (Image source = LoadUnlockedImage(path))
			{
				using Bitmap bitmap = CreateOutputBitmap(source, mode, cropX, cropY, zoom);
				bitmap.Save(temporaryPath, ImageFormat.Bmp);
			}
			VerifyOutput(temporaryPath);
			await CopyFileAsync(oldPath, backupPath, cancellationToken);
			File.Replace(temporaryPath, oldPath, null, ignoreMetadataErrors: true);
			return new
			{
				replaced = true,
				path = oldPath,
				fileName = Path.GetFileName(oldPath),
				backupPath = backupPath,
				backupFolder = backupFolder,
				previewDataUrl = CreateFilePreviewDataUrl(oldPath)
			};
		}
		catch (UnauthorizedAccessException innerException)
		{
			throw new IOException("The portrait could not be replaced. Close Black Desert Online and make sure the FaceTexture folder is writable, then try again.", innerException);
		}
		catch (IOException ex) when (IsLikelyLocked(ex))
		{
			throw new IOException("The portrait file is in use. Close or restart Black Desert Online, then try again.", ex);
		}
		finally
		{
			TryDelete(temporaryPath);
		}
	}

	public async Task<object> RestoreLastBackupAsync(string faceTextureFolder, string oldImagePath, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		string text = ValidateFolder(faceTextureFolder);
		string oldPath = ValidateOldImage(text, oldImagePath);
		string text2 = Path.Combine(text, "_BDOMultiToolBackups");
		if (!Directory.Exists(text2))
		{
			throw new InvalidOperationException("No portrait backup folder exists yet.");
		}
		string extension = Path.GetExtension(oldPath);
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(oldPath);
		string latestBackup = Directory.EnumerateFiles(text2, fileNameWithoutExtension + "_*" + extension, SearchOption.TopDirectoryOnly).OrderByDescending(File.GetLastWriteTimeUtc).FirstOrDefault();
		if (latestBackup == null)
		{
			throw new InvalidOperationException("No backup was found for " + Path.GetFileName(oldPath) + ".");
		}
		string currentBackup = CreateBackupPath(text2, oldPath, DateTime.Now, "before-restore");
		string temporaryPath = Path.Combine(text, $".bdo-portrait-restore-{Guid.NewGuid():N}.bmp");
		try
		{
			await CopyFileAsync(oldPath, currentBackup, cancellationToken);
			await CopyFileAsync(latestBackup, temporaryPath, cancellationToken);
			VerifyOutput(temporaryPath);
			File.Replace(temporaryPath, oldPath, null, ignoreMetadataErrors: true);
			return new
			{
				restored = true,
				path = oldPath,
				fileName = Path.GetFileName(oldPath),
				restoredFrom = latestBackup,
				currentBackup = currentBackup,
				previewDataUrl = CreateFilePreviewDataUrl(oldPath)
			};
		}
		catch (UnauthorizedAccessException innerException)
		{
			throw new IOException("The backup could not be restored. Close Black Desert Online and make sure the FaceTexture folder is writable.", innerException);
		}
		catch (IOException ex) when (IsLikelyLocked(ex))
		{
			throw new IOException("The portrait file is in use. Close or restart Black Desert Online, then try again.", ex);
		}
		finally
		{
			TryDelete(temporaryPath);
		}
	}

	public object OpenBackupFolder(string faceTextureFolder)
	{
		string text = Path.Combine(ValidateFolder(faceTextureFolder), "_BDOMultiToolBackups");
		Directory.CreateDirectory(text);
		Process.Start(new ProcessStartInfo
		{
			FileName = text,
			UseShellExecute = true
		});
		return new
		{
			opened = true,
			backupFolder = text
		};
	}

	internal static Bitmap CreateOutputBitmap(Image source, PortraitCropMode mode, double cropX = 50.0, double cropY = 50.0, double zoom = 1.0)
	{
		Bitmap bitmap = new Bitmap(624, 804, PixelFormat.Format24bppRgb);
		bitmap.SetResolution(96f, 96f);
		using Graphics graphics = Graphics.FromImage(bitmap);
		ConfigureGraphics(graphics);
		graphics.Clear(Color.Black);
		if (mode == PortraitCropMode.Stretch)
		{
			graphics.DrawImage(source, new Rectangle(0, 0, 624, 804), 0, 0, source.Width, source.Height, GraphicsUnit.Pixel);
			return bitmap;
		}
		double num = 0.7761194029850746;
		double num2 = (double)source.Width / (double)source.Height;
		double num3 = Math.Clamp(cropX, 0.0, 100.0) / 100.0;
		double num4 = Math.Clamp(cropY, 0.0, 100.0) / 100.0;
		double num5 = Math.Clamp(zoom, 1.0, 3.0);
		float num6;
		float num7;
		if (num2 > num)
		{
			num6 = (float)((double)source.Height * num / num5);
			num7 = (float)((double)source.Height / num5);
		}
		else
		{
			num6 = (float)((double)source.Width / num5);
			num7 = (float)((double)source.Width / num / num5);
		}
		RectangleF srcRect = new RectangleF((float)((double)((float)source.Width - num6) * num3), (float)((double)((float)source.Height - num7) * num4), num6, num7);
		graphics.DrawImage(source, new RectangleF(0f, 0f, 624f, 804f), srcRect, GraphicsUnit.Pixel);
		return bitmap;
	}

	private static Bitmap CreatePreviewBitmap(Image source, int maxWidth, int maxHeight)
	{
		double num = Math.Min(Math.Min((double)maxWidth / (double)source.Width, (double)maxHeight / (double)source.Height), 1.0);
		int width = Math.Max(1, (int)Math.Round((double)source.Width * num));
		int height = Math.Max(1, (int)Math.Round((double)source.Height * num));
		Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format24bppRgb);
		using Graphics graphics = Graphics.FromImage(bitmap);
		ConfigureGraphics(graphics);
		graphics.Clear(Color.Black);
		graphics.DrawImage(source, 0, 0, width, height);
		return bitmap;
	}

	private static void ConfigureGraphics(Graphics graphics)
	{
		graphics.CompositingMode = CompositingMode.SourceCopy;
		graphics.CompositingQuality = CompositingQuality.HighQuality;
		graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
		graphics.SmoothingMode = SmoothingMode.HighQuality;
		graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
	}

	private static Image LoadUnlockedImage(string path)
	{
		try
		{
			using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
			using Image original = Image.FromStream(stream, useEmbeddedColorManagement: true, validateImageData: true);
			return new Bitmap(original);
		}
		catch (OutOfMemoryException innerException)
		{
			throw new InvalidDataException("The selected image format is not supported. Use PNG, JPG, JPEG, or BMP.", innerException);
		}
		catch (ArgumentException innerException2)
		{
			throw new InvalidDataException("The selected image could not be read. Use PNG, JPG, JPEG, or BMP.", innerException2);
		}
	}

	private static string ValidateFolder(string value)
	{
		string fullPath = Path.GetFullPath(value.Trim());
		if (!Directory.Exists(fullPath))
		{
			throw new DirectoryNotFoundException("Select an existing BDO FaceTexture folder.");
		}
		return fullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
	}

	private static string ValidateOldImage(string folder, string filePath)
	{
		string text = ValidateImageFile(filePath, requireBitmap: true);
		if (!string.Equals(Path.GetDirectoryName(text)?.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), folder, StringComparison.OrdinalIgnoreCase))
		{
			throw new InvalidDataException("The old portrait must be selected directly from the chosen FaceTexture folder.");
		}
		return text;
	}

	private static string ValidateImageFile(string value, bool requireBitmap)
	{
		string fullPath = Path.GetFullPath(value.Trim());
		if (!File.Exists(fullPath))
		{
			throw new FileNotFoundException("The selected image no longer exists.", fullPath);
		}
		string text = Path.GetExtension(fullPath).ToLowerInvariant();
		bool flag;
		switch (text)
		{
		case ".png":
		case ".jpg":
		case ".jpeg":
		case ".bmp":
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (!flag || (requireBitmap && text != ".bmp"))
		{
			throw new InvalidDataException(requireBitmap ? "Select the existing .bmp portrait that BDO currently uses." : "Use a PNG, JPG, JPEG, or BMP image.");
		}
		return fullPath;
	}

	private static PortraitCropMode ParseCropMode(string value)
	{
		if (!string.Equals(value, "stretch", StringComparison.OrdinalIgnoreCase))
		{
			return PortraitCropMode.CropToFit;
		}
		return PortraitCropMode.Stretch;
	}

	private static string CreateBackupPath(string backupFolder, string oldPath, DateTime timestamp, string? suffix = null)
	{
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(oldPath);
		string extension = Path.GetExtension(oldPath);
		string value = (string.IsNullOrWhiteSpace(suffix) ? "" : ("_" + suffix));
		return Path.Combine(backupFolder, $"{fileNameWithoutExtension}_{timestamp:yyyy-MM-dd_HHmmssfff}{value}{extension}");
	}

	private static async Task CopyFileAsync(string source, string destination, CancellationToken cancellationToken)
	{
		await using FileStream input = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read);
		await using FileStream output = new FileStream(destination, FileMode.CreateNew, FileAccess.Write, FileShare.None);
		await input.CopyToAsync(output, cancellationToken);
		await output.FlushAsync(cancellationToken);
	}

	private static void VerifyOutput(string path)
	{
		using Image image = LoadUnlockedImage(path);
		if (image.Width != 624 || image.Height != 804)
		{
			throw new InvalidDataException($"The converted portrait was not {624}x{804}. The original file was not changed.");
		}
	}

	private static string CreateFilePreviewDataUrl(string path)
	{
		using Image source = LoadUnlockedImage(path);
		using Bitmap image = CreatePreviewBitmap(source, 624, 804);
		return ToPngDataUrl(image);
	}

	private static string ToPngDataUrl(Image image)
	{
		using MemoryStream memoryStream = new MemoryStream();
		image.Save(memoryStream, ImageFormat.Png);
		return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
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

