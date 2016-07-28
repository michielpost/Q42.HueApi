using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.Original
{
	/// <summary>
	/// Internal helper class, holds XY
	/// </summary>
	internal struct CGPoint
	{
		public double x;
		public double y;

		public CGPoint(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

	}
}
