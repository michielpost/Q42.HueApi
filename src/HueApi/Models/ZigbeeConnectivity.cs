using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class ZigbeeConnectivity : HueResource
  {
    /// <summary>
    /// connected if device has been recently been available. When indicating connectivity issues the device is powered off or has network issues When indicating unidirectional incoming the device only talks to bridge
    /// </summary>
    [JsonPropertyName("status")]
    public ConnectivityStatus? Status { get; set; }

    [JsonPropertyName("mac_address")]
    public string? MacAddress { get; set; }
  }

  
  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum ConnectivityStatus
  {
    connected, disconnected, connectivity_issue, unidirectional_incoming
  }
}
