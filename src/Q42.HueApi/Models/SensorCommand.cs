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
		// from General Sensor Resource
		// The human readable name of the sensor, can be changed by the user. Is not allowed to be empty on change.
		[DataMember(Name = "name")]
		public string Name { get; set; }

		// config object attributes
		// Turns the sensor on/off. When off, state changes of the sensor are not reflected in the sensor resource. Default is “true”
		[DataMember(Name = "on")]
		public bool? On { get; set; }

		// The current battery state in percent, only for battery powered devices. Not present when not provided on creation or modification (CLIP sensors).
		[DataMember(Name = "battery")]
		public int? Battery { get; set; }

		// Optional URL of the CLIP sensor. Not present when not provided on creation or modification.
		[DataMember(Name = "url")]
		public string Url { get; set; }

		// state object attributes
		// nothing

		// from CLIP Generic Status Sensor
		// already covered: on, battery, url
		// Sensor specific state attributes (you already had that one, just moved it here)
		[DataMember(Name = "status")]
		public int? Status { get; set; }

		// from CLIP Generic Flag Sensor
		// already covered: on, battery, url
		// Sensor specific state attributes
		[DataMember(Name = "flag")]
		public bool? Flag { get; set; }

		// from Daylight Sensor
		// already covered: on
		// Supported config attributes
		// GPS coordinate longitude in decimal degrees DDD.DDDD{W|E} with leading zeros required ending with W or E e.g. 000.3295W “none” .  In future versions this may change to null.
		[DataMember(Name = "long")]
		public string Longitude { get; set; }

		// GPS coordinate latitude in decimal degrees DDD.DDDD{N|S} with leading zeros required e.g. 010.5186N ending with N or S “none”.In future versions this may change to null.
		[DataMember(Name = "lat")]
		public string Latitude { get; set; }

		// Timeoffset in minutes to sunrise
		[DataMember(Name = "sunriseoffset")]
		public int? SunriseOffset { get; set; }

		// Timeoffset in minutes to sunset.
		[DataMember(Name = "sunsetoffset")]
		public int? SunsetOffset { get; set; }

		// from CLIP Humidity
		// already covered: on, battery, url
		// Sensor specific state attributes
		// Current humidity 0.01% steps (e.g. 2000 is 20%)
		[DataMember(Name = "humidity")]
		public int? Humidity { get; set; }

		// from CLIP Temperature
		// already covered: on, battery, url
		// Sensor specific state attributes
		// Current temperature in 0.01 degrees Celsius
		[DataMember(Name = "temperature")]
		public int? Temperature { get; set; }

		// from CLIP Presence
		// already covered: on, battery, url
		// Sensor specific state attributes
		// True if sensor detects presence
		[DataMember(Name = "presence")]
		public bool? Presence { get; set; }

		// from CLIP OpenClose
		// already covered: on, battery, url
		// Sensor specific state attributes
		// True if switch is currently open
		[DataMember(Name = "open")]
		public bool? Open { get; set; }

		// CLIP Switch
		// nothing

		// ZLL Switch
		// already covered: name on, battery, reachable

		// ZGP Switch
		// already covered: name, on


	}

}
