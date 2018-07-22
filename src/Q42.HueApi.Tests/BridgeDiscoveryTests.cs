using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;
using System.Linq;

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
      Assert.IsTrue(bridgeIPs.Count() > 0);

    }
  }
}
