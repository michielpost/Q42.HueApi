using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class BridgeHome : HueResource
  {
    /// <summary>
    /// Child devices/services to group by the derived group
    /// </summary>
    [JsonPropertyName("children")]
    public List<ResourceIdentifier> Children { get; set; } = new();
  }
}
