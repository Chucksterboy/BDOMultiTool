using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

internal static class Program
{
	[STAThread]
	private static void Main(string[] args)
	{
		SetSafeWorkingDirectory();
		if (args.Any(arg => string.Equals(arg, "--self-test", StringComparison.OrdinalIgnoreCase)))
		{
			Environment.ExitCode = InstallerSelfTest.Run();
			return;
		}

		ApplicationConfiguration.Initialize();
		Application.Run(new InstallerForm(args));
	}

	internal static void SetSafeWorkingDirectory()
	{
		string processPath = Environment.ProcessPath ?? string.Empty;
		string safeDirectory = Path.GetDirectoryName(processPath) ?? Path.GetTempPath();
		if (Directory.Exists(safeDirectory))
			Environment.CurrentDirectory = safeDirectory;
	}
}

internal sealed class InstallerForm : Form
{
	private const string MarketCollectorTaskName = "BDO Multi-Tool Market Collector";
	private const string SingleInstancePipeName = "BDOMultiTool.SingleInstance.Restore";

	private readonly CheckBox desktopShortcut;
	private readonly Button installButton;
	private readonly Button cancelButton;
	private readonly ProgressBar progress;
	private readonly Label status;
	private readonly string installPath;
	private readonly int? sourceProcessId;

	public InstallerForm(string[] args)
	{
		InstallTarget target = ResolveInstallTarget(args);
		installPath = target.Path;
		sourceProcessId = target.SourceProcessId;
		Text = InstallerConfig.AppName + " Setup";
		StartPosition = FormStartPosition.CenterScreen;
		FormBorderStyle = FormBorderStyle.FixedDialog;
		MaximizeBox = false;
		MinimizeBox = false;
		ClientSize = new Size(520, 285);
		BackColor = Color.FromArgb(12, 18, 28);
		ForeColor = Color.White;
		Font = new Font("Segoe UI", 9f);

		Label title = new Label
		{
			Text = "Install " + InstallerConfig.AppName,
			Font = new Font("Segoe UI", 18f, FontStyle.Bold),
			AutoSize = false,
			Location = new Point(24, 22),
			Size = new Size(470, 42)
		};

		Label body = new Label
		{
			Text = "This will install the application for your Windows user account.",
			AutoSize = false,
			Location = new Point(26, 72),
			Size = new Size(460, 24),
			ForeColor = Color.FromArgb(210, 220, 235)
		};

		Label path = new Label
		{
			Text = "Install location:\r\n" + installPath,
			AutoSize = false,
			Location = new Point(26, 105),
			Size = new Size(465, 45),
			ForeColor = Color.FromArgb(170, 185, 205)
		};

		desktopShortcut = new CheckBox
		{
			Text = "Create desktop shortcut",
			Checked = true,
			AutoSize = true,
			Location = new Point(29, 158),
			ForeColor = Color.White
		};

		progress = new ProgressBar
		{
			Location = new Point(28, 194),
			Size = new Size(464, 18),
			Style = ProgressBarStyle.Continuous
		};

		status = new Label
		{
			Text = "Ready to install.",
			AutoSize = false,
			Location = new Point(28, 218),
			Size = new Size(464, 22),
			ForeColor = Color.FromArgb(170, 185, 205)
		};

		installButton = new Button
		{
			Text = "Install",
			Location = new Point(316, 246),
			Size = new Size(84, 28)
		};
		installButton.Click += (_, _) => Install();

		cancelButton = new Button
		{
			Text = "Cancel",
			Location = new Point(408, 246),
			Size = new Size(84, 28)
		};
		cancelButton.Click += (_, _) => Close();

		Controls.AddRange(new Control[] { title, body, path, desktopShortcut, progress, status, installButton, cancelButton });
	}

