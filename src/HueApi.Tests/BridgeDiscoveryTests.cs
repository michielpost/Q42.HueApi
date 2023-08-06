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

    [TestMethod]
    public async Task TestHttpBridgeLocator()
    {
      IBridgeLocator locator = new HttpBridgeLocator();

      await TestBridgeLocatorWithTimeout(locator, TimeSpan.FromSeconds(5));
    }

    [TestMethod]
    public async Task TestSsdpBridgeLocator()
    {
      IBridgeLocator locator = new SsdpBridgeLocator();

      await TestBridgeLocatorWithTimeout(locator, TimeSpan.FromSeconds(5));
    }

    [TestMethod]
    public async Task TestLocalNetworkScanBridgeLocator()
    {
      IBridgeLocator locator = new LocalNetworkScanBridgeLocator();

      // The timeout here really depends on the network size, latency and the number of CPU
      // It takes roughly 20 seconds for a network of 254 IPs (/24) with an 8-core CPU
      await TestBridgeLocatorWithTimeout(locator, TimeSpan.FromSeconds(30));
    }

    [TestMethod]
    public async Task TestMdnsBridgeLocator()
    {
      IBridgeLocator locator = new MdnsBridgeLocator();

      await TestBridgeLocatorWithTimeout(locator, TimeSpan.FromSeconds(5));
    }

    [TestMethod]
    public async Task TestParallelLocators()
    {
      IBridgeLocator httpBridgeLocator = new HttpBridgeLocator();
      IBridgeLocator ssdpBridgeLocator = new SsdpBridgeLocator();
      IBridgeLocator mdnsBridgeLocator = new MdnsBridgeLocator();
      IBridgeLocator localNetworkScanBridgeLocator = new LocalNetworkScanBridgeLocator();

      await Task.WhenAll(new Task[] {
        TestBridgeLocatorWithTimeout(httpBridgeLocator, TimeSpan.FromSeconds(5)),
        TestBridgeLocatorWithTimeout(ssdpBridgeLocator, TimeSpan.FromSeconds(5)),
        TestBridgeLocatorWithTimeout(mdnsBridgeLocator, TimeSpan.FromSeconds(5)),
        TestBridgeLocatorWithTimeout(localNetworkScanBridgeLocator, TimeSpan.FromSeconds(30)),
      });
    }

    [TestMethod]
    public async Task TestComplete()
    {
      var result = await HueBridgeDiscovery.CompleteDiscoveryAsync(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15));

      Assert.IsNotNull(result);
    }


    private async Task TestBridgeLocatorWithTimeout(IBridgeLocator locator, TimeSpan timeout)
    {
      var startTime = DateTime.Now;
      var bridgeIPs = await locator.LocateBridgesAsync(timeout);

      var elapsed = DateTime.Now.Subtract(startTime);

      Assert.IsTrue(
        elapsed.Subtract(timeout) < TimeSpan.FromMilliseconds(1000),
        $"Must complete inside the timeout specified ±1s (took {elapsed.TotalMilliseconds}ms)");

      Assert.IsNotNull(bridgeIPs,
        "Must return list");

      Assert.IsTrue(bridgeIPs.Any(),
        "Must find bridges");
    }
  }
}
