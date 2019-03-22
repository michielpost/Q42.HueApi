using Q42.HueApi.Models.Gamut;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.Gamut
{
	public static class LightCommandExtensions
	{
		public static LightCommand SetColor(this LightCommand lightCommand, RGBColor color, CIE1931Gamut? gamut)
		{
			if (lightCommand == null)
				throw new ArgumentNullException(nameof(lightCommand));

			var point = HueColorConverter.RgbToXY(color, gamut);
			return lightCommand.SetColor(point.x, point.y);
		}
	}
}
