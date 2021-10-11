using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using System.Globalization;
using System.Net.Http;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Q42.HueApi.Tests
{
  [TestClass]
  public class ScheduleTests
  {
    private IHueClient _client;
    private string key;

    [TestInitialize]
    public void Initialize()
    {
      string ip = ConfigurationManager.AppSettings["ip"].ToString();
      key = ConfigurationManager.AppSettings["key"].ToString();

	  _client = new LocalHueClient(ip, key);
    }

    [TestMethod]
    public async Task GetAll()
    {
      var result = await _client.GetSchedulesAsync();

      Assert.AreNotEqual(0, result.Count);
    }

    [TestMethod]
    public async Task GetSingle()
    {
      var all = await _client.GetSchedulesAsync();

      Assert.IsNotNull(all);
      Assert.IsTrue(all.Any());

      var single = await _client.GetScheduleAsync(all.First().Id);

      Assert.IsNotNull(single);
    }

    [TestMethod]
    public async Task GetSingleDebugTest()
    {
      var single = await _client.GetScheduleAsync("1");

      Assert.IsNotNull(single);
    }

    [TestMethod]
    public async Task DeleteSingleDebugTest()
    {
      //Delete
      var deleteResult = await _client.DeleteScheduleAsync("1");

      Assert.IsNotNull(deleteResult);
    }


    [TestMethod]
    public async Task CreateScheduleSingle()
    {
      Schedule schedule = new Schedule();
      schedule.Name = "t1";
      schedule.Description = "test";
      schedule.LocalTime = new HueDateTime()
      {
        TimerTime = TimeSpan.FromHours(10),
        RecurringDay = RecurringDay.RecurringMonday | RecurringDay.RecurringThursday
			};
      schedule.Command = new InternalBridgeCommand();

      var commandBody = new LightCommand();
      commandBody.Alert = Alert.Once;
      schedule.Command.Body = commandBody;
      schedule.Command.Address = "/api/huelandspoor/lights/5/state";
      schedule.Command.Method = HttpMethod.Put;

      var result = await _client.CreateScheduleAsync(schedule);

      Assert.IsNotNull(result);
    }

	[TestMethod]
	public async Task CreateGenericScheduleSingle()
	{
		Schedule schedule = new Schedule();
		schedule.Name = "t1";
		schedule.Description = "test";
		schedule.LocalTime = new HueDateTime()
		{
			DateTime = DateTime.Now.AddDays(1)
		};
		schedule.Command = new InternalBridgeCommand();

		dynamic dynamicCOmmand = new ExpandoObject();
		dynamicCOmmand.status = 1;

		var jsonString = JsonConvert.SerializeObject(dynamicCOmmand);
		var commandBody = new GenericScheduleCommand(jsonString);
		schedule.Command.Body = commandBody;
		schedule.Command.Address = $"/api/{key}/lights/5/state";
		schedule.Command.Method = HttpMethod.Put;

		var result = await _client.CreateScheduleAsync(schedule);

		Assert.IsNotNull(result);
	}

		[TestMethod]
		public async Task DontClearIdWhenUpdatingSchedule()
		{
			Schedule schedule = new Schedule();
			schedule.Id = "1";
			schedule.Name = "test";
      schedule.Status = ScheduleStatus.Enabled;

			await _client.UpdateScheduleAsync(schedule.Id, schedule);

			Assert.IsNotNull(schedule.Id);

		}

		[TestMethod]
    public async Task UpdateSchedule()
    {
      Schedule schedule = new Schedule();
      schedule.Name = "t1";
      schedule.Description = "test";
      schedule.LocalTime = new HueDateTime
			{
				DateTime = DateTime.UtcNow.AddDays(1)
			};
      schedule.Command = new InternalBridgeCommand();
      var commandBody = new LightCommand();
      commandBody.Alert = Alert.Once;
      schedule.Command.Body = commandBody;
      schedule.Command.Address = $"/api/{key}/lights/5/state";
      schedule.Command.Method = HttpMethod.Put;

      var scheduleId = await _client.CreateScheduleAsync(schedule);

      //Update name
      schedule.Name = "t2";
      await _client.UpdateScheduleAsync(scheduleId, schedule);

      //Get saved schedule
      var savedSchedule = await _client.GetScheduleAsync(scheduleId);

      //Check 
      Assert.AreEqual(schedule.Name, savedSchedule.Name);

    }

    [TestMethod]
    public async Task DeleteSchedule()
    {
      Schedule schedule = new Schedule();
      schedule.Name = "t1";
      schedule.Description = "test";
      schedule.LocalTime = new HueDateTime()
			{
				DateTime = DateTime.UtcNow.AddDays(1)
			};
      schedule.Command = new InternalBridgeCommand();
      var commandBody = new LightCommand();
      commandBody.Alert = Alert.Once;
      schedule.Command.Body = commandBody;
      schedule.Command.Address = $"/api/{key}/lights/5/state";
      schedule.Command.Method = HttpMethod.Put;

      var scheduleId = await _client.CreateScheduleAsync(schedule);

      //Delete
      await _client.DeleteScheduleAsync(scheduleId);

    }
   
   
  }
}
