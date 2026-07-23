using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BDOMultiTool;

internal static class AtomicFile
{
	public static async Task<T?> ReadJsonAsync<T>(
		string path,
		JsonSerializerOptions options,
		CancellationToken cancellationToken)
	{
		foreach (string candidate in new[] { path, path + ".bak" })
		{
			if (!File.Exists(candidate))
			{
				continue;
			}

			try
			{
				string json = await File.ReadAllTextAsync(candidate, cancellationToken);
				T? value = JsonSerializer.Deserialize<T>(json, options);
				if (value is not null)
				{
					return value;
				}
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				throw;
			}
			catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException)
			{
				if (string.Equals(candidate, path, StringComparison.OrdinalIgnoreCase))
				{
					TryQuarantine(candidate);
				}
			}
		}

		return default;
	}

	public static async Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken)
	{
		string? directory = Path.GetDirectoryName(path);
		if (!string.IsNullOrWhiteSpace(directory))
		{
			Directory.CreateDirectory(directory);
		}

		string temporaryPath = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
		string backupPath = path + ".bak";
		try
		{
			await using (FileStream stream = new(
				temporaryPath,
				FileMode.CreateNew,
				FileAccess.Write,
				FileShare.None,
				4096,
				FileOptions.Asynchronous | FileOptions.WriteThrough))
			{
				byte[] bytes = Encoding.UTF8.GetBytes(contents);
				await stream.WriteAsync(bytes, cancellationToken);
				await stream.FlushAsync(cancellationToken);
				stream.Flush(flushToDisk: true);
			}

			if (File.Exists(path))
			{
				File.Replace(temporaryPath, path, backupPath, ignoreMetadataErrors: true);
			}
			else
			{
				File.Move(temporaryPath, path);
			}
		}
		finally
		{
			try
			{
				if (File.Exists(temporaryPath))
				{
					File.Delete(temporaryPath);
				}
			}
			catch
			{
			}
		}
	}

	private static void TryQuarantine(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				File.Move(
					path,
					path + $".corrupt-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}",
					overwrite: false);
			}
		}
		catch
		{
		}
	}
}
