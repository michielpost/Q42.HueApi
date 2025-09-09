using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class CreateUpdateServiceGroup : BaseResourceRequest
  {
    [JsonPropertyName("children")]
    public List<ResourceIdentifier>? Children { get; set; }
  }
}
