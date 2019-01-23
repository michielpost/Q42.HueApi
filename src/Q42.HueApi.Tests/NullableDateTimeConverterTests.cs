using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Q42.HueApi.Converters;

namespace Q42.HueApi.Tests
{
  [TestClass]
  public class NullableDateTimeConverterTests
  {
    [TestMethod]
    public void Handle_Regular_ISO8601_Value_Test()
    {
      string timeValue = "\"2014-09-20T19:35:26\"";
      string jsonString = "{\"Value\":" + timeValue + "}";

      var testSubject = JsonConvert.DeserializeObject<TestSubject>(jsonString);

      Assert.IsNotNull(testSubject);
      Assert.IsNotNull(testSubject.Value);
      Assert.AreEqual(new DateTime(2014, 9, 20, 19, 35, 26), testSubject.Value);

      string result = JsonConvert.SerializeObject(testSubject, new NullableDateTimeConverter());
      Assert.IsNotNull(result);
      Assert.AreEqual(jsonString, result);
    }

    [TestMethod]
    public void Handle_Custom_None_Value_Test()
    {
      string timeValue = "\"none\"";
      string jsonString = "{\"Value\":" + timeValue + "}";

      var testSubject = JsonConvert.DeserializeObject<TestSubject>(jsonString);

      Assert.IsNotNull(testSubject);
      Assert.IsNull(testSubject.Value);

      string result = JsonConvert.SerializeObject(testSubject, new NullableDateTimeConverter());
      Assert.IsNotNull(result);
      Assert.AreEqual("{\"Value\":null}", result);
    }
  }

  public class TestSubject
  {
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? Value { get; set; }
  }
}
