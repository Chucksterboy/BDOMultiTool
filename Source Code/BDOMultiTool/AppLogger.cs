using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BDOMultiTool;

internal sealed class AppLogger : IDisposable
{
	private const long MaxLogBytes = 5 * 1024 * 1024;

	private readonly string path;
	private readonly Channel<string> entries;
	private readonly Task writerTask;
	private bool disposed;

	public AppLogger(string path)
	{
		this.path = path;
		entries = Channel.CreateBounded<string>(new BoundedChannelOptions(2048)
		{
			SingleReader = true,
			SingleWriter = false,
			AllowSynchronousContinuations = false,
			FullMode = BoundedChannelFullMode.DropOldest
		});
		writerTask = Task.Run(WriteLoopAsync);
	}

	public void Info(string message) => Write("INFO", message);

	public void Warn(string message) => Write("WARN", message);

	public void Error(string message, Exception? exception = null)
	{
		Write("ERROR", exception is null ? message : $"{message}{Environment.NewLine}{exception}");
	}

	private void Write(string level, string message)
	{
		if (!disposed)
		{
			entries.Writer.TryWrite($"{DateTimeOffset.UtcNow:O} [{level}] {message}{Environment.NewLine}");
		}
	}

	private async Task WriteLoopAsync()
	{
		List<string> batch = new(64);
		await foreach (string entry in entries.Reader.ReadAllAsync())
		{
			batch.Add(entry);
			while (batch.Count < 64 && entries.Reader.TryRead(out string? queued))
			{
				batch.Add(queued);
			}

			try
			{
				RotateIfNeeded();
				await File.AppendAllTextAsync(path, string.Concat(batch)).ConfigureAwait(false);
			}
			catch
			{
				// Logging must never take down the application.
			}
			finally
			{
				batch.Clear();
			}
		}
	}

	private void RotateIfNeeded()
	{
		if (!File.Exists(path) || new FileInfo(path).Length <= MaxLogBytes)
		{
			return;
		}

		string previousPath = path + ".1";
		File.Delete(previousPath);
		File.Move(path, previousPath);
	}

	public void Dispose()
	{
		if (disposed)
		{
			return;
		}

		disposed = true;
		entries.Writer.TryComplete();
		try
		{
			writerTask.Wait(TimeSpan.FromSeconds(2));
		}
		catch
		{
		}
	}
}