	protected override void OnShown(EventArgs e)
	{
		base.OnShown(e);
		if (!Directory.Exists(installPath))
			return;

		BeginInvoke(new Action(() =>
		{
			try
			{
				UseWaitCursor = true;
				status.Text = "Closing running application...";
				CloseRunningInstalledApp();
				status.Text = "Ready to update.";
			}
			catch (Exception ex)
			{
				status.Text = "Close BDO Multi-Tool and try again.";
				MessageBox.Show(ex.Message, InstallerConfig.AppName + " Setup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			finally
			{
				UseWaitCursor = false;
			}
		}));
	}

	private void Install()
	{
		string stagingPath = null;
		try
		{
			installButton.Enabled = false;
			cancelButton.Enabled = false;
			UseWaitCursor = true;
			bool updating = Directory.Exists(installPath);
			status.Text = updating ? "Preparing update..." : "Installing files...";
			progress.Value = 10;

			if (updating)
			{
				UseWaitCursor = false;
				DialogResult updateChoice = MessageBox.Show(
					InstallerConfig.AppName + " is already installed.\r\n\r\nWould you like to update the existing files?\r\n\r\nYour saved settings, market samples, coupons, and other app data will be kept.",
					InstallerConfig.AppName + " Update",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button1);
				if (updateChoice != DialogResult.Yes)
				{
					status.Text = "Update cancelled.";
					installButton.Enabled = true;
					cancelButton.Enabled = true;
					return;
				}

			}

			UseWaitCursor = true;
			status.Text = "Preparing application files...";
			stagingPath = StagePayload();
			progress.Value = 20;

			if (updating)
			{
				status.Text = "Closing running app...";
				CloseRunningInstalledApp();
			}

			status.Text = updating ? "Updating files..." : "Installing files...";
			ReplaceApplicationFiles(stagingPath, installPath, updating);
			stagingPath = null;
			progress.Value = 65;

			string exePath = Path.Combine(installPath, InstallerConfig.ExeName);
			if (!File.Exists(exePath))
				throw new FileNotFoundException("Installed application executable was not found.", exePath);

			status.Text = "Creating shortcuts...";
			CreateStartMenuShortcut(exePath, exePath);
			if (desktopShortcut.Checked)
			{
				string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				CreateShortcut(Path.Combine(desktop, InstallerConfig.ShortcutName + ".lnk"), exePath, exePath);
			}
			status.Text = "Scheduling market collector...";
			bool marketCollectorScheduled = CreateMarketCollectorTask(exePath);
			progress.Value = 90;

			status.Text = marketCollectorScheduled ? "Market collector scheduled." : "Market collector will refresh when the app opens.";
			WriteUninstallHelper();
			progress.Value = 100;
			UseWaitCursor = false;

			DialogResult result = MessageBox.Show(
				InstallerConfig.AppName + (updating ? " has been updated." : " has been installed.") + "\r\n\r\nOpen it now?",
				InstallerConfig.AppName + " Setup",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Information);
			if (result == DialogResult.Yes)
				Process.Start(new ProcessStartInfo(exePath) { UseShellExecute = true });
			Close();
		}
		catch (Exception ex)
		{
			if (!string.IsNullOrWhiteSpace(stagingPath))
				TryDeleteDirectory(stagingPath);
			UseWaitCursor = false;
			installButton.Enabled = true;
			cancelButton.Enabled = true;
			status.Text = "Install failed.";
			MessageBox.Show(
				"The application could not be installed safely. No working installation was removed.\r\n\r\n" + ex.Message,
				InstallerConfig.AppName + " Setup",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error);
		}
	}

	private string StagePayload()
	{
		string parentPath = Directory.GetParent(installPath)?.FullName
			?? throw new InvalidOperationException("The application install folder is invalid.");
		Directory.CreateDirectory(parentPath);
		string stagingPath = Path.Combine(parentPath, $".{InstallerConfig.InstallFolderName}.staging-{Guid.NewGuid():N}");
		Directory.CreateDirectory(stagingPath);
		try
		{
			using Stream payload = Assembly.GetExecutingAssembly().GetManifestResourceStream("Payload.zip")
				?? throw new InvalidOperationException("Installer payload is missing.");
			using ZipArchive archive = new ZipArchive(payload, ZipArchiveMode.Read);
			archive.ExtractToDirectory(stagingPath, true);
			string stagedExe = Path.Combine(stagingPath, InstallerConfig.ExeName);
			if (!File.Exists(stagedExe))
				throw new FileNotFoundException("Installer payload does not contain the application executable.", stagedExe);
			return stagingPath;
		}
		catch
		{
			TryDeleteDirectory(stagingPath);
			throw;
		}
	}

	private void CloseRunningInstalledApp()
	{
		RequestGracefulAppShutdown();
		DateTime deadline = DateTime.UtcNow.AddSeconds(20);
		Exception lastError = null;
		do
		{
			Process[] processes = GetTargetProcesses();
			if (processes.Length == 0)
				return;

			foreach (Process process in processes)
			{
				using (process)
				{
					try
					{
						process.CloseMainWindow();
						process.WaitForExit(1500);
					}
					catch (InvalidOperationException)
					{
						// The process exited between enumeration and inspection.
					}
					catch (Exception ex)
					{
						lastError = ex;
					}
				}
			}

			Thread.Sleep(250);
		}
		while (DateTime.UtcNow < deadline);

		throw new InvalidOperationException(
			"The BDO Multi-Tool instance being updated is still running. Close it from the system tray and try again.",
			lastError);
	}

	private static void RequestGracefulAppShutdown()
	{
		try
		{
			using NamedPipeClientStream client = new NamedPipeClientStream(".", SingleInstancePipeName, PipeDirection.Out);
			client.Connect(1500);
			using StreamWriter writer = new StreamWriter(client);
			writer.WriteLine("shutdown-for-update");
			writer.Flush();
		}
		catch
		{
			// Older builds do not understand the shutdown command; their normal update flow still exits itself.
		}
	}

	private Process[] GetTargetProcesses()
	{
		string processName = Path.GetFileNameWithoutExtension(InstallerConfig.ExeName);
		string targetExe = Path.GetFullPath(Path.Combine(installPath, InstallerConfig.ExeName));
		List<Process> targets = new List<Process>();
		foreach (Process process in Process.GetProcessesByName(processName))
		{
			bool isTarget = sourceProcessId.HasValue && process.Id == sourceProcessId.Value;
			if (!isTarget)
			{
				try
				{
					string processPath = process.MainModule?.FileName ?? string.Empty;
					isTarget = string.Equals(Path.GetFullPath(processPath), targetExe, StringComparison.OrdinalIgnoreCase);
				}
				catch
				{
				}
			}

			if (isTarget)
				targets.Add(process);
			else
				process.Dispose();
		}
		return targets.ToArray();
	}

	private static InstallTarget ResolveInstallTarget(string[] args)
	{
		string defaultPath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
			"Programs",
			InstallerConfig.InstallFolderName);
		int? requestedProcessId = TryReadIntArgument(args, "--source-pid");
		string requestedPath = TryReadStringArgument(args, "--install-path");
		if (!string.IsNullOrWhiteSpace(requestedPath))
		{
			string fullPath = Path.GetFullPath(requestedPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			if (!string.Equals(fullPath, Path.GetFullPath(defaultPath), StringComparison.OrdinalIgnoreCase)
				&& !File.Exists(Path.Combine(fullPath, InstallerConfig.ExeName)))
			{
				throw new InvalidOperationException("The requested update folder does not contain BDO Multi-Tool.");
			}
			return new InstallTarget(fullPath, requestedProcessId);
		}

		List<InstallTarget> candidates = new List<InstallTarget>();
		int currentSessionId = Process.GetCurrentProcess().SessionId;
		foreach (Process process in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(InstallerConfig.ExeName)))
		{
			using (process)
			{
				try
				{
					if (process.SessionId != currentSessionId)
						continue;
					string processPath = process.MainModule?.FileName ?? string.Empty;
					string directory = Path.GetDirectoryName(processPath) ?? string.Empty;
					if (!string.IsNullOrWhiteSpace(directory))
						candidates.Add(new InstallTarget(Path.GetFullPath(directory), process.Id));
				}
				catch
				{
				}
			}
		}

		InstallTarget detected = candidates.FirstOrDefault(candidate =>
			string.Equals(candidate.Path, Path.GetFullPath(defaultPath), StringComparison.OrdinalIgnoreCase))
			?? candidates.FirstOrDefault();
		return detected ?? new InstallTarget(defaultPath, requestedProcessId);
	}

	private static string TryReadStringArgument(string[] args, string name)
	{
		for (int index = 0; index < args.Length - 1; index++)
		{
			if (string.Equals(args[index], name, StringComparison.OrdinalIgnoreCase))
				return args[index + 1];
		}
		return string.Empty;
	}

	private static int? TryReadIntArgument(string[] args, string name)
	{
		string value = TryReadStringArgument(args, name);
		return int.TryParse(value, out int parsed) && parsed > 0 ? parsed : null;
	}

	internal static void ReplaceApplicationFiles(string stagingPath, string targetPath, bool updating)
	{
		string backupPath = null;
		bool existingMoved = false;
		bool stagedFilesInstalled = false;
		try
		{
			if (updating)
			{
				backupPath = targetPath + $".backup-{Guid.NewGuid():N}";
				RetryFileSystemAction(
					() => Directory.Move(targetPath, backupPath),
					"Windows did not release the existing application files in time.");
				existingMoved = true;
			}

			RetryFileSystemAction(
				() => Directory.Move(stagingPath, targetPath),
				"The prepared application files could not be moved into place.");
			stagedFilesInstalled = true;

			if (!File.Exists(Path.Combine(targetPath, InstallerConfig.ExeName)))
				throw new InvalidDataException("The installed application executable is missing after replacement.");

			if (existingMoved && backupPath != null)
				TryDeleteDirectory(backupPath);
		}
		catch
		{
			if (stagedFilesInstalled)
				TryDeleteDirectory(targetPath);
			if (existingMoved && backupPath != null && Directory.Exists(backupPath) && !Directory.Exists(targetPath))
			{
				RetryFileSystemAction(
					() => Directory.Move(backupPath, targetPath),
					"The previous application files could not be restored.");
			}
			throw;
		}
		finally
		{
			if (Directory.Exists(stagingPath))
				TryDeleteDirectory(stagingPath);
		}
	}

	internal static void RetryFileSystemAction(Action action, string failureMessage)
	{
		Exception lastError = null;
		for (int attempt = 1; attempt <= 12; attempt++)
		{
			try
			{
				action();
				return;
			}
			catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
			{
				lastError = ex;
				if (attempt < 12)
					Thread.Sleep(500);
			}
		}

		throw new IOException(failureMessage, lastError);
	}

	internal static void TryDeleteDirectory(string path)
	{
		if (!Directory.Exists(path))
			return;

		try
		{
			foreach (string file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
				File.SetAttributes(file, FileAttributes.Normal);
			RetryFileSystemAction(() => Directory.Delete(path, true), "Temporary installer files could not be removed.");
		}
		catch
		{
			// A stale staging or backup folder is safer than removing a working installation.
		}
	}

	private static void CreateShortcut(string shortcutPath, string targetPath, string iconPath = null)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(shortcutPath)!);
		Type shellType = Type.GetTypeFromProgID("WScript.Shell")
			?? throw new InvalidOperationException("Windows shortcut support is not available.");
		dynamic shell = Activator.CreateInstance(shellType)!;
		dynamic shortcut = shell.CreateShortcut(shortcutPath);
		shortcut.TargetPath = targetPath;
		shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
		shortcut.IconLocation = (iconPath ?? targetPath) + ",0";
		shortcut.Save();
		Marshal.FinalReleaseComObject(shortcut);
		Marshal.FinalReleaseComObject(shell);
	}

