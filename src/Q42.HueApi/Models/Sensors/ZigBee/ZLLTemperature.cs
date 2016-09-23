using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors.ZigBee
{
	public interface ZLLTemperature : GeneralSensor
	{
	}

	public interface ZLLTemperatureConfig : GeneralSensorConfig
	{

	}

	public interface ZLLTemperatureState : GeneralSensorState
	{
		int? Temperature { get; set; }
	}
}
