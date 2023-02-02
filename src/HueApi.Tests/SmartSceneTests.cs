using HueApi.BridgeLocator;
using HueApi.ColorConverters;
using HueApi.ColorConverters.Original.Extensions;
using HueApi.Extensions.cs;
using HueApi.Models;
using HueApi.Models.Requests;
using HueApi.Models.Requests.SmartScene;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HueApi.Tests
{
  [TestClass]
  public class SmartSceneTests
  {
    private readonly LocalHueApi localHueClient;

    public SmartSceneTests()
    {
      var builder = new ConfigurationBuilder().AddUserSecrets<SceneTests>();
      var config = builder.Build();

      localHueClient = new LocalHueApi(config["ip"], key: config["key"]);
    }

    [TestMethod]
    public async Task Get()
    {
      var result = await localHueClient.GetSmartScenesAsync();

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);
    }

    [TestMethod]
    public async Task GetById()
    {
      var all = await localHueClient.GetSmartScenesAsync();
      var id = all.Data.First().Id;

      var result = await localHueClient.GetSmartSceneAsync(id);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);

    }


    [TestMethod]
    public async Task PutById()
    {
      var all = await localHueClient.GetSmartScenesAsync();
      var id = all.Data.Last().Id;

      UpdateSmartScene req = new UpdateSmartScene()
      {
      };
      var result = await localHueClient.UpdateSmartSceneAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task ActivateScene()
    {
      var all = await localHueClient.GetSmartScenesAsync();
      var id = all.Data.Last().Id;

      UpdateSmartScene req = new UpdateSmartScene()
      {
        Recall = new SmartSceneRecall() {  Action = SmartSceneRecallAction.activate }
      };
      var result = await localHueClient.UpdateSmartSceneAsync(id, req);

      Assert.IsNotNull(result);
      Assert.IsFalse(result.HasErrors);

      Assert.IsTrue(result.Data.Count == 1);
      Assert.AreEqual(id, result.Data.First().Rid);

    }

    [TestMethod]
    public async Task CreateAndDelete()
    {
      var all = await localHueClient.GetSmartScenesAsync();

      var scenes = await localHueClient.GetScenesAsync();
      var scene = scenes.Data.Last();

      var groups = await localHueClient.GetRoomsAsync();
      var group = groups.Data.Last();
      var existing = all.Data.Where(x => x.Metadata?.Name == "unittest").FirstOrDefault();

      Guid? deleteId = null;
      if (existing == null)
      {
        CreateSmartScene req = new CreateSmartScene()
        {
          Metadata = new Models.Metadata() { Name = "unittest" },
          Group = group.ToResourceIdentifier(),
          WeekTimeslots = new List<SmartSceneDayTimeslot>()
          {
            new SmartSceneDayTimeslot()
            {
               Timeslots = new List<SmartSceneTimeslot>()
               {
                 new SmartSceneTimeslot()
                 {
                    StartTime = new TimeslotStartTime()
                    {
                       Time = new TimeslotStartTimeTime()
                       {
                         Hour = 14,
                         Minute = 0,
                         Second = 0
                       }
                    },
                     Target = scene.ToResourceIdentifier()
                 }
               },
                Recurrence = new List<Weekday>()
                {
                  Weekday.thursday, Weekday.friday
                }
            }
          },
          Recall = new SmartSceneRecall() {  Action = SmartSceneRecallAction.activate }
        };

        var result = await localHueClient.CreateSmartSceneAsync(req);

        Assert.IsNotNull(result);
        Assert.IsFalse(result.HasErrors);

        deleteId = result.Data.First().Rid;
      }

      if (deleteId.HasValue)
      {
        var deleteResult = await localHueClient.DeleteSmartSceneAsync(deleteId.Value);

        Assert.IsNotNull(deleteResult);
        Assert.IsFalse(deleteResult.HasErrors);

        Assert.IsTrue(deleteResult.Data.Count == 1);
        Assert.AreEqual(deleteId.Value, deleteResult.Data.First().Rid);
      }

    }

   
  }
}
