using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors.CLIP
{
	/// <summary>
	/// CLIP Lightlevel and ZLL Lightlevel
	/// </summary>
	public interface CLIPLightlevel : GeneralSensor
	{
	}

	public interface CLIPLightlevelConfig : GeneralSensorConfig
	{
		/// <summary>
		/// Threshold the user configured to be used in rules to determine insufficient lightlevel (ie below threshold). Default value 16000
		/// </summary>
		long? TholdDark { get; set; }

		/// <summary>
		/// Threshold the user configured to be used in rules to determine sufficient lightlevel (ie above threshold). Specified as relative offset to the “dark” threshold. Shall be >=1. Default value 7000
		/// </summary>
		long? TholdOffset { get; set; }


	}

	public interface CLIPLightlevelState : GeneralSensorState
	{
		/// <summary>
		/// Light level in 10000 log10 (lux) +1 measured by sensor. Logarithm scale used because the human eye adjusts to light levels and small changes at low lux levels are more noticeable than at high lux levels.
		/// </summary>
		long? LightLevel { get; set; }

		/// <summary>
		/// lightlevel is at or below given dark threshold
		/// </summary>
		bool? Dark { get; set; }

		/// <summary>
		/// lightlevel is at or above light threshold (dark+offset).
		/// </summary>
		bool? Daylight { get; set; }

	}
}
