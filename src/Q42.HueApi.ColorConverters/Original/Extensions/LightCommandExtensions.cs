using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.Original
{
	public static class LightCommandExtensions
	{
		public static LightCommand SetColor(this LightCommand lightCommand, RGBColor color, string model = "LCT001")
		{
			if (lightCommand == null)
				throw new ArgumentNullException(nameof(lightCommand));

			var point = HueColorConverter.CalculateXY(color, model);
			return lightCommand.SetColor(point.x, point.y);
		}

	}
}
