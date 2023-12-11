using System.Text.Json.Serialization;

namespace HueApi.Models.Sensors
{
  public class TemperatureResource : HueResource
  {
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = default!;

    [JsonPropertyName("temperature")]
    public Temperature Temperature { get; set; } = default!;
  }

  public class Temperature
  {
    [JsonPropertyName("temperature_valid")]
    public bool TemperatureValid { get; set; }

  }

  public class TemperatureReport
  {
    [JsonPropertyName("changed")]
    public DateTimeOffset Changed { get; set; }

    [JsonPropertyName("temperature")]
    public decimal Temperature { get; set; }

  }
}
