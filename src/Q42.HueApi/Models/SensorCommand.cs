using System;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Interfaces;

namespace Q42.HueApi
{
	/// <summary>
	/// Sends a SensorCommand
	/// </summary>
	[DataContract]
	public class SensorCommand : ICommandBody
	{
		[DataMember(Name = "status")]
		public int? Status { get; set; }
	}

}
