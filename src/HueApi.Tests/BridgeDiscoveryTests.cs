using HueApi.BridgeLocator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class BridgeDiscoveryTests
  {
    [TestMethod]
    public async Task HttpDiscovery()
    {
      IBridgeLocator locator = new HttpBridgeLocator();
      var result = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(10));

      Assert.IsTrue(result.Any());
    }
  }
}