	private static void CreateStartMenuShortcut(string targetPath, string iconPath)
	{
		string programs = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
		string folder = Path.Combine(programs, InstallerConfig.ShortcutName);
		CreateShortcut(Path.Combine(folder, InstallerConfig.ShortcutName + ".lnk"), targetPath, iconPath);
	}

	private void WriteUninstallHelper()
	{
		string uninstallPath = Path.Combine(installPath, "Uninstall " + InstallerConfig.ShortcutName + ".cmd");
		string desktopShortcutPath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
			InstallerConfig.ShortcutName + ".lnk");
		string startMenuFolder = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.Programs),
			InstallerConfig.ShortcutName);
		string script = "@echo off\r\n"
			+ "echo Closing " + InstallerConfig.AppName + " if it is running...\r\n"
			+ "taskkill /IM \"" + InstallerConfig.ExeName + "\" /F >nul 2>nul\r\n"
			+ "schtasks /Delete /TN \"" + MarketCollectorTaskName + "\" /F >nul 2>nul\r\n"
			+ "schtasks /Delete /TN \"" + GetUserMarketCollectorTaskName() + "\" /F >nul 2>nul\r\n"
			+ "del \"" + desktopShortcutPath + "\" >nul 2>nul\r\n"
			+ "rmdir /S /Q \"" + startMenuFolder + "\" >nul 2>nul\r\n"
			+ "cd /d \"%TEMP%\"\r\n"
			+ "rmdir /S /Q \"" + installPath + "\"\r\n";
		File.WriteAllText(uninstallPath, script);
		CreateShortcut(Path.Combine(startMenuFolder, "Uninstall " + InstallerConfig.ShortcutName + ".lnk"), uninstallPath);
	}

	private bool CreateMarketCollectorTask(string exePath)
	{
		string details;
		if (TryCreateMarketCollectorTask(MarketCollectorTaskName, exePath, out details))
		{
			return true;
		}

		string userTaskName = GetUserMarketCollectorTaskName();
		if (!string.Equals(userTaskName, MarketCollectorTaskName, StringComparison.OrdinalIgnoreCase)
			&& TryCreateMarketCollectorTask(userTaskName, exePath, out details))
		{
			return true;
		}

		try
		{
			File.AppendAllText(
				Path.Combine(installPath, "install.log"),
				DateTime.Now.ToString("s") + " Market collector task was not created. " + details + Environment.NewLine);
		}
		catch
		{
		}

		return false;
	}

	private bool TryCreateMarketCollectorTask(string taskName, string exePath, out string details)
	{
		string xmlPath = Path.Combine(Path.GetTempPath(), "bdo-market-collector-task-" + Guid.NewGuid().ToString("N") + ".xml");
		try
		{
			string startBoundary = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-ddTHH:mm:ss");
			string workingDirectory = Path.GetDirectoryName(exePath) ?? installPath;
			string userId = WindowsIdentity.GetCurrent().Name;
			RunSchtasks("/Delete /TN \"" + taskName + "\" /F", 5000, out _);
			string xml = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<Task version=""1.4"" xmlns=""http://schemas.microsoft.com/windows/2004/02/mit/task"">
  <RegistrationInfo>
    <Author>BDO Multi-Tool</Author>
    <Description>Keeps BDO Multi-Tool EU market analytics samples fresh once per day.</Description>
  </RegistrationInfo>
  <Triggers>
    <LogonTrigger>
      <Enabled>true</Enabled>
    </LogonTrigger>
    <TimeTrigger>
      <Repetition>
        <Interval>PT24H</Interval>
        <StopAtDurationEnd>false</StopAtDurationEnd>
      </Repetition>
      <StartBoundary>" + SecurityElement.Escape(startBoundary) + @"</StartBoundary>
      <Enabled>true</Enabled>
    </TimeTrigger>
  </Triggers>
  <Principals>
    <Principal id=""Author"">
      <UserId>" + SecurityElement.Escape(userId) + @"</UserId>
      <LogonType>InteractiveToken</LogonType>
      <RunLevel>LeastPrivilege</RunLevel>
    </Principal>
  </Principals>
  <Settings>
    <MultipleInstancesPolicy>IgnoreNew</MultipleInstancesPolicy>
    <DisallowStartIfOnBatteries>false</DisallowStartIfOnBatteries>
    <StopIfGoingOnBatteries>false</StopIfGoingOnBatteries>
    <AllowHardTerminate>true</AllowHardTerminate>
    <StartWhenAvailable>true</StartWhenAvailable>
    <RunOnlyIfNetworkAvailable>true</RunOnlyIfNetworkAvailable>
    <IdleSettings>
      <StopOnIdleEnd>false</StopOnIdleEnd>
      <RestartOnIdle>false</RestartOnIdle>
    </IdleSettings>
    <AllowStartOnDemand>true</AllowStartOnDemand>
    <Enabled>true</Enabled>
    <Hidden>true</Hidden>
    <RunOnlyIfIdle>false</RunOnlyIfIdle>
    <WakeToRun>false</WakeToRun>
    <ExecutionTimeLimit>PT45M</ExecutionTimeLimit>
    <Priority>7</Priority>
  </Settings>
  <Actions Context=""Author"">
    <Exec>
      <Command>" + SecurityElement.Escape(exePath) + @"</Command>
      <Arguments>--market-scheduled-update</Arguments>
      <WorkingDirectory>" + SecurityElement.Escape(workingDirectory) + @"</WorkingDirectory>
    </Exec>
  </Actions>
</Task>";
			File.WriteAllText(xmlPath, xml, System.Text.Encoding.Unicode);
			int exitCode = RunSchtasks("/Create /TN \"" + taskName + "\" /XML \"" + xmlPath + "\" /F", 15000, out details);
			return exitCode == 0;
		}
		catch (Exception ex)
		{
			details = ex.Message;
			return false;
		}
		finally
		{
			try { if (File.Exists(xmlPath)) File.Delete(xmlPath); } catch { }
		}
	}

	private static int RunSchtasks(string arguments, int timeoutMilliseconds, out string output)
	{
		ProcessStartInfo startInfo = new ProcessStartInfo("schtasks.exe", arguments)
		{
			CreateNoWindow = true,
			UseShellExecute = false,
			RedirectStandardError = true,
			RedirectStandardOutput = true
		};
		using Process process = Process.Start(startInfo) ?? throw new InvalidOperationException("Could not start Task Scheduler.");
		if (!process.WaitForExit(timeoutMilliseconds))
		{
			try { process.Kill(entireProcessTree: true); } catch { }
			output = "Task Scheduler timed out.";
			return -1;
		}

		string error = process.StandardError.ReadToEnd();
		string standardOutput = process.StandardOutput.ReadToEnd();
		output = string.IsNullOrWhiteSpace(error) ? standardOutput.Trim() : error.Trim();
		return process.ExitCode;
	}

	private static string GetUserMarketCollectorTaskName()
	{
		string userName = Environment.UserName;
		char[] safeChars = userName.Select(ch => char.IsLetterOrDigit(ch) || ch == '-' || ch == '_' || ch == '.' ? ch : '_').ToArray();
		string safeUserName = new string(safeChars).Trim('_');
		if (string.IsNullOrWhiteSpace(safeUserName))
		{
			return MarketCollectorTaskName;
		}

		return MarketCollectorTaskName + " - " + safeUserName;
	}

	private sealed record InstallTarget(string Path, int? SourceProcessId);
}

