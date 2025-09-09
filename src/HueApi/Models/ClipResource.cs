using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class ClipResource : HueResource
  {

    /// <summary>
    /// A list of all public resource types supported by the bridge.
    /// </summary>
    [JsonPropertyName("resources")]
    public List<string> Resources { get; set; } = new();
  }

}
