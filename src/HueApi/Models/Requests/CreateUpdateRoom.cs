using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class CreateUpdateRoom : BaseResourceRequest
  {
    /// <summary>
    /// Child devices/services to group by the derived group. Required on create.
    /// </summary>
    [JsonPropertyName("children")]
    public List<ResourceIdentifier>? Children { get; set; }

    /// <summary>
    /// Data describing geometry information of the room. Only positioning of device services is supported.
    /// </summary>
    [JsonPropertyName("geometry")]
    public ResourceGeometry? Geometry { get; set; }
  }
}