internal static class InstallerSelfTest
{
	public static int Run()
	{
		string root = Path.Combine(Path.GetTempPath(), $"BDOMultiTool-Installer-SelfTest-{Guid.NewGuid():N}");
		try
		{
			Directory.CreateDirectory(root);
			TestInheritedWorkingDirectory(root);
			TestRetryBehavior();
			TestLockedInstallReplacement(root);
			TestExistingInstallReplacement(root);
			TestRollback(root);
			TestFreshInstall(root);
			return 0;
		}
		catch
		{
			return 1;
		}
		finally
		{
			InstallerForm.TryDeleteDirectory(root);
		}
	}

	private static void TestInheritedWorkingDirectory(string root)
	{
		string inheritedDirectory = Path.Combine(root, "inherited-working-directory");
		string movedDirectory = inheritedDirectory + "-moved";
		Directory.CreateDirectory(inheritedDirectory);
		Environment.CurrentDirectory = inheritedDirectory;
		Program.SetSafeWorkingDirectory();
		Directory.Move(inheritedDirectory, movedDirectory);
		if (!Directory.Exists(movedDirectory))
			throw new InvalidOperationException("Working-directory release self-test failed.");
	}

	private static void TestRetryBehavior()
	{
		int attempts = 0;
		InstallerForm.RetryFileSystemAction(() =>
		{
			attempts++;
			if (attempts < 3)
				throw new UnauthorizedAccessException("Simulated transient file lock.");
		}, "Retry self-test failed.");
		if (attempts != 3)
			throw new InvalidOperationException("The installer retry loop did not recover as expected.");
	}

