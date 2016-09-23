using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors.CLIP
{
	public interface CLIPTemperature : GeneralSensor
	{
	}

	public interface CLIPTemperatureConfig : GeneralSensorConfig
	{
	}

	public interface CLIPTemperatureState : GeneralSensorState
	{
		/// <summary>
		/// Current temperature in 0.01 degrees Celsius. (3000 is 30.00 degree) Bridge does not verify the range of the value.
		/// </summary>
		int? Temperature { get; set; }
	}
}