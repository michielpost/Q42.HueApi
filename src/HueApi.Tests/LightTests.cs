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
  public class LightTests
  {
    private readonly LocalHueApi localHueClient;

    public LightTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<LightTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.GetLights();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.GetLights();
      var id = all.Data.First().Id;

      var result = await localHueClient.GetLight(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.GetLights();
      var id = all.Data.Last().Id;

      UpdateLight req = new UpdateLight()
      {
        Alert = new UpdateAlert()
      };
      var result = await localHueClient.UpdateLight(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task ChangeLightColor()
    {
      var all = await localHueClient.GetLights();
      var id = all.Data.Last().Id;

      //Turn red
      var req = new UpdateLight()
        .TurnOn()
        .SetColor(new ColorConverters.RGBColor("FF0000"));

      var result = await localHueClient.UpdateLight(id, req);

      await Task.Delay(TimeSpan.FromSeconds(5));

      //Turn blue
      req = new UpdateLight()
        .SetColor(new ColorConverters.RGBColor("0000FF"));
      result = await localHueClient.UpdateLight(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public void ColorTest()
    {
      var request = new UpdateLight().SetColor(new HueApi.ColorConverters.RGBColor("FF0000"));

      Assert.IsNotNull(update);

    }
  }
}