	private static void TestExistingInstallReplacement(string root)
	{
		string target = Path.Combine(root, "update-target");
		string staging = Path.Combine(root, "update-staging");
		Directory.CreateDirectory(target);
		Directory.CreateDirectory(staging);
		File.WriteAllText(Path.Combine(target, InstallerConfig.ExeName), "old");
		File.WriteAllText(Path.Combine(target, "old-only.txt"), "old");
		File.WriteAllText(Path.Combine(staging, InstallerConfig.ExeName), "new");
		File.WriteAllText(Path.Combine(staging, "new-only.txt"), "new");

		InstallerForm.ReplaceApplicationFiles(staging, target, updating: true);
		if (File.ReadAllText(Path.Combine(target, InstallerConfig.ExeName)) != "new"
			|| File.Exists(Path.Combine(target, "old-only.txt"))
			|| !File.Exists(Path.Combine(target, "new-only.txt")))
		{
			throw new InvalidOperationException("Existing-install replacement self-test failed.");
		}
	}

	private static void TestLockedInstallReplacement(string root)
	{
		string target = Path.Combine(root, "locked-target");
		string staging = Path.Combine(root, "locked-staging");
		Directory.CreateDirectory(target);
		Directory.CreateDirectory(staging);
		string lockedExe = Path.Combine(target, InstallerConfig.ExeName);
		File.WriteAllText(lockedExe, "locked-old-version");
		File.WriteAllText(Path.Combine(staging, InstallerConfig.ExeName), "replacement");

		FileStream lockStream = new FileStream(lockedExe, FileMode.Open, FileAccess.Read, FileShare.Read);
		Thread releaseLock = new Thread(() =>
		{
			Thread.Sleep(1200);
			lockStream.Dispose();
		})
		{
			IsBackground = true
		};
		releaseLock.Start();
		try
		{
			InstallerForm.ReplaceApplicationFiles(staging, target, updating: true);
		}
		finally
		{
			lockStream.Dispose();
			releaseLock.Join(5000);
		}

		if (File.ReadAllText(Path.Combine(target, InstallerConfig.ExeName)) != "replacement")
			throw new InvalidOperationException("Locked-file replacement self-test failed.");
	}

