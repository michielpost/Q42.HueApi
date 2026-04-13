using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class Zone : HueResource
  {
    [JsonPropertyName("children")]
    public List<ResourceIdentifier> Children { get; set; } = new();

  }
}
