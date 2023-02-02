using Microsoft.VisualStudio.TestTools.UnitTesting;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.HSB;
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
    protected IHueClient _client;

    [TestInitialize]
    public void Initialize()
    {
      string ip = ConfigurationManager.AppSettings["ip"].ToString();
      string key = ConfigurationManager.AppSettings["key"].ToString();

      _client = new LocalHueClient(ip, key);
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
      //Get all lights
      var result = await _client.GetLightsAsync();

      Assert.IsNotNull(result);

    }

    [TestMethod]
    public async Task GetLightAsyncTest()
    {
      //Get single light
      var result = await _client.GetLightAsync("2");

      Assert.IsNotNull(result);


    }

    [TestMethod]
    public async Task UpdateLightConfigAsync()
    {
      //var updateResult = await _client.LightConfigUpdate("2", new Models.LightConfigUpdate() { Startup = new LightStartup() { Mode = StartupMode.LastOnState } });
      var updateResult = await _client.LightConfigUpdate("2", new Models.LightConfigUpdate() { Startup = new LightStartup() {  CustomSettings = new LightCommand() { Brightness = 150 } } });
      Assert.IsFalse(updateResult.Errors.Any());


      //Get single light
      var result = await _client.GetLightAsync("2");
      Assert.IsNotNull(result);
      Assert.AreEqual(result.Config.Startup.Mode, StartupMode.LastOnState);


    }


    [TestMethod]
    public async Task SendCommandAsync()
    {
      //Create command
      var command = new LightCommand();
      command.TurnOn();
      command.SetColor(new RGBColor("#225566"));

      List<string> lights = new List<string>() { "1", "2", "3" };

      //Send Command
      var result = await _client.SendCommandAsync(command);
      var result2 = await _client.SendCommandAsync(command, lights);

      Assert.IsTrue(result.Count > 0 && result.Any(r => r.Error == null));
      Assert.IsTrue(result2.Count > 0 && result2.Any(r => r.Error == null));

    }

    [TestMethod]
    public async Task SendColorLoopCommandAsync()
    {
      //Create command
      var command = new LightCommand();
      command.TurnOn();
      command.Effect = Effect.ColorLoop;

      List<string> lights = new List<string>() { "8", "9" };

      //Send Command
      var result = await _client.SendCommandAsync(command, lights);

      Assert.IsTrue(result.Count > 0 && result.Any(r => r.Error == null));

    }

    [TestMethod]
    public async Task DeleteLightsAsyncTest()
    {
      var result = await _client.DeleteLightAsync("1");

      Assert.IsNotNull(result);

    }
  
  }
}
