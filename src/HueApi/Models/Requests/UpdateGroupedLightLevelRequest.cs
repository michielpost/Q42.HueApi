using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdateGroupedLightLevelRequest : BaseResourceRequest
  {
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }
  }
}