	private static void TestRollback(string root)
	{
		string target = Path.Combine(root, "rollback-target");
		string staging = Path.Combine(root, "rollback-staging");
		Directory.CreateDirectory(target);
		Directory.CreateDirectory(staging);
		File.WriteAllText(Path.Combine(target, InstallerConfig.ExeName), "working-old-version");
		File.WriteAllText(Path.Combine(staging, "incomplete.txt"), "missing executable");

		try
		{
			InstallerForm.ReplaceApplicationFiles(staging, target, updating: true);
			throw new InvalidOperationException("Rollback self-test did not reject an incomplete payload.");
		}
		catch (InvalidDataException)
		{
		}

		if (!File.Exists(Path.Combine(target, InstallerConfig.ExeName))
			|| File.ReadAllText(Path.Combine(target, InstallerConfig.ExeName)) != "working-old-version")
		{
			throw new InvalidOperationException("Rollback self-test did not restore the working installation.");
		}
	}

	private static void TestFreshInstall(string root)
	{
		string target = Path.Combine(root, "fresh-target");
		string staging = Path.Combine(root, "fresh-staging");
		Directory.CreateDirectory(staging);
		File.WriteAllText(Path.Combine(staging, InstallerConfig.ExeName), "fresh");

		InstallerForm.ReplaceApplicationFiles(staging, target, updating: false);
		if (!File.Exists(Path.Combine(target, InstallerConfig.ExeName)))
			throw new InvalidOperationException("Fresh-install self-test failed.");
	}
}
