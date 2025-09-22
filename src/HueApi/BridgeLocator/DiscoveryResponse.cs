using System.Text.Json.Serialization;

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
