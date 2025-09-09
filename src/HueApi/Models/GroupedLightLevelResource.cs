using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class GroupedLightLevelResource : HueResource
  {
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    [JsonPropertyName("light")]
    public LightLevel? LightLevel { get; set; }
  }

  public class LightLevel
  {
    [JsonPropertyName("light_level_report")]
    public LightLevelReport? LightLevelReport { get; set; }
  }

  public class LightLevelReport
  {
    [JsonPropertyName("Changed")]
    public DateTimeOffset changed { get; set; }

    [JsonPropertyName("light_level")]
    public int LightLevel { get; set; }

  }
}
