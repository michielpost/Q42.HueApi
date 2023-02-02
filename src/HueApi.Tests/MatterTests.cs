using HueApi.BridgeLocator;
using HueApi.Models;
using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class MatterTests
  {
    private readonly LocalHueApi localHueClient;

    public MatterTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<RoomTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.GetMatterItemsAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.GetMatterItemsAsync();
      var id = all.Data.First().Id;

      var result = await localHueClient.GetMatterItemAsync(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.GetMatterItemsAsync();
      var last = all.Data.Last();

      //Warning, calls a reset on the bridge?
      //var result = await localHueClient.UpdateMatterItemAsync(last.Id, new MatterItemUpdate());

      //Assert.IsNotNull(result);
      //Assert.IsFalse(result.HasErrors);

      //Assert.IsTrue(result.Data.Count == 1);
      //Assert.AreEqual(last.Id, result.Data.First().Rid);

    }
   
  }
}
