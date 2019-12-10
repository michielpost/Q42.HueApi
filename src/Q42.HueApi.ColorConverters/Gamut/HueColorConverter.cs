using Q42.HueApi.ColorConverters;
using Q42.HueApi.Models.Gamut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.Gamut
{
  /// <summary>
  /// Used to convert colors between XY and RGB
  /// internal: Do not expose
  /// </summary>
  /// <remarks>
  /// Based on http://www.developers.meethue.com/documentation/color-conversions-rgb-xy
  /// </remarks>
  public static class HueColorConverter
  {
    public static CIE1931Point RgbToXY(RGBColor color, CIE1931Gamut? gamut)
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

      if (gamut.HasValue)
      {
        // The point, adjusted it to the nearest point that is within the gamut of the lamp, if neccessary.
        return gamut.Value.NearestContainedPoint(xyPoint);
      }
      return xyPoint;
    }


    /// <summary>
    /// Returns hexvalue from Light State
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static string HexFromState(State state, CIE1931Gamut? gamut)
    {
      var rgb = RgbFromState(state, gamut);

      return rgb.ToHex();
    }

    /// <summary>
    /// Returns the proper gamut per lamp model, or wide if not found
    /// </summary>
    /// <param name="modelId">The model Id to look up</param>
    /// <returns></returns>
    public static CIE1931Gamut GetLightGamut(string modelId) {
      /*"""Gets the correct color gamut for the provided model id.
      Docs: http://www.developers.meethue.com/documentation/supported-lights
      """*/
      string[] modelsA = { "LST001", "LLC010", "LLC011", "LLC012", "LLC006", "LLC007", "LLC013"};
      string[] modelsB = {"LCT001", "LCT007", "LCT002", "LCT003", "LLM001"};
      string[] modelsC = {"LCT010", "LCT014", "LCT011", "LLC020", "LST002"};
      if (Array.Exists(modelsA, element => element == modelId)) {
        return CIE1931Gamut.ModelTypeA;
      }

      if (Array.Exists(modelsB, element => element == modelId)) {
        return CIE1931Gamut.ModelTypeB;
      }

      if (Array.Exists(modelsC, element => element == modelId)) {
        return CIE1931Gamut.ModelTypeC;
      }

      return CIE1931Gamut.PhilipsWideGamut;
    }

    /// <summary>
    /// Returns hexvalue from Light State
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static RGBColor RgbFromState(State state, CIE1931Gamut? gamut)
    {
      if (state == null)
        throw new ArgumentNullException(nameof(state));
      if (state.On == false || state.Brightness <= 5) //Off or low brightness
        return new RGBColor(0, 0, 0);
      if (state.ColorCoordinates != null && state.ColorCoordinates.Length == 2) //Based on XY value
      {
        var color = XYToRgb(new CIE1931Point(state.ColorCoordinates[0], state.ColorCoordinates[1]), gamut);
        return color;
      }

      return new RGBColor(1, 1, 1); ;
    }

    public static RGBColor XYToRgb(CIE1931Point point, CIE1931Gamut? gamut)
    {
      if (gamut.HasValue)
      {
        // If the color is outside the lamp's gamut, adjust to the nearest color
        // inside the lamp's gamut.
        point = gamut.Value.NearestContainedPoint(point);
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

