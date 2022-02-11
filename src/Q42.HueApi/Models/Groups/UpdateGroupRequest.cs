using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace Q42.HueApi.Models.Groups
{
  internal class UpdateGroupRequest
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// The IDs of the lights that are in the group.
    /// </summary>
    [JsonProperty("lights")]
    public IEnumerable<string> Lights { get; set; }

    /// <summary>
    /// Category of the Room type. Default is "Other".
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [JsonProperty("class")]
    public RoomClass? Class { get; set; }
  }
}
