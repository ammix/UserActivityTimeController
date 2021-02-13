using System;

namespace ParentControlService
{
	static class UserScreen
	{
		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern void LockWorkStation();

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern int OpenDesktopA(string lpszDesktop, int dwFlags, bool fInherit, int dwDesiredAccess);

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern int SwitchDesktop(int hDesktop);

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern int ExitWindowsEx(int uFlags, int dwReason);

		[System.Runtime.InteropServices.DllImport("user32.dll")]
		private static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, int uType);

		public static void Lock()
		{
			LockWorkStation();
		}

		public static bool IsLocked()
		{
			var hDesktop = OpenDesktopA("default", 0, false, 0x0100);
			var screenIsLocked = SwitchDesktop(hDesktop);

			return screenIsLocked == 0;
		}

		public static void LogOff()
		{
			ExitWindowsEx(0, 0);
		}

		public static void ShowMessage(string text)
		{
			MessageBox(IntPtr.Zero, text, "Попередження", 0);
		}
	}
}
