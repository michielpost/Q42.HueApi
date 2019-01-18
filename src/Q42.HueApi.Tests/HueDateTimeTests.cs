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
using Q42.HueApi.Converters;

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
            string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"localtime\": " + timeValue + "}";

            Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

            Assert.IsNotNull(schedule);
            Assert.IsNotNull(schedule.LocalTime);
            Assert.IsTrue(schedule.LocalTime.DateTime.HasValue);
            Assert.AreEqual(new DateTime(2014, 9, 20, 19, 35, 26), schedule.LocalTime.DateTime.Value);

            string result = JsonConvert.SerializeObject(schedule.LocalTime, new JsonConverter[] { new HueDateTimeConverter() });
            Assert.IsNotNull(result);
            Assert.AreEqual(timeValue, result);
        }

        [TestMethod]
        public void RandomizedDateTimeType()
        {
            string timeValue = "\"2014-09-20T19:35:26A00:30:00\"";
            string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"localtime\": " + timeValue + "}";

            Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

            Assert.IsNotNull(schedule);
            Assert.IsNotNull(schedule.LocalTime);
            Assert.IsTrue(schedule.LocalTime.DateTime.HasValue);
            Assert.AreEqual(new DateTime(2014, 9, 20, 19, 35, 26), schedule.LocalTime.DateTime.Value);
            Assert.AreEqual(new TimeSpan(0, 30, 0), schedule.LocalTime.RandomizedTime);

            string result = JsonConvert.SerializeObject(schedule.LocalTime, new JsonConverter[] { new HueDateTimeConverter() });
            Assert.IsNotNull(result);
            Assert.AreEqual(timeValue, result);
        }

        [TestMethod]
        public void RecurringDateTimeType()
        {
            string timeValue = "\"W32/T19:45:00\"";
            string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"localtime\": " + timeValue + "}";

            Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

            Assert.IsNotNull(schedule);
            Assert.IsNotNull(schedule.LocalTime);
            Assert.IsNull(schedule.LocalTime.RandomizedTime);
            Assert.IsTrue(schedule.LocalTime.TimerTime.HasValue);
            Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.LocalTime.TimerTime.Value);
            Assert.AreEqual(RecurringDay.RecurringTuesday, schedule.LocalTime.RecurringDay); //W32

            string result = JsonConvert.SerializeObject(schedule.LocalTime, new JsonConverter[] { new HueDateTimeConverter() });
            Assert.IsNotNull(result);
            Assert.AreEqual(timeValue, result);
        }

        [TestMethod]
        public void RecurringRandomizedDateTimeType()
        {
            string timeValue = "\"W127/T19:45:00A00:30:00\"";
            string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"localtime\": " + timeValue + "}";

            Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

            Assert.IsNotNull(schedule);
            Assert.IsNotNull(schedule.LocalTime);
            Assert.IsTrue(schedule.LocalTime.TimerTime.HasValue);
            Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.LocalTime.TimerTime.Value);
            Assert.AreEqual(new TimeSpan(0, 30, 0), schedule.LocalTime.RandomizedTime);
            Assert.AreEqual(RecurringDay.RecurringAlldays, schedule.LocalTime.RecurringDay); //W127

            string result = JsonConvert.SerializeObject(schedule.LocalTime, new JsonConverter[] { new HueDateTimeConverter() });
            Assert.IsNotNull(result);
            Assert.AreEqual(timeValue, result);
        }

        [TestMethod]
        public void NormalTimerDateTimeType()
        {
            string timeValue = "\"PT19:45:00\"";
            string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"localtime\": " + timeValue + "}";

            Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

            Assert.IsNotNull(schedule);
            Assert.IsNotNull(schedule.LocalTime);
            Assert.IsNull(schedule.LocalTime.RandomizedTime);
            Assert.IsTrue(schedule.LocalTime.TimerTime.HasValue);
            Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.LocalTime.TimerTime.Value);

            string result = JsonConvert.SerializeObject(schedule.LocalTime, new JsonConverter[] { new HueDateTimeConverter() });
            Assert.IsNotNull(result);
            Assert.AreEqual(timeValue, result);
        }

        [TestMethod]
        public void TimerRandomizedDateTimeType()
        {
            string timeValue = "\"PT19:45:00A00:30:00\"";
            string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"localtime\": " + timeValue + "}";

            Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

            Assert.IsNotNull(schedule);
            Assert.IsNotNull(schedule.LocalTime);
            Assert.IsTrue(schedule.LocalTime.TimerTime.HasValue);
            Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.LocalTime.TimerTime.Value);
            Assert.AreEqual(new TimeSpan(0, 30, 0), schedule.LocalTime.RandomizedTime);

            string result = JsonConvert.SerializeObject(schedule.LocalTime, new JsonConverter[] { new HueDateTimeConverter() });
            Assert.IsNotNull(result);
            Assert.AreEqual(timeValue, result);
        }

        [TestMethod]
        public void RecurringTimerDateTimeType()
        {
            string timeValue = "\"R65/PT19:45:00\"";
            string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"localtime\": " + timeValue + "}";

            Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

            Assert.IsNotNull(schedule);
            Assert.IsNotNull(schedule.LocalTime);
            Assert.IsNull(schedule.LocalTime.RandomizedTime);
            Assert.IsTrue(schedule.LocalTime.TimerTime.HasValue);
            Assert.IsTrue(schedule.LocalTime.NumberOfRecurrences.HasValue);
            Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.LocalTime.TimerTime.Value);
            Assert.AreEqual<int?>(65, schedule.LocalTime.NumberOfRecurrences.Value);

            string result = JsonConvert.SerializeObject(schedule.LocalTime, new JsonConverter[] { new HueDateTimeConverter() });
            Assert.IsNotNull(result);
            Assert.AreEqual(timeValue, result);

        }

        [TestMethod]
        public void RecurringTimerRandomizedDateTimeType()
        {
            string timeValue = "\"R65/PT19:45:00A00:30:00\"";
            string jsonString = "{ \"name\": \"some name\",\"description\": \"\",\"localtime\": " + timeValue + "}";

            Schedule schedule = JsonConvert.DeserializeObject<Schedule>(jsonString);

            Assert.IsNotNull(schedule);
            Assert.IsNotNull(schedule.LocalTime);
            Assert.IsTrue(schedule.LocalTime.TimerTime.HasValue);
            Assert.IsTrue(schedule.LocalTime.NumberOfRecurrences.HasValue);
            Assert.AreEqual(new TimeSpan(19, 45, 00), schedule.LocalTime.TimerTime.Value);
            Assert.AreEqual<int?>(65, schedule.LocalTime.NumberOfRecurrences.Value);
            Assert.AreEqual(new TimeSpan(0, 30, 0), schedule.LocalTime.RandomizedTime);

            string result = JsonConvert.SerializeObject(schedule.LocalTime, new JsonConverter[] { new HueDateTimeConverter() });
            Assert.IsNotNull(result);
            Assert.AreEqual(timeValue, result);
        }
    }
}
