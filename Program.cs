using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Linq.Expressions;

namespace GameStopper
{
	static class OptionsParser
	{
		public static IEnumerable<DateTime[]> Parse(string forbidTime)
		{
			var forbidTimeList = forbidTime.Split(',').Select(x => x.Trim());

			var forbidTimeFrames = new List<DateTime[]>();
			foreach (var timeFrame in forbidTimeList)
			{
				var times = timeFrame.Split('-').Select(DateTime.Parse).ToArray();
				forbidTimeFrames.Add(new[] { times[0], times[1] });
			}

			return forbidTimeFrames;
		}
	}

	class GameStopper
	{
		Dictionary<DateTime, Dictionary<string, TimeSpan>> processesHistory;

		public GameStopper()
		{
			processesHistory = new Dictionary<DateTime, Dictionary<string, TimeSpan>>();
		}

		public TimeSpan UpdateProcessesTime(string[] processes) //HowLongProcessesIsRunToday Get...
		{
			var today = DateTime.Today;
			if (!processesHistory.Keys.Contains(today.Date))
				processesHistory.Add(today.Date, new Dictionary<string, TimeSpan>());



			//var now = DateTime.Now;
			var processesToControl = processes.Select(process => Process.GetProcessesByName(process)[0]);

			foreach (var processToControl in processesToControl)
			{
				var processHistoryForToday = processesHistory[DateTime.Today];

				if (!processHistoryForToday.Keys.Contains(processToControl.ProcessName))
					processHistoryForToday.Add(processToControl.ProcessName, new TimeSpan());
				else
				{
					var alreadyTime = processHistoryForToday[processToControl.ProcessName];
					processHistoryForToday[processToControl.ProcessName] = DateTime.Now - processToControl.StartTime; <-------------
				}

			}


			processesToControl.Select(x => processesHistory[DateTime.Today].Add x.StartTime - DateTime.Now);


			var q = processesToControl.First().StartTime - DateTime.Now;
			processesHistory[DateTime.Now].Add();

			//foreach (var process in processes)
			//{
			//	processUnderControl = Process.GetProcessesByName(process);
			//}



			return new TimeSpan();
		}
	}

	class Program
	{
		static bool IsTimeInForbidFrame(IEnumerable<DateTime[]> forbidTimeFrames, DateTime time)
		{
			foreach (var forbidTimeFrame in forbidTimeFrames)
			{
				if (forbidTimeFrame[0] < time && time < forbidTimeFrame[1])
					return true;
			}

			return false;
		}

		static void Main(string[] args)
		{
			var processes = ConfigurationManager.AppSettings["ProcessesToStop"].Split(',').Select(x => x.Trim());
			var quotaPerDay = int.Parse(ConfigurationManager.AppSettings["QuotaPerDay"].Replace("h", ""));
			var forbidTime = OptionsParser.Parse(ConfigurationManager.AppSettings["ForbidTime"]);
			var stopFlag = bool.Parse(ConfigurationManager.AppSettings["StopImmediately"]);


			


			string gameProcessName = "age2_x1";
			TimeSpan eachDayTimeLimit = new TimeSpan(0, 3, 0, 0);
			TimeSpan checkTimeRhythm = new TimeSpan(0, 0, 1, 0); // 1 per minute

			//string gameProcessName = "Calculator";
			//TimeSpan eachDayTimeLimit = new TimeSpan(0, 0, 4, 0);
			//TimeSpan checkTimeRhythm = new TimeSpan(0, 0, 0, 30);


			DateTime currentDay = DateTime.Today;
			DateTime previousDay = currentDay;
			TimeSpan timeInPlayToday = eachDayTimeLimit;

			while (true)
			{
				currentDay = DateTime.Today;
				if (currentDay != previousDay)
				{
					timeInPlayToday = eachDayTimeLimit;
					Console.WriteLine($"New play day started: {currentDay}");
				}

				Thread.Sleep(checkTimeRhythm);

				var gameProcesses = Process.GetProcessesByName(gameProcessName);
				if (gameProcesses.Length != 0)
				{
					Console.WriteLine($"For game {gameProcessName} left to play: {timeInPlayToday}");
					timeInPlayToday = timeInPlayToday.Subtract(checkTimeRhythm);

					if (timeInPlayToday.TotalMinutes > 1 && timeInPlayToday.TotalMinutes < 3)
					//if (timeInPlayToday.TotalSeconds < 60 && timeInPlayToday.TotalSeconds > 49)
						Task.Factory.StartNew(ShowAlert);

					if (timeInPlayToday.Ticks <= 0)
						gameProcesses[0].Kill();
				}
				else
					Console.WriteLine($"Game {gameProcessName} is not running");

				previousDay = currentDay;
			}
		}

		static void ShowAlert()
		{
			string message = "Серафім, ти граєш в Епоху Імперій вже 3 години. Через 3 хвилини гра буде зупинена. Збережи гру і виходь.";
			string caption = "ПОПЕРЕДЖЕННЯ !!!";

			SendKeys.SendWait("%{Tab}");
			using (var dummy = new Form() { TopMost = true })
			{
				MessageBox.Show(dummy, message, caption);
			}
		}
	}
}
