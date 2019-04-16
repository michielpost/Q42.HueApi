using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.HSB
{
  public static class LightCommandExtensions
  {

    public static LightCommand SetColor(this LightCommand lightCommand, RGBColor color)
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      var hsb = color.GetHSB();
      lightCommand.Brightness = (byte)hsb.Brightness;
      lightCommand.Hue = hsb.Hue;
      lightCommand.Saturation = hsb.Saturation;

      return lightCommand;
    }

  }
}
