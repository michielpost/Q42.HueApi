using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors.CLIP
{
	public interface CLIPGenericFlag : GeneralSensor
	{
	}

	public interface CLIPGenericFlagConfig : GeneralSensorConfig
	{
	}

	public interface CLIPGenericFlagState : GeneralSensorState
	{
		bool? Flag { get; set; }
	}
}