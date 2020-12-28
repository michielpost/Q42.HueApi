using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using System.Globalization;

namespace Q42.HueApi.Tests
{
  [TestClass]
  public class SceneTests
  {
    private IHueClient _client;

    [TestInitialize]
    public void Initialize()
    {
      string ip = ConfigurationManager.AppSettings["ip"].ToString();
      string key = ConfigurationManager.AppSettings["key"].ToString();

      _client = new LocalHueClient(ip, key);
    }

    [TestMethod]
    public async Task GetAll()
    {
      var result = await _client.GetScenesAsync();

      Assert.AreNotEqual(0, result.Count());
    }

    [TestMethod]
    public async Task GetSingle()
    {
      var result = await _client.GetSceneAsync("VA8wtm8-L8JqqwZ");

      Assert.AreNotEqual(0, result.LightStates.Count);
    }

    [TestMethod]
    public async Task SceneCRUDTest()
    {
      Scene test = new Scene();
      test.Name = "scene1";
      test.Lights = new List<string> { "2" };

      var result = await _client.CreateSceneAsync(test);

      Assert.IsNotNull(result);

      //Get scene
      var newScene = await _client.GetSceneAsync(result);
      Assert.IsNotNull(newScene);

      var firstState = newScene.LightStates.First();
      firstState.Value.On = false;

      var modifyResult = await _client.UpdateSceneAsync(result, newScene);

      //Delete scene
      var deleteResult = await _client.DeleteSceneAsync(result);

      Assert.IsTrue(deleteResult.Any());

      var deletedScene = await _client.GetSceneAsync(result);
      Assert.IsNull(deletedScene);

    }

    [TestMethod]
    public async Task GroupSceneCRUDTest()
    {
      var groups = await _client.GetGroupsAsync();

      Scene test = new Scene();
      test.Name = "scene1";
      test.Group = groups.First().Id;
      test.Type = SceneType.GroupScene;

      var result = await _client.CreateSceneAsync(test);

      Assert.IsNotNull(result);

      //Get scene
      var newScene = await _client.GetSceneAsync(result);
      Assert.IsNotNull(newScene);

      var firstState = newScene.LightStates.First();
      firstState.Value.On = false;

      var modifyResult = await _client.UpdateSceneAsync(result, newScene);

      //Delete scene
      var deleteResult = await _client.DeleteSceneAsync(result);

      Assert.IsTrue(deleteResult.Any());

      var deletedScene = await _client.GetSceneAsync(result);
      Assert.IsNull(deletedScene);

    }

    [TestMethod]
    public async Task ModifyScene()
    {
      var result = await _client.ModifySceneAsync("VA8wtm8-L8JqqwZ", "2", new LightCommand() { Brightness = 60 });

      Assert.AreNotEqual(0, result.Count);
    }

    [TestMethod]
    public async Task UpdateScene()
    {
      Scene scene = new Scene()
      {
        Id = "scene1",
        Recycle = true,
        Name = "test"
      };

      var result = await _client.UpdateSceneAsync("scene1", scene);

      Assert.AreNotEqual(0, result.Count);
    }

  }
}
