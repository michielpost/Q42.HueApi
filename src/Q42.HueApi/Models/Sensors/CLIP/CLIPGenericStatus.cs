using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors.CLIP
{
	public interface CLIPGenericStatus : GeneralSensor
	{
	}

	public interface CLIPGenericStatusConfig : GeneralSensorConfig
	{
	}

	public interface CLIPGenericStatusState : GeneralSensorState
	{
		int? Status { get; set; }
	}
}
