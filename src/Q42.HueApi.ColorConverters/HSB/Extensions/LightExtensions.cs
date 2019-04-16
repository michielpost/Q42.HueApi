using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.HSB
{
  public static class LightExtensions
  {
    public static string ToHex(this Light light)
    {
      return light.State.ToRgb().ToHex();
    }
  }
}
