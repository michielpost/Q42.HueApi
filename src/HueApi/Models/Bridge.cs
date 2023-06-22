using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class Bridge : HueResource
  {
    [JsonPropertyName("bridge_id")]
    public string? BridgeId { get; set; }

    [JsonPropertyName("time_zone")]
    public TimeZoneConfig? TimeZone { get; set; }
  }

  public class TimeZoneConfig
  {
    /// <summary>
    /// Time zone where the user's home is located (as Olson ID).
    /// </summary>
    [JsonPropertyName("time_zone")]
    public string? TimeZone { get; set; }
  }
}
