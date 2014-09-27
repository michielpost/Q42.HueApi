using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using System.Globalization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Q42.HueApi.Tests
{
  [TestClass]
  public class HueDateTimeTests
  {

		//Deserializiation Tests
    [TestMethod]
    public void AbsoluteTimeHueDateTimeType()
    {
      string json = "{ \"name\": \"some name\",\"description\": \"\",\"time\": \"2014-09-20T19:35:26\"}";

      Schedule schedule = JsonConvert.DeserializeObject<Schedule>(json);

      Assert.IsNotNull(schedule);
      Assert.IsNotNull(schedule.HueTime);
			Assert.IsTrue(schedule.HueTime.DateTime.HasValue);
			Assert.AreEqual(new DateTime(2014,9,20,19,35,26), schedule.HueTime.DateTime.Value);			
    }

    [TestMethod]
    public void RandomizedDateTimeType()
    {
      string json = "{ \"name\": \"some name\",\"description\": \"\",\"time\": \"2014-09-20T19:35:26A00:30:00\"}";

      Schedule schedule = JsonConvert.DeserializeObject<Schedule>(json);

      Assert.IsNotNull(schedule);
      Assert.IsNotNull(schedule.HueTime);
			Assert.IsTrue(schedule.HueTime.DateTime.HasValue);
			Assert.AreEqual(new DateTime(2014, 9, 20, 19, 35, 26), schedule.HueTime.DateTime.Value);
			Assert.AreEqual(new TimeSpan(0, 30, 0), schedule.HueTime.RandomizedTime);
    }

    [TestMethod]
    public void RecurringDateTimeType()
    {
      string json = "{ \"name\": \"some name\",\"description\": \"\",\"time\": \"W32/T19:45:00\"}";

      Schedule schedule = JsonConvert.DeserializeObject<Schedule>(json);

      Assert.IsNotNull(schedule);
      Assert.IsNotNull(schedule.HueTime);
			Assert.IsNull(schedule.HueTime.RandomizedTime);
			Assert.IsTrue(schedule.HueTime.TimerTime.HasValue);
			Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.HueTime.TimerTime.Value);
			Assert.AreEqual(RecurringDay.RecurringTuesday, schedule.HueTime.RecurringDay); //W32
    }

    [TestMethod]
    public void RecurringRandomizedDateTimeType()
    {
      string json = "{ \"name\": \"some name\",\"description\": \"\",\"time\": \"W127/T19:45:00A00:30:00\"}";

      Schedule schedule = JsonConvert.DeserializeObject<Schedule>(json);

      Assert.IsNotNull(schedule);
      Assert.IsNotNull(schedule.HueTime);
			Assert.IsTrue(schedule.HueTime.TimerTime.HasValue);
			Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.HueTime.TimerTime.Value);
      Assert.AreEqual(new TimeSpan(0, 30, 0), schedule.HueTime.RandomizedTime);
			Assert.AreEqual(RecurringDay.RecurringAlldays, schedule.HueTime.RecurringDay); //W127
    }

    [TestMethod]
    public void NormalTimerDateTimeType()
    {
      string json = "{ \"name\": \"some name\",\"description\": \"\",\"time\": \"PT19:45:00\"}";

      Schedule schedule = JsonConvert.DeserializeObject<Schedule>(json);

      Assert.IsNotNull(schedule);
      Assert.IsNotNull(schedule.HueTime);
			Assert.IsNull(schedule.HueTime.RandomizedTime);
			Assert.IsTrue(schedule.HueTime.TimerTime.HasValue);
			Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.HueTime.TimerTime.Value);
    }

    [TestMethod]
    public void TimerRandomizedDateTimeType()
    {
      string json = "{ \"name\": \"some name\",\"description\": \"\",\"time\": \"PT19:45:00A00:30:00\"}";

      Schedule schedule = JsonConvert.DeserializeObject<Schedule>(json);

      Assert.IsNotNull(schedule);
      Assert.IsNotNull(schedule.HueTime);
			Assert.IsTrue(schedule.HueTime.TimerTime.HasValue);
			Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.HueTime.TimerTime.Value);
			Assert.AreEqual(new TimeSpan(0, 30, 0), schedule.HueTime.RandomizedTime);
    }

    [TestMethod]
    public void RecurringTimerDateTimeType()
    {
      string json = "{ \"name\": \"some name\",\"description\": \"\",\"time\": \"R65/PT19:45:00\"}";

      Schedule schedule = JsonConvert.DeserializeObject<Schedule>(json);

      Assert.IsNotNull(schedule);
      Assert.IsNotNull(schedule.HueTime);
			Assert.IsNull(schedule.HueTime.RandomizedTime);
			Assert.IsTrue(schedule.HueTime.TimerTime.HasValue);
			Assert.IsTrue(schedule.HueTime.NumberOfRecurrences.HasValue);
			Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.HueTime.TimerTime.Value);
			Assert.AreEqual<int?>(65, schedule.HueTime.NumberOfRecurrences.Value);
			
    }

    [TestMethod]
    public void RecurringTimerRandomizedDateTimeType()
    {
      string json = "{ \"name\": \"some name\",\"description\": \"\",\"time\": \"R65/PT19:45:00A00:30:00\"}";

      Schedule schedule = JsonConvert.DeserializeObject<Schedule>(json);

			Assert.IsNotNull(schedule);
			Assert.IsNotNull(schedule.HueTime);			
			Assert.IsTrue(schedule.HueTime.TimerTime.HasValue);
			Assert.IsTrue(schedule.HueTime.NumberOfRecurrences.HasValue);
			Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.HueTime.TimerTime.Value);
			Assert.AreEqual<int?>(65, schedule.HueTime.NumberOfRecurrences.Value);
			Assert.AreEqual(new TimeSpan(0, 30, 0), schedule.HueTime.RandomizedTime);
    }
		
		//SerializationTests
		[TestMethod]
		public void SerializeRecurringTimerRandomizedDateTimeType()
		{
			string expectedResult = "\"R65/PT19:45:00A00:30:00\"";

			HueDateTime hueDateTime = new HueDateTime();
			hueDateTime.NumberOfRecurrences = 65;
			hueDateTime.TimerTime = new TimeSpan(19, 45, 00);
			hueDateTime.RandomizedTime = new TimeSpan(00, 30, 00);

			string result = JsonConvert.SerializeObject(hueDateTime, new JsonConverter[]{new HueDateTimeConverter()});

			Assert.IsNotNull(result);
			Assert.AreEqual(expectedResult, result);
		}
  }
}
