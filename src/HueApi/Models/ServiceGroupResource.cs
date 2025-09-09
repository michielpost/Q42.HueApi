using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class ServiceGroupResource : HueResource
  {
    [JsonPropertyName("children")]
    public List<ResourceIdentifier>? Children { get; set; }
  }
}
