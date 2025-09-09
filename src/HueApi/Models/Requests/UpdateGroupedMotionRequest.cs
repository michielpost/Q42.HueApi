using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdateGroupedMotionRequest : BaseResourceRequest
  {
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }
  }
}
