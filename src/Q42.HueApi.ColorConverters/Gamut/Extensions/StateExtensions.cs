using Q42.HueApi.Models.Gamut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.Gamut
{
	public static class StateExtensions
	{
		public static string ToHex(this State state, CIE1931Gamut? gamut)
		{
			return HueColorConverter.HexFromState(state, gamut);
		}

		public static RGBColor ToRgb(this State state, CIE1931Gamut? gamut)
		{
			return HueColorConverter.RgbFromState(state, gamut);
		}
	}
}
