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
  public class GroupTest
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
    public async Task CreateGroup()
    {
      List<string> lights = new List<string>() { "1", "2" };

      string groupId = await _client.CreateGroupAsync(lights);

      Assert.IsFalse(string.IsNullOrEmpty(groupId));

      //Get group and check lights
      var group = await _client.GetGroupAsync(groupId);

      await _client.DeleteGroupAsync(groupId);


      Assert.IsTrue(group.Lights.Any());
    }

    [TestMethod]
    public async Task DeleteGroup()
    {
      string groupId = "16";

      await _client.DeleteGroupAsync(groupId);

    }

    [TestMethod]
    public async Task GetGroups()
    {
      var groups = await _client.GetGroupsAsync();

       Assert.IsTrue(groups.Any());
     
    }

    [TestMethod]
    public async Task GetGroupTest()
    {
      var group = await _client.GetGroupAsync("1");

      Assert.IsNotNull(group);


    }

    [TestMethod]
    public async Task UpdateGroupTest()
    {
      var lights = await _client.GetLightsAsync();

      List<string> newLights = new List<string>();
      newLights.Add(lights.First().Id);
      newLights.Add(lights.Last().Id);

      await _client.UpdateGroupAsync("1", newLights, "test update");

      var group = await _client.GetGroupAsync("1");


      Assert.IsNotNull(group);
      Assert.IsTrue(group.Lights.Count == 2);

    }
  }
}
