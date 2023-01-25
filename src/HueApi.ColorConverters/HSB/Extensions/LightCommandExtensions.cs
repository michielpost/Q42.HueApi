using HueApi.ColorConverters.HSB;
using HueApi.Models.Requests;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi.ColorConverters.HSB.Extensions
{
  public static class LightCommandExtensions
  {

    //TODO
    //public static UpdateLight SetColor(this UpdateLight UpdateLight, RGBColor color)
    //{
    //  if (UpdateLight == null)
    //    throw new ArgumentNullException(nameof(UpdateLight));

    //  var hsb = color.GetHSB();
    //  UpdateLight.Dimming = new Models.Dimming() { Brightness = (byte)hsb.Brightness };

    //  //UpdateLight.Hue = hsb.Hue;
    //  //UpdateLight.Saturation = hsb.Saturation;

    //  return UpdateLight;
    //}

  }
}
