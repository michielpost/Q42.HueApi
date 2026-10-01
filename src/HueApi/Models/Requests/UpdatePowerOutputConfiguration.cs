using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdatePowerOutputConfiguration : BaseResourceRequest
  {
    /// <summary>
    /// Requested output mode of the power output configuration. Only the mode property should be set.
    /// </summary>
    [JsonPropertyName("output_mode")]
    public PowerOutputMode? OutputMode { get; set; }
  }
}
