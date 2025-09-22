using System.Text.Json.Serialization;

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
