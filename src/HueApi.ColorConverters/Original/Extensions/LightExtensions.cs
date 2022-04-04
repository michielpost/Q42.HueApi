using HueApi.Models;
using HueApi.ColorConverters.Original;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
