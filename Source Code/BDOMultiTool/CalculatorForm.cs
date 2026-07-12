using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace BDOMultiTool;

internal sealed class CalculatorForm : Form
{
	[ComImport]
	[Guid("56FDF344-FD6D-11d0-958A-006097C9A090")]
	private sealed class CTaskbarList
	{
	}

	[ComImport]
	[Guid("EA1AFB91-9E28-4B86-90E9-9E9F8A5EEFAF")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	private interface ITaskbarList3
	{
		void HrInit();
		void AddTab(IntPtr hwnd);
		void DeleteTab(IntPtr hwnd);
		void ActivateTab(IntPtr hwnd);
		void SetActiveAlt(IntPtr hwnd);
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
		void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);
		void SetProgressState(IntPtr hwnd, int tbpFlags);
		void RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);
		void UnregisterTab(IntPtr hwndTab);
		void SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);
		void SetTabActive(IntPtr hwndTab, IntPtr hwndMDI, uint dwReserved);
		void ThumbBarAddButtons(IntPtr hwnd, uint cButtons, IntPtr pButton);
		void ThumbBarUpdateButtons(IntPtr hwnd, uint cButtons, IntPtr pButton);
		void ThumbBarSetImageList(IntPtr hwnd, IntPtr himl);
		void SetOverlayIcon(IntPtr hwnd, IntPtr hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);
		void SetThumbnailTooltip(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);
		void SetThumbnailClip(IntPtr hwnd, IntPtr prcClip);
	}

	[DllImport("winmm.dll", CharSet = CharSet.Unicode)]
	private static extern int mciSendString(string command, StringBuilder? returnValue, int returnLength, IntPtr callback);

	[DllImport("user32.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool DestroyIcon(IntPtr hIcon);

	private const int WmNcHitTest = 132;

	private const int WmNcLButtonDown = 161;

	private const int HtCaption = 2;

	private const int HtLeft = 10;
	private const int HtRight = 11;
	private const int HtTop = 12;
	private const int HtTopLeft = 13;
	private const int HtTopRight = 14;
	private const int HtBottom = 15;
	private const int HtBottomLeft = 16;
	private const int HtBottomRight = 17;
	private const int WsThickFrame = 0x00040000;
	private const int WsMaximizeBox = 0x00010000;
	private const int ResizeBorder = 9;
	private const int ResizeCorner = 18;

	private readonly AppPaths paths;

	private readonly AppLogger logger;

	private readonly WebView2 webView;

	private WebView2? eventsBrowserView;

	private readonly Label loadingLabel;

	private readonly PortraitReplacerService portraitReplacerService;

	private readonly FontChangerService fontChangerService;

	private readonly CouponService couponService;

	private readonly EventService eventService;

	private readonly UpdateCheckerService updateCheckerService;

	private MarketAnalyticsService? marketService;

	private readonly NotifyIcon trayIcon;

	private readonly Icon appIcon;

	private bool minimizeToTray = AppBehaviorSettings.Default.MinimizeToTray;

	private bool forceCloseFromTray;

	private int alarmPlayId;

	private ITaskbarList3? taskbarList;

	private Icon? taskbarBadgeIcon;

	private Icon? trayBadgeIcon;

	private int couponBadgeCount;

	private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		PropertyNameCaseInsensitive = true
	};

	public CalculatorForm(AppPaths paths, AppLogger logger)
	{
		this.paths = paths;
		this.logger = logger;
		portraitReplacerService = new PortraitReplacerService(paths);
		fontChangerService = new FontChangerService(paths);
		couponService = new CouponService(paths, logger);
		eventService = new EventService(paths, logger);
		updateCheckerService = new UpdateCheckerService(logger);
		Text = "BDO Multi-Tool";
		appIcon = (Icon?)System.Drawing.Icon.ExtractAssociatedIcon(Environment.ProcessPath)?.Clone() ?? SystemIcons.Application;
		base.Icon = appIcon;
		trayIcon = CreateTrayIcon();
		base.StartPosition = FormStartPosition.CenterScreen;
		MinimumSize = new Size(980, 680);
		base.Size = new Size(1400, 900);
		BackColor = Color.FromArgb(7, 17, 31);
		base.FormBorderStyle = FormBorderStyle.None;
		base.Padding = Padding.Empty;
		SetStyle(ControlStyles.ResizeRedraw, true);
		loadingLabel = new Label
		{
			Dock = DockStyle.Fill,
			Text = "Loading BDO Multi-Tool...",
			TextAlign = ContentAlignment.MiddleCenter,
			ForeColor = Color.FromArgb(36, 49, 63),
			BackColor = BackColor,
			Font = new Font("Segoe UI", 13f)
		};
		webView = new WebView2
		{
			Dock = DockStyle.Fill,
			Visible = false
		};
		base.Controls.Add(webView);
		base.Controls.Add(loadingLabel);
	}

	private NotifyIcon CreateTrayIcon()
	{
		ContextMenuStrip menu = new ContextMenuStrip();
		menu.Items.Add("Open BDO Multi-Tool", null, delegate
		{
			RestoreFromTray();
		});
		menu.Items.Add("Exit", null, delegate
		{
			forceCloseFromTray = true;
			TrySetTrayVisible(false);
			Close();
		});

		NotifyIcon icon = new NotifyIcon
		{
			Icon = appIcon,
			Text = "BDO Multi-Tool",
			ContextMenuStrip = menu,
			Visible = false
		};
		icon.DoubleClick += delegate
		{
			RestoreFromTray();
		};
		return icon;
	}

	private void RestoreFromTray()
	{
		try
		{
			TrySetTrayVisible(false);
			ShowInTaskbar = true;
			Show();
			if (WindowState == FormWindowState.Minimized)
				WindowState = FormWindowState.Normal;
			Activate();
			ApplyTaskbarCouponBadge(couponBadgeCount);
			PostEvent("updateCheckRequested", new { source = "trayRestore" });
		}
		catch (Exception ex)
		{
			logger.Error("Could not restore the app from the system tray.", ex);
		}
	}

	public void RestoreFromExternalLaunch()
	{
		if (InvokeRequired)
		{
			BeginInvoke(RestoreFromExternalLaunch);
			return;
		}

		RestoreFromTray();
	}

	private bool MinimizeToSystemTray()
	{
		try
		{
			if (WindowState == FormWindowState.Maximized)
			{
				WindowState = FormWindowState.Normal;
			}

			if (!TrySetTrayVisible(true))
			{
				WindowState = FormWindowState.Minimized;
				ShowInTaskbar = true;
				return false;
			}

			Hide();
			ShowInTaskbar = false;
			return true;
		}
		catch (Exception ex)
		{
			logger.Error("Could not minimize the app to the system tray.", ex);
			try
			{
				WindowState = FormWindowState.Minimized;
				ShowInTaskbar = true;
			}
			catch
			{
			}

			return false;
		}
	}

	private void ShowDesktopNotification(string title, string message)
	{
		string safeTitle = string.IsNullOrWhiteSpace(title) ? "BDO Multi-Tool" : title.Trim();
		string safeMessage = string.IsNullOrWhiteSpace(message) ? "Notification" : message.Trim();
		if (safeTitle.Length > 63)
		{
			safeTitle = safeTitle[..60] + "...";
		}
		if (safeMessage.Length > 255)
		{
			safeMessage = safeMessage[..252] + "...";
		}

		try
		{
			bool wasHidden = !trayIcon.Visible;
			trayIcon.BalloonTipTitle = safeTitle;
			trayIcon.BalloonTipText = safeMessage;
			trayIcon.BalloonTipIcon = ToolTipIcon.Info;
			if (!TrySetTrayVisible(true))
			{
				return;
			}
			trayIcon.ShowBalloonTip(8000);
			if (wasHidden && Visible)
			{
				System.Windows.Forms.Timer cleanupTimer = new System.Windows.Forms.Timer { Interval = 9000 };
				cleanupTimer.Tick += delegate
				{
					cleanupTimer.Stop();
					cleanupTimer.Dispose();
					if (Visible && ShowInTaskbar)
					{
						TrySetTrayVisible(false);
					}
				};
				cleanupTimer.Start();
			}
		}
		catch (Exception ex)
		{
			logger.Warn("Could not show desktop notification: " + ex.Message);
		}
	}

	private void SetCouponBadgeCount(int count)
	{
		if (InvokeRequired)
		{
			BeginInvoke(new Action<int>(SetCouponBadgeCount), count);
			return;
		}

		int safeCount = Math.Max(0, count);
		couponBadgeCount = safeCount;
		UpdateTrayCouponBadge(safeCount);
		ApplyTaskbarCouponBadge(safeCount);
	}

	private void ApplyTaskbarCouponBadge(int count)
	{
		if (InvokeRequired)
		{
			BeginInvoke(new Action<int>(ApplyTaskbarCouponBadge), count);
			return;
		}

		int safeCount = Math.Max(0, count);
		try
		{
			if (taskbarList == null)
			{
				object taskbarObject = new CTaskbarList();
				taskbarList = (ITaskbarList3)taskbarObject;
			}
			taskbarList.HrInit();
			taskbarBadgeIcon?.Dispose();
			taskbarBadgeIcon = null;
			if (safeCount <= 0)
			{
				taskbarList.SetOverlayIcon(Handle, IntPtr.Zero, string.Empty);
				return;
			}

			taskbarBadgeIcon = CreateCouponNumberBadgeIcon(Math.Min(safeCount, 99));
			taskbarList.SetOverlayIcon(Handle, taskbarBadgeIcon.Handle, safeCount == 1 ? "1 new coupon" : $"{safeCount} new coupons");
		}
		catch (Exception ex)
		{
			logger.Warn("Could not update taskbar coupon badge: " + ex.Message);
		}
	}

	private void UpdateTrayCouponBadge(int count)
	{
		try
		{
			Icon? previousBadge = trayBadgeIcon;
			trayBadgeIcon = null;
			if (count <= 0)
			{
				trayIcon.Icon = appIcon;
				trayIcon.Text = "BDO Multi-Tool";
				previousBadge?.Dispose();
				return;
			}

			trayBadgeIcon = CreateTrayIconWithCouponDot(appIcon);
			trayIcon.Icon = trayBadgeIcon;
			trayIcon.Text = count == 1 ? "BDO Multi-Tool - 1 new coupon" : "BDO Multi-Tool - new coupons available";
			previousBadge?.Dispose();
		}
		catch (Exception ex)
		{
			logger.Warn("Could not update system tray coupon badge: " + ex.Message);
		}
	}

	private bool TrySetTrayVisible(bool visible)
	{
		try
		{
			trayIcon.Visible = visible;
			return true;
		}
		catch (Exception ex)
		{
			logger.Warn("Could not change system tray visibility: " + ex.Message);
			return false;
		}
	}

	private static Icon CreateCouponNumberBadgeIcon(int count)
	{
		using Bitmap bitmap = new Bitmap(32, 32, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
		using Graphics graphics = Graphics.FromImage(bitmap);
		graphics.SmoothingMode = SmoothingMode.AntiAlias;
		graphics.Clear(Color.Transparent);
		using GraphicsPath shadowPath = new GraphicsPath();
		shadowPath.AddEllipse(2, 3, 27, 27);
		using PathGradientBrush shadow = new PathGradientBrush(shadowPath)
		{
			CenterColor = Color.FromArgb(175, 0, 0, 0),
			SurroundColors = new[] { Color.Transparent }
		};
		graphics.FillPath(shadow, shadowPath);
		using LinearGradientBrush fill = new LinearGradientBrush(new Rectangle(2, 1, 28, 28), Color.FromArgb(255, 79, 234, 117), Color.FromArgb(255, 245, 236, 65), LinearGradientMode.ForwardDiagonal);
		using Pen border = new Pen(Color.White, 2.2f);
		graphics.FillEllipse(fill, 2, 1, 28, 28);
		graphics.DrawEllipse(border, 2.5f, 1.5f, 27, 27);
		string text = count > 9 ? "9+" : count.ToString(System.Globalization.CultureInfo.InvariantCulture);
		using Font font = new Font("Segoe UI", text.Length > 1 ? 11.5f : 15f, FontStyle.Bold, GraphicsUnit.Pixel);
		using StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
		using Brush textShadow = new SolidBrush(Color.FromArgb(155, 0, 0, 0));
		using Brush textBrush = new SolidBrush(Color.FromArgb(20, 28, 16));
		RectangleF textRect = new RectangleF(2, 1, 28, 27);
		graphics.DrawString(text, font, textShadow, new RectangleF(textRect.X + 1, textRect.Y + 1, textRect.Width, textRect.Height), format);
		graphics.DrawString(text, font, textBrush, textRect, format);
		IntPtr handle = bitmap.GetHicon();
		try
		{
			return (Icon)Icon.FromHandle(handle).Clone();
		}
		finally
		{
			DestroyIcon(handle);
		}
	}

	private static Icon CreateTrayIconWithCouponDot(Icon baseIcon)
	{
		using Bitmap bitmap = new Bitmap(32, 32, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
		using Graphics graphics = Graphics.FromImage(bitmap);
		graphics.SmoothingMode = SmoothingMode.AntiAlias;
		graphics.Clear(Color.Transparent);
		graphics.DrawIcon(baseIcon, new Rectangle(0, 0, 32, 32));
		DrawCouponDot(graphics, new RectangleF(20f, 2f, 10f, 10f), 1.6f);
		IntPtr handle = bitmap.GetHicon();
		try
		{
			return (Icon)Icon.FromHandle(handle).Clone();
		}
		finally
		{
			DestroyIcon(handle);
		}
	}

	private static void DrawCouponDot(Graphics graphics, RectangleF bounds, float borderWidth)
	{
		using GraphicsPath shadowPath = new GraphicsPath();
		shadowPath.AddEllipse(bounds.X - 2f, bounds.Y + 1.5f, bounds.Width + 4f, bounds.Height + 4f);
		using PathGradientBrush shadow = new PathGradientBrush(shadowPath)
		{
			CenterColor = Color.FromArgb(135, 0, 0, 0),
			SurroundColors = new[] { Color.Transparent }
		};
		graphics.FillPath(shadow, shadowPath);
		using LinearGradientBrush fill = new LinearGradientBrush(Rectangle.Round(bounds), Color.FromArgb(255, 255, 242, 69), Color.FromArgb(255, 255, 170, 26), LinearGradientMode.ForwardDiagonal);
		using Pen border = new Pen(Color.White, borderWidth);
		graphics.FillEllipse(fill, bounds);
		graphics.DrawEllipse(border, bounds.X, bounds.Y, bounds.Width, bounds.Height);
	}

	private void PlayAlarmSound()
	{
		string alarmPath = Path.Combine(AppContext.BaseDirectory, "Assets", "Alarm.mp3");
		if (!File.Exists(alarmPath))
		{
			logger.Warn("Alarm.mp3 was not found at " + alarmPath);
			return;
		}

		string alias = "bdoAlarm" + Interlocked.Increment(ref alarmPlayId).ToString(System.Globalization.CultureInfo.InvariantCulture);
		string safePath = alarmPath.Replace("\"", "");
		mciSendString($"open \"{safePath}\" type mpegvideo alias {alias}", null, 0, IntPtr.Zero);
		mciSendString($"play {alias} notify", null, 0, IntPtr.Zero);
		System.Windows.Forms.Timer cleanupTimer = new System.Windows.Forms.Timer { Interval = 30000 };
		cleanupTimer.Tick += delegate
		{
			cleanupTimer.Stop();
			cleanupTimer.Dispose();
			mciSendString($"close {alias}", null, 0, IntPtr.Zero);
		};
		cleanupTimer.Start();
	}

	private void SpeakText(string text)
	{
		string safeText = string.IsNullOrWhiteSpace(text) ? "BDO Multi-Tool alert." : text.Trim();
		_ = Task.Run(delegate
		{
			try
			{
				Type? voiceType = Type.GetTypeFromProgID("SAPI.SpVoice");
				if (voiceType == null)
				{
					throw new InvalidOperationException("Windows text to speech is not available.");
				}
				dynamic voice = Activator.CreateInstance(voiceType)!;
				voice.Speak(safeText, 1);
			}
			catch (Exception ex)
			{
				logger.Error("Text to speech failed.", ex);
			}
		});
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams parameters = base.CreateParams;
			parameters.Style |= WsThickFrame | WsMaximizeBox;
			return parameters;
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);
		WindowChrome.ApplyDark(this);
		WindowChrome.ApplyRoundedCorners(this);
	}

	protected override void WndProc(ref Message m)
	{
		if (m.Msg == WmNcHitTest && base.WindowState == FormWindowState.Normal)
		{
			Point point = PointToClient(new Point(
				unchecked((short)((long)m.LParam & 0xffff)),
				unchecked((short)(((long)m.LParam >> 16) & 0xffff))));
			int border = Math.Max(ResizeBorder, (int)Math.Round(ResizeBorder * DeviceDpi / 96d));
			int corner = Math.Max(ResizeCorner, (int)Math.Round(ResizeCorner * DeviceDpi / 96d));
			bool left = point.X >= 0 && point.X <= border;
			bool right = point.X <= ClientSize.Width && point.X >= ClientSize.Width - border;
			bool top = point.Y >= 0 && point.Y <= border;
			bool bottom = point.Y <= ClientSize.Height && point.Y >= ClientSize.Height - border;

			if (point.X <= corner && point.Y <= corner)
				m.Result = (IntPtr)HtTopLeft;
			else if (point.X >= ClientSize.Width - corner && point.Y <= corner)
				m.Result = (IntPtr)HtTopRight;
			else if (point.X <= corner && point.Y >= ClientSize.Height - corner)
				m.Result = (IntPtr)HtBottomLeft;
			else if (point.X >= ClientSize.Width - corner && point.Y >= ClientSize.Height - corner)
				m.Result = (IntPtr)HtBottomRight;
			else if (left)
				m.Result = (IntPtr)HtLeft;
			else if (right)
				m.Result = (IntPtr)HtRight;
			else if (top)
				m.Result = (IntPtr)HtTop;
			else if (bottom)
				m.Result = (IntPtr)HtBottom;
			else
			{
				base.WndProc(ref m);
			}
			return;
		}
		base.WndProc(ref m);
	}

	protected override async void OnShown(EventArgs e)
	{
		base.OnShown(e);
		await InitializeAsync();
	}

	protected override void OnFormClosing(FormClosingEventArgs e)
	{
		if (!forceCloseFromTray && minimizeToTray && e.CloseReason == CloseReason.UserClosing)
		{
			e.Cancel = true;
			MinimizeToSystemTray();
			return;
		}
		base.OnFormClosing(e);
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		try { SetCouponBadgeCount(0); } catch { }
		try { marketService?.Dispose(); } catch { }
		try { couponService.Dispose(); } catch { }
		try { eventService.Dispose(); } catch { }
		try { updateCheckerService.Dispose(); } catch { }
		TrySetTrayVisible(false);
		try { trayIcon.Dispose(); } catch { }
		try { taskbarBadgeIcon?.Dispose(); } catch { }
		try { trayBadgeIcon?.Dispose(); } catch { }
		try { appIcon.Dispose(); } catch { }
		try { eventsBrowserView?.Dispose(); } catch { }
		try { webView.Dispose(); } catch { }
		base.OnFormClosed(e);
	}

	private async Task InitializeAsync()
	{
		_ = 2;
		try
		{
			MarketDatabase database = new MarketDatabase(paths.DatabasePath);
			minimizeToTray = (await AppBehaviorSettings.LoadAsync(paths, CancellationToken.None)).MinimizeToTray;
			BlackDesertMarketProvider provider = new BlackDesertMarketProvider(logger);
			marketService = new MarketAnalyticsService(database, provider, logger);
			await marketService.InitializeAsync(CancellationToken.None);
			marketService.DataChanged += delegate
			{
				PostEvent("dataChanged", null);
			};
			marketService.StatusChanged += delegate(object? _, string message)
			{
				PostEvent("status", new { message });
			};
			CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, paths.WebViewDataPath);
			await webView.EnsureCoreWebView2Async(environment);
			webView.CoreWebView2.Settings.AreDevToolsEnabled = false;
			webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
			webView.CoreWebView2.Settings.IsStatusBarEnabled = false;
			webView.CoreWebView2.NavigationStarting += delegate(object? _, CoreWebView2NavigationStartingEventArgs args)
			{
				if (!IsTrustedLocalUi(args.Uri))
				{
					args.Cancel = true;
				}
			};
			webView.CoreWebView2.NewWindowRequested += delegate(object? _, CoreWebView2NewWindowRequestedEventArgs args)
			{
				args.Handled = true;
			};
			webView.CoreWebView2.PermissionRequested += delegate(object? _, CoreWebView2PermissionRequestedEventArgs args)
			{
				args.State = CoreWebView2PermissionState.Deny;
			};
			webView.CoreWebView2.WebMessageReceived += OnWebMessageReceived;
			webView.CoreWebView2.DocumentTitleChanged += delegate
			{
				string documentTitle = webView.CoreWebView2.DocumentTitle;
				if (!string.IsNullOrWhiteSpace(documentTitle))
				{
					Text = documentTitle;
				}
			};
			webView.CoreWebView2.Navigate(new Uri(paths.HtmlPath).AbsoluteUri);
			webView.Visible = true;
			loadingLabel.Visible = false;
		}
		catch (Exception ex)
		{
			logger.Error("Application startup failed.", ex);
			ShowError("Could not start the calculator." + Environment.NewLine + Environment.NewLine + ex.Message);
		}
	}

	private async void OnWebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
	{
		string requestId = null;
		try
		{
			using JsonDocument document = JsonDocument.Parse(e.WebMessageAsJson);
			if (!IsTrustedLocalUi(e.Source))
			{
				throw new InvalidOperationException("The request did not originate from the local application interface.");
			}
			JsonElement rootElement = document.RootElement;
			requestId = rootElement.GetProperty("id").GetString();
			string command = rootElement.GetProperty("command").GetString() ?? "";
			JsonElement value;
			JsonElement payload = (rootElement.TryGetProperty("payload", out value) ? value : default(JsonElement));
			PostResponse(requestId, ok: true, await ExecuteCommandAsync(command, payload), null);
		}
		catch (Exception ex)
		{
			logger.Error("Market Analytics command failed.", ex);
			PostResponse(requestId, ok: false, null, ex.Message);
		}
	}

	private async Task<object?> ExecuteCommandAsync(string command, JsonElement payload)
	{
		switch (command)
		{
		case "windowMinimize":
			base.WindowState = FormWindowState.Minimized;
			return new
			{
				state = "minimized"
			};
		case "windowToggleMaximize":
			base.WindowState = ((base.WindowState != FormWindowState.Maximized) ? FormWindowState.Maximized : FormWindowState.Normal);
			return new
			{
				state = ((base.WindowState == FormWindowState.Maximized) ? "maximized" : "normal")
			};
		case "windowClose":
			BeginInvoke(new Action(Close));
			return new
			{
				state = "closing"
			};
		case "windowDrag":
			ReleaseCapture();
			SendMessage(base.Handle, 161, 2, 0);
			return new
			{
				state = "dragging"
			};
		case "openExternalUrl":
		{
			string url = payload.TryGetProperty("url", out JsonElement urlValue)
				? urlValue.GetString() ?? string.Empty
				: string.Empty;
			if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uri)
				|| uri.Scheme != Uri.UriSchemeHttps
				|| !IsAllowedExternalHost(uri.Host))
			{
				throw new InvalidOperationException("That external link is not allowed.");
			}
			Process.Start(new ProcessStartInfo(uri.AbsoluteUri)
			{
				UseShellExecute = true
			});
			return new { opened = true };
		}
		case "initializeCoupons":
			return await couponService.InitializeAsync(CancellationToken.None);
		case "refreshCoupons":
			return await couponService.RefreshAsync(CancellationToken.None);
		case "initializeEvents":
			return await LoadEventsWithBrowserFallbackAsync(forceRefresh: false);
		case "refreshEvents":
			return await LoadEventsWithBrowserFallbackAsync(forceRefresh: true);
		case "checkForUpdates":
			return await updateCheckerService.CheckAsync(CancellationToken.None);
		case "downloadAndInstallUpdate":
			return await DownloadAndLaunchUpdateInstallerAsync(CancellationToken.None);
		case "saveCouponSettings":
		{
			CouponSettings settings = JsonSerializer.Deserialize<CouponSettings>(payload.GetRawText(), JsonOptions)
				?? new CouponSettings(true, true, "", "all");
			return await couponService.SaveSettingsAsync(settings, CancellationToken.None);
		}
		case "exportCoupons":
			return await ExportCouponsAsync();
		case "setCouponBadgeCount":
		{
			int count = payload.TryGetProperty("count", out JsonElement countValue) && countValue.TryGetInt32(out int parsedCount)
				? parsedCount
				: 0;
			SetCouponBadgeCount(count);
			return new { count = Math.Max(0, count) };
		}
		case "getAppBehaviorSettings":
			return await AppBehaviorSettings.LoadAsync(paths, CancellationToken.None);
		case "saveAppBehaviorSettings":
		{
			JsonElement value;
			bool enabled = !payload.TryGetProperty("minimizeToTray", out value) || value.GetBoolean();
			AppBehaviorSettings settings = await AppBehaviorSettings.SaveAsync(paths, new AppBehaviorSettings(enabled), CancellationToken.None);
			minimizeToTray = settings.MinimizeToTray;
			return settings;
		}
		case "showDesktopNotification":
		{
			string title = payload.TryGetProperty("title", out JsonElement titleValue)
				? titleValue.GetString() ?? "BDO Multi-Tool"
				: "BDO Multi-Tool";
			string message = payload.TryGetProperty("message", out JsonElement messageValue)
				? messageValue.GetString() ?? string.Empty
				: string.Empty;
			ShowDesktopNotification(title, message);
			return new { shown = true };
		}
		case "playAlarmSound":
			PlayAlarmSound();
			return new { played = true };
		case "speakText":
		{
			string text = payload.TryGetProperty("text", out JsonElement textValue)
				? textValue.GetString() ?? string.Empty
				: string.Empty;
			SpeakText(text);
			return new { spoken = true };
		}
		default:
		{
			MarketAnalyticsService service = marketService ?? throw new InvalidOperationException("Market Analytics is not ready.");
			switch (command)
			{
			case "initialize":
			{
				MarketSettings settings = service.Settings;
				string providerName = service.ProviderName;
				return new
				{
					settings = settings,
					provider = providerName,
					items = await service.GetTrackedItemsAsync(CancellationToken.None)
				};
			}
			case "getRegionState":
			{
				JsonElement value8;
				string region = payload.TryGetProperty("region", out value8) ? value8.GetString() ?? service.Settings.Region : service.Settings.Region;
				return new
				{
					region = region,
					items = await service.GetTrackedItemsAsync(region, CancellationToken.None),
					outfits = await service.GetOutfitReportAsync(region, CancellationToken.None)
				};
			}
			case "search":
				return await service.SearchAsync(payload.GetProperty("query").GetString() ?? "", CancellationToken.None);
			case "getVariants":
				return await service.GetVariantsAsync(payload.GetProperty("itemId").GetInt64(), CancellationToken.None);
			case "addTracked":
			{
				MarketItem item = JsonSerializer.Deserialize<MarketItem>(payload.GetRawText(), JsonOptions) ?? throw new InvalidDataException("Invalid item selection.");
				await service.AddTrackedItemAsync(item, CancellationToken.None);
				return await service.GetTrackedItemsAsync(CancellationToken.None);
			}
			case "removeTracked":
				await service.RemoveTrackedItemAsync(payload.GetProperty("itemId").GetInt64(), payload.GetProperty("enhancement").GetInt32(), CancellationToken.None);
				return await service.GetTrackedItemsAsync(CancellationToken.None);
			case "getAnalytics":
			{
				JsonElement value9;
				JsonElement value12;
				string region = payload.TryGetProperty("region", out value12) ? value12.GetString() ?? service.Settings.Region : service.Settings.Region;
				return await service.GetAnalyticsAsync(payload.GetProperty("itemId").GetInt64(), payload.GetProperty("enhancement").GetInt32(), region, payload.TryGetProperty("days", out value9) ? value9.GetInt32() : 30, CancellationToken.None);
			}
			case "getOutfitReport":
			{
				JsonElement value10;
				string region = payload.TryGetProperty("region", out value10) ? value10.GetString() ?? service.Settings.Region : service.Settings.Region;
				return await service.GetOutfitReportAsync(region, CancellationToken.None);
			}
			case "refreshOutfits":
				await service.RefreshOutfitsAsync(600, CancellationToken.None);
			{
				JsonElement value11;
				string region = payload.TryGetProperty("region", out value11) ? value11.GetString() ?? service.Settings.Region : service.Settings.Region;
				return await service.GetOutfitReportAsync(region, CancellationToken.None);
			}
			case "refresh":
				await service.RefreshAllAsync(CancellationToken.None);
			{
				JsonElement value13;
				string region = payload.TryGetProperty("region", out value13) ? value13.GetString() ?? service.Settings.Region : service.Settings.Region;
				return new
				{
					region = region,
					items = await service.GetTrackedItemsAsync(region, CancellationToken.None),
					outfits = await service.GetOutfitReportAsync(region, CancellationToken.None)
				};
			}
			case "saveSettings":
			{
				await service.SaveSettingsAsync(payload.GetProperty("region").GetString() ?? "na", service.Settings.IntervalMinutes, CancellationToken.None);
				MarketSettings settings = service.Settings;
				return new
				{
					settings = settings,
					items = await service.GetTrackedItemsAsync(CancellationToken.None)
				};
			}
			case "exportCsv":
				return await ExportCsvAsync(service);
			case "getPortraitSettings":
				return await portraitReplacerService.GetSettingsAsync(CancellationToken.None);
			case "selectFaceTextureFolder":
				return await SelectFaceTextureFolderAsync(payload);
			case "selectOldPortrait":
				return SelectOldPortrait(payload);
			case "selectNewPortrait":
				return SelectNewPortrait(payload);
			case "previewPortrait":
			{
				JsonElement value5;
				JsonElement value6;
				JsonElement value7;
				JsonElement value8;
				return portraitReplacerService.DescribeImage(payload.GetProperty("newImagePath").GetString() ?? "", renderFinal: true, payload.TryGetProperty("cropMode", out value5) ? (value5.GetString() ?? "crop") : "crop", payload.TryGetProperty("cropX", out value6) ? value6.GetDouble() : 50.0, payload.TryGetProperty("cropY", out value7) ? value7.GetDouble() : 50.0, payload.TryGetProperty("zoom", out value8) ? value8.GetDouble() : 1.0);
			}
			case "replacePortrait":
			{
				JsonElement value;
				JsonElement value2;
				JsonElement value3;
				JsonElement value4;
				return await portraitReplacerService.ReplaceAsync(payload.GetProperty("faceTextureFolder").GetString() ?? "", payload.GetProperty("oldImagePath").GetString() ?? "", payload.GetProperty("newImagePath").GetString() ?? "", payload.TryGetProperty("cropMode", out value) ? (value.GetString() ?? "crop") : "crop", payload.TryGetProperty("cropX", out value2) ? value2.GetDouble() : 50.0, payload.TryGetProperty("cropY", out value3) ? value3.GetDouble() : 50.0, payload.TryGetProperty("zoom", out value4) ? value4.GetDouble() : 1.0, CancellationToken.None);
			}
			case "openPortraitBackupFolder":
				return portraitReplacerService.OpenBackupFolder(payload.GetProperty("faceTextureFolder").GetString() ?? "");
			case "restoreLastPortraitBackup":
				return await portraitReplacerService.RestoreLastBackupAsync(payload.GetProperty("faceTextureFolder").GetString() ?? "", payload.GetProperty("oldImagePath").GetString() ?? "", CancellationToken.None);
			case "getFontChangerSettings":
				return await fontChangerService.GetSettingsAsync(CancellationToken.None);
			case "getFontPresets":
				return fontChangerService.GetPresetGallery();
			case "selectBdoFolder":
				return await SelectBdoFolderAsync(payload);
			case "selectCustomFont":
				return SelectCustomFont(payload);
			case "applyPresetFont":
				return await fontChangerService.ApplyPresetAsync(payload.GetProperty("bdoFolder").GetString() ?? "", payload.GetProperty("presetId").GetString() ?? "", CancellationToken.None);
			case "applyCustomFont":
				return await fontChangerService.ApplyCustomAsync(payload.GetProperty("bdoFolder").GetString() ?? "", payload.GetProperty("fontPath").GetString() ?? "", CancellationToken.None);
			case "restoreLastFontBackup":
				return await fontChangerService.RestoreLastBackupAsync(payload.GetProperty("bdoFolder").GetString() ?? "", CancellationToken.None);
			case "removeCustomFont":
				return await fontChangerService.RemoveCustomFontAsync(payload.GetProperty("bdoFolder").GetString() ?? "", CancellationToken.None);
			case "openBdoFontFolder":
				return fontChangerService.OpenFontFolder(payload.GetProperty("bdoFolder").GetString() ?? "");
			default:
				throw new InvalidOperationException("Unknown command: " + command);
			}
		}
		}
	}

	private async Task<object> LoadEventsWithBrowserFallbackAsync(bool forceRefresh)
	{
		object dashboard = forceRefresh
			? await eventService.RefreshAsync(CancellationToken.None)
			: await eventService.InitializeAsync(CancellationToken.None);

		if (EventDashboardHasEvents(dashboard))
			return dashboard;

		try
		{
			logger.Info("Events browser fallback started.");
			string html = await ReadOfficialEventsHtmlWithBrowserAsync(CancellationToken.None);
			object refreshed = await eventService.RefreshFromRenderedHtmlAsync(html, CancellationToken.None);
			logger.Info("Events browser fallback succeeded.");
			return refreshed;
		}
		catch (Exception ex)
		{
			logger.Warn("Events browser fallback failed: " + ex.Message);
			return dashboard;
		}
	}

	private static bool EventDashboardHasEvents(object dashboard)
	{
		try
		{
			JsonElement json = JsonSerializer.SerializeToElement(dashboard, JsonOptions);
			return json.TryGetProperty("totalCount", out JsonElement total)
				&& total.ValueKind == JsonValueKind.Number
				&& total.GetInt32() > 0;
		}
		catch
		{
			return false;
		}
	}

	private async Task<string> ReadOfficialEventsHtmlWithBrowserAsync(CancellationToken cancellationToken)
	{
		using CancellationTokenSource timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
		timeout.CancelAfter(TimeSpan.FromSeconds(35));
		WebView2 browser = await EnsureEventsBrowserAsync(timeout.Token);
		await NavigateEventsBrowserAsync(browser, EventService.OfficialEventsUrl, timeout.Token);

		string latestHtml = "";
		for (int attempt = 0; attempt < 18; attempt++)
		{
			timeout.Token.ThrowIfCancellationRequested();
			latestHtml = await ReadBrowserHtmlAsync(browser);
			if (LooksLikeOfficialEventsPage(latestHtml))
				return latestHtml;
			await Task.Delay(1000, timeout.Token);
		}

		return latestHtml;
	}

	private async Task<WebView2> EnsureEventsBrowserAsync(CancellationToken cancellationToken)
	{
		if (eventsBrowserView is { IsDisposed: false, CoreWebView2: not null })
			return eventsBrowserView;

		eventsBrowserView?.Dispose();
		eventsBrowserView = new WebView2
		{
			Location = new Point(-32000, -32000),
			Size = new Size(1, 1),
			TabStop = false,
			Visible = true
		};
		Controls.Add(eventsBrowserView);
		eventsBrowserView.SendToBack();

		string userDataFolder = Path.Combine(paths.WebViewDataPath, "events-page");
		CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, userDataFolder);
		await eventsBrowserView.EnsureCoreWebView2Async(environment);
		eventsBrowserView.CoreWebView2.Settings.AreDevToolsEnabled = false;
		eventsBrowserView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
		eventsBrowserView.CoreWebView2.Settings.IsStatusBarEnabled = false;
		eventsBrowserView.CoreWebView2.NewWindowRequested += delegate(object? _, CoreWebView2NewWindowRequestedEventArgs args)
		{
			args.Handled = true;
		};
		cancellationToken.ThrowIfCancellationRequested();
		return eventsBrowserView;
	}

	private static async Task NavigateEventsBrowserAsync(WebView2 browser, string url, CancellationToken cancellationToken)
	{
		TaskCompletionSource navigation = new(TaskCreationOptions.RunContinuationsAsynchronously);
		void Handler(object? _, CoreWebView2NavigationCompletedEventArgs args)
		{
			if (args.IsSuccess)
				navigation.TrySetResult();
			else
				navigation.TrySetException(new InvalidDataException("The official Events page did not finish loading in the hidden browser."));
		}

		browser.CoreWebView2.NavigationCompleted += Handler;
		using CancellationTokenRegistration registration = cancellationToken.Register(() => navigation.TrySetCanceled(cancellationToken));
		try
		{
			browser.CoreWebView2.Navigate(url);
			await navigation.Task;
		}
		finally
		{
			browser.CoreWebView2.NavigationCompleted -= Handler;
		}
	}

	private static async Task<string> ReadBrowserHtmlAsync(WebView2 browser)
	{
		string json = await browser.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML");
		return JsonSerializer.Deserialize<string>(json, JsonOptions) ?? "";
	}

	private static bool LooksLikeOfficialEventsPage(string html)
	{
		if (string.IsNullOrWhiteSpace(html))
			return false;
		if (html.Contains("_Incapsula_Resource", StringComparison.OrdinalIgnoreCase)
			|| html.Contains("Incapsula incident", StringComparison.OrdinalIgnoreCase)
			|| html.Contains("Request unsuccessful", StringComparison.OrdinalIgnoreCase))
			return false;
		return html.Contains("groupContentNo=", StringComparison.OrdinalIgnoreCase)
			|| html.Contains("event_list", StringComparison.OrdinalIgnoreCase);
	}

	private async Task<object> DownloadAndLaunchUpdateInstallerAsync(CancellationToken cancellationToken)
	{
		UpdateCheckResult update = await updateCheckerService.CheckAsync(cancellationToken);
		if (!update.UpdateAvailable)
		{
			return new
			{
				started = false,
				latestVersion = update.LatestVersion,
				message = "You are on the latest version."
			};
		}

		if (!Uri.TryCreate(update.Url, UriKind.Absolute, out Uri? uri)
			|| uri.Scheme != Uri.UriSchemeHttps
			|| !IsAllowedUpdateDownloadHost(uri.Host))
		{
			throw new InvalidOperationException("The update download link is not allowed.");
		}

		if (!uri.AbsolutePath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
		{
			throw new InvalidOperationException("The latest release does not include a direct Windows installer download yet.");
		}

		string safeVersion = new string(update.LatestVersion.Where(ch => char.IsLetterOrDigit(ch) || ch is '.' or '-' or '_').ToArray());
		if (string.IsNullOrWhiteSpace(safeVersion))
			safeVersion = "latest";

		string directory = Path.Combine(Path.GetTempPath(), "BDO-Multi-Tool-Updates");
		Directory.CreateDirectory(directory);
		string installerPath = Path.Combine(directory, $"BDO-Multi-Tool-Installer-{safeVersion}.exe");

		using HttpClient client = new()
		{
			Timeout = TimeSpan.FromMinutes(2)
		};
		client.DefaultRequestHeaders.UserAgent.ParseAdd("BDO-Multi-Tool/" + AppVersion.Current);
		using HttpResponseMessage response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
		response.EnsureSuccessStatusCode();

		await using (Stream input = await response.Content.ReadAsStreamAsync(cancellationToken))
		await using (FileStream output = new FileStream(installerPath, FileMode.Create, FileAccess.Write, FileShare.None))
		{
			await input.CopyToAsync(output, cancellationToken);
		}

		FileInfo downloaded = new FileInfo(installerPath);
		if (!downloaded.Exists || downloaded.Length < 128 * 1024)
			throw new InvalidOperationException("Downloaded installer was incomplete.");

		Process.Start(new ProcessStartInfo(installerPath)
		{
			UseShellExecute = true
		});
		BeginInvoke(new Action(Close));

		return new
		{
			started = true,
			latestVersion = update.LatestVersion,
			installerPath
		};
	}

	private async Task<object> ExportCouponsAsync()
	{
		IReadOnlyList<CouponEntry> coupons = await couponService.GetCouponsAsync(CancellationToken.None);
		using SaveFileDialog dialog = new SaveFileDialog
		{
			Title = "Export coupon list",
			Filter = "CSV files (*.csv)|*.csv",
			FileName = $"bdo-coupons-{DateTime.Now:yyyy-MM-dd}.csv",
			OverwritePrompt = true
		};
		if (dialog.ShowDialog(this) != DialogResult.OK)
			return new { cancelled = true };
		StringBuilder csv = new StringBuilder();
		csv.AppendLine("Code,Added,Expiry,Status,Rewards,Source");
		foreach (CouponEntry coupon in coupons)
		{
			string rewards = string.Join("; ", coupon.Rewards.Select(reward => $"{reward.Quantity}x {reward.ItemName}"));
			csv.AppendLine(string.Join(",", Csv(coupon.Code), Csv(coupon.AddedText), Csv(coupon.ExpiryText), Csv(coupon.IsExpired ? "Expired" : "Available"), Csv(rewards), Csv(coupon.Source)));
		}
		await File.WriteAllTextAsync(dialog.FileName, csv.ToString(), new UTF8Encoding(true));
		return new { cancelled = false, path = dialog.FileName };
	}

	private static string Csv(string value) => "\"" + value.Replace("\"", "\"\"") + "\"";

	private static bool IsAllowedExternalHost(string host)
	{
		string[] allowedHosts =
		[
			"payment.naeu.playblackdesert.com",
			"www.naeu.playblackdesert.com",
			"blackdesert.pearlabyss.com",
			"github.com"
		];
		return allowedHosts.Any(x => x.Equals(host, StringComparison.OrdinalIgnoreCase));
	}

	private static bool IsAllowedUpdateDownloadHost(string host)
	{
		string[] allowedHosts =
		[
			"github.com",
			"objects.githubusercontent.com",
			"github-releases.githubusercontent.com"
		];
		return allowedHosts.Any(x => x.Equals(host, StringComparison.OrdinalIgnoreCase))
			|| host.EndsWith(".github.com", StringComparison.OrdinalIgnoreCase);
	}

	[DllImport("user32.dll")]
	private static extern bool ReleaseCapture();

	[DllImport("user32.dll")]
	private static extern nint SendMessage(nint hWnd, int msg, nint wParam, nint lParam);

	private async Task<object> SelectBdoFolderAsync(JsonElement payload)
	{
		JsonElement value;
		string text = (payload.TryGetProperty("currentPath", out value) ? (value.GetString() ?? "") : "");
		using FolderBrowserDialog dialog = new FolderBrowserDialog
		{
			Description = "Select the main Black Desert Online folder",
			UseDescriptionForTitle = true,
			ShowNewFolderButton = false,
			SelectedPath = (Directory.Exists(text) ? text : FontChangerService.DefaultBdoFolder)
		};
		if (dialog.ShowDialog(this) != DialogResult.OK)
		{
			return new
			{
				cancelled = true
			};
		}
		return new
		{
			cancelled = false,
			bdoFolder = (await fontChangerService.SaveBdoFolderAsync(dialog.SelectedPath, CancellationToken.None)).BdoFolder
		};
	}

	private object SelectCustomFont(JsonElement payload)
	{
		JsonElement value;
		string path = (payload.TryGetProperty("currentPath", out value) ? (value.GetString() ?? "") : "");
		using OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Title = "Choose a custom TrueType font",
			InitialDirectory = (File.Exists(path) ? Path.GetDirectoryName(path) : Environment.GetFolderPath(Environment.SpecialFolder.Personal)),
			Filter = "TrueType fonts (*.ttf)|*.ttf",
			CheckFileExists = true,
			Multiselect = false
		};
		if (openFileDialog.ShowDialog(this) != DialogResult.OK)
		{
			return new
			{
				cancelled = true
			};
		}
		return new
		{
			cancelled = false,
			font = fontChangerService.DescribeCustomFont(openFileDialog.FileName)
		};
	}

	private async Task<object> SelectFaceTextureFolderAsync(JsonElement payload)
	{
		JsonElement value;
		string text = (payload.TryGetProperty("currentPath", out value) ? (value.GetString() ?? "") : "");
		using FolderBrowserDialog dialog = new FolderBrowserDialog
		{
			Description = "Select the Black Desert Online FaceTexture folder",
			UseDescriptionForTitle = true,
			ShowNewFolderButton = false,
			SelectedPath = (Directory.Exists(text) ? text : PortraitReplacerService.DefaultFaceTextureFolder)
		};
		if (dialog.ShowDialog(this) != DialogResult.OK)
		{
			return new
			{
				cancelled = true
			};
		}
		return new
		{
			cancelled = false,
			faceTextureFolder = (await portraitReplacerService.SaveFaceTextureFolderAsync(dialog.SelectedPath, CancellationToken.None)).FaceTextureFolder
		};
	}

	private object SelectOldPortrait(JsonElement payload)
	{
		JsonElement value;
		string text = (payload.TryGetProperty("faceTextureFolder", out value) ? (value.GetString() ?? "") : "");
		if (!Directory.Exists(text))
		{
			throw new DirectoryNotFoundException("Select the BDO FaceTexture folder first.");
		}
		using OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Title = "Select the existing BDO portrait",
			InitialDirectory = text,
			Filter = "BDO bitmap portraits (*.bmp)|*.bmp",
			CheckFileExists = true,
			Multiselect = false
		};
		if (openFileDialog.ShowDialog(this) != DialogResult.OK)
		{
			return new
			{
				cancelled = true
			};
		}
		string directoryName = Path.GetDirectoryName(Path.GetFullPath(openFileDialog.FileName));
		string b = Path.GetFullPath(text).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		if (!string.Equals(directoryName, b, StringComparison.OrdinalIgnoreCase))
		{
			throw new InvalidDataException("Select the old portrait directly from the chosen FaceTexture folder.");
		}
		object image = portraitReplacerService.DescribeImage(openFileDialog.FileName, renderFinal: false);
		return new
		{
			cancelled = false,
			image = image
		};
	}

	private object SelectNewPortrait(JsonElement payload)
	{
		JsonElement value;
		string path = (payload.TryGetProperty("currentPath", out value) ? (value.GetString() ?? "") : "");
		using OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Title = "Select the new portrait image",
			InitialDirectory = (File.Exists(path) ? Path.GetDirectoryName(path) : Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)),
			Filter = "Supported images (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|PNG images (*.png)|*.png|JPEG images (*.jpg;*.jpeg)|*.jpg;*.jpeg|Bitmap images (*.bmp)|*.bmp",
			CheckFileExists = true,
			Multiselect = false
		};
		if (openFileDialog.ShowDialog(this) != DialogResult.OK)
		{
			return new
			{
				cancelled = true
			};
		}
		object image = portraitReplacerService.DescribeImage(openFileDialog.FileName, renderFinal: false);
		return new
		{
			cancelled = false,
			image = image
		};
	}

	private async Task<object> ExportCsvAsync(MarketAnalyticsService service)
	{
		using SaveFileDialog dialog = new SaveFileDialog
		{
			Title = "Export Market Analytics",
			Filter = "CSV files (*.csv)|*.csv",
			FileName = $"bdo-market-{service.Settings.Region}-{DateTime.Now:yyyyMMdd-HHmm}.csv",
			AddExtension = true,
			DefaultExt = "csv"
		};
		if (dialog.ShowDialog(this) != DialogResult.OK)
		{
			return new
			{
				cancelled = true
			};
		}
		await service.ExportCsvAsync(dialog.FileName, CancellationToken.None);
		return new
		{
			cancelled = false,
			path = dialog.FileName
		};
	}

	private void PostResponse(string? id, bool ok, object? data, string? error)
	{
		PostJson(new { id, ok, data, error });
	}

	private void PostEvent(string name, object? data)
	{
		PostJson(new
		{
			eventName = name,
			data = data
		});
	}

	private void PostJson(object value)
	{
		if (base.IsDisposed)
		{
			return;
		}
		if (base.InvokeRequired)
		{
			BeginInvoke(delegate
			{
				PostJson(value);
			});
		}
		else if (webView.CoreWebView2 != null)
		{
			webView.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(value, JsonOptions));
		}
	}

	private void ShowError(string message)
	{
		loadingLabel.Text = message;
		loadingLabel.Visible = true;
		webView.Visible = false;
	}

	private bool IsTrustedLocalUi(string value)
	{
		if (!Uri.TryCreate(value, UriKind.Absolute, out Uri result) || !result.IsFile)
		{
			return false;
		}
		return string.Equals(Path.GetFullPath(result.LocalPath), Path.GetFullPath(paths.HtmlPath), StringComparison.OrdinalIgnoreCase);
	}
}

