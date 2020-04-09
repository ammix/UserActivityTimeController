/*
 Епоха імперій (процес age2_x1) та браузер Хром (процес chrome) повинні працювати не більше ?? годин
 */

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameStopper
{
	class Program
	{
		static void Main(string[] args)
		{
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
