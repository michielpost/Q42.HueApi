using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q42.HueApi.ColorConverters.HSB;

namespace Q42.HueApi.ColorConverters.Tests.HSBTest
{
	[TestClass]
  public class HSBTests
  {
		[TestMethod]
    public void TestHSBContructor()
    {
      var hsb = new HSB.HSB(0, 0, 0);

      Assert.IsTrue(hsb.Hue == 0);

      hsb.Hue += 65535 + 10;

      Assert.IsTrue(hsb.Hue == 10);

    }

    [TestMethod]
    public void TestHSBOverflowContructor()
    {
      var hsb = new HSB.HSB(65535 + 1, 0, 0);

      Assert.IsTrue(hsb.Hue == 1);
    }

    [TestMethod]
    public void TestHSBNegativeContructor()
    {
      var hsb = new HSB.HSB(-1, 0, 0);

      Assert.IsTrue(hsb.Hue == HSB.HSB.HueMaxValue - 1);
    }
  }
}
