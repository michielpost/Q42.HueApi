using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Groups;
using Newtonsoft.Json;

namespace Q42.HueApi.Tests
{
  [TestClass]
  public class GroupTest
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
    public async Task CreateGroup()
    {
      var lights = await _client.GetLightsAsync();

      List<string> newLights = new List<string>();
      newLights.Add(lights.First().Id);
      newLights.Add(lights.Last().Id);

      string groupId = await _client.CreateGroupAsync(newLights, groupType: GroupType.LightGroup);

      Assert.IsFalse(string.IsNullOrEmpty(groupId));

      //Get group and check lights
      var group = await _client.GetGroupAsync(groupId);

      await _client.DeleteGroupAsync(groupId);

      Assert.IsTrue(group.Lights.Any());
      Assert.AreEqual<int>(newLights.Count, group.Lights.Count, "Should have the same number of lights");
    }

    [TestMethod]
    public async Task CreateGroupWithName()
    {
      //make sure you have lights 1 and 2 in your HUE environment
      List<string> lights = new List<string>() { "1", "2" };
      string groupName = "testgroupName";
      string groupId = await _client.CreateGroupAsync(lights, groupName);

      Assert.IsFalse(string.IsNullOrEmpty(groupId));
      Assert.IsFalse(string.IsNullOrEmpty(groupName));

      //Get group and check lights
      var group = await _client.GetGroupAsync(groupId);

      //cleanup
      await _client.DeleteGroupAsync(groupId);

      Assert.IsTrue(group.Lights.Any());
      Assert.AreEqual<int>(lights.Count, group.Lights.Count, "Should have the same number of lights");
      Assert.IsNotNull(group.Name);
      Assert.AreEqual<string>(groupName, group.Name, "Name should be the same");
    }

    [TestMethod]
    public async Task DeleteGroup()
    {
      string groupId = "16";

      await _client.DeleteGroupAsync(groupId);

    }

    [TestMethod]
    public async Task SendGroupCommand()
    {
      var scenes = await _client.GetScenesAsync();
      var scene = scenes.First();

      var command = new SceneCommand { Scene = scene.Id };

      await _client.SendGroupCommandAsync(command, scene.Group);

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

      var grouap = await _client.GetGroupAsync("1");

      await _client.UpdateGroupAsync("1", lights.Select(x => x.Id).ToList(), "Keuken", RoomClass.Kitchen);

      var group = await _client.GetGroupAsync("1");

      Assert.IsNotNull(group);
      Assert.IsTrue(group.Lights.Count == lights.Count());

    }

    [TestMethod]
    public async Task UpdateEmptyGroupTest()
    {
      var oldGroup = await _client.GetGroupAsync("1");

      List<string> newLights = new List<string>();

      await _client.UpdateGroupAsync("1", newLights, "test update");

      var group = await _client.GetGroupAsync("1");

      Assert.IsNotNull(group);
      Assert.IsTrue(group.Lights.Count == 0);

    }

    [TestMethod]
    public async Task CreateRoomGroupWithName()
    {
      //make sure you have lights 1 and 2 in your HUE environment
      List<string> lights = new List<string>() { "1", "2" };
      string groupName = "testgroupName";
      string groupId = await _client.CreateGroupAsync(lights, groupName, Models.Groups.RoomClass.LivingRoom);

      Assert.IsFalse(string.IsNullOrEmpty(groupId));
      Assert.IsFalse(string.IsNullOrEmpty(groupName));

      //Get group and check lights
      var group = await _client.GetGroupAsync(groupId);

      //cleanup
      await _client.DeleteGroupAsync(groupId);

      Assert.IsTrue(group.Lights.Any());
      Assert.AreEqual<int>(lights.Count, group.Lights.Count, "Should have the same number of lights");
      Assert.IsNotNull(group.Name);
      Assert.AreEqual<string>(groupName, group.Name, "Name should be the same");
      Assert.AreEqual(RoomClass.LivingRoom, group.Class);
    }

    [TestMethod]
    public void EnumUnknownTest()
    {
      string json = @"{ ""type"": ""unknown""}";
      Group group = JsonConvert.DeserializeObject<Group>(json);

      Assert.IsNull(group.Type);

    }

    [TestMethod]
    public void EnumTest()
    {
      string json = @"{ ""type"": ""room""}";
      Group group = JsonConvert.DeserializeObject<Group>(json);

      Assert.AreEqual(GroupType.Room, group.Type);
    }

    [TestMethod]
    public void NewLightLocationTest()
    {
      var location = new LightLocation(1, 0, 0.5);

      Assert.IsNotNull(location);
      Assert.AreEqual(1, location.X);
      Assert.AreEqual(0, location.Y);
      Assert.AreEqual(0.5, location.Z);
    }
  }
}
