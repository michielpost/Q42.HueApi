using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.HSB
{
	public static class LightCommandExtensions
	{
		/// <summary>
		/// Helper to set the color based on RGB
		/// </summary>
		/// <param name="lightCommand"></param>
		/// <param name="red"></param>
		/// <param name="green"></param>
		/// <param name="blue"></param>
		/// <returns></returns>
		public static LightCommand SetColor(this LightCommand lightCommand, int red, int green, int blue)
		{
			if (lightCommand == null)
				throw new ArgumentNullException("lightCommand");

			var rgb = new RGBColor(red / 255.0, green / 255.0, blue / 255.0);

			return lightCommand.SetColor(rgb);
		}

		public static LightCommand SetColor(this LightCommand lightCommand, RGBColor color)
		{
			if (lightCommand == null)
				throw new ArgumentNullException("lightCommand");

			lightCommand.Brightness = (byte)color.GetBrightness();
			lightCommand.Hue = color.GetHue();
			lightCommand.Saturation = color.GetSaturation();

			return lightCommand;
		}

		/// <summary>
		/// Helper to set the color based on a HEX value
		/// </summary>
		/// <param name="lightCommand"></param>
		/// <param name="hexColor"></param>
		/// <returns></returns>
		public static LightCommand SetColor(this LightCommand lightCommand, string hexColor)
		{
			if (lightCommand == null)
				throw new ArgumentNullException("lightCommand");
			if (hexColor == null)
				throw new ArgumentNullException("hexColor");

			//Clean hexColor value, remove the #
			hexColor = hexColor.Replace("#", string.Empty);

			if (hexColor.Length != 6)
				throw new ArgumentException("hexColor should contains 6 characters", "hexColor");

			int red = int.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier);
			int green = int.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier);
			int blue = int.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier);

			return lightCommand.SetColor(red, green, blue);
		}

		/// <summary>
		/// Helper to set the color based on RGB strings
		/// </summary>
		/// <param name="lightCommand"></param>
		/// <param name="red"></param>
		/// <param name="green"></param>
		/// <param name="blue"></param>
		/// <returns></returns>
		public static LightCommand SetColor(this LightCommand lightCommand, string red, string green, string blue)
		{
			if (lightCommand == null)
				throw new ArgumentNullException("lightCommand");

			return lightCommand.SetColor(int.Parse(red), int.Parse(green), int.Parse(blue));
		}
	}
}
