using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class ZigbeeDeviceDiscovery : HueResource
  {
    [JsonPropertyName("status")]
    public ZigbeeDeviceDiscoveryStatus? Status { get; set; }

  }


  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum ZigbeeDeviceDiscoveryStatus
  {
    active, ready
  }
}
