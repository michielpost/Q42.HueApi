using HueApi.ColorConverters.Original.Extensions;
using HueApi.Models.Requests;
using Q42.HueApi.ColorConverters.Original;
using HueApi.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi.ColorConverters.Original.Extensions
{
  public static class UpdateLightExtensions
  {
    public static UpdateLight SetColor(this UpdateLight UpdateLight, RGBColor color, string model = "LCT001")
    {
      if (UpdateLight == null)
        throw new ArgumentNullException(nameof(UpdateLight));

      var point = HueColorConverter.CalculateXY(color, model);
      return UpdateLight.SetColor(point.x, point.y);
    }

  }
}
