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
  public class RuleTests
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
    public async Task GetAll()
    {
      var result = await _client.GetRulesAsync();

      Assert.AreNotEqual(0, result.Count());
    }

    [TestMethod]
    public async Task GetSingle()
    {
      var result = await _client.GetRuleAsync("1");

      Assert.IsNotNull(result);
    }


  
  }
}
