using Microsoft.VisualStudio.TestTools.UnitTesting;
using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Tests
{
  [TestClass]
  public class LightsTests
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
    public async Task SetLightNameTest()
    {
      await _client.SetLightNameAsync("1", "test");
      var result = await _client.GetLightAsync("1");
      Assert.AreEqual("test", result.Name);

      await _client.SetLightNameAsync("1", "test change");
      result = await _client.GetLightAsync("1");
      Assert.AreEqual("test change", result.Name);


    }

    [TestMethod]
    public async Task GetNewLightsTest()
    {
      //Search for new lights
      //await _client.SearchNewLightsAsync();

      //Get new lights found
      var newLights = await _client.GetNewLightsAsync();

      Assert.AreEqual(0, newLights.Count);

    }

    [TestMethod]
    public async Task SearchNewLightsAsyncTest()
    {
      //Search for new lights
      await _client.SearchNewLightsAsync();

    }

    [TestMethod]
    public async Task SearchNewLightsWithDeviceIdsAsyncTest()
    {
      List<string> deviceIds = new List<string>();
      deviceIds.Add("45AF34");
      deviceIds.Add("543636");
      deviceIds.Add("34AFBE");

      //Search for new lights
      await _client.SearchNewLightsAsync(deviceIds);

    }

    [TestMethod]
    public async Task GetLightsAsyncTest()
    {
      //Search for new lights
      await _client.GetLightsAsync();

    }


    [TestMethod]
    public async Task SendCommandAsync()
    {
      //Create command
      var command = new LightCommand();
      command.TurnOn();
      command.SetColor("#225566");

      List<string> lights = new List<string>();

      //Send Command
      await _client.SendCommandAsync(command);
      await _client.SendCommandAsync(command, lights);

    }

   

  
  }
}
