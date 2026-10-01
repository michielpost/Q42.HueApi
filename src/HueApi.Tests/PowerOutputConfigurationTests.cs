using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class PowerOutputConfigurationTests
  {
    private readonly LocalHueApi localHueClient;

    public PowerOutputConfigurationTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<PowerOutputConfigurationTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.PowerOutputConfiguration.GetAllAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.PowerOutputConfiguration.GetAllAsync();
      var id = all.Data.First().Id;

      var result = await localHueClient.PowerOutputConfiguration.GetByIdAsync(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
    }

    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.PowerOutputConfiguration.GetAllAsync();
      var id = all.Data.Last().Id;

      UpdatePowerOutputConfiguration req = new UpdatePowerOutputConfiguration();
      var result = await localHueClient.PowerOutputConfiguration.UpdateAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);
    }
  }
}
