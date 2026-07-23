using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BDOMultiTool;

internal sealed class AppStateStore
{
	private const int MaxSessionCount = 50_000;
	private const int MaxSerializedBytes = 25 * 1024 * 1024;

	private readonly AppPaths paths;
	private readonly AppLogger logger;
	private readonly SemaphoreSlim writeLock = new(1, 1);

	public AppStateStore(AppPaths paths, AppLogger logger)
	{
		this.paths = paths;
		this.logger = logger;
	}

	public async Task<JsonElement> LoadGrindSessionsAsync(CancellationToken cancellationToken)
	{
		if (!File.Exists(paths.GrindSessionsPath) && !File.Exists(paths.GrindSessionsBackupPath))
		{
			return EmptyArray();
		}

		try
		{
			return await ReadSessionFileAsync(paths.GrindSessionsPath, cancellationToken);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex) when (ex is JsonException or IOException or UnauthorizedAccessException or InvalidDataException)
		{
			logger.Error("Could not read Grind Tracker sessions. Trying the last known-good backup.", ex);
			QuarantineInvalidFile(paths.GrindSessionsPath);
			try
			{
				JsonElement backup = await ReadSessionFileAsync(paths.GrindSessionsBackupPath, cancellationToken);
				await SaveGrindSessionsAsync(backup, cancellationToken);
				return backup;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception backupError) when (backupError is JsonException or IOException or UnauthorizedAccessException or InvalidDataException)
			{
				logger.Error("Could not recover Grind Tracker sessions from the backup.", backupError);
				QuarantineInvalidFile(paths.GrindSessionsBackupPath);
				return EmptyArray();
			}
		}
	}

	public async Task<int> SaveGrindSessionsAsync(JsonElement sessions, CancellationToken cancellationToken)
	{
		if (sessions.ValueKind != JsonValueKind.Array)
		{
			throw new InvalidDataException("Grind Tracker sessions must be a JSON array.");
		}
		int count = sessions.GetArrayLength();
		if (count > MaxSessionCount)
		{
			throw new InvalidDataException($"Grind Tracker supports up to {MaxSessionCount:N0} saved sessions.");
		}

		byte[] json = Encoding.UTF8.GetBytes(sessions.GetRawText());
		if (json.Length > MaxSerializedBytes)
		{
			throw new InvalidDataException("The Grind Tracker session history is too large to save safely.");
		}

		await writeLock.WaitAsync(cancellationToken);
		string tempPath = paths.GrindSessionsPath + $".{Guid.NewGuid():N}.tmp";
		try
		{
			await using (FileStream stream = new(
				tempPath,
				FileMode.CreateNew,
				FileAccess.Write,
				FileShare.None,
				81920,
				FileOptions.Asynchronous | FileOptions.WriteThrough))
			{
				await stream.WriteAsync(json, cancellationToken);
				await stream.FlushAsync(cancellationToken);
				stream.Flush(flushToDisk: true);
			}

			if (File.Exists(paths.GrindSessionsPath))
			{
				File.Replace(tempPath, paths.GrindSessionsPath, paths.GrindSessionsBackupPath, ignoreMetadataErrors: true);
			}
			else
			{
				File.Move(tempPath, paths.GrindSessionsPath);
			}
			return count;
		}
		finally
		{
			try
			{
				if (File.Exists(tempPath))
				{
					File.Delete(tempPath);
				}
			}
			catch
			{
			}
			writeLock.Release();
		}
	}

	private static JsonElement EmptyArray()
	{
		using JsonDocument document = JsonDocument.Parse("[]");
		return document.RootElement.Clone();
	}

	private static async Task<JsonElement> ReadSessionFileAsync(string path, CancellationToken cancellationToken)
	{
		if (!File.Exists(path))
		{
			throw new FileNotFoundException("The Grind Tracker session file does not exist.", path);
		}

		await using FileStream stream = new(
			path,
			FileMode.Open,
			FileAccess.Read,
			FileShare.Read,
			81920,
			FileOptions.Asynchronous | FileOptions.SequentialScan);
		if (stream.Length > MaxSerializedBytes)
		{
			throw new InvalidDataException("The Grind Tracker session file is unexpectedly large.");
		}
		using JsonDocument document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
		if (document.RootElement.ValueKind != JsonValueKind.Array
			|| document.RootElement.GetArrayLength() > MaxSessionCount)
		{
			throw new InvalidDataException("The Grind Tracker session file has an invalid structure.");
		}
		return document.RootElement.Clone();
	}

	private static void QuarantineInvalidFile(string path)
	{
		try
		{
			if (!File.Exists(path))
			{
				return;
			}
			string quarantinePath = path + $".corrupt-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid():N}";
			File.Move(path, quarantinePath, overwrite: false);
		}
		catch
		{
		}
	}

}
