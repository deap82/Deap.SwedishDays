using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Deap.SwedishDays
{
	public static class SwedishDaysAPI
	{	
		/// <summary>
		/// Returns an array of SwedishDayInfo objects for all dates within the given year that has a "helgdag" description.
		/// </summary>
		public static SwedishDayInfo[] RequestHolidays(int year)
		{
			return RequestHolidays(new DateTime(year, 1, 1), new DateTime(year, 12, 31));
		}

		/// <summary>
		/// Returns an array of SwedishDayInfo objects for all dates within the given month that has a "helgdag" description.
		/// </summary>
		public static SwedishDayInfo[] RequestHolidays(int year, int month)
		{
			int daysInMonth = DateTime.DaysInMonth(year, month);
			return RequestHolidays(new DateTime(year, month, 1), new DateTime(year, month, daysInMonth));
		}

		/// <summary>
		/// Returns an array of SwedishDayInfo objects for all dates within the given timespan that has a "helgdag" description.
		/// </summary>
		public static SwedishDayInfo[] RequestHolidays(DateTime startDate, DateTime endDate)
		{
			List<SwedishDaysResponse> dayRequests = new List<SwedishDaysResponse>();
			if (startDate.Year == endDate.Year 
				&& startDate.Month == 1 && startDate.Day == 1 
				&& endDate.Month == 12 && endDate.Day == 31)
			{
				dayRequests.Add(Request<SwedishDaysResponse>(startDate.Year));
			}
			else if (startDate.Year == endDate.Year
				&& startDate.Month == endDate.Month)
			{
				dayRequests.Add(Request<SwedishDaysResponse>(startDate.Year, startDate.Month));
			}
			else
			{
				DateTime date = startDate;
				while (date <= endDate)
				{
					dayRequests.Add(Request<SwedishDaysResponse>(date.Year, date.Month));
					date = date.AddMonths(1);
				}
			}
			
			List<SwedishDayInfo> result = new List<SwedishDayInfo>();
			foreach (SwedishDaysResponse dayRequest in dayRequests)
			{
				foreach (DateTime date in dayRequest.Days.Keys)
				{
					SwedishDayInfo day = dayRequest.Days[date];
					if (day.IsHoliday && startDate <= date && endDate >= date)
					{
						result.Add(day);
					}
				}
			}

			return result.ToArray();
		}

		/// <summary>
		/// Returns a full API response for dates within the period based on the input parameters.
		/// Only give the year to get all dates for a year. Add month to get that month. Add day to just get that date. 
		/// </summary>
		public static SwedishDaysResponse Request(int year, int? month = null, int? day = null)
		{
			return Request<SwedishDaysResponse>(year, month, day);
		}

		internal static T Request<T>(int year, int? month = null, int? day = null) where T : class
		{
			MemoryStream ms = MakeRequest(year, month, day);
			if (ms != null)
			{
				T result = Deserialize<T>(ms);
				return result;
			}
			else
			{
				return null;
			}
		}

		private static MemoryStream MakeRequest(int year, int? month = null, int? day = null)
		{
			string commandUrl = "https://api.dryg.net/dagar/v2/" + year;
			if (month.HasValue)
			{
				commandUrl += "/" + month.Value;

				if (day.HasValue)
				{
					commandUrl += "/" + day.Value;
				}
			}

			try
			{
				WebClient wc = new WebClient();
				byte[] originalData = wc.DownloadData(commandUrl);
				MemoryStream result = new MemoryStream(originalData);
				result.Position = 0;
				return result;
			}
			catch (Exception ex)
			{
				string errorMessage = "Failed to request data from " + commandUrl;
				string errorDetails = "Unknown error";

				WebException webException = ex as WebException;
				if (webException != null)
				{
					HttpWebResponse response = (HttpWebResponse)webException.Response;
					errorDetails = new StreamReader(response.GetResponseStream()).ReadToEnd();
				}

				throw new Exception(errorMessage + " " + errorDetails, ex);
			}
		}

		private static T Deserialize<T>(MemoryStream ms) where T : class
		{
			try
			{
				T result = JsonConvert.DeserializeObject<T>(new StreamReader(ms).ReadToEnd());

				//DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
				//T result = (T)ser.ReadObject(ms);
				
				return result;
			}
			catch (Exception ex)
			{
				ms.Position = 0;
				string apiError = new StreamReader(ms).ReadToEnd().Replace("\"", "");
				throw new Exception(apiError, ex);
			}
		}
	}
}
