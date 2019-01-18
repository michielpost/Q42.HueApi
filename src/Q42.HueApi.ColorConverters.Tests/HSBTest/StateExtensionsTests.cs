using Microsoft.VisualStudio.TestTools.UnitTesting;
using Q42.HueApi.ColorConverters.HSB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.ColorConverters.Tests.HSBTest
{
	[TestClass]
	public class StateExtensionsTests
	{

		[TestMethod]
		public void ColorConversionBlackPoint()
		{
			State state = new State()
			{
				Hue = 0,
				Brightness = 0,
				Saturation = 0
			};

			var rgb = state.ToRgb();

			Assert.AreEqual(0, rgb.R);
			Assert.AreEqual(0, rgb.G);
			Assert.AreEqual(0, rgb.B);
		
		}

		[TestMethod]
		public void ColorConversionWhitePoint()
		{
			string color = "FFFFFF";

			RGBColor rgbColor = new RGBColor(color);
			var hsb = rgbColor.GetHSB();
			State state = new State()
			{
				Hue = hsb.Hue,
				Brightness = (byte)hsb.Brightness,
				Saturation = hsb.Saturation
			};

			var rgb = state.ToRgb();

			Assert.AreEqual(1, rgb.R);
			Assert.AreEqual(1, rgb.G);
			Assert.AreEqual(1, rgb.B);

		}

		[TestMethod]
		public void ColorConversionRedPoint()
		{
			string color = "FF0000";

			RGBColor rgbColor = new RGBColor(color);
			var hsb = rgbColor.GetHSB();
			State state = new State()
			{
				Hue = hsb.Hue,
				Brightness = (byte)hsb.Brightness,
				Saturation = hsb.Saturation
			};

			var rgb = state.ToRgb();

			Assert.AreEqual(1, rgb.R);
			Assert.AreEqual(0, rgb.G);
			Assert.AreEqual(0, rgb.B);

		}

		[TestMethod]
		public void ColorConversionDarkSeaGreenPoint()
		{
			string color = "8FBC8B";

			RGBColor rgbColor = new RGBColor(color);
			var hsb = rgbColor.GetHSB();
			State state = new State()
			{
				Hue = hsb.Hue,
				Brightness = (byte)hsb.Brightness,
				Saturation = hsb.Saturation
			};

			var rgb = state.ToRgb();

      Assert.AreEqual(143d/255d, rgb.R, 0.002d);
      Assert.AreEqual(188d/255d, rgb.G, 0.002d);
      Assert.AreEqual(139d/255d, rgb.B, 0.002d);

    }
	}
}
