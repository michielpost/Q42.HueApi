using HueApi.ColorConverters.Original.Extensions;
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
      var id = all.Data.Last().Id;

      var result = await localHueClient.GetEntertainmentConfigurationAsync(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

      //Turn all lights in this entertainment group on
      var entServices = result.Data.First().Locations.ServiceLocations.Select(x => x.Service?.Rid).ToList();
      var allResources = await localHueClient.GetResourcesAsync();

      var devices = allResources.Data.Where(x => entServices.Contains(x.Id)).Select(x => x.Owner?.Rid).ToList();

      var lights = allResources.Data.Where(x => devices.Contains(x.Id)).Select(x => x.Services?.Where(x => x.Rtype == "light").FirstOrDefault()?.Rid).ToList();

      UpdateLight update = new UpdateLight()
        .TurnOn()
        .SetColor(new ColorConverters.RGBColor("FF0000"));

      foreach(var light in lights.Where(x => x.HasValue))
      {
        await localHueClient.UpdateLightAsync(light!.Value, update);
      }

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.GetEntertainmentConfigurationsAsync();
      var current = all.Data.Last();
      var id = current.Id;

      UpdateEntertainmentConfiguration req = new UpdateEntertainmentConfiguration();
      req.Locations = current.Locations;
      //foreach(var location in current.Locations.ServiceLocations)
      //{
      //  //location.EqualizationFactor = null;
      //  location.Service.Rtype = null;
      //}

      var result = await localHueClient.UpdateEntertainmentConfigurationAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task SetGradientLightStripLocation()
    {
      var all = await localHueClient.GetEntertainmentConfigurationsAsync();
      var current = all.Data.Last();

      await SetAndCheckPositions(current, new HuePosition(-0.5, -0.2, 0), new HuePosition(-0.1, -0.1, 0));

    }

    private async Task SetAndCheckPositions(EntertainmentConfiguration current, HuePosition pos1, HuePosition pos2)
    {
      var id = current.Id;

      UpdateEntertainmentConfiguration req = new UpdateEntertainmentConfiguration();
      req.Locations = current.Locations;
      req.Locations.ServiceLocations[0].Positions[0] = pos1;
      req.Locations.ServiceLocations[0].Positions[1] = pos2;

      var result = await localHueClient.UpdateEntertainmentConfigurationAsync(id, req);
      Assert.IsFalse(result.HasErrors);

      var newConfig = await localHueClient.GetEntertainmentConfigurationAsync(id);

      Assert.IsNotNull(newConfig);
      Assert.AreEqual(newConfig.Data.First().Locations.ServiceLocations[0].Positions[0], pos1);
      Assert.AreEqual(newConfig.Data.First().Locations.ServiceLocations[0].Positions[1], pos2);

      Assert.AreEqual(newConfig.Data.First().Channels[0].Position, pos1);
      Assert.AreEqual(newConfig.Data.First().Channels[2].Position, pos2);
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
