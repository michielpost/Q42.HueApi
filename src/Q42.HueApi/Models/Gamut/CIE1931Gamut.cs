using Newtonsoft.Json;
using Q42.HueApi.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Gamut
{
  /// <summary>
  /// Represents a gamut with red, green and blue primaries in CIE1931 color space.
  /// </summary>
  [JsonConverter(typeof(GamutConverter))]
  public struct CIE1931Gamut
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

    public static CIE1931Gamut ModelTypeA => new CIE1931Gamut(
                    red: new CIE1931Point(0.704, 0.296),
                    green: new CIE1931Point(0.2151, 0.7106),
                    blue: new CIE1931Point(0.138, 0.08)
                );

    public static CIE1931Gamut ModelTypeB => new CIE1931Gamut(
                    red: new CIE1931Point(0.675, 0.322),
                    green: new CIE1931Point(0.409, 0.518),
                    blue: new CIE1931Point(0.167, 0.04)
                );

    public static CIE1931Gamut ModelTypeC => new CIE1931Gamut(
                      red: new CIE1931Point(0.692, 0.308),
                      green: new CIE1931Point(0.17, 0.7),
                      blue: new CIE1931Point(0.153, 0.048)
                  );

    public static CIE1931Gamut All => new CIE1931Gamut(
                    red: new CIE1931Point(1.0F, 0.0F),
                    green: new CIE1931Point(0.0F, 1.0F),
                    blue: new CIE1931Point(0.0F, 0.0F)
                );


    [Obsolete("List of Models is no longer updated by Philips. Use The Capabilities.GamutType or Gamut from the Light")]
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
                "LST001" /* Light Strips */,
        "LLC014" /* Bloom, Aura (gen III) */
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
                "LST002" /* Hue LightStrips Plus */,
                "LCT011" /* Hue BR30 */,
                "LCT012" /* Hue color candle */,
        "LCT010" /* Hue A19 gen 3 */,
        "LCT014" /* Hue A19 gen 3 */,
        "LCT015" /* Hue A19 gen 3 */,
        "LCT016" /* Hue A19 gen 3 */
            };

      if (gamutA.Contains(modelId))
      {
        return CIE1931Gamut.ModelTypeA;
      }
      else if (gamutB.Contains(modelId))
      {
        return CIE1931Gamut.ModelTypeB;
      }
      else if (gamutC.Contains(modelId))
      {
        return CIE1931Gamut.ModelTypeC;
      }
      else
      {
        // A gamut containing all colors (and then some!)
        return CIE1931Gamut.All;
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
}
