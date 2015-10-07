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
    private ILocalHueClient _client;

    [TestInitialize]
    public void Initialize()
    {
      string ip = ConfigurationManager.AppSettings["ip"].ToString();
      string key = ConfigurationManager.AppSettings["key"].ToString();

	  _client = new LocalHueClient(ip, key);
    }

	[TestMethod]
	public void ValidHueIpTest()
	{
		_client = new LocalHueClient("127.0.0.1");

		Assert.IsNotNull(_client);
	}

	[TestMethod]
	[ExpectedException(typeof(System.Exception))]
	public void InValidHueIpTest()
	{
		_client = new LocalHueClient("//127.0.0@@.1in.v.alid");
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
		ILocalHueClient client = new LocalHueClient("42.1.1.1", ConfigurationManager.AppSettings["key"].ToString());

      var result = await client.CheckConnection();

      Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CheckConnectionWrongKeyTest()
    {
		ILocalHueClient client = new LocalHueClient(ConfigurationManager.AppSettings["ip"].ToString(), "wrongkey123");

      var result = await client.CheckConnection();

      Assert.IsFalse(result);
    }
  }
}
