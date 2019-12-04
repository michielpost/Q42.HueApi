using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Q42.HueApi.Interfaces;

namespace Q42.HueApi.Tests
{
  [TestClass]
  public class BridgeDiscoveryTests
  {
    [TestMethod]
    public async Task TestHttpBridgeLocator()
    {
      IBridgeLocator locator = new HttpBridgeLocator();

      var bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

      Assert.IsNotNull(bridgeIPs);
      Assert.IsTrue(bridgeIPs.Any());
    }

    [TestMethod]
    public async Task TestUPnPBridgeLocator()
    {
      IBridgeLocator locator = new SsdpBridgeLocator();

      var bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

      Assert.IsNotNull(bridgeIPs);
      Assert.IsTrue(bridgeIPs.Any());
    }
  }
}
