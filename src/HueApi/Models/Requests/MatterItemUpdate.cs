using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class MatterItemUpdate
  {
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; } = "matter_reset";
  }
}
