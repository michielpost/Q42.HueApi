using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class BaseResourceRequest
  {
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata? Metadata { get; set; }
  }
}
