using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;
using System.Linq;

namespace Q42.HueApi.Tests
{
  [TestClass]
  public class SSDPBridgeDiscoveryTests
  {
    [TestMethod]
    public async Task TestSSDPBridgeLocator()
    {
      IBridgeLocator locator = new SSDPBridgeLocator();

      var bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

      Assert.IsNotNull(bridgeIPs);
      Assert.IsTrue(bridgeIPs.Count() > 0);

    }
  }
}
