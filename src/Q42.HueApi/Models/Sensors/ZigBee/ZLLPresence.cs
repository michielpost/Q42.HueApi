using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors.ZigBee
{
	/// <summary>
	/// Hue Motion Sensor
	/// </summary>
	public interface ZLLPresence : GeneralSensor
	{
		string ProductId { get; set; }

		string SwConfigId { get; set; }

		
	}

	public interface ZLLPresenceConfig : GeneralSensorConfig
	{
		int? Sensitivity { get; set; }

		int? SensitivityMax { get; set; }
	}

	public interface ZLLPresenceState : GeneralSensorState
	{
		bool? Presence { get; set; }
	
	}
}
