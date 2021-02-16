using Newtonsoft.Json;
using Q42.HueApi.Converters;
using System;

namespace Q42.HueApi
{
  public class WhiteList
  {
    public string Id { get; set; }

    [JsonProperty("last use date")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? LastUsedDate { get; set; }

    [JsonProperty("create date")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? CreateDate { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

  }
}
