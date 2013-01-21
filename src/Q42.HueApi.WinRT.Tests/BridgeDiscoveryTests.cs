using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;

namespace Q42.HueApi.WinRT.Tests
{
  [TestClass]
  public class BridgeDiscoveryTests
  {
    [TestMethod]
    public async Task TestSSDPBridgeLocator()
    {
      IBridgeLocator locator = new SSDPBridgeLocator();

      var bridgIps = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

      Assert.IsNotNull(bridgIps);
      Assert.IsTrue(bridgIps.Count() > 0);

    }


    [TestMethod]
    public async Task TestHttpBridgeLocator()
    {
      IBridgeLocator locator = new HttpBridgeLocator();

      var bridgIps = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

      Assert.IsNotNull(bridgIps);
      Assert.IsTrue(bridgIps.Count() > 0);

    }
  }
}
