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
  public class SceneTests
  {
    private readonly LocalHueApi localHueClient;

    public SceneTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<SceneTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.GetScenes();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.GetScenes();
      var id = all.Data.First().Id;

      var result = await localHueClient.GetScene(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.GetScenes();
      var id = all.Data.Last().Id;

      UpdateScene req = new UpdateScene()
      {
      };
      var result = await localHueClient.UpdateScene(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task CreateAndDelete()
    {
      var all = await localHueClient.GetScenes();
      var existing = all.Data.Where(x => x.Metadata?.Name == "unittest").FirstOrDefault();

      Guid? deleteId = null;
      if(existing == null)
      {
        //TODO: Get Group
        ResourceIdentifier group = new ResourceIdentifier();

        CreateScene req = new CreateScene(null, new Models.Metadata() { Name = "unittest" }, group);
        var result = await localHueClient.CreateScene(req);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.HasErrors);

        deleteId = result.Data.First().Rid;
      }

      if (deleteId.HasValue)
      {
        var deleteResult = await localHueClient.DeleteScene(deleteId.Value);

        Assert.IsNotNull(deleteResult);
        Assert.IsFalse(deleteResult.HasErrors);

        Assert.IsTrue(deleteResult.Data.Count == 1);
        Assert.AreEqual(deleteResult, deleteResult.Data.First().Rid);
      }

    }
  }
}
