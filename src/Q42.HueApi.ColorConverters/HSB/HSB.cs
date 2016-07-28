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
	internal class HSB
	{
		public int Hue { get; set; }
		public int Saturation { get; set; }
		public int Brightness { get; set; }        // = Brightness

		public RGBColor GetRGB()
		{
			var hue = (double)Hue;
			var saturation = (double)Saturation;
			var brightness = (double)Brightness;

			//Convert Hue into degrees for HSB
			hue = hue / 182.04;
			//Bri and Sat must be values from 0-1 (~percentage)
			brightness = brightness / 255.0;
			saturation = saturation / 255.0;

			double r = 0;
			double g = 0;
			double b = 0;

			if (saturation == 0)
			{
				r = g = b = brightness;
			}
			else
			{
				// the color wheel consists of 6 sectors.
				double sectorPos = hue / 60.0;
				int sectorNumber = (int)(Math.Floor(sectorPos));
				// get the fractional part of the sector
				double fractionalSector = sectorPos - sectorNumber;

				// calculate values for the three axes of the color. 
				double p = brightness * (1.0 - saturation);
				double q = brightness * (1.0 - (saturation * fractionalSector));
				double t = brightness * (1.0 - (saturation * (1 - fractionalSector)));

				// assign the fractional colors to r, g, and b based on the sector the angle is in.
				switch (sectorNumber)
				{
					case 0:
						r = brightness;
						g = t;
						b = p;
						break;
					case 1:
						r = q;
						g = brightness;
						b = p;
						break;
					case 2:
						r = p;
						g = brightness;
						b = t;
						break;
					case 3:
						r = p;
						g = q;
						b = brightness;
						break;
					case 4:
						r = t;
						g = p;
						b = brightness;
						break;
					case 5:
						r = brightness;
						g = p;
						b = q;
						break;
				}
			}

			//Check if any value is out of byte range
			if (r < 0)
			{
				r = 0;
			}
			if (g < 0)
			{
				g = 0;
			}
			if (b < 0)
			{
				b = 0;
			}
			return new RGBColor(r,g,b);
		}
	}
}
