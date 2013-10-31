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
    public async Task SetLightNameGroup()
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

   

  
  }
}
