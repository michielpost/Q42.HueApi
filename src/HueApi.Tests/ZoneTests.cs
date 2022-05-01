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
  public class ZoneTests
  {
    private readonly LocalHueApi localHueClient;

    public ZoneTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<ZoneTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.GetZonesAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.GetZonesAsync();
      var id = all.Data.First().Id;

      var result = await localHueClient.GetZoneAsync(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.GetZonesAsync();
      var last = all.Data.Last();

      var req = new UpdateZone() { Metadata = new Models.Metadata() { Name = last.Metadata!.Name } };
      var result = await localHueClient.UpdateZoneAsync(last.Id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(last.Id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task CreateAndDelete()
    {
      var all = await localHueClient.GetZonesAsync();
      var allLights = await localHueClient.GetLightsAsync();
      var firstLight = allLights.Data.First();

      var existing = all.Data.Where(x => x.Metadata?.Name == "unittest").FirstOrDefault();

      Guid? deleteId = null;
      if(existing == null)
      {
        var req = new CreateZone() {
          Type = "zone",
          Metadata = new Models.Metadata() { Name = "unittest", Archetype = "other" }
        };
        //req.Children.Add(new ResourceIdentifier
        //{
        //  Rid = firstLight.Id,
        //  Rtype = firstLight.Type!
        //});

        var result = await localHueClient.CreateZoneAsync(req);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.HasErrors);

        deleteId = result.Data.First().Rid;
      }

      if (deleteId.HasValue)
      {
        var deleteResult = await localHueClient.DeleteZoneAsync(deleteId.Value);

        Assert.IsNotNull(deleteResult);
        Assert.IsFalse(deleteResult.HasErrors);

        Assert.IsTrue(deleteResult.Data.Count == 1);
        Assert.AreEqual(deleteId.Value, deleteResult.Data.First().Rid);
      }

    }
  }
}
