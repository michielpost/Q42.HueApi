using HueApi.Models;

namespace HueApi.ColorConverters.Original.Extensions
{
  public static class LightExtensions
  {
    public static string ToHex(this Light state, string model = "LCT001")
    {
      return HueColorConverter.HexColorFromState(state, model);
    }

    public static RGBColor ToRGBColor(this Light state, string model = "LCT001")
    {
      return HueColorConverter.RGBColorFromState(state, model);
    }
  }
}
