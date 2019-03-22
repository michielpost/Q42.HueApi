using Q42.HueApi.Models.Gamut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.Gamut
{
	public static class LightExtensions
	{
    public static string ToHex(this Light light)
    {
      return light.ToHex(light.Capabilities?.Control?.ColorGamut);
    }

    public static string ToHex(this Light light, CIE1931Gamut? gamut)
		{
			return HueColorConverter.HexFromState(light.State, gamut);
		}
	}
}
