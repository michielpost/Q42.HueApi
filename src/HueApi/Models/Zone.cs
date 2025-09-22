using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class Zone : HueResource
  {
    [JsonPropertyName("children")]
    public List<ResourceIdentifier> Children { get; set; } = new();

    /// <summary>
    /// https://developers.meethue.com/develop/hue-api-v2/api-reference/#resource_zone_get
    /// </summary>
    [Obsolete("References to aggregated control services Deprecated: use services")]
    [JsonPropertyName("grouped_services")]
    public List<ResourceIdentifier> GroupedServices { get; set; } = new();

  }
}
