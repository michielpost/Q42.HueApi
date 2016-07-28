using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.OriginalWithModel
{
	public static class LightExtensions
	{
		public static string ToHex(this Light light)
		{
			return HueColorConverter.HexFromState(light.State, light.ModelId);
		}
	}
}
