using Newtonsoft.Json;
using Q42.HueApi.Converters;
using System;
using System.Runtime.Serialization;

namespace Q42.HueApi
{
  [DataContract]
  public class WhiteList
  {
    [DataMember(Name = "id")]
    public string Id { get; set; }

    [DataMember(Name = "last use date")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? LastUsedDate { get; set; }

    [DataMember(Name = "create date")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? CreateDate { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

  }
}
