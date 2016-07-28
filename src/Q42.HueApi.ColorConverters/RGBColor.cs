using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters
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

		/// <summary>
		/// RGB Color
		/// </summary>
		/// <param name="red">Between 0.0 and 1.0</param>
		/// <param name="green">Between 0.0 and 1.0</param>
		/// <param name="blue">Between 0.0 and 1.0</param>
		public RGBColor(double red, double green, double blue)
		{
			R = red;
			G = green;
			B = blue;
		}

		public RGBColor(int red, int green, int blue)
		{
			R = red / 255.0;
			G = green / 255.0;
			B = blue / 255.0;
		}

		/// <summary>
		/// RGB Color from hex
		/// </summary>
		/// <param name="hexColor"></param>
		public RGBColor(string hexColor)
		{
			if (string.IsNullOrEmpty(hexColor))
				throw new ArgumentNullException(nameof(hexColor));

			//Clean hexColor value, remove the #
			hexColor = hexColor.Replace("#", string.Empty).Trim();

			if (hexColor.Length != 6)
				throw new ArgumentException("hexColor should contains 6 characters", nameof(hexColor));

			int red = int.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier);
			int green = int.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier);
			int blue = int.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier);

			R = red / 255.0;
			G = green / 255.0;
			B = blue / 255.0;
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
