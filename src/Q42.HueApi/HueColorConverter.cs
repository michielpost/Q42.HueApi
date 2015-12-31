using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
    /// <summary>
    /// Represents a point in CIE1931 color space.
    /// </summary>
    internal struct CIE1931Point
    {
        /// <summary>
        /// The D65 White Point.
        /// </summary>
        public static readonly CIE1931Point D65White = new CIE1931Point(0.312713, 0.329016);

        /// <summary>
        /// The slightly-off D65 White Point used by Philips.
        /// </summary>
        public static readonly CIE1931Point PhilipsWhite = new CIE1931Point(0.322727, 0.32902);

        public CIE1931Point(double x, double y)
        {
            this.x = x;
            this.y = y;
            this.z = 1.0 - x - y;
        }

        public readonly double x;
        public readonly double y;
        public readonly double z;

        public override string ToString()
        {
            // Makes debugging easier.
            return string.Format("{0}, {1}", x, y);
        }
    }

    /// <summary>
    /// Represents a gamut with red, green and blue primaries in CIE1931 color space.
    /// </summary>
    internal struct CIE1931Gamut
    {
        public readonly CIE1931Point Red;
        public readonly CIE1931Point Green;
        public readonly CIE1931Point Blue;
        
        public CIE1931Gamut(CIE1931Point red, CIE1931Point green, CIE1931Point blue)
        {
            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        public static readonly CIE1931Gamut PhilipsWideGamut = new CIE1931Gamut(
                red: new CIE1931Point(0.700607, 0.299301),
                green: new CIE1931Point(0.172416, 0.746797),
                blue: new CIE1931Point(0.135503, 0.039879)
                );

        public static CIE1931Gamut ForModel(string modelId)
        {
            // Details from http://www.developers.meethue.com/documentation/supported-lights

            List<string> gamutA = new List<string>() {
                "LLC001" /* Monet, Renoir, Mondriaan (gen II) */,
                "LLC005" /* Bloom (gen II) */,
                "LLC006" /* Iris (gen III) */,
                "LLC007" /* Bloom, Aura (gen III) */,
                "LLC010" /* Iris */,
                "LLC011" /* Hue Bloom */,
                "LLC012" /* Hue Bloom */,
                "LLC013" /* Storylight */,
                "LST001" /* Light Strips */
            };

            List<string> gamutB = new List<string>() {
                "LCT001" /* Hue A19 */,
                "LCT007" /* Hue A19 */,
                "LCT002" /* Hue BR30 */,
                "LCT003" /* Hue GU10 */,
                "LLM001" /* Color Light Module */
            };

            List<string> gamutC = new List<string>() {
                "LLC020" /* Hue Go */,
                "LST002" /* Hue LightStrips Plus */
            };

            if (gamutA.Contains(modelId))
            {
                return new CIE1931Gamut(
                    red: new CIE1931Point(0.704, 0.296),
                    green: new CIE1931Point(0.2151, 0.7106),
                    blue: new CIE1931Point(0.138, 0.08)
                );
            }
            else if (gamutB.Contains(modelId))
            {
                return new CIE1931Gamut(
                    red: new CIE1931Point(0.675, 0.322),
                    green: new CIE1931Point(0.409, 0.518),
                    blue: new CIE1931Point(0.167, 0.04)
                );
            }
            else if (gamutC.Contains(modelId))
            {
                return new CIE1931Gamut(
                      red: new CIE1931Point(0.692, 0.308),
                      green: new CIE1931Point(0.17, 0.7),
                      blue: new CIE1931Point(0.153, 0.048)
                  );
            }
            else
            {
                // A gamut containing all colors (and then some!)
                return new CIE1931Gamut(
                    red: new CIE1931Point(1.0F, 0.0F),
                    green: new CIE1931Point(0.0F, 1.0F),
                    blue: new CIE1931Point(0.0F, 0.0F)
                );
            }
        }
        
        public bool Contains(CIE1931Point point)
        {
            // Arrangement of points in color space:
            // 
            //   ^             G
            //  y|             
            //   |                  R
            //   |   B         
            //   .------------------->
            //                      x
            //
            return IsBelow(Blue, Green, point) &&
                IsBelow(Green, Red, point) &&
                IsAbove(Red, Blue, point);
        }

        private static bool IsBelow(CIE1931Point a, CIE1931Point b, CIE1931Point point)
        {
            double slope = (a.y - b.y) / (a.x - b.x);
            double intercept = a.y - slope * a.x;

            double maxY = point.x * slope + intercept;
            return point.y <= maxY;
        }

        private static bool IsAbove(CIE1931Point blue, CIE1931Point red, CIE1931Point point)
        {
            double slope = (blue.y - red.y) / (blue.x - red.x);
            double intercept = blue.y - slope * blue.x;

            double minY = point.x * slope + intercept;
            return point.y >= minY;
        }

        public CIE1931Point NearestContainedPoint(CIE1931Point point)
        {
            if (Contains(point))
            {
                // If this gamut already contains the point, then no adjustment is required.
                return point;
            }

            // Find the closest point on each line in the triangle.
            CIE1931Point pAB = GetClosestPointOnLine(Red, Green, point);
            CIE1931Point pAC = GetClosestPointOnLine(Red, Blue, point);
            CIE1931Point pBC = GetClosestPointOnLine(Green, Blue, point);

            //Get the distances per point and see which point is closer to our Point.
            double dAB = GetDistanceBetweenTwoPoints(point, pAB);
            double dAC = GetDistanceBetweenTwoPoints(point, pAC);
            double dBC = GetDistanceBetweenTwoPoints(point, pBC);

            double lowest = dAB;
            CIE1931Point closestPoint = pAB;

            if (dAC < lowest)
            {
                lowest = dAC;
                closestPoint = pAC;
            }
            if (dBC < lowest)
            {
                lowest = dBC;
                closestPoint = pBC;
            }
            return closestPoint;
        }

        private static CIE1931Point GetClosestPointOnLine(CIE1931Point a, CIE1931Point b, CIE1931Point p)
        {
            CIE1931Point AP = new CIE1931Point(p.x - a.x, p.y - a.y);
            CIE1931Point AB = new CIE1931Point(b.x - a.x, b.y - a.y);

            double ab2 = AB.x * AB.x + AB.y * AB.y;
            double ap_ab = AP.x * AB.x + AP.y * AB.y;

            double t = ap_ab / ab2;

            // Bound to ends of line between A and B.
            if (t < 0.0f)
            {
                t = 0.0f;
            }
            else if (t > 1.0f)
            {
                t = 1.0f;
            }

            return new CIE1931Point(a.x + AB.x * t, a.y + AB.y * t);
        }

        private static double GetDistanceBetweenTwoPoints(CIE1931Point one, CIE1931Point two)
        {
            double dx = one.x - two.x; // horizontal difference
            double dy = one.y - two.y; // vertical difference
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }


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

    /// <summary>
    /// Used to convert colors between XY and RGB
    /// internal: Do not expose
    /// </summary>
    /// <remarks>
    /// Based on http://www.developers.meethue.com/documentation/color-conversions-rgb-xy
    /// </remarks>
    internal static partial class HueColorConverter
    {
		public static CIE1931Point RgbToXY(RGBColor color, string model)
		{
            // Apply gamma correction. Convert non-linear RGB colour components
            // to linear color intensity levels.
            double r = InverseGamma(color.R);
            double g = InverseGamma(color.G);
            double b = InverseGamma(color.B);

            // Hue bulbs (depending on the type) can display colors outside the sRGB gamut supported
            // by most computer screens.
            // To make sure all colors are selectable by the user, Philips in its implementation
            // decided to interpret all RGB colors as if they were from a wide (non-sRGB) gamut.
            // The effect of this is to map colors in sRGB to a broader gamut of colors the hue lights
            // can produce.
            //
            // This also results in some deviation of color on screen vs color in real-life.
            // 
            // The Philips implementation describes the matrix below with the comment 
            // "Wide Gamut D65", but the values suggest this is infact not a standard
            // gamut but some custom gamut. 
            // 
            // The coordinates of this gamut have been identified as follows:
            //  red: (0.700607, 0.299301)
            //  green: (0.172416, 0.746797)
            //  blue: (0.135503, 0.039879)
            // 
            // (substitute r = 1, g = 1, b = 1 in sequence into array below and convert
            //  from XYZ to xyY coordinates).
            // The plotted chart can be seen here: http://imgur.com/zelKnSk
            // 
            // Also of interest, the white point is not D65 (0.31271, 0.32902), but a slightly
            // shifted version at (0.322727, 0.32902). This might be because true D65 is slightly
            // outside Gamut B (the position of D65 in the linked chart is slightly inaccurate).
            double X = r * 0.664511f + g * 0.154324f + b * 0.162028f;
            double Y = r * 0.283881f + g * 0.668433f + b * 0.047685f;
            double Z = r * 0.000088f + g * 0.072310f + b * 0.986039f;
            
            CIE1931Point xyPoint = new CIE1931Point(0.0, 0.0);

            if ((X + Y + Z) > 0.0)
            {
                // Convert from CIE XYZ to CIE xyY coordinates.
                xyPoint = new CIE1931Point(X / (X + Y + Z), Y / (X + Y + Z));
            }

            if (model != null)
            {
                //Check if the given XY value is within the colourreach of our lamps.
                CIE1931Gamut gamut = CIE1931Gamut.ForModel(model);

                // The point, adjusted it to the nearest point that is within the gamut of the lamp, if neccessary.
                return gamut.NearestContainedPoint(xyPoint);
            }
            return xyPoint;
		}
        

		/// <summary>
		/// Returns hexvalue from Light State
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public static string HexFromState(State state, string model)
		{
			if (state == null)
				throw new ArgumentNullException("state");
			if (state.On == false || state.Brightness <= 5) //Off or low brightness
				return "000000";
			if (state.ColorCoordinates != null && state.ColorCoordinates.Length == 2) //Based on XY value
			{
				var color = XYToRgb(new CIE1931Point(state.ColorCoordinates[0], state.ColorCoordinates[1]), model);
                return color.ToHex();
			}

			return "FFFFFF"; //White
		}

		public static RGBColor XYToRgb(CIE1931Point point, string model)
		{
            if (model != null)
            {
                CIE1931Gamut gamut = CIE1931Gamut.ForModel(model);

                // If the color is outside the lamp's gamut, adjust to the nearest color
                // inside the lamp's gamut.
                point = gamut.NearestContainedPoint(point);
            }

            // Also adjust it to be in the Philips "Wide Gamut" if not already.
            // The wide gamut used for XYZ->RGB conversion does not quite contain all colors
            // all of the hue bulbs support.
            point = CIE1931Gamut.PhilipsWideGamut.NearestContainedPoint(point);

            // Convert from xyY to XYZ coordinates.
            double Y = 1.0; // Luminance
            double X = (Y / point.y) * point.x;
            double Z = (Y / point.y) * point.z;

            // The Philips implementation comments this matrix with "sRGB D65 conversion"
            // However, this is not the XYZ -> RGB conversion matrix for sRGB. Instead
            // the matrix that is the inverse of that in RgbToXY() is used.
            // See comment in RgbToXY() for more info.
            double r = X * 1.656492 - Y * 0.354851 - Z * 0.255038;
            double g = -X * 0.707196 + Y * 1.655397 + Z * 0.036152;
            double b = X * 0.051713 - Y * 0.121364 + Z * 1.011530;
            
            // Downscale color components so that largest component has an intensity of 1.0,
            // as we can't display colors brighter than that.
            double maxComponent = Math.Max(Math.Max(r, g), b);
            if (maxComponent > 1.0)
            {
                r /= maxComponent;
                g /= maxComponent;
                b /= maxComponent;
            }
            
            // We now have the (linear) amounts of R, G and B corresponding to the specified XY coordinates.
            // Since displays are non-linear, we must apply a gamma correction to get the pixel value.
            // For example, a pixel red value of 1.0 (255) is more than twice as bright as 0.5 (127).
            // We need to correct for this non-linearity.
            r = Gamma(r);
            g = Gamma(g);
            b = Gamma(b);

            // Philips applies a second round of downscaling here, but that should be unnecessary given
            // gamma returns a value between 0.0 and 1.0 for every input between 0.0 and 1.0.
            return new RGBColor(r, g, b);
		}

        /// <summary>
        /// Converts a gamma-corrected value (e.g. as used in RGB pixel components) to
        /// a linear color intensity level. All values are between 0.0 and 1.0.
        /// Used when converting to XY chroma coordinates.
        /// </summary>
        private static double InverseGamma(double value)
        {
            double result;
            if (value > 0.04045)
            {
                result = Math.Pow((value + 0.055) / (1.0 + 0.055), 2.4);
            }
            else
            {
                result = value / 12.92;
            }
            // The gamma function returns values between 0.0 and 1.0 for all inputs
            // between 0.0 and 1.0, but in case there is a slight rounding error...
            return Bound(result);
        }

        /// <summary>
        /// Converts a linear color intensity level to a gamma-corrected value for display
        /// on a screen. All values are between 0.0 and 1.0.
        /// Used when converting to 'RGB' pixel outputs.
        /// </summary>
        private static double Gamma(double value)
        {
            double result;
            if (value <= 0.0031308)
            {
                result = 12.92 * value;
            }
            else
            {
                result = (1.0 + 0.055) * Math.Pow(value, (1.0 / 2.4)) - 0.055;
            }
            // The gamma function returns values between 0.0 and 1.0 for all inputs
            // between 0.0 and 1.0, but in case there is a slight rounding error...
            return Bound(result);
        }
        
        /// <summary>
        /// Bounds the specified value to between 0.0 and 1.0. 
        /// </summary>
        private static double Bound(double value)
        {
            return Math.Max(0.0, Math.Min(1.0, value));
        }
    }
}

