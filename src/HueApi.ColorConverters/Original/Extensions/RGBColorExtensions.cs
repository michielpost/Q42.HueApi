using HueApi.Models;

namespace HueApi.ColorConverters.Original.Extensions
{
  public static class RGBColorExtensions
  {
    public static Color ToColor(this RGBColor rgbColor)
    {
      var xy = HueColorConverter.CalculateXY(rgbColor, String.Empty);

      var color = new Color
      {
        Xy = new XyPosition()
        {
          X = xy.x,
          Y = xy.y
        }
      };

      return color;
    }
  }
}
