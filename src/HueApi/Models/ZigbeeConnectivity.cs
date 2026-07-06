using System.Text.Json.Serialization;

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

    [JsonPropertyName("channel")]
    public ZigbeeChannel? Channel { get; set; }

    /// <summary>
    /// Extended pan id of the zigbee network (pattern: ^[0-9a-f]{16}$)
    /// </summary>
    [JsonPropertyName("extended_pan_id")]
    public string? ExtendedPanId { get; set; }
  }

  public class ZigbeeChannel
  {
    /// <summary>
    /// One of set, changing. Only used in GET responses, should not be set in PUT requests.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Current value of the zigbee channel. If recently changed, the value will reflect the channel that is currently being changed to.
    /// One of channel_11, channel_15, channel_20, channel_25, not_configured
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }
  }


  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum ConnectivityStatus
  {
    connected, disconnected, connectivity_issue, unidirectional_incoming, pending_discovery
  }
}
