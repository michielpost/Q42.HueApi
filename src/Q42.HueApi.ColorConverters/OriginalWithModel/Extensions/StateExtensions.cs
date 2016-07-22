using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.OriginalWithModel
{
	public static class StateExtensions
	{
		public static string ToHex(this State state, string model = "LCT001")
		{
			return HueColorConverter.HexFromState(state, model);
		}

		public static RGBColor ToRgb(this State state, string model = "LCT001")
		{
			return HueColorConverter.RgbFromState(state, model);
		}
	}
}
