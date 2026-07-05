using System.Text.Json.Serialization;

namespace HueApi.Models
{
  /// <summary>
  /// Switch input configuration services. These are offered by devices with configurable switch modes.
  /// </summary>
  public class SwitchInputConfigurationResource : HueResource
  {
    [JsonPropertyName("switch_mode")]
    public SwitchMode? SwitchMode { get; set; }

    /// <summary>
    /// List of associated services
    /// </summary>
    [JsonPropertyName("linked_services")]
    public List<ResourceIdentifier>? LinkedServices { get; set; }
  }

  public class SwitchMode
  {
    /// <summary>
    /// One of set, changing. Only used in GET responses, should not be set in PUT requests.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Current mode (on read) or requested mode (on write) of the switch.
    /// One of switch_single_rocker, switch_single_pushbutton, switch_dual_rocker, switch_dual_pushbutton
    /// </summary>
    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    /// <summary>
    /// The modes that the switch supports. Only used in GET responses.
    /// </summary>
    [JsonPropertyName("mode_values")]
    public List<string>? ModeValues { get; set; }
  }
}
