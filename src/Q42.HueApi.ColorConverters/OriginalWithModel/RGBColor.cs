using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.OriginalWithModel
{
	/// <summary>
	/// Represents a color with red, green and blue components.
	/// All values are between 0.0 and 1.0.
	/// </summary>
	public struct RGBColor
	{
		public readonly double R;
		public readonly double G;
		public readonly double B;

		public RGBColor(double red, double green, double blue)
		{
			R = red;
			G = green;
			B = blue;
		}

		/// <summary>
		/// Returns the color as a six-digit hexadecimal string, in the form RRGGBB.
		/// </summary>
		public string ToHex()
		{
			int red = (int)(R * 255.99);
			int green = (int)(G * 255.99);
			int blue = (int)(B * 255.99);

			return string.Format("{0}{1}{2}", red.ToString("X2"), green.ToString("X2"), blue.ToString("X2"));
		}
	}
}
