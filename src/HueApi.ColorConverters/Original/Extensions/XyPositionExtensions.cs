using HueApi.Models;

namespace HueApi.ColorConverters.Original.Extensions
{
  public static class XyPositionExtensions
  {
    public static string ToHex(this XyPosition position)
    {
      return HueColorConverter.HexFromXy(position.X, position.Y);
    }

    public static RGBColor ToRGBColor(this XyPosition position)
    {
      return new RGBColor(position.ToHex());
    }

    public static RGBColor ToRGBColor(this XyPosition position, string model)
    {
      return HueColorConverter.ColorFromXY(new CGPoint(position.X, position.Y), model );
    }
  }
}
