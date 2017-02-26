using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.Original
{
	public static class LightExtensions
	{
		public static string ToHex(this Light light)
		{
			return HueColorConverter.HexColorFromState(light.State, light.ModelId);
		}

    public static RGBColor ToRGBColor(this Light light)
    {
      return HueColorConverter.RGBColorFromState(light.State, light.ModelId);
    }
  }
}
