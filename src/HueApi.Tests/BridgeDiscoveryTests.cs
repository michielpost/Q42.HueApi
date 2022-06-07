using HueApi.BridgeLocator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
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

    [TestMethod]
    public async Task TestBridgeId_Are_Equal()
    {
      IBridgeLocator httpBridgeLocator = new HttpBridgeLocator();

      var bridges = await httpBridgeLocator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
      foreach (var bridge in bridges)
      {
        var descriptionBridgeId = await HueApi.BridgeLocator.BridgeLocator.CheckHueDescriptor(IPAddress.Parse(bridge.IpAddress), TimeSpan.FromSeconds(5));

        Assert.AreEqual(bridge.BridgeId, descriptionBridgeId);
      }
    }
  }
}
