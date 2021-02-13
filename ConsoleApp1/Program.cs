using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;

namespace ConsoleApp1
{
	public struct UserStatus
	{
		public WhatDoWithUser Action;
		public string Message;
	}

	public enum WhatDoWithUser
	{
		ShowMessage,
		LockScreen,
		LogOffUser
	}

	class UserScreen
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

		public static int ShowMessage(string text)
		{
			return MessageBox(IntPtr.Zero, text, "Попередження", 0);
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			//var q = UserScreen.ShowMessage("Зберігай гру. Час за комп'ютером на сьогодні закінчився.");

			//var request = WebRequest.Create("http://bookkeeping.somee.com/parentcontrolsettings1");

			//var response = request.GetResponse().GetResponseStream();

			//var streamReader = new StreamReader(response);
			//var content = streamReader.ReadToEnd();
			//streamReader.Close();


			//RestClient client = new RestClient("http://bookkeeping.somee.com/parentcontrolsettings1");
			//RestRequest request = new RestRequest(Method.GET);
			//var response = client.Execute(request);

			//var w = client.Execute<dynamic>(request);

			//var json = JsonConvert.DeserializeObject<dynamic>("");

			//response.Content.ParseJsonDate();

			var restClient = new RestClient("http://bookkeeping.somee.com/parentcontrolsettings1");
			var q = restClient.Get<UserStatus>(new RestRequest(Method.GET));
			

			Console.WriteLine(q.Data);

			//Console.ReadKey();

			//UserScreen.Lock();
			//Thread.Sleep(2000);



			//string userName = Environment.UserName;
			//Console.WriteLine(userName);
			//Console.WriteLine(UserScreen.IsLocked());

			Console.ReadKey();
		}
	}
}
