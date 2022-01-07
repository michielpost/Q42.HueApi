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
  public class RoomTests
  {
    private readonly LocalHueClient localHueClient;

    public RoomTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<RegisterAppTests>();
      var config = builder.Build();

      localHueClient = new LocalHueClient(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.GetRooms();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.GetRooms();
      var id = all.Data.First().Id;

      var result = await localHueClient.GetRoom(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.GetRooms();
      var last = all.Data.Last();

      BaseResourceRequest req = new BaseResourceRequest() { Metadata = new Models.Metadata() { Name = last.Metadata.Name } };
      var result = await localHueClient.UpdateRoom(last.Id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(last.Id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task CreateAndDelete()
    {
      var all = await localHueClient.GetRooms();
      var existing = all.Data.Where(x => x.Metadata.Name == "unittest").FirstOrDefault();

      Guid? deleteId = null;
      if(existing == null)
      {
        BaseResourceRequest req = new BaseResourceRequest() { Metadata = new Models.Metadata() { Name = "unittest" } };
        var result = await localHueClient.CreateRoom(req);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.HasErrors);

        deleteId = result.Data.First().Rid;
      }

      if (deleteId.HasValue)
      {
        var deleteResult = await localHueClient.DeleteRoom(deleteId.Value);

        Assert.IsNotNull(deleteResult);
        Assert.IsFalse(deleteResult.HasErrors);

        Assert.IsTrue(deleteResult.Data.Count == 1);
        Assert.AreEqual(deleteResult, deleteResult.Data.First().Rid);
      }

    }
  }
}
