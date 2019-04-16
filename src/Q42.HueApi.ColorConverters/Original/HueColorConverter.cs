using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.Original
{
  /// <summary>
  /// Used to convert colors between XY and RGB
  /// internal: Do not expose
  /// </summary>
  internal static partial class HueColorConverter
  {
    private static CGPoint Red = new CGPoint(0.675F, 0.322F);
    private static CGPoint Lime = new CGPoint(0.4091F, 0.518F);
    private static CGPoint Blue = new CGPoint(0.167F, 0.04F);
    private static float factor = 10000.0f;
    private static int maxX = 452;
    private static int maxY = 302;

    /// <summary>
    /// Get XY from red,green,blue strings / ints
    /// </summary>
    /// <param name="red"></param>
    /// <param name="green"></param>
    /// <param name="blue"></param>
    /// <returns></returns>
    private static CGPoint XyFromColor(string red, string green, string blue)
    {
      return XyFromColor(int.Parse(red), int.Parse(green), int.Parse(blue));
    }

    /// <summary>
    ///  Get XY from red,green,blue ints
    /// </summary>
    /// <param name="red"></param>
    /// <param name="green"></param>
    /// <param name="blue"></param>
    /// <returns></returns>
    public static CGPoint XyFromColor(int red, int green, int blue)
    {
      double r = (red > 0.04045f) ? Math.Pow((red + 0.055f) / (1.0f + 0.055f), 2.4f) : (red / 12.92f);
      double g = (green > 0.04045f) ? Math.Pow((green + 0.055f) / (1.0f + 0.055f), 2.4f) : (green / 12.92f);
      double b = (blue > 0.04045f) ? Math.Pow((blue + 0.055f) / (1.0f + 0.055f), 2.4f) : (blue / 12.92f);

      double X = r * 0.4360747f + g * 0.3850649f + b * 0.0930804f;
      double Y = r * 0.2225045f + g * 0.7168786f + b * 0.0406169f;
      double Z = r * 0.0139322f + g * 0.0971045f + b * 0.7141733f;

      double cx = X / (X + Y + Z);
      double cy = Y / (X + Y + Z);

      if (Double.IsNaN(cx))
      {
        cx = 0.0f;
      }

      if (Double.IsNaN(cy))
      {
        cy = 0.0f;
      }

      //Check if the given XY value is within the colourreach of our lamps.
      CGPoint xyPoint = new CGPoint(cx, cy);
      bool inReachOfLamps = HueColorConverter.CheckPointInLampsReach(xyPoint);

      if (!inReachOfLamps)
      {
        //It seems the colour is out of reach
        //let's find the closes colour we can produce with our lamp and send this XY value out.

        //Find the closest point on each line in the triangle.
        CGPoint pAB = HueColorConverter.GetClosestPointToPoint(Red, Lime, xyPoint);
        CGPoint pAC = HueColorConverter.GetClosestPointToPoint(Blue, Red, xyPoint);
        CGPoint pBC = HueColorConverter.GetClosestPointToPoint(Lime, Blue, xyPoint);

        //Get the distances per point and see which point is closer to our Point.
        double dAB = HueColorConverter.GetDistanceBetweenTwoPoints(xyPoint, pAB);
        double dAC = HueColorConverter.GetDistanceBetweenTwoPoints(xyPoint, pAC);
        double dBC = HueColorConverter.GetDistanceBetweenTwoPoints(xyPoint, pBC);

        double lowest = dAB;
        CGPoint closestPoint = pAB;

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

        //Change the xy value to a value which is within the reach of the lamp.
        cx = closestPoint.x;
        cy = closestPoint.y;
      }

      return new CGPoint(cx, cy);
    }

    /// <summary>
    ///  Method to see if the given XY value is within the reach of the lamps.
    /// </summary>
    /// <param name="p">p the point containing the X,Y value</param>
    /// <returns>true if within reach, false otherwise.</returns>
    private static bool CheckPointInLampsReach(CGPoint p)
    {
      CGPoint v1 = new CGPoint(Lime.x - Red.x, Lime.y - Red.y);
      CGPoint v2 = new CGPoint(Blue.x - Red.x, Blue.y - Red.y);

      CGPoint q = new CGPoint(p.x - Red.x, p.y - Red.y);

      double s = HueColorConverter.CrossProduct(q, v2) / HueColorConverter.CrossProduct(v1, v2);
      double t = HueColorConverter.CrossProduct(v1, q) / HueColorConverter.CrossProduct(v1, v2);

      if ((s >= 0.0f) && (t >= 0.0f) && (s + t <= 1.0f))
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    /// <summary>
    /// Calculates crossProduct of two 2D vectors / points.
    /// </summary>
    /// <param name="p1"> p1 first point used as vector</param>
    /// <param name="p2">p2 second point used as vector</param>
    /// <returns>crossProduct of vectors</returns>
    //private static double CrossProduct(CGPoint p1, CGPoint p2)
    //{
    //  return (p1.x * p2.y - p1.y * p2.x);
    //}

    /// <summary>
    /// Find the closest point on a line.
    /// This point will be within reach of the lamp.
    /// </summary>
    /// <param name="A">A the point where the line starts</param>
    /// <param name="B">B the point where the line ends</param>
    /// <param name="P">P the point which is close to a line.</param>
    /// <returns> the point which is on the line.</returns>
    private static CGPoint GetClosestPointToPoint(CGPoint A, CGPoint B, CGPoint P)
    {
      CGPoint AP = new CGPoint(P.x - A.x, P.y - A.y);
      CGPoint AB = new CGPoint(B.x - A.x, B.y - A.y);
      double ab2 = AB.x * AB.x + AB.y * AB.y;
      double ap_ab = AP.x * AB.x + AP.y * AB.y;

      double t = ap_ab / ab2;

      if (t < 0.0f)
        t = 0.0f;
      else if (t > 1.0f)
        t = 1.0f;

      CGPoint newPoint = new CGPoint(A.x + AB.x * t, A.y + AB.y * t);
      return newPoint;
    }

    /// <summary>
    /// Find the distance between two points.
    /// </summary>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <returns>the distance between point one and two</returns>
    //private static double GetDistanceBetweenTwoPoints(CGPoint one, CGPoint two)
    //{
    //  double dx = one.x - two.x; // horizontal difference
    //  double dy = one.y - two.y; // vertical difference
    //  double dist = Math.Sqrt(dx * dx + dy * dy);

    //  return dist;
    //}

    /// <summary>
    /// Returns hexvalue from Light State
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static string HexFromState(State state)
    {
      if (state == null)
        throw new ArgumentNullException(nameof(state));
      if (state.On == false || state.Brightness <= 5) //Off or low brightness
        return "000000";
      if (state.ColorCoordinates != null && state.ColorCoordinates.Length == 2) //Based on XY value
        return HexFromXy(state.ColorCoordinates[0], state.ColorCoordinates[1]);

      return "FFFFFF"; //White
    }

    /// <summary>
    /// Get the HEX color from an XY value
    /// </summary>
    /// <param name="xNumber"></param>
    /// <param name="yNumber"></param>
    /// <returns></returns>
    private static string HexFromXy(double xNumber, double yNumber)
    {
      if (xNumber == 0 && yNumber == 0)
      {
        return "ffffff";
      }

      int closestValue = Int32.MaxValue;
      int closestX = 0, closestY = 0;

      double fX = xNumber;
      double fY = yNumber;

      int intX = (int)(fX * factor);
      int intY = (int)(fY * factor);

      for (int y = 0; y < maxY; y++)
      {
        for (int x = 0; x < maxX; x++)
        {
          int differenceForPixel = 0;
          differenceForPixel += Math.Abs(xArray[x, y] - intX);
          differenceForPixel += Math.Abs(yArray[x, y] - intY);

          if (differenceForPixel < closestValue)
          {
            closestX = x;
            closestY = y;
            closestValue = differenceForPixel;
          }
        }
      }

      int color = cArray[closestX, closestY];
      int red = (color >> 16) & 0xFF;
      int green = (color >> 8) & 0xFF;
      int blue = color & 0xFF;

      return string.Format("{0}{1}{2}", red.ToString("X2"), green.ToString("X2"), blue.ToString("X2"));
    }


    /*****************************************************************************************
    The code below is based on http://www.developers.meethue.com/documentation/color-conversions-rgb-xy
    Converted to C# by Niels Laute
    *****************************************************************************************/

    public static CGPoint CalculateXY(RGBColor color, string model)
    {
      //CGColorRef cgColor = [color CGColor];

      //const CGFloat* components = CGColorGetComponents(cgColor);
      //long numberOfComponents = CGColorGetNumberOfComponents(cgColor);

      // Default to white
      double red = 1.0f;
      double green = 1.0f;
      double blue = 1.0f;

      //if (numberOfComponents == 4)
      //{
      // Full color
      red = color.R;
      green = color.G;
      blue = color.B;
      //}
      //else if (numberOfComponents == 2)
      //{
      //    // Greyscale color
      //    red = green = blue = color.A;
      //}

      // Apply gamma correction
      double r;
      double g;
      double b;

      if (red > 0.04045f)
      {
        r = (float)Math.Pow((red + 0.055f) / (1.0f + 0.055f), 2.4f);
      }
      else
      {
        r = red / 12.92f;
      }

      if (green > 0.04045f)
      {
        g = (float)Math.Pow((green + 0.055f) / (1.0f + 0.055f), 2.4f);
      }
      else
      {
        g = green / 12.92f;
      }

      if (blue > 0.04045f)
      {
        b = (float)Math.Pow((blue + 0.055f) / (1.0f + 0.055f), 2.4f);
      }
      else
      {
        b = blue / 12.92f;
      }


      // Wide gamut conversion D65
      double X = r * 0.664511f + g * 0.154324f + b * 0.162028f;
      double Y = r * 0.283881f + g * 0.668433f + b * 0.047685f;
      double Z = r * 0.000088f + g * 0.072310f + b * 0.986039f;

      double cx = 0.0f;

      if ((X + Y + Z) != 0)
        cx = X / (X + Y + Z);

      double cy = 0.0f;
      if ((X + Y + Z) != 0)
        cy = Y / (X + Y + Z);

      //Check if the given XY value is within the colourreach of our lamps.

      CGPoint xyPoint = new CGPoint(cx, cy);
      List<CGPoint> colorPoints = ColorPointsForModel(model);
      bool inReachOfLamps = CheckPointInLampsReach(xyPoint, colorPoints);

      if (!inReachOfLamps)
      {
        //It seems the colour is out of reach
        //let's find the closest colour we can produce with our lamp and send this XY value out.

        //Find the closest point on each line in the triangle.
        CGPoint pAB = GetClosestPointToPoints(colorPoints[0], colorPoints[1], xyPoint);
        CGPoint pAC = GetClosestPointToPoints(colorPoints[2], colorPoints[0], xyPoint);
        CGPoint pBC = GetClosestPointToPoints(colorPoints[1], colorPoints[2], xyPoint);

        //Get the distances per point and see which point is closer to our Point.
        float dAB = GetDistanceBetweenTwoPoints(xyPoint, pAB);
        float dAC = GetDistanceBetweenTwoPoints(xyPoint, pAC);
        float dBC = GetDistanceBetweenTwoPoints(xyPoint, pBC);

        float lowest = dAB;
        CGPoint closestPoint = pAB;

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

        //Change the xy value to a value which is within the reach of the lamp.
        cx = (float)closestPoint.x;
        cy = (float)closestPoint.y;
      }

      return new CGPoint(cx, cy);
    }

    private static List<CGPoint> ColorPointsForModel(string ModelID)
    {
      List<CGPoint> colorPoints = new List<CGPoint>();
      List<string> hueBulbs = new List<string>() {
        "LCT001" /* Hue A19 */,
           "LCT002" /* Hue BR30 */,
                           "LCT003" /* Hue GU10 */

    };


      List<string> livingColors = new List<string>() {
                "LLC001" /* Monet, Renoir, Mondriaan (gen II) */,
                             "LLC005" /* Bloom (gen II) */,
                             "LLC006" /* Iris (gen III) */,
                             "LLC007" /* Bloom, Aura (gen III) */,
                             "LLC011" /* Hue Bloom */,
                             "LLC012" /* Hue Bloom */,
                             "LLC013" /* Storylight */,
                             "LST001" /* Light Strips */ };


      if (hueBulbs.Contains(ModelID))
      {
        // Hue bulbs color gamut triangle
        colorPoints.Add(new CGPoint(0.674F, 0.322F)); // Red
        colorPoints.Add(new CGPoint(0.408F, 0.517F)); // Green
        colorPoints.Add(new CGPoint(0.168F, 0.041F)); // Blue
      }
      else if (livingColors.Contains(ModelID))
      {
        // LivingColors color gamut triangle
        colorPoints.Add(new CGPoint(0.703F, 0.296F)); // Red
        colorPoints.Add(new CGPoint(0.214F, 0.709F)); // Green
        colorPoints.Add(new CGPoint(0.139F, 0.081F)); // Blue
      }
      else
      {
        // Default construct triangle wich contains all values
        colorPoints.Add(new CGPoint(1.0F, 0.0F)); // Red
        colorPoints.Add(new CGPoint(0.0F, 1.0F)); // Green
        colorPoints.Add(new CGPoint(0.0F, 0.0F)); // Blue
      }

      return colorPoints;
    }

    private static CGPoint GetClosestPointToPoints(CGPoint A, CGPoint B, CGPoint P)
    {
      CGPoint AP = new CGPoint(P.x - A.x, P.y - A.y);
      CGPoint AB = new CGPoint(B.x - A.x, B.y - A.y);
      float ab2 = (float)(AB.x * AB.x + AB.y * AB.y);
      float ap_ab = (float)(AP.x * AB.x + AP.y * AB.y);

      float t = ap_ab / ab2;

      if (t < 0.0f)
      {
        t = 0.0f;
      }
      else if (t > 1.0f)
      {
        t = 1.0f;
      }

      return new CGPoint(A.x + AB.x * t, A.y + AB.y * t);
    }

    private static float GetDistanceBetweenTwoPoints(CGPoint one, CGPoint two)
    {
      float dx = (float)(one.x - two.x); // horizontal difference
      float dy = (float)(one.y - two.y); // vertical difference
      return (float)Math.Sqrt(dx * dx + dy * dy);
    }

    private static bool CheckPointInLampsReach(CGPoint p, List<CGPoint> colorPoints)
    {
      CGPoint red = colorPoints[0];
      CGPoint green = colorPoints[1];
      CGPoint blue = colorPoints[2];

      CGPoint v1 = new CGPoint(green.x - red.x, green.y - red.y);
      CGPoint v2 = new CGPoint(blue.x - red.x, blue.y - red.y);

      CGPoint q = new CGPoint(p.x - red.x, p.y - red.y);

      float s = CrossProduct(q, v2) / CrossProduct(v1, v2);
      float t = CrossProduct(v1, q) / CrossProduct(v1, v2);

      if ((s >= 0.0f) && (t >= 0.0f) && (s + t <= 1.0f))
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    private static float CrossProduct(CGPoint p1, CGPoint p2)
    {
      return (float)(p1.x * p2.y - p1.y * p2.x);
    }

    /// <summary>
    /// Returns hexvalue from Light State
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static string HexColorFromState(State state, string model)
    {
      if (state == null)
        throw new ArgumentNullException(nameof(state));
      if (state.On == false || state.Brightness <= 1) //Off or low brightness
        return "000000";
      if (state.ColorCoordinates != null && state.ColorCoordinates.Length == 2) //Based on XY value
      {
        RGBColor color = ColorFromXY(new CGPoint(state.ColorCoordinates[0], state.ColorCoordinates[1]), model);

        //Brightness of state (0.0-1.0)
        double b = Convert.ToDouble(state.Brightness) / 255;
        //color with brightness
        RGBColor colorWithB = new RGBColor(color.R * b, color.G * b, color.B * b);
        return colorWithB.ToHex();
      }

      return "FFFFFF"; //White
    }

    /// <summary>
    /// Returns hexvalue from Light State
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static RGBColor RGBColorFromState(State state, string model)
    {
      if (state == null)
        throw new ArgumentNullException(nameof(state));
      //if (state.On == false || state.Brightness <= 5) //Off or low brightness
      //  return new RGBColor("000000");
      if (state.ColorCoordinates != null && state.ColorCoordinates.Length == 2) //Based on XY value
      {
        return ColorFromXY(new CGPoint(state.ColorCoordinates[0], state.ColorCoordinates[1]), model);
      }
      return new RGBColor("FFFFFF"); //White
    }

    private static RGBColor ColorFromXY(CGPoint xy, string model)
    {
      List<CGPoint> colorPoints = ColorPointsForModel(model);
      bool inReachOfLamps = CheckPointInLampsReach(xy, colorPoints);

      if (!inReachOfLamps)
      {
        //It seems the colour is out of reach
        //let's find the closest colour we can produce with our lamp and send this XY value out.

        //Find the closest point on each line in the triangle.
        CGPoint pAB = GetClosestPointToPoints(colorPoints[0], colorPoints[1], xy);
        CGPoint pAC = GetClosestPointToPoints(colorPoints[2], colorPoints[0], xy);
        CGPoint pBC = GetClosestPointToPoints(colorPoints[1], colorPoints[2], xy);

        //Get the distances per point and see which point is closer to our Point.
        float dAB = GetDistanceBetweenTwoPoints(xy, pAB);
        float dAC = GetDistanceBetweenTwoPoints(xy, pAC);
        float dBC = GetDistanceBetweenTwoPoints(xy, pBC);

        float lowest = dAB;
        CGPoint closestPoint = pAB;

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

        //Change the xy value to a value which is within the reach of the lamp.
        xy.x = closestPoint.x;
        xy.y = closestPoint.y;
      }

      float x = (float)xy.x;
      float y = (float)xy.y;
      float z = 1.0f - x - y;

      float Y = 1.0f;
      float X = (Y / y) * x;
      float Z = (Y / y) * z;

      // sRGB D65 conversion
      float r = X * 1.656492f - Y * 0.354851f - Z * 0.255038f;
      float g = -X * 0.707196f + Y * 1.655397f + Z * 0.036152f;
      float b = X * 0.051713f - Y * 0.121364f + Z * 1.011530f;

      if (r > b && r > g && r > 1.0f)
      {
        // red is too big
        g = g / r;
        b = b / r;
        r = 1.0f;
      }
      else if (g > b && g > r && g > 1.0f)
      {
        // green is too big
        r = r / g;
        b = b / g;
        g = 1.0f;
      }
      else if (b > r && b > g && b > 1.0f)
      {
        // blue is too big
        r = r / b;
        g = g / b;
        b = 1.0f;
      }

      // Apply gamma correction
      if (r <= 0.0031308f)
      {
        r = 12.92f * r;
      }
      else
      {
        r = (1.0f + 0.055f) * (float)Math.Pow(r, (1.0f / 2.4f)) - 0.055f;
      }

      if (g <= 0.0031308f)
      {
        g = 12.92f * g;
      }
      else
      {
        g = (1.0f + 0.055f) * (float)Math.Pow(g, (1.0f / 2.4f)) - 0.055f;
      }

      if (b <= 0.0031308f)
      {
        b = 12.92f * b;
      }
      else
      {
        b = (1.0f + 0.055f) * (float)Math.Pow(b, (1.0f / 2.4f)) - 0.055f;
      }

      if (r > b && r > g)
      {
        // red is biggest
        if (r > 1.0f)
        {
          g = g / r;
          b = b / r;
          r = 1.0f;
        }
      }
      else if (g > b && g > r)
      {
        // green is biggest
        if (g > 1.0f)
        {
          r = r / g;
          b = b / g;
          g = 1.0f;
        }
      }
      else if (b > r && b > g)
      {
        // blue is biggest
        if (b > 1.0f)
        {
          r = r / b;
          g = g / b;
          b = 1.0f;
        }
      }

      return new RGBColor(r, g, b);
    }
  }
}

