using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.HSB
{
  public static class StateExtensions
  {
    public static RGBColor ToRgb(this State state)
    {
      HSB hsb = new HSB(state.Hue ?? 0, state.Saturation ?? 0, state.Brightness);
      return hsb.GetRGB();
    }

    public static string ToHex(this State state)
    {
      return state.ToRgb().ToHex();
    }
  }
}
