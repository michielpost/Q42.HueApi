using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class GroupedLightLevelTests
  {
    private readonly LocalHueApi localHueClient;

    public GroupedLightLevelTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<GroupedLightLevelTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.GetGroupedLightLevelsAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.GetGroupedLightLevelsAsync();
      var id = all.Data.First().Id;

      var result = await localHueClient.GetGroupedLightLevelAsync(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.GetGroupedLightLevelsAsync();
      var id = all.Data.Last().Id;

      var req = new UpdateGroupedLightLevelRequest();
      var result = await localHueClient.UpdateGroupedLightLevelAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }
  }
}
