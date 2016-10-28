using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Sensors.CLIP;
using Q42.HueApi.Models.Sensors.ZigBee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{

	public class Sensor :
		CLIPGenericFlag,
		CLIPGenericStatus,
		CLIPHumidity,
		CLIPLightlevel,
		CLIPOpenClose,
		CLIPPresence,
		CLIPSwitch,
		CLIPTemperature,
		DaylightSensor,
		ZGPSwitch,
		ZLLPresence,
		ZLLSwitch,
		ZLLTemperature
	{
		public SensorState State { get; set; }
		public SensorConfig Config { get; set; }

		public string Id { get; set; }

		public string ManufacturerName { get; set; }

		public string ModelId { get; set; }

		public string Name { get; set; }

		public string ProductId { get; set; }

		public int? Sensitivity { get; set; }

		public int? SensitivityMax { get; set; }

		public string SwConfigId { get; set; }
		public string SwVersion { get; set; }
		public string Type { get; set; }

		public string UniqueId { get; set; }
	}

	public class SensorState : ICommandBody,
		CLIPGenericFlagState,
		CLIPGenericStatusState,
		CLIPHumidityState,
		CLIPLightlevelState,
		CLIPOpenCloseState,
		CLIPPresenceState,
		CLIPSwitchState,
		CLIPTemperatureState,
		DaylightSensorState,
		ZGPSwitchState,
		ZLLPresenceState,
		ZLLSwitchState,
		ZLLTemperatureState
	{
		public int? ButtonEvent { get; set; }

		public bool? Dark { get; set; }
		public bool? Daylight { get; set; }

		public bool? Flag { get; set; }
		public int? Humidity { get; set; }

		public string Lastupdated { get; set; }

		public long? LightLevel { get; set; }

		public bool? Open { get; set; }

		public bool? Presence { get; set; }

		public int? Status { get; set; }

		public int? Temperature { get; set; }
	}

	public class SensorConfig :
		CLIPGenericFlagConfig,
		CLIPGenericStatusConfig,
		CLIPHumidityConfig,
		CLIPLightlevelConfig,
		CLIPOpenCloseConfig,
		CLIPPresenceConfig,
		CLIPSwitchConfig,
		CLIPTemperatureConfig,
		DaylightSensorConfig,
		ZGPSwitchConfig,
		ZLLPresenceConfig,
		ZLLSwitchConfig,
		ZLLTemperatureConfig
	{
		public string Alert { get; set; }

		public int? Battery { get; set; }

		public bool? Configured { get; }

		public string Lat { get;  set; }

		public string Long { get; set; }

		public bool? LedIndication { get; set; }

		public bool? On { get; set; }

		public List<string> Pending { get; set; }

		public bool? Reachable { get; set; }

		public int? SunriseOffset { get; set; }

		public int? SunsetOffset { get; set; }

		public long? TholdDark { get; set; }

		public long? TholdOffset { get; set; }

		public string Url { get; set; }

		public bool? Usertest { get; set; }
	}
}
