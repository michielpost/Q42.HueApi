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
  public class SensorTests
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
      var result = await _client.GetSensorsAsync();

      Assert.AreNotEqual(0, result.Count());
    }

    [TestMethod]
    public async Task DeleteSensorAsyncTest()
    {
      var result = await _client.DeleteSensorAsync("1");

      Assert.IsNotNull(result);

    }

	[TestMethod]
	public async Task ChangeSensorConfigAsyncTest()
	{
		var result = await _client.ChangeSensorConfigAsync("1", new SensorConfig() { On = false, Reachable = true, Pending = new List<string>() { "test"} });

		Assert.IsNotNull(result);

	}

	}
}
