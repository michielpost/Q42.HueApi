using HueApi.ColorConverters.Original.Extensions;
using HueApi.Models.Requests;
using HueApi.Models.Requests.Interface;

namespace HueApi.ColorConverters.Original.Extensions
{
  public static class LightCommandExtensions
  {
    public static T SetColor<T>(this T UpdateLight, RGBColor color, string model = "LCT001") where T : IUpdateColor
    {
      if (UpdateLight == null)
        throw new ArgumentNullException(nameof(UpdateLight));

      var point = HueColorConverter.CalculateXY(color, model);
      return UpdateLight.SetColor(point.x, point.y);
    }

  }
}
