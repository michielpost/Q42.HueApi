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
  public class HueApiTests
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
	public void ValidHueIpTest()
	{
		_client = new HueClient("127.0.0.1");

		Assert.IsNotNull(_client);
	}

	[TestMethod]
	[ExpectedException(typeof(System.Exception))]
	public void InValidHueIpTest()
	{
		_client = new HueClient("//127.0.0@@.1in.v.alid");
	}


    [TestMethod]
    public async Task CheckConnectionTest()
    {
      var result = await _client.CheckConnection();

      Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task CheckConnectionWrongIpTest()
    {
      IHueClient client = new HueClient("42.1.1.1", ConfigurationManager.AppSettings["key"].ToString());

      var result = await client.CheckConnection();

      Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CheckConnectionWrongKeyTest()
    {
      IHueClient client = new HueClient(ConfigurationManager.AppSettings["ip"].ToString(), "wrongkey123");

      var result = await client.CheckConnection();

      Assert.IsFalse(result);
    }
  }
}
