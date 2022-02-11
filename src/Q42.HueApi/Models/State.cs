using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Converters;
using System;

namespace Q42.HueApi
{
	public class State
	{
		[JsonProperty("on")]
		public bool On { get; set; }

		[JsonProperty("bri")]
		public byte Brightness { get; set; }

		[JsonProperty("hue")]
		public int? Hue { get; set; }

		[JsonProperty("sat")]
		public int? Saturation { get; set; }

		[JsonProperty("xy")]
		public double[] ColorCoordinates { get; set; }

		[JsonProperty("ct")]
		public int? ColorTemperature { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty("alert")]
		public Alert? Alert { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty("effect")]
		public Effect? Effect { get; set; }

		[JsonProperty("colormode")]
		public string ColorMode { get; set; }

		[JsonProperty("reachable")]
		public bool? IsReachable { get; set; }

		[JsonProperty("transitiontime")]
		[JsonConverter(typeof(TransitionTimeConverter))]
		public TimeSpan? TransitionTime { get; set; }

    [JsonProperty("mode")]
    public string Mode { get; set; }


  }
}
