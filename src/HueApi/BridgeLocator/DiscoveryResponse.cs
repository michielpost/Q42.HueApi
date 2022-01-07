using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.BridgeLocator
{
  public class DiscoveryResponse
  {
    [JsonPropertyName("id")]
    public string Id { get; set; } = default!;

    [JsonPropertyName("internalipaddress")]
    public string InternalIpAddress { get; set; } = default!;

    [JsonPropertyName("port")]
    public int Port { get; set; }
  }
}
