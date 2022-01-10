using HueApi.BridgeLocator;
using HueApi.ColorConverters.Original.Extensions;
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
    private readonly LocalHueApi localHueClient;

    public GroupedLightTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<GroupedLightTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
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

    [TestMethod]
    public async Task ChangeLightColor()
    {
      var all = await localHueClient.GetGroupedLights();
      var id = all.Data.First().Id; //All

      //Turn red
      var req = new UpdateGroupedLight()
        .TurnOn()
        .SetColor(new ColorConverters.RGBColor("FF0000"));

      var result = await localHueClient.UpdateGroupedLight(id, req);

      await Task.Delay(TimeSpan.FromSeconds(5));

      //Turn blue
      req = new UpdateGroupedLight()
        .SetColor(new ColorConverters.RGBColor("0000FF"));
      result = await localHueClient.UpdateGroupedLight(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }
  }
}
