using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdateSwitchInputConfiguration : BaseResourceRequest
  {
    /// <summary>
    /// Requested mode of the switch. Only the mode property should be set.
    /// </summary>
    [JsonPropertyName("switch_mode")]
    public SwitchMode? SwitchMode { get; set; }
  }
}
