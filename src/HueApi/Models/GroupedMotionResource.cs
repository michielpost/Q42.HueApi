using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class GroupedMotionResource : HueResource
  {
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    [JsonPropertyName("motion")]
    public Motion? Motion { get; set; }
  }

  public class Motion
  {
    [JsonPropertyName("motion_report")]
    public MotionReport? MotionReport { get; set; }
  }

  public class MotionReport
  {
    [JsonPropertyName("changed")]
    public DateTimeOffset changed { get; set; }

    [JsonPropertyName("motion")]
    public bool motion { get; set; }

  }
}
