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
  public class ResourceTests
  {
    private readonly LocalHueApi localHueClient;

    public ResourceTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<ResourceTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);

      //localHueClient.SetBaseAddress(new Uri("https://localhost:44372/hueproxy/192.168.1.57/"));
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.Resource.GetAllAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task LoopAllTest()
    {
      var all = await localHueClient.Resource.GetAllAsync();

      foreach(var res in all.Data)
      {
        var resById = await localHueClient.GetResourceAsync(res);

        Assert.IsNotNull(resById);
        Assert.IsFalse(resById.HasErrors);
        Assert.IsTrue(resById.Data.Count == 1);
        Assert.AreEqual(res.Id, resById.Data.First().Id);

        //Wait to not overload the bridge
        await Task.Delay(TimeSpan.FromMilliseconds(400));
      }
    }

    [TestMethod]
    public async Task LoopAllClipTest()
    {
      //var all = await localHueClient.GetResourceAsync("clip");
      var all = await localHueClient.Clip.GetAllAsync();

      foreach (var rtype in all.Data.First().Resources)
      {
        var resByType = await localHueClient.GetResourceAsync(rtype);

        Assert.IsNotNull(resByType);
        Assert.IsFalse(resByType.HasErrors);

        //Wait to not overload the bridge
        await Task.Delay(TimeSpan.FromMilliseconds(400));
      }
    }

  }
}
