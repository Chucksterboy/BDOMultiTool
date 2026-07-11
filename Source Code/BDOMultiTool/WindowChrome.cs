using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace BDOMultiTool;

internal static class WindowChrome
{
	private const int UseImmersiveDarkMode = 20;

	private const int UseImmersiveDarkModeLegacy = 19;

	private const int BorderColor = 34;

	private const int CaptionColor = 35;

	private const int TextColor = 36;

	private const int WindowCornerPreference = 33;

	public static void ApplyDark(Form form)
	{
		if (OperatingSystem.IsWindows())
		{
			int value = 1;
			if (DwmSetWindowAttribute(form.Handle, 20, ref value, 4) != 0)
			{
				DwmSetWindowAttribute(form.Handle, 19, ref value, 4);
			}
			int value2 = ColorTranslator.ToWin32(Color.FromArgb(7, 17, 31));
			int value3 = ColorTranslator.ToWin32(Color.FromArgb(18, 43, 61));
			int value4 = ColorTranslator.ToWin32(Color.FromArgb(237, 246, 255));
			DwmSetWindowAttribute(form.Handle, 35, ref value2, 4);
			DwmSetWindowAttribute(form.Handle, 34, ref value3, 4);
			DwmSetWindowAttribute(form.Handle, 36, ref value4, 4);
		}
	}

	public static void ApplyRoundedCorners(Form form)
	{
		if (OperatingSystem.IsWindows())
		{
			int value = 2;
			DwmSetWindowAttribute(form.Handle, 33, ref value, 4);
		}
	}

	[DllImport("dwmapi.dll")]
	private static extern int DwmSetWindowAttribute(nint hwnd, int attribute, ref int value, int valueSize);
}

