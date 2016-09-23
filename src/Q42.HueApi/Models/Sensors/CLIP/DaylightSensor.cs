using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors.CLIP
{
	public interface DaylightSensor : GeneralSensor
	{
	}

	public interface DaylightSensorConfig : GeneralSensorConfig
	{
		string Long { get; set; }

		string Lat { get; set; }

		bool? Configured { get; set; }

		int? SunriseOffset { get; set; }

		int? SunsetOffset { get; set; }

	}

	public interface DaylightSensorState : GeneralSensorState
	{
		bool? Daylight { get; set; }

	}
}
