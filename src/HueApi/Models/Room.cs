using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class Room : HueResource
  {
    [JsonPropertyName("children")]
    public List<ResourceIdentifier> Children { get; set; } = new();

    [JsonPropertyName("grouped_services")]
    public List<ResourceIdentifier> GroupedServices { get; set; } = new();

    /// <summary>
    /// Data describing geometry information of the room. Only positioning of device services is supported.
    /// </summary>
    [JsonPropertyName("geometry")]
    public ResourceGeometry? Geometry { get; set; }

  }
}
