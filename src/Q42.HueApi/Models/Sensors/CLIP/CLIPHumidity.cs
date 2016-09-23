using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors.CLIP
{
	public interface CLIPHumidity : GeneralSensor
	{
	}

	public interface CLIPHumidityConfig : GeneralSensorConfig
	{
	}

	public interface CLIPHumidityState : GeneralSensorState
	{
		/// <summary>
		/// Current humidity 0.01% steps (e.g. 2000 is 20%)The bridge does not enforce range/resolution.
		/// </summary>
		int? Humidity { get; set; }
	}
}