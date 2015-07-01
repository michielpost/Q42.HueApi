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
    public async Task CreateOrUpdate()
    {
      var result = await _client.CreateOrUpdateSceneAsync("scene1", "test", new List<string> { "2" });

      Assert.AreNotEqual(0, result.Count);
    }

    [TestMethod]
    public async Task ModifyScene()
    {
      var result = await _client.ModifySceneAsync("scene1", "2", new LightCommand().TurnOn());

      Assert.AreNotEqual(0, result.Count);
    }

  }
}
