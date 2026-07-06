using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class ZigbeeDeviceDiscovery : HueResource
  {
    [JsonPropertyName("status")]
    public ZigbeeDeviceDiscoveryStatus? Status { get; set; }

    [JsonPropertyName("action")]
    public ZigbeeDeviceDiscoveryAction? Action { get; set; }

  }

  public class ZigbeeDeviceDiscoveryAction
  {
    /// <summary>
    /// Possible action values you can set (one of search, search_allow_default_link_key). Only used in GET responses.
    /// </summary>
    [JsonPropertyName("action_type_values")]
    public List<string>? ActionTypeValues { get; set; }

    /// <summary>
    /// One of search, search_allow_default_link_key. Only used in PUT requests.
    /// </summary>
    [JsonPropertyName("action_type")]
    public string? ActionType { get; set; }

    /// <summary>
    /// Only used in PUT requests (maxItems: 10)
    /// </summary>
    [JsonPropertyName("search_codes")]
    public List<string>? SearchCodes { get; set; }

    /// <summary>
    /// One of all, primary (default: all). Ignored if search_codes are not provided. Only used in PUT requests.
    /// </summary>
    [JsonPropertyName("search_channels")]
    public string? SearchChannels { get; set; }
  }


  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum ZigbeeDeviceDiscoveryStatus
  {
    active, ready
  }
}
