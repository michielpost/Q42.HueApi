using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class Room : HueResource
  {
    [JsonPropertyName("children")]
    public List<ResourceIdentifier> Children { get; set; } = new();

    [JsonPropertyName("grouped_services")]
    public List<ResourceIdentifier> GroupedServices { get; set; } = new();

  }
}
