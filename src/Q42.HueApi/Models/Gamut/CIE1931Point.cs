using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Gamut
{
  /// <summary>
  /// Represents a point in CIE1931 color space.
  /// </summary>
  public struct CIE1931Point
	{
		/// <summary>
		/// The D65 White Point.
		/// </summary>
		public static readonly CIE1931Point D65White = new CIE1931Point(0.312713, 0.329016);

		/// <summary>
		/// The slightly-off D65 White Point used by Philips.
		/// </summary>
		public static readonly CIE1931Point PhilipsWhite = new CIE1931Point(0.322727, 0.32902);

		public CIE1931Point(double x, double y)
		{
			this.x = x;
			this.y = y;
			this.z = 1.0 - x - y;
		}

		public readonly double x;
		public readonly double y;
		public readonly double z;

		public override string ToString()
		{
			// Makes debugging easier.
			return string.Format("{0}, {1}", x, y);
		}
	}
}
