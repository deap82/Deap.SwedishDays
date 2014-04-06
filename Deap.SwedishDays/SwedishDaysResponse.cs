using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Deap.SwedishDays
{
	[DataContract]
	public sealed class SwedishDaysResponse
	{
		/// <summary>
		/// cachetid
		/// </summary>
		[DataMember(Name = "cachetid")]
		public DateTime CacheTime { get; internal set; }

		/// <summary>
		/// version
		/// </summary>
		[DataMember(Name = "version")]
		public string Version { get; internal set; }

		/// <summary>
		/// uri
		/// </summary>
		[DataMember(Name = "uri")]
		public string Uri { get; internal set; }

		/// <summary>
		/// startdatum
		/// </summary>
		[DataMember(Name = "startdatum")]
		public DateTime StartDate { get; internal set; }

		/// <summary>
		/// slutdatum
		/// </summary>
		[DataMember(Name = "slutdatum")]
		public DateTime EndDate { get; internal set; }

		/// <summary>
		/// dagar
		/// </summary>
		[DataMember(Name = "dagar")]
		public Dictionary<DateTime, SwedishDayInfo> Days { get; set; }

		internal SwedishDaysResponse()
		{
		}
	}
}
