/*
 Залишилось:
2. Рефакторінг: ParentControlService -> щось типу WindowsUserAgent -> в github
2. Конфігураційний файл:
	- урл
	- частота запитів
3. Закінчити/розібратись із класом ProjectInstaller
4. Інсталяція
	- Покласти файли в папку
	- Установити сервіс
	- Створити реакції (net start/stop {сервіс} або аналог powershell) на user log-in, user log-off
5. Добавити ще вимикання комп'ютера
6. Добавити ще логування
 */

using System;
using System.Runtime.Remoting.Channels;
using System.ServiceProcess;
using System.Timers;

namespace ParentControlService
{
	public partial class ParentControlService : ServiceBase
	{
		readonly Timer timer = new Timer();

		public ParentControlService()
		{
			InitializeComponent();
			//eventLog = new EventLog();
			//if (!EventLog.SourceExists("ParentControlService"))
			//	EventLog.CreateEventSource("ParentControlService", "");
			//eventLog.Source = "ParentControlService";
			//eventLog.Log = "";

			timer.Elapsed += OnTimer;
		}

		protected override void OnStart(string[] args)
		{
			//eventLog.WriteEntry("Service started");

			timer.Interval = 30000;
			timer.Start();
		}

		protected override void OnStop()
		{
			//eventLog.WriteEntry("Service stopped");

			timer.Stop();
		}

		public void OnTimer(object sender, ElapsedEventArgs args)
		{
			//eventLog.WriteEntry("Monitoring children games", EventLogEntryType.Information);

			if (UserScreen.IsLocked()) return;

			var userStatus = Helper.GetUserStatus(Environment.UserName); //, (int)timer.Interval/1000);
			switch (userStatus.Action)
			{
				case WhatDoWithUser.ShowMessage:
					UserScreen.ShowMessage(userStatus.Message);
					break;
				case WhatDoWithUser.LockScreen:
					UserScreen.Lock();
					break;
				case WhatDoWithUser.LogOffUser:
					UserScreen.LogOff();
					break;
				default:
					UserScreen.ShowMessage(userStatus.Message);
					UserScreen.Lock();
					break;
			}
		}

		//private void eventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
		//{

		//}
	}
}
