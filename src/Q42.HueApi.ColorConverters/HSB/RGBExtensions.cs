using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.HSB
{
	/// <summary>
	/// Based on code contributed by https://github.com/CharlyTheKid
	/// </summary>
	internal static class RGBExtensions
	{
		internal static HSB GetHSB(this RGBColor rgb)
		{
			var hsb = new HSB
			{
				Hue = (int)rgb.GetHue(),
				Saturation = (int)rgb.GetSaturation(),
				Brightness = (int)rgb.GetBrightness()
			};
			return hsb;
		}

		internal static double GetHue(this RGBColor rgb)
		{
			var R = rgb.R;
			var G = rgb.G;
			var B = rgb.B;

			if (R == G && G == B)
				return 0;

			double hue;

			var min = Numbers.Min(R, G, B);
			var max = Numbers.Max(R, G, B);

			var delta = max - min;

			if (R.AlmostEquals(max))
				hue = (G - B) / delta; // between yellow & magenta
			else if (G.AlmostEquals(max))
				hue = 2 + (B - R) / delta; // between cyan & yellow
			else
				hue = 4 + (R - G) / delta; // between magenta & cyan

			hue *= 60; // degrees

			if (hue < 0)
				hue += 360;

			return hue * 182.04f;
		}

		internal static double GetSaturation(this RGBColor rgb)
		{
			var R = rgb.R;
			var G = rgb.G;
			var B = rgb.B;

			var min = Numbers.Min(R, G, B);
			var max = Numbers.Max(R, G, B);

			if (max.AlmostEquals(min))
				return 0;
			return ((max.AlmostEquals(0f)) ? 0f : 1f - (1f * min / max)) * 255;
		}

		internal static double GetBrightness(this RGBColor rgb)
		{
			var R = rgb.R;
			var G = rgb.G;
			var B = rgb.B;

			return Numbers.Max(R, G, B) * 255;
		}
	}
}
