using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Net;

namespace Deap.SwedishDays
{
	[DataContract]
	public sealed class SwedishDayInfo
	{
		/// <summary>
		/// API name: datum
		/// </summary>
		[DataMember(Name = "datum")]
		public DateTime Date { get; internal set; }

		/// <summary>
		/// API name: arbetsfri dag
		/// </summary>
		[DataMember(Name = "arbetsfri dag")]
		public string WorkFreeDay { get; internal set; }

		/// <summary>
		/// True if it is a work free day.
		/// </summary>
		public bool IsWorkFreeDay
		{
			get
			{
				return WorkFreeDay.ToLowerInvariant().Equals("ja");
			}
		}

		/// <summary>
		/// API name: dag före arbetsfri helgdag
		/// </summary>
		[DataMember(Name = "dag före arbetsfri helgdag")]
		public string DayBeforeWorkFreeDay { get; internal set; }

		/// <summary>
		/// True if it is a day before work free day.
		/// </summary>
		public bool IsDayBeforeWorkFreeDay
		{
			get
			{
				return DayBeforeWorkFreeDay.ToLowerInvariant().Equals("ja");
			}
		}

		/// <summary>
		/// API name: röd dag
		/// </summary>
		[DataMember(Name = "röd dag")]
		public string RedDay { get; internal set; }

		/// <summary>
		/// True if it is a red day.
		/// </summary>
		public bool IsRedDay
		{
			get
			{
				return RedDay.ToLowerInvariant().Equals("ja");
			}
		}
			
		/// <summary>
		/// API name: helgdag
		/// </summary>
		[DataMember(Name = "helgdag")]
		public string Holiday { get; internal set; }

		/// <summary>
		/// True if it is a holiday. Get holiday name from the Holiday property.
		/// </summary>
		public bool IsHoliday
		{
			get
			{
				return !String.IsNullOrEmpty(Holiday);
			}
		}

		/// <summary>
		/// API name: namnsdag
		/// </summary>
		[DataMember(Name = "namnsdag")]
		public string[] Names { get; internal set; }

		internal SwedishDayInfo()
		{
		}
	}
}