using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using Windows.Media.Ocr;
using Windows.Storage;
using Windows.Storage.Streams;

namespace BDOMultiTool;

internal sealed class CalculatorForm : Form
{
	private const string LocalAppHost = "app.bdo.local";
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

	private readonly GrindMarketPriceProvider grindMarketPriceProvider;

	private readonly NotifyIcon trayIcon;

	private readonly Icon appIcon;

	private readonly CancellationTokenSource lifetimeCancellation = new();

	private readonly ConcurrentDictionary<string, CancellationTokenSource> activeBridgeRequests = new(StringComparer.Ordinal);

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
		grindMarketPriceProvider = new GrindMarketPriceProvider(logger);
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
		lifetimeCancellation.Cancel();
		base.OnFormClosing(e);
	}

	protected override void OnFormClosed(FormClosedEventArgs e)
	{
		try { SetCouponBadgeCount(0); } catch { }
		try { marketService?.Dispose(); } catch { }
		try { grindMarketPriceProvider.Dispose(); } catch { }
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
		try { lifetimeCancellation.Dispose(); } catch { }
		base.OnFormClosed(e);
	}

	private async Task InitializeAsync()
	{
		_ = 2;
		try
		{
			CancellationToken cancellationToken = lifetimeCancellation.Token;
			MarketDatabase database = new MarketDatabase(paths.DatabasePath);
			minimizeToTray = (await AppBehaviorSettings.LoadAsync(paths, cancellationToken)).MinimizeToTray;
			BlackDesertMarketProvider provider = new BlackDesertMarketProvider(logger);
			marketService = new MarketAnalyticsService(database, provider, logger);
			await marketService.InitializeAsync(cancellationToken);
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
			webView.CoreWebView2.SetVirtualHostNameToFolderMapping(LocalAppHost, paths.Root, CoreWebView2HostResourceAccessKind.DenyCors);
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
			TaskCompletionSource<bool> navigationReady = new(TaskCreationOptions.RunContinuationsAsynchronously);
			void NavigationCompleted(object? _, CoreWebView2NavigationCompletedEventArgs args)
			{
				if (args.IsSuccess)
				{
					navigationReady.TrySetResult(true);
				}
				else
				{
					navigationReady.TrySetException(new InvalidOperationException($"The interface could not be loaded ({args.WebErrorStatus})."));
				}
			}
			webView.CoreWebView2.NavigationCompleted += NavigationCompleted;
			try
			{
				webView.CoreWebView2.Navigate($"https://{LocalAppHost}/{Path.GetFileName(paths.HtmlPath)}");
				await navigationReady.Task.WaitAsync(TimeSpan.FromSeconds(20), cancellationToken);
			}
			finally
			{
				webView.CoreWebView2.NavigationCompleted -= NavigationCompleted;
			}
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
		string? requestId = null;
		CancellationTokenSource? requestCancellation = null;
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
			if (string.Equals(command, "cancelRequest", StringComparison.Ordinal))
			{
				string targetId = payload.TryGetProperty("requestId", out JsonElement target) ? target.GetString() ?? string.Empty : string.Empty;
				if (activeBridgeRequests.TryGetValue(targetId, out CancellationTokenSource? active))
				{
					active.Cancel();
				}
				PostResponse(requestId, ok: true, new { cancelled = !string.IsNullOrWhiteSpace(targetId) }, null);
				return;
			}

			requestCancellation = CancellationTokenSource.CreateLinkedTokenSource(lifetimeCancellation.Token);
			requestCancellation.CancelAfter(GetCommandTimeout(command));
			if (string.IsNullOrWhiteSpace(requestId) || !activeBridgeRequests.TryAdd(requestId, requestCancellation))
			{
				throw new InvalidOperationException("The request identifier is invalid or already active.");
			}

			PostResponse(requestId, ok: true, await ExecuteCommandAsync(command, payload, requestCancellation.Token), null);
		}
		catch (OperationCanceledException)
		{
			PostResponse(requestId, ok: false, null, "The request timed out or was cancelled.");
		}
		catch (Exception ex)
		{
			logger.Error("Market Analytics command failed.", ex);
			PostResponse(requestId, ok: false, null, ex.Message);
		}
		finally
		{
			if (!string.IsNullOrWhiteSpace(requestId))
			{
				activeBridgeRequests.TryRemove(requestId, out _);
			}
			requestCancellation?.Dispose();
		}
	}

	private static TimeSpan GetCommandTimeout(string command)
	{
		return command switch
		{
			"selectGrindLootImage" or "scanGrindLootImage" => TimeSpan.FromMinutes(2),
			"downloadAndInstallUpdate" => TimeSpan.FromMinutes(10),
			"refreshEvents" or "initializeEvents" => TimeSpan.FromSeconds(75),
			_ => TimeSpan.FromSeconds(45)
		};
	}

	private async Task<object?> ExecuteCommandAsync(string command, JsonElement payload, CancellationToken cancellationToken)
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
			return await couponService.InitializeAsync(cancellationToken);
		case "refreshCoupons":
			return await couponService.RefreshAsync(cancellationToken);
		case "initializeEvents":
			return await LoadEventsWithBrowserFallbackAsync(forceRefresh: false, cancellationToken);
		case "refreshEvents":
			return await LoadEventsWithBrowserFallbackAsync(forceRefresh: true, cancellationToken);
		case "getAppVersion":
			return new { version = AppVersion.Current };
		case "checkForUpdates":
			return await updateCheckerService.CheckAsync(cancellationToken);
		case "downloadAndInstallUpdate":
			return await DownloadAndLaunchUpdateInstallerAsync(cancellationToken);
		case "saveCouponSettings":
		{
			CouponSettings settings = JsonSerializer.Deserialize<CouponSettings>(payload.GetRawText(), JsonOptions)
				?? new CouponSettings(true, true, "", "all");
			return await couponService.SaveSettingsAsync(settings, cancellationToken);
		}
		case "exportCoupons":
			return await ExportCouponsAsync(cancellationToken);
		case "setCouponBadgeCount":
		{
			int count = payload.TryGetProperty("count", out JsonElement countValue) && countValue.TryGetInt32(out int parsedCount)
				? parsedCount
				: 0;
			SetCouponBadgeCount(count);
			return new { count = Math.Max(0, count) };
		}
		case "getAppBehaviorSettings":
			return await AppBehaviorSettings.LoadAsync(paths, cancellationToken);
		case "saveAppBehaviorSettings":
		{
			JsonElement value;
			bool enabled = !payload.TryGetProperty("minimizeToTray", out value) || value.GetBoolean();
			AppBehaviorSettings settings = await AppBehaviorSettings.SaveAsync(paths, new AppBehaviorSettings(enabled), cancellationToken);
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
		case "selectGrindLootImage":
			return await SelectGrindLootImageAsync(payload, cancellationToken);
		case "scanGrindLootImage":
			return await ScanGrindLootImageAsync(payload, cancellationToken);
		case "speakText":
		{
			string text = payload.TryGetProperty("text", out JsonElement textValue)
				? textValue.GetString() ?? string.Empty
				: string.Empty;
			SpeakText(text);
			return new { spoken = true };
		}
		case "getGrindMarketPrices":
		{
			string region = payload.TryGetProperty("region", out JsonElement regionValue)
				? regionValue.GetString() ?? "eu"
				: "eu";
			List<long> itemIds = new();
			if (payload.TryGetProperty("itemIds", out JsonElement itemValues) && itemValues.ValueKind == JsonValueKind.Array)
			{
				foreach (JsonElement itemValue in itemValues.EnumerateArray())
				{
					long numericId;
					if (itemValue.ValueKind == JsonValueKind.Number && itemValue.TryGetInt64(out numericId))
					{
						itemIds.Add(numericId);
					}
					else if (itemValue.ValueKind == JsonValueKind.String && long.TryParse(itemValue.GetString(), out numericId))
					{
						itemIds.Add(numericId);
					}
				}
			}
			return await grindMarketPriceProvider.GetPricesAsync(itemIds, region, cancellationToken);
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
					items = await service.GetTrackedItemsAsync(cancellationToken)
				};
			}
			case "getRegionState":
			{
				JsonElement value8;
				string region = payload.TryGetProperty("region", out value8) ? value8.GetString() ?? service.Settings.Region : service.Settings.Region;
				return new
				{
					region = region,
					items = await service.GetTrackedItemsAsync(region, cancellationToken),
					outfits = await service.GetOutfitReportAsync(region, cancellationToken)
				};
			}
			case "search":
				return await service.SearchAsync(payload.GetProperty("query").GetString() ?? "", cancellationToken);
			case "getVariants":
				return await service.GetVariantsAsync(payload.GetProperty("itemId").GetInt64(), cancellationToken);
			case "addTracked":
			{
				MarketItem item = JsonSerializer.Deserialize<MarketItem>(payload.GetRawText(), JsonOptions) ?? throw new InvalidDataException("Invalid item selection.");
				await service.AddTrackedItemAsync(item, cancellationToken);
				return await service.GetTrackedItemsAsync(cancellationToken);
			}
			case "removeTracked":
				await service.RemoveTrackedItemAsync(payload.GetProperty("itemId").GetInt64(), payload.GetProperty("enhancement").GetInt32(), cancellationToken);
				return await service.GetTrackedItemsAsync(cancellationToken);
			case "getAnalytics":
			{
				JsonElement value9;
				JsonElement value12;
				string region = payload.TryGetProperty("region", out value12) ? value12.GetString() ?? service.Settings.Region : service.Settings.Region;
				return await service.GetAnalyticsAsync(payload.GetProperty("itemId").GetInt64(), payload.GetProperty("enhancement").GetInt32(), region, payload.TryGetProperty("days", out value9) ? value9.GetInt32() : 30, cancellationToken);
			}
			case "getOutfitReport":
			{
				JsonElement value10;
				string region = payload.TryGetProperty("region", out value10) ? value10.GetString() ?? service.Settings.Region : service.Settings.Region;
				return await service.GetOutfitReportAsync(region, cancellationToken);
			}
			case "refreshOutfits":
				await service.RefreshOutfitsAsync(MarketAnalyticsService.DefaultOutfitsPerScan, cancellationToken);
			{
				JsonElement value11;
				string region = payload.TryGetProperty("region", out value11) ? value11.GetString() ?? service.Settings.Region : service.Settings.Region;
				return await service.GetOutfitReportAsync(region, cancellationToken);
			}
			case "refresh":
				await service.RefreshAllAsync(cancellationToken);
			{
				JsonElement value13;
				string region = payload.TryGetProperty("region", out value13) ? value13.GetString() ?? service.Settings.Region : service.Settings.Region;
				return new
				{
					region = region,
					items = await service.GetTrackedItemsAsync(region, cancellationToken),
					outfits = await service.GetOutfitReportAsync(region, cancellationToken)
				};
			}
			case "saveSettings":
			{
				await service.SaveSettingsAsync("eu", service.Settings.IntervalMinutes, cancellationToken);
				MarketSettings settings = service.Settings;
				return new
				{
					settings = settings,
					items = await service.GetTrackedItemsAsync(cancellationToken)
				};
			}
			case "exportCsv":
				return await ExportCsvAsync(service, cancellationToken);
			case "getPortraitSettings":
				return await portraitReplacerService.GetSettingsAsync(cancellationToken);
			case "selectFaceTextureFolder":
				return await SelectFaceTextureFolderAsync(payload, cancellationToken);
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
				string imagePath = payload.GetProperty("newImagePath").GetString() ?? "";
				string cropMode = payload.TryGetProperty("cropMode", out value5) ? value5.GetString() ?? "crop" : "crop";
				double cropX = payload.TryGetProperty("cropX", out value6) ? value6.GetDouble() : 50.0;
				double cropY = payload.TryGetProperty("cropY", out value7) ? value7.GetDouble() : 50.0;
				double zoom = payload.TryGetProperty("zoom", out value8) ? value8.GetDouble() : 1.0;
				return await Task.Run(() => portraitReplacerService.DescribeImage(imagePath, renderFinal: true, cropMode, cropX, cropY, zoom), cancellationToken);
			}
			case "replacePortrait":
			{
				JsonElement value;
				JsonElement value2;
				JsonElement value3;
				JsonElement value4;
				return await portraitReplacerService.ReplaceAsync(payload.GetProperty("faceTextureFolder").GetString() ?? "", payload.GetProperty("oldImagePath").GetString() ?? "", payload.GetProperty("newImagePath").GetString() ?? "", payload.TryGetProperty("cropMode", out value) ? (value.GetString() ?? "crop") : "crop", payload.TryGetProperty("cropX", out value2) ? value2.GetDouble() : 50.0, payload.TryGetProperty("cropY", out value3) ? value3.GetDouble() : 50.0, payload.TryGetProperty("zoom", out value4) ? value4.GetDouble() : 1.0, cancellationToken);
			}
			case "openPortraitBackupFolder":
				return portraitReplacerService.OpenBackupFolder(payload.GetProperty("faceTextureFolder").GetString() ?? "");
			case "restoreLastPortraitBackup":
				return await portraitReplacerService.RestoreLastBackupAsync(payload.GetProperty("faceTextureFolder").GetString() ?? "", payload.GetProperty("oldImagePath").GetString() ?? "", cancellationToken);
			case "getFontChangerSettings":
				return await fontChangerService.GetSettingsAsync(cancellationToken);
			case "getFontPresets":
				return await Task.Run(fontChangerService.GetPresetGallery, cancellationToken);
			case "selectBdoFolder":
				return await SelectBdoFolderAsync(payload, cancellationToken);
			case "selectCustomFont":
				return await SelectCustomFontAsync(payload, cancellationToken);
			case "applyPresetFont":
				return await fontChangerService.ApplyPresetAsync(payload.GetProperty("bdoFolder").GetString() ?? "", payload.GetProperty("presetId").GetString() ?? "", cancellationToken);
			case "applyCustomFont":
				return await fontChangerService.ApplyCustomAsync(payload.GetProperty("bdoFolder").GetString() ?? "", payload.GetProperty("fontPath").GetString() ?? "", cancellationToken);
			case "restoreLastFontBackup":
				return await fontChangerService.RestoreLastBackupAsync(payload.GetProperty("bdoFolder").GetString() ?? "", cancellationToken);
			case "removeCustomFont":
				return await fontChangerService.RemoveCustomFontAsync(payload.GetProperty("bdoFolder").GetString() ?? "", cancellationToken);
			case "openBdoFontFolder":
				return fontChangerService.OpenFontFolder(payload.GetProperty("bdoFolder").GetString() ?? "");
			default:
				throw new InvalidOperationException("Unknown command: " + command);
			}
		}
		}
	}

	private async Task<object> LoadEventsWithBrowserFallbackAsync(bool forceRefresh, CancellationToken cancellationToken)
	{
		object dashboard = forceRefresh
			? await eventService.RefreshAsync(cancellationToken)
			: await eventService.InitializeAsync(cancellationToken);

		if (EventDashboardHasEvents(dashboard))
			return dashboard;

		try
		{
			logger.Info("Events browser fallback started.");
			string html = await ReadOfficialEventsHtmlWithBrowserAsync(cancellationToken);
			object refreshed = await eventService.RefreshFromRenderedHtmlAsync(html, cancellationToken);
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
		string partialInstallerPath = installerPath + ".download";

		try
		{
			File.Delete(partialInstallerPath);
			using HttpClient client = new()
			{
				Timeout = TimeSpan.FromMinutes(2)
			};
			client.DefaultRequestHeaders.UserAgent.ParseAdd("BDO-Multi-Tool/" + AppVersion.Current);
			using HttpResponseMessage response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
			response.EnsureSuccessStatusCode();
			long? expectedLength = response.Content.Headers.ContentLength;

			await using (Stream input = await response.Content.ReadAsStreamAsync(cancellationToken))
			await using (FileStream output = new FileStream(
				partialInstallerPath,
				FileMode.Create,
				FileAccess.Write,
				FileShare.None,
				81920,
				FileOptions.Asynchronous | FileOptions.SequentialScan))
			{
				await input.CopyToAsync(output, cancellationToken);
			}

			FileInfo downloaded = new FileInfo(partialInstallerPath);
			if (!downloaded.Exists || downloaded.Length < 128 * 1024)
				throw new InvalidOperationException("Downloaded installer was incomplete.");
			if (expectedLength.HasValue && downloaded.Length != expectedLength.Value)
				throw new InvalidOperationException("Downloaded installer size did not match the release asset.");

			if (!string.IsNullOrWhiteSpace(update.Sha256))
			{
				await using FileStream verificationStream = new FileStream(
					partialInstallerPath,
					FileMode.Open,
					FileAccess.Read,
					FileShare.Read,
					81920,
					FileOptions.Asynchronous | FileOptions.SequentialScan);
				string actualSha256 = Convert.ToHexString(await SHA256.HashDataAsync(verificationStream, cancellationToken));
				if (!string.Equals(actualSha256, update.Sha256, StringComparison.OrdinalIgnoreCase))
					throw new InvalidOperationException("Downloaded installer failed its SHA-256 integrity check.");
			}

			File.Move(partialInstallerPath, installerPath, overwrite: true);
		}
		catch
		{
			File.Delete(partialInstallerPath);
			throw;
		}

		ProcessStartInfo installerStart = new ProcessStartInfo(installerPath)
		{
			UseShellExecute = true,
			WorkingDirectory = directory
		};
		installerStart.ArgumentList.Add("--install-path");
		installerStart.ArgumentList.Add(AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
		installerStart.ArgumentList.Add("--source-pid");
		installerStart.ArgumentList.Add(Environment.ProcessId.ToString());
		Process.Start(installerStart);
		BeginInvoke(new Action(() =>
		{
			forceCloseFromTray = true;
			TrySetTrayVisible(false);
			Application.Exit();
		}));

		return new
		{
			started = true,
			latestVersion = update.LatestVersion,
			installerPath,
			integrityVerified = !string.IsNullOrWhiteSpace(update.Sha256)
		};
	}

	private async Task<object> ExportCouponsAsync(CancellationToken cancellationToken)
	{
		IReadOnlyList<CouponEntry> coupons = await couponService.GetCouponsAsync(cancellationToken);
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
			"ko-fi.com",
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

	private async Task<object> SelectBdoFolderAsync(JsonElement payload, CancellationToken cancellationToken)
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
			bdoFolder = (await fontChangerService.SaveBdoFolderAsync(dialog.SelectedPath, cancellationToken)).BdoFolder
		};
	}

	private async Task<object> SelectCustomFontAsync(JsonElement payload, CancellationToken cancellationToken)
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
		object font = await Task.Run(() => fontChangerService.DescribeCustomFont(openFileDialog.FileName), cancellationToken);
		return new
		{
			cancelled = false,
			font
		};
	}

	private async Task<object> SelectFaceTextureFolderAsync(JsonElement payload, CancellationToken cancellationToken)
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
			faceTextureFolder = (await portraitReplacerService.SaveFaceTextureFolderAsync(dialog.SelectedPath, cancellationToken)).FaceTextureFolder
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

	private async Task<object> ExportCsvAsync(MarketAnalyticsService service, CancellationToken cancellationToken)
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
		await service.ExportCsvAsync(dialog.FileName, cancellationToken);
		return new
		{
			cancelled = false,
			path = dialog.FileName
		};
	}

	private async Task<object> SelectGrindLootImageAsync(JsonElement payload, CancellationToken cancellationToken)
	{
		List<GrindScanDrop> drops = ReadGrindScanDrops(payload);
		using OpenFileDialog dialog = new OpenFileDialog
		{
			Title = "Select loot screenshot",
			Filter = "Supported images (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|PNG images (*.png)|*.png|JPEG images (*.jpg;*.jpeg)|*.jpg;*.jpeg|Bitmap images (*.bmp)|*.bmp",
			CheckFileExists = true,
			Multiselect = false
		};
		if (dialog.ShowDialog(this) != DialogResult.OK)
		{
			return new { cancelled = true };
		}

		string fileName = Path.GetFileName(dialog.FileName);
		return await BuildGrindLootImageScanResponseAsync(fileName, dialog.FileName, dialog.FileName, drops, cancellationToken);
	}

	private async Task<object> ScanGrindLootImageAsync(JsonElement payload, CancellationToken cancellationToken)
	{
		List<GrindScanDrop> drops = ReadGrindScanDrops(payload);
		string fileName = payload.TryGetProperty("fileName", out JsonElement fileNameValue)
			? fileNameValue.GetString() ?? "Loot screenshot"
			: "Loot screenshot";
		string dataUrl = payload.TryGetProperty("dataUrl", out JsonElement dataUrlValue)
			? dataUrlValue.GetString() ?? string.Empty
			: string.Empty;
		if (string.IsNullOrWhiteSpace(dataUrl))
		{
			throw new InvalidOperationException("No image data was provided.");
		}

		byte[] bytes = DecodeDataUrl(dataUrl);
		string extension = Path.GetExtension(fileName);
		if (string.IsNullOrWhiteSpace(extension) || extension.Length > 8)
		{
			extension = ".png";
		}
		string tempPath = Path.Combine(Path.GetTempPath(), $"bdo-grind-loot-{Guid.NewGuid():N}{extension}");
		await File.WriteAllBytesAsync(tempPath, bytes, cancellationToken);
		try
		{
			return await BuildGrindLootImageScanResponseAsync(fileName, tempPath, null, drops, cancellationToken);
		}
		finally
		{
			try { File.Delete(tempPath); } catch { }
		}
	}

	private async Task<object> BuildGrindLootImageScanResponseAsync(string fileName, string imagePath, string? sourcePath, IReadOnlyList<GrindScanDrop> drops, CancellationToken cancellationToken)
	{
		Task<string> previewTask = Task.Run(() => CreateImagePreviewDataUrlAsync(imagePath), cancellationToken);
		Task<string> textTask = TryReadImageTextAsync(imagePath);
		Task<List<GrindLootImageMatch>> scanTask = Task.Run(() => ScanGrindLootImageForDropsAsync(imagePath, drops, cancellationToken), cancellationToken);
		await Task.WhenAll(previewTask, textTask, scanTask);
		string dataUrl = await previewTask;
		string screenshotText = await textTask;
		List<GrindLootImageMatch> matches = await scanTask;
		return new
		{
			cancelled = false,
			fileName,
			path = sourcePath ?? string.Empty,
			dataUrl,
			screenshotText,
			screenshotTextAvailable = !string.IsNullOrWhiteSpace(screenshotText),
			matches
		};
	}

	private static byte[] DecodeDataUrl(string dataUrl)
	{
		int comma = dataUrl.IndexOf(',');
		string payload = comma >= 0 ? dataUrl[(comma + 1)..] : dataUrl;
		return Convert.FromBase64String(payload);
	}

	private List<GrindScanDrop> ReadGrindScanDrops(JsonElement payload)
	{
		List<GrindScanDrop> drops = new();
		if (!payload.TryGetProperty("drops", out JsonElement dropValues) || dropValues.ValueKind != JsonValueKind.Array)
		{
			return drops;
		}

		foreach (JsonElement dropValue in dropValues.EnumerateArray())
		{
			string id = dropValue.TryGetProperty("id", out JsonElement idValue)
				? idValue.ToString()
				: string.Empty;
			string name = dropValue.TryGetProperty("name", out JsonElement nameValue)
				? nameValue.GetString() ?? string.Empty
				: string.Empty;
			string icon = dropValue.TryGetProperty("icon", out JsonElement iconValue)
				? iconValue.GetString() ?? string.Empty
				: string.Empty;
			string iconPath = ResolveGrindIconPath(icon);
			if (!string.IsNullOrWhiteSpace(id) && File.Exists(iconPath))
			{
				drops.Add(new GrindScanDrop(id, name, iconPath));
			}
		}
		return drops;
	}

	private string ResolveGrindIconPath(string icon)
	{
		if (string.IsNullOrWhiteSpace(icon))
		{
			return string.Empty;
		}
		if (Path.IsPathFullyQualified(icon))
		{
			return icon;
		}
		return Path.Combine(paths.Root, icon.Replace('/', Path.DirectorySeparatorChar));
	}

	private static async Task<string> TryReadImageTextAsync(string imagePath)
	{
		try
		{
			OcrEngine? engine = OcrEngine.TryCreateFromUserProfileLanguages()
				?? OcrEngine.TryCreateFromLanguage(new Language("en-US"));
			if (engine is null)
			{
				return string.Empty;
			}

			StorageFile file = await StorageFile.GetFileFromPathAsync(imagePath);
			using IRandomAccessStream stream = await file.OpenReadAsync();
			BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
			BitmapTransform transform = new BitmapTransform();
			uint maxDimension = OcrEngine.MaxImageDimension;
			if (decoder.PixelWidth > maxDimension || decoder.PixelHeight > maxDimension)
			{
				double scale = Math.Min((double)maxDimension / Math.Max(1, decoder.PixelWidth), (double)maxDimension / Math.Max(1, decoder.PixelHeight));
				transform.ScaledWidth = Math.Max(1, (uint)Math.Round(decoder.PixelWidth * scale));
				transform.ScaledHeight = Math.Max(1, (uint)Math.Round(decoder.PixelHeight * scale));
			}

			using SoftwareBitmap bitmap = await decoder.GetSoftwareBitmapAsync(
				BitmapPixelFormat.Bgra8,
				BitmapAlphaMode.Premultiplied,
				transform,
				ExifOrientationMode.IgnoreExifOrientation,
				ColorManagementMode.DoNotColorManage);
			OcrResult result = await engine.RecognizeAsync(bitmap);
			return string.Join(Environment.NewLine, result.Lines.Select(line => line.Text));
		}
		catch
		{
			return string.Empty;
		}
	}

	private static async Task<string> CreateImagePreviewDataUrlAsync(string imagePath)
	{
		using Bitmap source = await LoadBitmapAsync(imagePath);
		const int maxWidth = 900;
		const int maxHeight = 520;
		double scale = Math.Min(1.0, Math.Min((double)maxWidth / Math.Max(1, source.Width), (double)maxHeight / Math.Max(1, source.Height)));
		int width = Math.Max(1, (int)Math.Round(source.Width * scale));
		int height = Math.Max(1, (int)Math.Round(source.Height * scale));
		using Bitmap preview = new Bitmap(width, height);
		using (Graphics graphics = Graphics.FromImage(preview))
		{
			graphics.Clear(Color.Transparent);
			graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
			graphics.DrawImage(source, new Rectangle(0, 0, width, height));
		}
		using MemoryStream stream = new MemoryStream();
		preview.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
		return "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());
	}

	private static async Task<List<GrindLootImageMatch>> ScanGrindLootImageForDropsAsync(string imagePath, IReadOnlyList<GrindScanDrop> drops, CancellationToken cancellationToken)
	{
		List<GrindLootImageMatch> matches = new();
		if (drops.Count == 0)
		{
			return matches;
		}

		using Bitmap screenshot = await LoadBitmapAsync(imagePath);
		List<LoadedGrindScanDrop> icons = new();
		foreach (GrindScanDrop drop in drops)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				using Bitmap bitmap = await LoadBitmapAsync(drop.IconPath);
				icons.Add(new LoadedGrindScanDrop(drop, ResizeBitmap(bitmap, 40)));
			}
			catch
			{
			}
		}

		try
		{
			Dictionary<string, GrindLootImageMatch> byId = new(StringComparer.OrdinalIgnoreCase);
			foreach (Rectangle candidate in FindInventoryIconCandidates(screenshot, cancellationToken))
			{
				cancellationToken.ThrowIfCancellationRequested();
				using Bitmap crop = screenshot.Clone(candidate, screenshot.PixelFormat);
				GrindIconMatch match = FindBestGrindDropMatch(crop, icons, cancellationToken);
				if (!IsConfidentGrindDropMatch(match))
				{
					continue;
				}
				LoadedGrindScanDrop drop = match.Drop!;

				int count = ReadInventoryStackCount(crop);
				if (count <= 0)
				{
					count = 1;
				}

				if (byId.TryGetValue(drop.Drop.Id, out GrindLootImageMatch existing))
				{
					byId[drop.Drop.Id] = existing with
					{
						Count = existing.Count + count,
						Score = Math.Max(existing.Score, match.Score.Composite)
					};
				}
				else
				{
					byId[drop.Drop.Id] = new GrindLootImageMatch(drop.Drop.Id, drop.Drop.Name, count, Math.Round(match.Score.Composite, 4));
				}
			}
			matches.AddRange(byId.Values.OrderBy(match => match.Name, StringComparer.OrdinalIgnoreCase));
			return matches;
		}
		finally
		{
			foreach (LoadedGrindScanDrop icon in icons)
			{
				icon.Bitmap.Dispose();
			}
		}
	}

	private static GrindIconMatch FindBestGrindDropMatch(Bitmap candidate, IReadOnlyList<LoadedGrindScanDrop> icons, CancellationToken cancellationToken)
	{
		LoadedGrindScanDrop? best = null;
		LoadedGrindScanDrop? runnerUpDrop = null;
		GrindIconScore bestScore = GrindIconScore.Empty;
		GrindIconScore runnerUp = GrindIconScore.Empty;
		foreach (LoadedGrindScanDrop icon in icons)
		{
			cancellationToken.ThrowIfCancellationRequested();
			GrindIconScore score = ScoreIconMatch(candidate, icon.Bitmap);
			if (score.Composite > bestScore.Composite)
			{
				runnerUpDrop = best;
				runnerUp = bestScore;
				bestScore = score;
				best = icon;
			}
			else if (score.Composite > runnerUp.Composite)
			{
				runnerUpDrop = icon;
				runnerUp = score;
			}
		}
		if (best is not null
			&& runnerUpDrop is not null
			&& string.Equals(best.Drop.Name, "Black Stone", StringComparison.OrdinalIgnoreCase)
			&& !string.Equals(runnerUpDrop.Drop.Name, "Black Stone", StringComparison.OrdinalIgnoreCase)
			&& runnerUp.Composite > 0.79
			&& bestScore.Composite - runnerUp.Composite < 0.025)
		{
			return new GrindIconMatch(runnerUpDrop, runnerUp, bestScore);
		}
		return new GrindIconMatch(best, bestScore, runnerUp);
	}

	private static bool IsConfidentGrindDropMatch(GrindIconMatch match)
	{
		if (match.Drop is null)
		{
			return false;
		}

		GrindIconScore score = match.Score;
		double lead = score.Composite - match.RunnerUp.Composite;
		if (score.Rgb < 0.80 || score.Composite < 0.80)
		{
			return false;
		}
		if (score.Hue < 0.50 && score.Rgb < 0.88)
		{
			return false;
		}
		if (score.Edge < 0.68 && score.Rgb < 0.88)
		{
			return false;
		}
		if (score.Shape < 0.40 && score.Rgb < 0.88)
		{
			return false;
		}
		return score.Composite >= 0.84 || lead >= 0.03;
	}

	private static GrindIconScore ScoreIconMatch(Bitmap candidate, Bitmap icon)
	{
		GrindIconScore best = GrindIconScore.Empty;
		for (int inset = 0; inset <= 6; inset += 2)
		{
			using Bitmap cropped = CropCenteredSquare(candidate, inset);
			GrindIconScore score = ScoreIconMatchAtSize(cropped, icon);
			if (score.Composite > best.Composite)
			{
				best = score;
			}
		}
		return best;
	}

	private static GrindIconScore ScoreIconMatchAtSize(Bitmap candidate, Bitmap icon)
	{
		using Bitmap a = ResizeBitmap(candidate, 40);
		Bitmap b = icon;
		double diff = 0;
		double weight = 0;
		double edgeDiff = 0;
		double edgeWeight = 0;
		double shapeIntersection = 0;
		double shapeUnion = 0;
		double[] candidateHue = new double[18];
		double[] iconHue = new double[18];
		for (int y = 0; y < 40; y++)
		{
			for (int x = 0; x < 40; x++)
			{
				Color ca = a.GetPixel(x, y);
				Color cb = b.GetPixel(x, y);
				if (x > 23 && y > 23 && IsWhiteCountPixel(ca))
				{
					continue;
				}
				double brightnessA = (ca.R + ca.G + ca.B) / 3.0;
				double brightnessB = (cb.R + cb.G + cb.B) / 3.0;
				if (brightnessA < 8 && brightnessB < 8)
				{
					continue;
				}

				double saturationA = Math.Max(ca.R, Math.Max(ca.G, ca.B)) - Math.Min(ca.R, Math.Min(ca.G, ca.B));
				double saturationB = Math.Max(cb.R, Math.Max(cb.G, cb.B)) - Math.Min(cb.R, Math.Min(cb.G, cb.B));
				double localWeight = 1 + ((saturationA + saturationB) / 160.0);
				if (x < 7 || y < 7 || x > 34 || y > 34)
				{
					localWeight *= 0.55;
				}
				diff += localWeight * (Math.Abs(ca.R - cb.R) + Math.Abs(ca.G - cb.G) + Math.Abs(ca.B - cb.B)) / (3.0 * 255.0);
				weight += localWeight;

				bool candidateShape = brightnessA > 26 || saturationA > 22;
				bool iconShape = brightnessB > 26 || saturationB > 22;
				if (candidateShape || iconShape)
				{
					shapeUnion++;
					if (candidateShape && iconShape)
					{
						shapeIntersection++;
					}
				}
				if (brightnessA > 28 && saturationA > 30)
				{
					candidateHue[Math.Clamp((int)(ca.GetHue() / 20), 0, candidateHue.Length - 1)] += localWeight;
				}
				if (brightnessB > 28 && saturationB > 30)
				{
					iconHue[Math.Clamp((int)(cb.GetHue() / 20), 0, iconHue.Length - 1)] += localWeight;
				}
			}
		}

		for (int y = 1; y < 39; y++)
		{
			for (int x = 1; x < 39; x++)
			{
				double candidateEdge = EdgeStrength(a, x, y);
				double iconEdge = EdgeStrength(b, x, y);
				if (candidateEdge < 10 && iconEdge < 10)
				{
					continue;
				}
				double localWeight = 1 + Math.Min(2, (candidateEdge + iconEdge) / 130.0);
				edgeDiff += localWeight * Math.Abs(candidateEdge - iconEdge) / 255.0;
				edgeWeight += localWeight;
			}
		}

		double rgb = weight <= 0 ? 0 : 1 - (diff / weight);
		double hue = HistogramIntersection(candidateHue, iconHue);
		double edge = edgeWeight <= 0 ? 0 : 1 - (edgeDiff / edgeWeight);
		double shape = shapeUnion <= 0 ? 0 : shapeIntersection / shapeUnion;
		double composite = (rgb * 0.58) + (hue * 0.18) + (edge * 0.16) + (shape * 0.08);
		return new GrindIconScore(rgb, hue, edge, shape, composite);
	}

	private static double HistogramIntersection(double[] left, double[] right)
	{
		double leftTotal = left.Sum();
		double rightTotal = right.Sum();
		if (leftTotal <= 0 || rightTotal <= 0)
		{
			return 0;
		}

		double overlap = 0;
		for (int index = 0; index < left.Length && index < right.Length; index++)
		{
			overlap += Math.Min(left[index] / leftTotal, right[index] / rightTotal);
		}
		return overlap;
	}

	private static double EdgeStrength(Bitmap bitmap, int x, int y)
	{
		double gx = PixelBrightness(bitmap.GetPixel(x + 1, y)) - PixelBrightness(bitmap.GetPixel(x - 1, y));
		double gy = PixelBrightness(bitmap.GetPixel(x, y + 1)) - PixelBrightness(bitmap.GetPixel(x, y - 1));
		return Math.Min(255, Math.Sqrt((gx * gx) + (gy * gy)));
	}

	private static double PixelBrightness(Color color)
	{
		return (color.R + color.G + color.B) / 3.0;
	}

	private static unsafe List<Rectangle> FindInventoryIconCandidates(Bitmap image, CancellationToken cancellationToken)
	{
		int width = image.Width;
		int height = image.Height;
		using Bitmap pixels = new(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
		using (Graphics graphics = Graphics.FromImage(pixels))
		{
			graphics.DrawImageUnscaled(image, 0, 0);
		}

		bool[] foreground = new bool[width * height];
		System.Drawing.Imaging.BitmapData data = pixels.LockBits(
			new Rectangle(0, 0, width, height),
			System.Drawing.Imaging.ImageLockMode.ReadOnly,
			System.Drawing.Imaging.PixelFormat.Format32bppArgb);
		try
		{
			for (int y = 0; y < height; y++)
			{
				cancellationToken.ThrowIfCancellationRequested();
				byte* row = (byte*)data.Scan0 + (y * data.Stride);
				int rowOffset = y * width;
				for (int x = 0; x < width; x++)
				{
					byte* pixel = row + (x * 4);
					foreground[rowOffset + x] = IsInventoryForegroundPixel(pixel[2], pixel[1], pixel[0]);
				}
			}
		}
		finally
		{
			pixels.UnlockBits(data);
		}

		bool[] mask = new bool[foreground.Length];
		for (int y = 0; y < height; y++)
		{
			cancellationToken.ThrowIfCancellationRequested();
			for (int x = 0; x < width; x++)
			{
				if (!foreground[(y * width) + x])
				{
					continue;
				}
				for (int yy = Math.Max(0, y - 3); yy <= Math.Min(height - 1, y + 3); yy++)
				{
					int offset = yy * width;
					for (int xx = Math.Max(0, x - 3); xx <= Math.Min(width - 1, x + 3); xx++)
					{
						mask[offset + xx] = true;
					}
				}
			}
		}

		bool[] seen = new bool[mask.Length];
		List<Rectangle> candidates = new();
		int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
		int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
		Queue<int> queue = new();
		for (int y = 0; y < height; y++)
		{
			cancellationToken.ThrowIfCancellationRequested();
			for (int x = 0; x < width; x++)
			{
				int start = (y * width) + x;
				if (!mask[start] || seen[start])
				{
					continue;
				}

				queue.Clear();
				queue.Enqueue(start);
				seen[start] = true;
				int minX = x;
				int maxX = x;
				int minY = y;
				int maxY = y;
				int count = 0;
				while (queue.Count > 0)
				{
					int point = queue.Dequeue();
					int pointX = point % width;
					int pointY = point / width;
					count++;
					minX = Math.Min(minX, pointX);
					maxX = Math.Max(maxX, pointX);
					minY = Math.Min(minY, pointY);
					maxY = Math.Max(maxY, pointY);
					for (int index = 0; index < dx.Length; index++)
					{
						int nextX = pointX + dx[index];
						int nextY = pointY + dy[index];
						if (nextX < 0 || nextY < 0 || nextX >= width || nextY >= height)
						{
							continue;
						}
						int next = (nextY * width) + nextX;
						if (mask[next] && !seen[next])
						{
							seen[next] = true;
							queue.Enqueue(next);
						}
					}
				}

				Rectangle bounds = Rectangle.FromLTRB(minX, minY, maxX + 1, maxY + 1);
				if (count > 50 && bounds.Width >= 12 && bounds.Height >= 12 && bounds.Width <= 80 && bounds.Height <= 80)
				{
					candidates.Add(bounds);
				}
			}
		}
		return candidates;
	}

	private static int ReadInventoryStackCount(Bitmap crop)
	{
		List<DigitComponent> components = FindCountDigitComponents(crop);
		if (components.Count == 0)
		{
			return 1;
		}

		string text = string.Concat(components.Select(component => GuessInventoryDigit(component.Mask)));
		if (text.Length == 0 || text.Any(ch => !char.IsDigit(ch)))
		{
			return 1;
		}
		return int.TryParse(text, out int parsed) ? Math.Max(1, parsed) : 1;
	}

	private static List<DigitComponent> FindCountDigitComponents(Bitmap crop)
	{
		List<DigitComponent> raw = FindRawDigitComponents(crop);
		if (raw.Count == 0)
		{
			return raw;
		}

		DigitComponent rightmost = raw.OrderByDescending(component => component.Bounds.Right).First();
		if (rightmost.Bounds.Right < crop.Width - 8)
		{
			return new List<DigitComponent>();
		}

		List<DigitComponent> cluster = new() { rightmost };
		int left = rightmost.Bounds.Left;
		while (true)
		{
			DigitComponent? next = raw
				.Where(component => component.Bounds.Right <= left
					&& left - component.Bounds.Right <= 4
					&& Math.Abs(component.Bounds.Bottom - rightmost.Bounds.Bottom) <= 2
					&& Math.Abs(component.Bounds.Top - rightmost.Bounds.Top) <= 6)
				.OrderByDescending(component => component.Bounds.Right)
				.FirstOrDefault();
			if (next is null)
			{
				break;
			}
			cluster.Add(next);
			left = next.Bounds.Left;
		}
		return cluster.OrderBy(component => component.Bounds.Left).ToList();
	}

	private static List<DigitComponent> FindRawDigitComponents(Bitmap crop)
	{
		int width = crop.Width;
		int height = crop.Height;
		bool[,] mask = new bool[width, height];
		for (int y = 22; y < Math.Min(height, 42); y++)
		{
			for (int x = 8; x < width; x++)
			{
				if (IsWhiteCountPixel(crop.GetPixel(x, y)))
				{
					mask[x, y] = true;
				}
			}
		}

		bool[,] seen = new bool[width, height];
		List<DigitComponent> components = new();
		int[] dx = { -1, 0, 1, -1, 1, -1, 0, 1 };
		int[] dy = { -1, -1, -1, 0, 0, 1, 1, 1 };
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				if (!mask[x, y] || seen[x, y])
				{
					continue;
				}

				Queue<Point> queue = new();
				List<Point> points = new();
				queue.Enqueue(new Point(x, y));
				seen[x, y] = true;
				int minX = x;
				int maxX = x;
				int minY = y;
				int maxY = y;
				while (queue.Count > 0)
				{
					Point point = queue.Dequeue();
					points.Add(point);
					minX = Math.Min(minX, point.X);
					maxX = Math.Max(maxX, point.X);
					minY = Math.Min(minY, point.Y);
					maxY = Math.Max(maxY, point.Y);
					for (int index = 0; index < dx.Length; index++)
					{
						int nextX = point.X + dx[index];
						int nextY = point.Y + dy[index];
						if (nextX >= 0 && nextY >= 0 && nextX < width && nextY < height && mask[nextX, nextY] && !seen[nextX, nextY])
						{
							seen[nextX, nextY] = true;
							queue.Enqueue(new Point(nextX, nextY));
						}
					}
				}

				Rectangle bounds = Rectangle.FromLTRB(minX, minY, maxX + 1, maxY + 1);
				if (points.Count < 3 || bounds.Height < 5 || bounds.Width < 1 || bounds.Width > 10)
				{
					continue;
				}

				bool[,] componentMask = new bool[bounds.Width, bounds.Height];
				foreach (Point point in points)
				{
					componentMask[point.X - bounds.X, point.Y - bounds.Y] = true;
				}
				components.Add(new DigitComponent(bounds, componentMask));
			}
		}
		return components.OrderBy(component => component.Bounds.Left).ToList();
	}

	private static char GuessInventoryDigit(bool[,] mask)
	{
		int width = mask.GetLength(0);
		int height = mask.GetLength(1);
		double top = Occupancy(mask, 1, 0, width - 1, 2);
		double middle = Occupancy(mask, 1, (height / 2) - 1, width - 1, (height / 2) + 2);
		double bottom = Occupancy(mask, 1, height - 2, width - 1, height);
		double upperLeft = Occupancy(mask, 0, 1, 2, height / 2);
		double upperRight = Occupancy(mask, width - 2, 1, width, height / 2);
		double lowerLeft = Occupancy(mask, 0, height / 2, 2, height - 1);
		double lowerRight = Occupancy(mask, width - 2, height / 2, width, height - 1);
		double center = Occupancy(mask, width / 3, height / 3, Math.Max((width / 3) + 1, (2 * width) / 3), Math.Max((height / 3) + 1, (2 * height) / 3));
		bool t = top > 0.28;
		bool m = middle > 0.20;
		bool b = bottom > 0.28;
		bool ul = upperLeft > 0.18;
		bool ur = upperRight > 0.18;
		bool ll = lowerLeft > 0.18;
		bool lr = lowerRight > 0.18;

		if (width <= 3 || (width <= 5 && center > 0.3 && upperLeft < 0.15 && upperRight < 0.35 && lowerLeft < 0.15 && lowerRight < 0.35))
		{
			return '1';
		}
		if (t && b && ul && ur && ll && lr && !m)
		{
			return '0';
		}
		if (t && m && b && ul && ll && upperRight < 0.40)
		{
			return '6';
		}
		if (t && m && b && ul && ur && lr && !ll)
		{
			return '9';
		}
		if (t && m && b && ur && lr && !ul && !ll)
		{
			return '3';
		}
		if (t && m && b && ul && lr && !ur && !ll)
		{
			return '5';
		}
		if (t && m && b && ur && ll && !ul && !lr)
		{
			return '2';
		}
		if (t && ur && !ul && !ll && lowerRight < 0.25)
		{
			return '7';
		}
		if (m && ul && ur && lr && !ll && !t && !b)
		{
			return '4';
		}
		if (t && m && b && ul && ur && ll && lr)
		{
			return '8';
		}
		return '?';
	}

	private static double Occupancy(bool[,] mask, int x1, int y1, int x2, int y2)
	{
		int width = mask.GetLength(0);
		int height = mask.GetLength(1);
		x1 = Math.Clamp(x1, 0, width);
		x2 = Math.Clamp(x2, 0, width);
		y1 = Math.Clamp(y1, 0, height);
		y2 = Math.Clamp(y2, 0, height);
		int total = 0;
		int filled = 0;
		for (int y = y1; y < y2; y++)
		{
			for (int x = x1; x < x2; x++)
			{
				total++;
				if (mask[x, y])
				{
					filled++;
				}
			}
		}
		return total == 0 ? 0 : (double)filled / total;
	}

	private static bool IsInventoryForegroundPixel(Color color)
	{
		return IsInventoryForegroundPixel(color.R, color.G, color.B);
	}

	private static bool IsInventoryForegroundPixel(byte red, byte green, byte blue)
	{
		int max = Math.Max(red, Math.Max(green, blue));
		int min = Math.Min(red, Math.Min(green, blue));
		double brightness = (red + green + blue) / 3.0;
		return (max - min > 35 && brightness > 35) || (brightness > 105 && max - min < 95);
	}

	private static bool IsWhiteCountPixel(Color color)
	{
		int max = Math.Max(color.R, Math.Max(color.G, color.B));
		int min = Math.Min(color.R, Math.Min(color.G, color.B));
		double brightness = (color.R + color.G + color.B) / 3.0;
		return brightness > 115 && max - min < 75;
	}

	private static Bitmap CropCenteredSquare(Bitmap source, int inset)
	{
		int size = Math.Max(1, Math.Min(source.Width, source.Height) - (2 * inset));
		int x = Math.Max(0, (source.Width - size) / 2);
		int y = Math.Max(0, (source.Height - size) / 2);
		Bitmap bitmap = new Bitmap(size, size);
		using Graphics graphics = Graphics.FromImage(bitmap);
		graphics.Clear(Color.Black);
		graphics.DrawImage(source, new Rectangle(0, 0, size, size), new Rectangle(x, y, size, size), GraphicsUnit.Pixel);
		return bitmap;
	}

	private static Bitmap ResizeBitmap(Bitmap source, int size)
	{
		Bitmap bitmap = new Bitmap(size, size);
		using Graphics graphics = Graphics.FromImage(bitmap);
		graphics.Clear(Color.Black);
		graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
		graphics.SmoothingMode = SmoothingMode.HighQuality;
		graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
		graphics.DrawImage(source, new Rectangle(0, 0, size, size));
		return bitmap;
	}

	private static async Task<Bitmap> LoadBitmapAsync(string imagePath)
	{
		try
		{
			using Image image = Image.FromFile(imagePath);
			return new Bitmap(image);
		}
		catch
		{
			StorageFile file = await StorageFile.GetFileFromPathAsync(imagePath);
			using IRandomAccessStream stream = await file.OpenReadAsync();
			BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
			PixelDataProvider pixels = await decoder.GetPixelDataAsync(
				BitmapPixelFormat.Bgra8,
				BitmapAlphaMode.Premultiplied,
				new BitmapTransform(),
				ExifOrientationMode.IgnoreExifOrientation,
				ColorManagementMode.DoNotColorManage);
			byte[] bytes = pixels.DetachPixelData();
			Bitmap bitmap = new Bitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
			System.Drawing.Imaging.BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmap.PixelFormat);
			try
			{
				Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
			}
			finally
			{
				bitmap.UnlockBits(data);
			}
			return bitmap;
		}
	}

	private sealed record GrindScanDrop(string Id, string Name, string IconPath);

	private sealed record LoadedGrindScanDrop(GrindScanDrop Drop, Bitmap Bitmap);

	private sealed record GrindIconMatch(LoadedGrindScanDrop? Drop, GrindIconScore Score, GrindIconScore RunnerUp);

	private readonly record struct GrindIconScore(double Rgb, double Hue, double Edge, double Shape, double Composite)
	{
		public static GrindIconScore Empty { get; } = new(0, 0, 0, 0, 0);
	}

	private sealed record GrindLootImageMatch(string Id, string Name, int Count, double Score);

	private sealed record DigitComponent(Rectangle Bounds, bool[,] Mask);

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
		if (!Uri.TryCreate(value, UriKind.Absolute, out Uri result))
		{
			return false;
		}
		if (result.Scheme == Uri.UriSchemeHttps && string.Equals(result.Host, LocalAppHost, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		if (!result.IsFile)
		{
			return false;
		}
		string localPath = Path.GetFullPath(result.LocalPath);
		string htmlPath = Path.GetFullPath(paths.HtmlPath);
		if (string.Equals(localPath, htmlPath, StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}

		string appRoot = Path.GetFullPath(paths.Root).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
		return localPath.StartsWith(appRoot, StringComparison.OrdinalIgnoreCase);
	}
}


