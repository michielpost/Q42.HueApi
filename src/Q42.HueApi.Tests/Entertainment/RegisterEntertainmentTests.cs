using Microsoft.VisualStudio.TestTools.UnitTesting;
using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Tests.Entertainment
{
  [TestClass]
  public class RegisterEntertainmentTests
  {
    private ILocalHueClient _client;

    [TestInitialize]
    public void Initialize()
    {
      string ip = ConfigurationManager.AppSettings["ip"].ToString();
      string key = ConfigurationManager.AppSettings["key"].ToString();
      string entertainmentKey = ConfigurationManager.AppSettings["streamingKey"]?.ToString();

      _client = new LocalHueClient(ip, key, entertainmentKey);
    }

    [TestMethod]
    public async Task GetClientKey()
    {
      string ip = ConfigurationManager.AppSettings["ip"].ToString();
      ip = "10.42.39.194";
      var client = new LocalHueClient(ip);
      var result = await client.RegisterAsync("unittest", "getclientkey", true);

      Assert.IsTrue(!string.IsNullOrWhiteSpace(result.StreamingClientKey));
      Assert.IsTrue(!string.IsNullOrWhiteSpace(result.Username));
    }

    [TestMethod]
    public async Task CreateEntertainmentGroup()
    {
      var groups = await _client.GetGroupsAsync();
      if(!groups.Where(x => x.Type == Models.Groups.GroupType.Entertainment).Any())
      {
        var allLights = await _client.GetLightsAsync();
        var createResult = await _client.CreateGroupAsync(allLights.Select(x => x.Id), "Entertainment", Models.Groups.RoomClass.TV, Models.Groups.GroupType.Entertainment);

        Assert.IsNotNull(createResult);
      }

    }

    [TestMethod]
    public async Task EnableStreaming()
    {
      var groups = await _client.GetGroupsAsync();
      var entertainmentGroup = groups.Where(x => x.Type == Models.Groups.GroupType.Entertainment).First();
      var enableResult = await _client.SetStreamingAsync(entertainmentGroup.Id);

      Assert.IsNotNull(enableResult);

    }
  }
}
