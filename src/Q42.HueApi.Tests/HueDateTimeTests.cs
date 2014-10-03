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
using Newtonsoft.Json.Linq;

namespace Q42.HueApi.Tests
{
	[TestClass]
	public class HueDateTimeTests
	{

		//Deserializiation Tests
		[TestMethod]
		public void AbsoluteTimeHueDateTimeType()
		{
			string timeValue = "\"2014-09-20T19:35:26\"";
			string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"time\": " + timeValue + "}";

			Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

			Assert.IsNotNull(schedule);
			Assert.IsNotNull(schedule.HueTime);
			Assert.IsTrue(schedule.HueTime.DateTime.HasValue);
			Assert.AreEqual(new DateTime(2014, 9, 20, 19, 35, 26), schedule.HueTime.DateTime.Value);

			string result = JsonConvert.SerializeObject(schedule.HueTime, new JsonConverter[] { new HueDateTimeConverter() });
			Assert.IsNotNull(result);
			Assert.AreEqual(timeValue, result);
		}

		[TestMethod]
		public void RandomizedDateTimeType()
		{
			string timeValue = "\"2014-09-20T19:35:26A00:30:00\"";
			string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"time\": " + timeValue + "}";

			Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

			Assert.IsNotNull(schedule);
			Assert.IsNotNull(schedule.HueTime);
			Assert.IsTrue(schedule.HueTime.DateTime.HasValue);
			Assert.AreEqual(new DateTime(2014, 9, 20, 19, 35, 26), schedule.HueTime.DateTime.Value);
			Assert.AreEqual(new TimeSpan(0, 30, 0), schedule.HueTime.RandomizedTime);

			string result = JsonConvert.SerializeObject(schedule.HueTime, new JsonConverter[] { new HueDateTimeConverter() });
			Assert.IsNotNull(result);
			Assert.AreEqual(timeValue, result);
		}

		[TestMethod]
		public void RecurringDateTimeType()
		{
			string timeValue = "\"W32/T19:45:00\"";
			string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"time\": " + timeValue + "}";

			Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

			Assert.IsNotNull(schedule);
			Assert.IsNotNull(schedule.HueTime);
			Assert.IsNull(schedule.HueTime.RandomizedTime);
			Assert.IsTrue(schedule.HueTime.TimerTime.HasValue);
			Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.HueTime.TimerTime.Value);
			Assert.AreEqual(RecurringDay.RecurringTuesday, schedule.HueTime.RecurringDay); //W32

			string result = JsonConvert.SerializeObject(schedule.HueTime, new JsonConverter[] { new HueDateTimeConverter() });
			Assert.IsNotNull(result);
			Assert.AreEqual(timeValue, result);
		}

		[TestMethod]
		public void RecurringRandomizedDateTimeType()
		{
			string timeValue = "\"W127/T19:45:00A00:30:00\"";
			string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"time\": " + timeValue + "}";

			Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

			Assert.IsNotNull(schedule);
			Assert.IsNotNull(schedule.HueTime);
			Assert.IsTrue(schedule.HueTime.TimerTime.HasValue);
			Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.HueTime.TimerTime.Value);
			Assert.AreEqual(new TimeSpan(0, 30, 0), schedule.HueTime.RandomizedTime);
			Assert.AreEqual(RecurringDay.RecurringAlldays, schedule.HueTime.RecurringDay); //W127

			string result = JsonConvert.SerializeObject(schedule.HueTime, new JsonConverter[] { new HueDateTimeConverter() });
			Assert.IsNotNull(result);
			Assert.AreEqual(timeValue, result);
		}

		[TestMethod]
		public void NormalTimerDateTimeType()
		{
			string timeValue = "\"PT19:45:00\"";
			string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"time\": " + timeValue + "}";

			Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

			Assert.IsNotNull(schedule);
			Assert.IsNotNull(schedule.HueTime);
			Assert.IsNull(schedule.HueTime.RandomizedTime);
			Assert.IsTrue(schedule.HueTime.TimerTime.HasValue);
			Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.HueTime.TimerTime.Value);

			string result = JsonConvert.SerializeObject(schedule.HueTime, new JsonConverter[] { new HueDateTimeConverter() });
			Assert.IsNotNull(result);
			Assert.AreEqual(timeValue, result);
		}

		[TestMethod]
		public void TimerRandomizedDateTimeType()
		{
			string timeValue = "\"PT19:45:00A00:30:00\"";
			string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"time\": " + timeValue + "}";

			Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

			Assert.IsNotNull(schedule);
			Assert.IsNotNull(schedule.HueTime);
			Assert.IsTrue(schedule.HueTime.TimerTime.HasValue);
			Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.HueTime.TimerTime.Value);
			Assert.AreEqual(new TimeSpan(0, 30, 0), schedule.HueTime.RandomizedTime);

			string result = JsonConvert.SerializeObject(schedule.HueTime, new JsonConverter[] { new HueDateTimeConverter() });
			Assert.IsNotNull(result);
			Assert.AreEqual(timeValue, result);
		}

		[TestMethod]
		public void RecurringTimerDateTimeType()
		{
			string timeValue = "\"R65/PT19:45:00\"";
			string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"time\": " + timeValue + "}";

			Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

			Assert.IsNotNull(schedule);
			Assert.IsNotNull(schedule.HueTime);
			Assert.IsNull(schedule.HueTime.RandomizedTime);
			Assert.IsTrue(schedule.HueTime.TimerTime.HasValue);
			Assert.IsTrue(schedule.HueTime.NumberOfRecurrences.HasValue);
			Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.HueTime.TimerTime.Value);
			Assert.AreEqual<int?>(65, schedule.HueTime.NumberOfRecurrences.Value);

			string result = JsonConvert.SerializeObject(schedule.HueTime, new JsonConverter[] { new HueDateTimeConverter() });
			Assert.IsNotNull(result);
			Assert.AreEqual(timeValue, result);

		}

		[TestMethod]
		public void RecurringTimerRandomizedDateTimeType()
		{
			string timeValue = "\"R65/PT19:45:00A00:30:00\"";
			string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"time\": " + timeValue + "}";			

			Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

			Assert.IsNotNull(schedule);
			Assert.IsNotNull(schedule.HueTime);
			Assert.IsTrue(schedule.HueTime.TimerTime.HasValue);
			Assert.IsTrue(schedule.HueTime.NumberOfRecurrences.HasValue);
			Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.HueTime.TimerTime.Value);
			Assert.AreEqual<int?>(65, schedule.HueTime.NumberOfRecurrences.Value);
			Assert.AreEqual(new TimeSpan(0, 30, 0), schedule.HueTime.RandomizedTime);

			string result = JsonConvert.SerializeObject(schedule.HueTime, new JsonConverter[] { new HueDateTimeConverter() });
			Assert.IsNotNull(result);
			Assert.AreEqual(timeValue, result);
		}
	}
}
