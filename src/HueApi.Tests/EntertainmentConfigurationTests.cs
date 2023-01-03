using HueApi.Extensions.cs;
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
  public class EntertainmentConfigurationTests
  {
    private readonly LocalHueApi localHueClient;

    public EntertainmentConfigurationTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<EntertainmentConfigurationTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.GetEntertainmentConfigurationsAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.GetEntertainmentConfigurationsAsync();
      var id = all.Data.First().Id;

      var result = await localHueClient.GetEntertainmentConfigurationAsync(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.GetEntertainmentConfigurationsAsync();
      var id = all.Data.Last().Id;

      UpdateEntertainmentConfiguration req = new UpdateEntertainmentConfiguration();
      var result = await localHueClient.UpdateEntertainmentConfigurationAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task CreateGetDeleteEntertainmentConfiguration()
    {
      //Get light to use in the entertainment area
      var lights = await localHueClient.GetEntertainmentServicesAsync();

      UpdateEntertainmentConfiguration req = new UpdateEntertainmentConfiguration
      {
        Metadata = new Models.Metadata
        {
          Name = "Test Config",
        },
        ConfigurationType = Models.EntertainmentConfigurationType.other,
        Locations = new Models.Locations()
      };

      foreach (var light in lights.Data.Where(x => x.Renderer))
      {
        var lightPosition = new HueServiceLocation
        {
          Service = light.ToResourceIdentifier(),
          Positions = new System.Collections.Generic.List<HuePosition>
           {
             new HuePosition
             {
                X = 0.42, Y = 0.5, Z = 0
             }
           }
        };

        req.Locations.ServiceLocations.Add(lightPosition);
      }


      var result = await localHueClient.CreateEntertainmentConfigurationAsync(req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
      Assert.IsTrue(result.Data.Count == 1);

      var newId = result.Data.First().Rid;

      //Get it
      var getResult = await localHueClient.GetEntertainmentConfigurationAsync(newId);

      //Delete it
      var deleteResult = await localHueClient.DeleteEntertainmentConfigurationAsync(newId);


    }

  }

}
