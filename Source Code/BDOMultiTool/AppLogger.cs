using System;
using System.IO;

namespace BDOMultiTool;

internal sealed class AppLogger : IDisposable
{
	private readonly string path;

	private readonly object sync = new object();

	public AppLogger(string path)
	{
		this.path = path;
	}

	public void Info(string message)
	{
		Write("INFO", message);
	}

	public void Warn(string message)
	{
		Write("WARN", message);
	}

	public void Error(string message, Exception? exception = null)
	{
		Write("ERROR", (exception == null) ? message : $"{message}{Environment.NewLine}{exception}");
	}

	private void Write(string level, string message)
	{
		string contents = $"{DateTimeOffset.UtcNow:O} [{level}] {message}{Environment.NewLine}";
		lock (sync)
		{
			File.AppendAllText(path, contents);
			if (new FileInfo(path).Length > 5242880)
			{
				string destFileName = path + ".1";
				File.Delete(destFileName);
				File.Move(path, destFileName);
			}
		}
	}

	public void Dispose()
	{
	}
}

