using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdateDevice : BaseResourceRequest
  {
    [JsonPropertyName("identify")]
    public Identify? Identify { get; set; }
  }

  public class Identify
  {
    [JsonPropertyName("action")]
    public string Action { get; set; } = "identify";
  }
}
