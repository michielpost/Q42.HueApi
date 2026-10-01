using System.Text.Json.Serialization;

namespace HueApi.Models
{
  /// <summary>
  /// Power output configuration services. These are offered by devices that can control the power output of connected lights.
  /// </summary>
  public class PowerOutputConfigurationResource : HueResource
  {
    /// <summary>
    /// List of associated services
    /// </summary>
    [JsonPropertyName("linked_services")]
    public List<ResourceIdentifier>? LinkedServices { get; set; }

    [JsonPropertyName("output_mode")]
    public PowerOutputMode? OutputMode { get; set; }
  }

  public class PowerOutputMode
  {
    /// <summary>
    /// One of set, changing. Only used in GET responses, should not be set in PUT requests.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Current mode (on read) or requested mode (on write) of the (linked) light service.
    /// One of controllable, always_on
    /// </summary>
    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    /// <summary>
    /// The modes that the device supports. Only used in GET responses.
    /// </summary>
    [JsonPropertyName("mode_values")]
    public List<string>? ModeValues { get; set; }
  }
}
