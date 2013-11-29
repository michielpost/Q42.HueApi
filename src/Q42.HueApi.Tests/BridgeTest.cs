using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;

namespace Q42.HueApi.Tests
{
  [TestClass]
  public class BridgeTest
  {
    private IHueClient _client;

    [TestInitialize]
    public void Initialize()
    {
      string ip = ConfigurationManager.AppSettings["ip"].ToString();
      string key = ConfigurationManager.AppSettings["key"].ToString();

      _client = new HueClient(ip, key);
    }

    [TestMethod]
    public async Task GetConfig()
    {
      var result = await _client.GetBridgeAsync();

      Assert.IsFalse(string.IsNullOrEmpty(result.Config.Name));
    }

    [TestMethod]
    public async Task ChangeConfig()
    {
      var newName = Guid.NewGuid().ToString();

      var bridge = await _client.GetBridgeAsync();
      BridgeConfig update = bridge.Config;
      update.Name = "test1";

      await _client.UpdateBridgeConfigAsync(update);

      var result = await _client.GetBridgeAsync();

      Assert.AreEqual(newName, result.Config.Name);

    }
  }
}
