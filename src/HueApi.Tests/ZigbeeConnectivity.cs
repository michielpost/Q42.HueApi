using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class ZigbeeConnectivityTests
  {
    private readonly LocalHueClient localHueClient;

    public ZigbeeConnectivityTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<ZigbeeConnectivityTests>();
      var config = builder.Build();

      localHueClient = new LocalHueClient(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.GetZigbeeConnectivity();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.GetZigbeeConnectivity();
      var id = all.Data.First().Id;

      var result = await localHueClient.GetZigbeeConnectivity(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.GetZigbeeConnectivity();
      var id = all.Data.Last().Id;

      BaseResourceRequest req = new BaseResourceRequest();
      var result = await localHueClient.UpdateZigbeeConnectivity(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }
  }
}
