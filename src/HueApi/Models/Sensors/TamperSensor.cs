using System.Text.Json.Serialization;

namespace HueApi.Models.Sensors
{
  public class TamperSensor : HueResource
  {
    [JsonPropertyName("tamper_reports")]
    public List<TamperReports> TamperReports { get; set; } = new();
  }

  public class TamperReports
  {
    [JsonPropertyName("changed")]
    public DateTimeOffset Changed { get; set; }

    [JsonPropertyName("source")]
    public string? Source { get; set; }

    [JsonPropertyName("state")]
    public TamperState State { get; set; }

  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum TamperState
  {
    tampered, not_tampered
  }
}
