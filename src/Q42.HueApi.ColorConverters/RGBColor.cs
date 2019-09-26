using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Q42.HueApi.ColorConverters.Tests")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Q42.HueApi.NET.Tests")]

namespace Q42.HueApi.ColorConverters
{
  /// <summary>
  /// Represents a color with red, green and blue components.
  /// All values are between 0.0 and 1.0.
  /// </summary>
  public struct RGBColor : IEquatable<RGBColor>
  {
    public double R;
    public double G;
    public double B;

    /// <summary>
    /// RGB Color
    /// </summary>
    /// <param name="red">Between 0.0 and 1.0</param>
    /// <param name="green">Between 0.0 and 1.0</param>
    /// <param name="blue">Between 0.0 and 1.0</param>
    public RGBColor(double red, double green, double blue)
    {
      if (red < 0)
        red = 0;
      else if (red > 1)
        red = 1;

      if (green < 0)
        green = 0;
      else if (green > 1)
        green = 1;

      if (blue < 0)
        blue = 0;
      else if (blue > 1)
        blue = 1;

      R = red;
      G = green;
      B = blue;
    }

    public RGBColor(int red, int green, int blue)
    {
      red = red > 255 ? 255 : red;
      green = green > 255 ? 255 : green;
      blue = blue > 255 ? 255 : blue;

      red = red < 0 ? 0 : red;
      green = green < 0 ? 0 : green;
      blue = blue < 0 ? 0 : blue;

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

    public bool Equals(RGBColor other)
    {
      return R == other.R && G == other.G && B == other.B;
    }

    public static bool operator ==(RGBColor c1, RGBColor c2)
    {
      return c1.Equals(c2);
    }

    public static bool operator !=(RGBColor c1, RGBColor c2)
    {
      return !c1.Equals(c2);
    }

    public static RGBColor Random(Random? r = null)
    {
      r = r ?? new Random();
      return new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
    }
  }
}
