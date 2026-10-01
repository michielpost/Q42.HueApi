using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class MotionAreaCandidateResource : HueResource
  {
    [JsonPropertyName("capabilities")]
    public List<string>? Capabilities { get; set; }

    /// <summary>
    /// Array of MotionAreaCharacteristic
    /// </summary>
    [JsonPropertyName("characteristics")]
    public List<string>? Characteristics { get; set; }
  }
}
