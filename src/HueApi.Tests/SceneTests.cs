using HueApi.ColorConverters;
using HueApi.ColorConverters.Original.Extensions;
using HueApi.Extensions.cs;
using HueApi.Models;
using HueApi.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class SceneTests
  {
    private readonly LocalHueApi localHueClient;

    public SceneTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<SceneTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.Scene.GetAllAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.Scene.GetAllAsync();
      var id = all.Data.First().Id;

      var result = await localHueClient.Scene.GetByIdAsync(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.Scene.GetAllAsync();
      var id = all.Data.Last().Id;

      UpdateScene req = new UpdateScene()
      {
      };
      var result = await localHueClient.Scene.UpdateAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task ActivateScene()
    {
      var all = await localHueClient.Scene.GetAllAsync();
      var id = all.Data.Last().Id;

      UpdateScene req = new UpdateScene()
      {
        Recall = new Recall() { Action = SceneRecallAction.active }
      };
      var result = await localHueClient.Scene.UpdateAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task CreateAndDelete()
    {
      var all = await localHueClient.Scene.GetAllAsync();
      var groups = await localHueClient.Room.GetAllAsync();
      var group = groups.Data.Skip(1).First();
      var groupLights = await localHueClient.Room.GetByIdAsync(group.Id);
      var existing = all.Data.Where(x => x.Metadata?.Name == "unittest").FirstOrDefault();

      Guid? deleteId = null;
      if (existing == null)
      {
        CreateScene req = new CreateScene(new Models.Metadata() { Name = "unittest" }, group.ToResourceIdentifier());
        foreach (var light in groupLights.Data.SelectMany(x => x.Services.Where(x => x.Rtype == "light")))
        {
          req.Actions.Add(new SceneAction
          {
            Target = light,
            Action = new LightAction().TurnOn()
          });
        }

        var result = await localHueClient.Scene.CreateAsync(req);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.HasErrors);

        deleteId = result.Data.First().Rid;
      }

      if (deleteId.HasValue)
      {
        var deleteResult = await localHueClient.Scene.DeleteAsync(deleteId.Value);

        Assert.IsNotNull(deleteResult);
        Assert.IsFalse(deleteResult.HasErrors);

        Assert.IsTrue(deleteResult.Data.Count == 1);
        Assert.AreEqual(deleteId.Value, deleteResult.Data.First().Rid);
      }

    }


    [TestMethod]
    public async Task CreateDynamicAndActivate()
    {
      var all = await localHueClient.Scene.GetAllAsync();
      var groups = await localHueClient.Room.GetAllAsync();
      var group = groups.Data.First();
      var room = await localHueClient.Room.GetByIdAsync(group.Id);
      List<ResourceIdentifier> lights = new List<ResourceIdentifier>();
      foreach (var device in room.Data.First().Children)
      {
        var light = await localHueClient.Device.GetByIdAsync(device.Rid);
        lights.AddRange(light.Data.First().Services.Where(x => x.Rtype == "light").ToList());
      }

      var groupLights2 = await localHueClient.GroupedLight.GetByIdAsync(group.Services.First().Rid);
      var existing = all.Data.Where(x => x.Metadata?.Name == "testdynamic").FirstOrDefault();

      Guid? sceneId = existing?.Id;
      if (existing == null)
      {
        CreateScene req = new CreateScene(new Models.Metadata() { Name = "testdynamic" }, group.ToResourceIdentifier());
        foreach (var light in lights)
        {
          req.Actions.Add(new SceneAction
          {
            Target = light,
            Action = new LightAction().TurnOn()
          });
        }

        var result = await localHueClient.Scene.CreateAsync(req);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.HasErrors);

        sceneId = result.Data.First().Rid;
      }

      if (sceneId.HasValue)
      {
        //Create a dynamic scene
        UpdateScene reqDynamic = new UpdateScene()
        {
          Palette = new Palette()
          {
            Color = new List<ColorPalette>()
             {
               new ColorPalette { Color = new RGBColor("#00FF00").ToColor(), Dimming = new Dimming() { Brightness = 100 } }, //Green
               new ColorPalette { Color = new RGBColor("#FF0000").ToColor() }, //Red
               new ColorPalette { Color = new RGBColor("#0000FF").ToColor() } //Blue
             },
            Dimming = new System.Collections.Generic.List<Dimming>()
            {
              new Dimming() { Brightness = 90}
            }
          },
          Speed = 0.9
        };
        var resultDynamic = await localHueClient.Scene.UpdateAsync(sceneId.Value, reqDynamic);

        Assert.IsNotNull(resultDynamic);
        Assert.IsFalse(resultDynamic.HasErrors);



        //Activate scene
        UpdateScene req = new UpdateScene()
        {
          Recall = new Recall() { Action = SceneRecallAction.dynamic_palette }
        };
        var result = await localHueClient.Scene.UpdateAsync(sceneId.Value, req);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.HasErrors);
      }



    }
  }
}
