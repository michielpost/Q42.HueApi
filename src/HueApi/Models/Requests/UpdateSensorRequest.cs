using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdateSensorRequest : BaseResourceRequest
  {
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

  }
}
