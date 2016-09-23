using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors.ZigBee
{
	/// <summary>
	/// Hue Tap
	/// </summary>
	public interface ZGPSwitch : GeneralSensor
	{
	}

	public interface ZGPSwitchConfig : GeneralSensorConfig
	{
	}

	public interface ZGPSwitchState : GeneralSensorState
	{
		/// <summary>
		///34: Button 1
		///16: Button 2
		///17: Button 3
		///18: Button 4
		/// </summary>
		int? ButtonEvent { get; set; }
	}
}
