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

      await TestBridgeLocatorWithTimeout(locator, TimeSpan.FromSeconds(5));
    }

    [TestMethod]
    public async Task TestUPnPBridgeLocator()
    {
      IBridgeLocator locator = new SsdpBridgeLocator();

      await TestBridgeLocatorWithTimeout(locator, TimeSpan.FromSeconds(5));
    }

    private async Task TestBridgeLocatorWithTimeout(IBridgeLocator locator, TimeSpan timeout)
    {
      var startTime = DateTime.Now;
      var bridgeIPs = await locator.LocateBridgesAsync(timeout);

      Assert.IsTrue(
        DateTime.Now.Subtract(startTime).Subtract(timeout) < TimeSpan.FromMilliseconds(250),
        "Must complete inside the timeout specified (plus 250 ms)");

      Assert.IsNotNull(bridgeIPs,
        "Must return list");

      Assert.IsTrue(bridgeIPs.Any(),
        "Must find bridges");
    }
  }
}
