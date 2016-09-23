using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors.CLIP
{
	public interface CLIPPresence : GeneralSensor
	{
	}

	public interface CLIPPresenceConfig : GeneralSensorConfig
	{
	}

	public interface CLIPPresenceState : GeneralSensorState
	{
		bool? Presence { get; set; }
	}
}