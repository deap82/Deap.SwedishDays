using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Deap.SwedishDays;

namespace SwedishDaysConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			SwedishDayInfo[] redDays2013 = SwedishDaysAPI.RequestHolidays(new DateTime(2013, 08, 15), new DateTime(2014, 06, 15));
			foreach (SwedishDayInfo day in redDays2013)
			{
				Console.WriteLine(day.Date);
				Console.WriteLine("Helgdag: " + day.Holiday);
				Console.WriteLine("Veckodag: " + day.Date.DayOfWeek);
				Console.WriteLine();
			}
			Console.ReadKey();
		}
	}
}
