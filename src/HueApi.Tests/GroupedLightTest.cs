using HueApi.BridgeLocator;
using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class GroupedLightTests
  {
    private readonly LocalHueClient localHueClient;

    public GroupedLightTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<GroupedLightTests>();
      var config = builder.Build();

      localHueClient = new LocalHueClient(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.GetGroupedLights();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.GetGroupedLights();
      var id = all.Data.First().Id;

      var result = await localHueClient.GetGroupedLight(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.GetGroupedLights();
      var id = all.Data.Last().Id;

      BaseResourceRequest req = new BaseResourceRequest();
      var result = await localHueClient.UpdateGroupedLight(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }
  }
}
