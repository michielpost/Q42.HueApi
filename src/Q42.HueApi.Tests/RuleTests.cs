using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using System.Globalization;
using System.Net.Http;

namespace Q42.HueApi.Tests
{
  [TestClass]
  public class RuleTests
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

    [TestMethod]
    public async Task CreateRuleTest()
    {
      Rule rule = new Rule()
      {
        Name = "test",
        Actions = new List<InternalBridgeCommand>() { 
          new InternalBridgeCommand() { Address = "/groups/0/action", Body = new SceneCommand() { Scene = "S3" }, Method = HttpMethod.Put } ,
          new InternalBridgeCommand() { Address = "/groups/1/action", Body = new LightCommand() { On = true }, Method = HttpMethod.Put } 
        },
        Conditions = new List<RuleCondition>() { new RuleCondition() { Address = "/sensors/2/state/buttonevent", Operator = RuleOperator.Equal, Value = "16" } }
      };

      var result = await _client.CreateRule(rule);

      Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task UpdateRuleTest()
    {
      Rule rule = new Rule()
      {
        Id = "10",
        Name = "test",
        Actions = new List<InternalBridgeCommand>() {
          new InternalBridgeCommand() { Address = "/groups/0/action", Body = new SceneCommand() { Scene = "S3" }, Method = HttpMethod.Put } ,
          new InternalBridgeCommand() { Address = "/groups/1/action", Body = new LightCommand() { On = true }, Method = HttpMethod.Put }
        },
        Conditions = new List<RuleCondition>() { new RuleCondition() { Address = "/sensors/2/state/buttonevent", Operator = RuleOperator.Equal } }
      };

      var result = await _client.UpdateRule(rule);

      Assert.IsNotNull(result);
    }



  }
}
