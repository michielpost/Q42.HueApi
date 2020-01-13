using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Converters;
using System;
using System.Runtime.Serialization;

namespace Q42.HueApi
{
	[DataContract]
	public class State
	{
		[DataMember(Name = "on")]
		public bool On { get; set; }

		[DataMember(Name = "bri")]
		public byte Brightness { get; set; }

		[DataMember(Name = "hue")]
		public int? Hue { get; set; }

		[DataMember(Name = "sat")]
		public int? Saturation { get; set; }

		[DataMember(Name = "xy")]
		public double[] ColorCoordinates { get; set; }

		[DataMember(Name = "ct")]
		public int? ColorTemperature { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		[DataMember(Name = "alert")]
		public Alert? Alert { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		[DataMember(Name = "effect")]
		public Effect? Effect { get; set; }

		[DataMember(Name = "colormode")]
		public string ColorMode { get; set; }

		[DataMember(Name = "reachable")]
		public bool? IsReachable { get; set; }

		[DataMember(Name = "transitiontime")]
		[JsonConverter(typeof(TransitionTimeConverter))]
		public TimeSpan? TransitionTime { get; set; }

    [DataMember(Name = "mode")]
    public string Mode { get; set; }


  }
}
