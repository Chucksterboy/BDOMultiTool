using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		ApplicationConfiguration.Initialize();
		Application.Run(new InstallerForm());
	}
}

internal sealed class InstallerForm : Form
{
	private const string MarketCollectorTaskName = "BDO Multi-Tool Market Collector";

	private readonly CheckBox desktopShortcut;
	private readonly Button installButton;
	private readonly Button cancelButton;
	private readonly ProgressBar progress;
	private readonly Label status;
	private readonly string installPath = Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
		"Programs",
		InstallerConfig.InstallFolderName);

	public InstallerForm()
	{
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

	private void Install()
	{
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

				UseWaitCursor = true;
				status.Text = "Closing running app...";
				CloseRunningInstalledApp();
				progress.Value = 15;
				status.Text = "Updating files...";
				Directory.Delete(installPath, true);
			}
			else
			{
				status.Text = "Installing files...";
				progress.Value = 15;
			}
			Directory.CreateDirectory(installPath);

			using Stream payload = Assembly.GetExecutingAssembly().GetManifestResourceStream("Payload.zip")
				?? throw new InvalidOperationException("Installer payload is missing.");
			using ZipArchive archive = new ZipArchive(payload, ZipArchiveMode.Read);
			archive.ExtractToDirectory(installPath, true);
			progress.Value = 65;

			string exePath = Path.Combine(installPath, InstallerConfig.ExeName);
			if (!File.Exists(exePath))
			{
				throw new FileNotFoundException("Installed application executable was not found.", exePath);
			}

			status.Text = "Creating shortcuts...";
			CreateStartMenuShortcut(exePath, exePath);
			if (desktopShortcut.Checked)
			{
				string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				CreateShortcut(Path.Combine(desktop, InstallerConfig.ShortcutName + ".lnk"), exePath, exePath);
			}
			status.Text = "Scheduling market collector...";
			CreateMarketCollectorTask(exePath);
			progress.Value = 90;

			status.Text = "Finishing...";
			WriteUninstallHelper();
			progress.Value = 100;
			UseWaitCursor = false;

			DialogResult result = MessageBox.Show(
				InstallerConfig.AppName + (updating ? " has been updated." : " has been installed.") + "\r\n\r\nOpen it now?",
				InstallerConfig.AppName + " Setup",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Information);
			if (result == DialogResult.Yes)
			{
				Process.Start(new ProcessStartInfo(exePath) { UseShellExecute = true });
			}
			Close();
		}
		catch (Exception ex)
		{
			UseWaitCursor = false;
			installButton.Enabled = true;
			cancelButton.Enabled = true;
			status.Text = "Install failed.";
			MessageBox.Show(ex.Message, InstallerConfig.AppName + " Setup", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}

	private void CloseRunningInstalledApp()
	{
		string expectedExe = Path.Combine(installPath, InstallerConfig.ExeName);
		foreach (Process process in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(InstallerConfig.ExeName)))
		{
			try
			{
				string path = process.MainModule?.FileName ?? string.Empty;
				if (string.Equals(path, expectedExe, StringComparison.OrdinalIgnoreCase))
				{
					process.CloseMainWindow();
					if (!process.WaitForExit(3000))
					{
						process.Kill(entireProcessTree: true);
						process.WaitForExit(3000);
					}
				}
			}
			catch
			{
			}
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
			+ "del \"" + desktopShortcutPath + "\" >nul 2>nul\r\n"
			+ "rmdir /S /Q \"" + startMenuFolder + "\" >nul 2>nul\r\n"
			+ "cd /d \"%TEMP%\"\r\n"
			+ "rmdir /S /Q \"" + installPath + "\"\r\n";
		File.WriteAllText(uninstallPath, script);
		CreateShortcut(Path.Combine(startMenuFolder, "Uninstall " + InstallerConfig.ShortcutName + ".lnk"), uninstallPath);
	}

	private void CreateMarketCollectorTask(string exePath)
	{
		string xmlPath = Path.Combine(Path.GetTempPath(), "bdo-market-collector-task.xml");
		try
		{
			string startBoundary = DateTime.Now.AddMinutes(5).ToString("yyyy-MM-ddTHH:mm:ss");
			string workingDirectory = Path.GetDirectoryName(exePath) ?? installPath;
			string xml = @"<?xml version=""1.0"" encoding=""UTF-16""?>
<Task version=""1.4"" xmlns=""http://schemas.microsoft.com/windows/2004/02/mit/task"">
  <RegistrationInfo>
    <Author>BDO Multi-Tool</Author>
    <Description>Keeps BDO Multi-Tool market analytics samples fresh for EU and NA.</Description>
  </RegistrationInfo>
  <Triggers>
    <LogonTrigger>
      <Enabled>true</Enabled>
    </LogonTrigger>
    <TimeTrigger>
      <Repetition>
        <Interval>PT3H</Interval>
        <StopAtDurationEnd>false</StopAtDurationEnd>
      </Repetition>
      <StartBoundary>" + SecurityElement.Escape(startBoundary) + @"</StartBoundary>
      <Enabled>true</Enabled>
    </TimeTrigger>
  </Triggers>
  <Principals>
    <Principal id=""Author"">
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
			ProcessStartInfo startInfo = new ProcessStartInfo("schtasks.exe", "/Create /TN \"" + MarketCollectorTaskName + "\" /XML \"" + xmlPath + "\" /F")
			{
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardError = true,
				RedirectStandardOutput = true
			};
			using Process process = Process.Start(startInfo) ?? throw new InvalidOperationException("Could not start Task Scheduler.");
			process.WaitForExit(15000);
			if (process.ExitCode != 0)
			{
				string error = process.StandardError.ReadToEnd();
				string output = process.StandardOutput.ReadToEnd();
				MessageBox.Show(
					"The app installed, but Windows would not create the background market collector task.\r\n\r\n"
					+ (string.IsNullOrWhiteSpace(error) ? output : error),
					InstallerConfig.AppName + " Setup",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(
				"The app installed, but the background market collector task could not be created.\r\n\r\n" + ex.Message,
				InstallerConfig.AppName + " Setup",
				MessageBoxButtons.OK,
				MessageBoxIcon.Warning);
		}
		finally
		{
			try { if (File.Exists(xmlPath)) File.Delete(xmlPath); } catch { }
		}
	}
}
