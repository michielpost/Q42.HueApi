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

	  _client = new LocalHueClient(ip, key);
    }

    [TestMethod]
    public async Task GetBridge()
    {
      var result = await _client.GetBridgeAsync();

      Assert.IsFalse(string.IsNullOrEmpty(result.Config.Name));
    }

    [TestMethod]
    public async Task GetWhiteList()
    {
      string ip = ConfigurationManager.AppSettings["ip"].ToString();
      var noApiKeyclient = new LocalHueClient(ip);
      var result = await noApiKeyclient.GetWhiteListAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(string.IsNullOrEmpty(result.First().Id));
    }

    [TestMethod]
    public async Task GetConfig()
    {
      string ip = ConfigurationManager.AppSettings["ip"].ToString();
      var noApiKeyclient = new LocalHueClient(ip);
      var result = await noApiKeyclient.GetConfigAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(string.IsNullOrEmpty(result.BridgeId));
    }

    [TestMethod]
    public async Task ChangeConfig()
    {
      var newName = "test" + DateTime.Now.Second;

      var bridge = await _client.GetBridgeAsync();
      BridgeConfigUpdate update = new BridgeConfigUpdate();
      update.Name = newName;

      await _client.UpdateBridgeConfigAsync(update);

      var result = await _client.GetBridgeAsync();

      Assert.AreEqual(newName, result.Config.Name);

    }

	[TestMethod]
	public async Task UpdateFirmwareTest()
	{
		var config = await _client.GetBridgeAsync();

		//UpdateState 2 means there is an update available to apply
		Assert.AreEqual(2, config.Config.SoftwareUpdate.UpdateState);

		//Apply the update
		BridgeConfigUpdate update = new BridgeConfigUpdate();
		update.SoftwareUpdate = new SoftwareUpdate() { UpdateState = 3 }; 

		await _client.UpdateBridgeConfigAsync(update);

		var result = await _client.GetBridgeAsync();

		//Check if the bridge is updating
		Assert.AreEqual(3, result.Config.SoftwareUpdate.UpdateState);

	}

  }
}
