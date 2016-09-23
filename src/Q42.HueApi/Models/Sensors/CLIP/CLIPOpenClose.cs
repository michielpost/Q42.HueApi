using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors.CLIP
{
	public interface CLIPOpenClose : GeneralSensor
	{
	}

	public interface CLIPOpenCloseConfig : GeneralSensorConfig
	{
	}

	public interface CLIPOpenCloseState : GeneralSensorState
	{
		bool? Open { get; set; }
	}
}
