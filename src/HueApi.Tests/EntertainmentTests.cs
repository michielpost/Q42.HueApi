using HueApi.Models;
using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class EntertainmentServiceTests
  {
    private readonly LocalHueApi localHueClient;

    public EntertainmentServiceTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<EntertainmentServiceTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.GetEntertainmentServicesAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.GetEntertainmentServicesAsync();
      var id = all.Data.First().Id;

      var result = await localHueClient.GetEntertainmentServiceAsync(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.GetEntertainmentServicesAsync();
      var id = all.Data.Last().Id;

      UpdateEntertainment req = new UpdateEntertainment();
      var result = await localHueClient.UpdateEntertainmentServiceAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task StreamingIsNotActive()
    {
      //Optional: Check if streaming is currently active
      var entServices = await localHueClient.GetEntertainmentServicesAsync();
      Assert.IsNotNull(entServices.Data);

      var numSupported = entServices.Data.Sum(x => x.MaxStreams);

      var entConfigs = await localHueClient.GetEntertainmentConfigurationsAsync();
      Assert.IsNotNull(entConfigs.Data);

      var active = entConfigs.Data.Where(x => x.Status == EntertainmentConfigurationStatus.active).Count();

      var streamingChannelsLeft = numSupported - active;

      Console.WriteLine($"{streamingChannelsLeft} our of {numSupported} streaming channels left");

      Assert.IsTrue(streamingChannelsLeft > 0);

    }
  }
}
