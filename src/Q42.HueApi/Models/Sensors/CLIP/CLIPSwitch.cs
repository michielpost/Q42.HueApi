using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors.CLIP
{
	public interface CLIPSwitch : GeneralSensor
	{
	}

	public interface CLIPSwitchConfig : GeneralSensorConfig
	{

	}

	public interface CLIPSwitchState : GeneralSensorState
	{
    int? ButtonEvent { get; set; }
  }
}
