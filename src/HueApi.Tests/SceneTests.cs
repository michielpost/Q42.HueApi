using HueApi.BridgeLocator;
using HueApi.Extensions.cs;
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
      var result = await localHueClient.GetScenesAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.GetScenesAsync();
      var id = all.Data.First().Id;

      var result = await localHueClient.GetSceneAsync(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.GetScenesAsync();
      var id = all.Data.Last().Id;

      UpdateScene req = new UpdateScene()
      {
      };
      var result = await localHueClient.UpdateSceneAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task CreateAndDelete()
    {
      var all = await localHueClient.GetScenesAsync();
      var groups = await localHueClient.GetRoomsAsync();
      var group = groups.Data.Skip(1).First();
      var groupLights = await localHueClient.GetRoomAsync(group.Id);
      var existing = all.Data.Where(x => x.Metadata?.Name == "unittest").FirstOrDefault();

      Guid? deleteId = null;
      if(existing == null)
      {
        CreateScene req = new CreateScene(new Models.Metadata() { Name = "unittest" }, group.ToResourceIdentifier());
        foreach(var light in groupLights.Data.SelectMany(x => x.Services.Where(x => x.Rtype == "light")))
        {
          req.Actions.Add(new SceneAction
          {
             Target = light,
              Action = new LightAction().TurnOn()
          });
        }

        var result = await localHueClient.CreateSceneAsync(req);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.HasErrors);

        deleteId = result.Data.First().Rid;
      }

      if (deleteId.HasValue)
      {
        var deleteResult = await localHueClient.DeleteSceneAsync(deleteId.Value);

        Assert.IsNotNull(deleteResult);
        Assert.IsFalse(deleteResult.HasErrors);

        Assert.IsTrue(deleteResult.Data.Count == 1);
        Assert.AreEqual(deleteId.Value, deleteResult.Data.First().Rid);
      }

    }
  }
}
