using HueApi.ColorConverters.Original.Extensions;
using HueApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace HueApi.ColorConverters.Tests
{
  [TestClass]
  public sealed class XyPositionExtensionsTests
  {
    [TestMethod]
    public void TestHexRgb()
    {
      var position = new XyPosition { X = 1, Y = 1 };
      var hex = position.ToHex();
      var rgb = position.ToRGBColor();

      Assert.IsNotNull(hex);
      Assert.IsNotNull(rgb);
    }
  }
}
