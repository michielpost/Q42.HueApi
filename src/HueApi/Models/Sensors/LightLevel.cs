using System.Text.Json.Serialization;

namespace HueApi.Models.Sensors
{
  public class LightLevel : HueResource
  {
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = default!;

    [JsonPropertyName("light")]
    public Light Light { get; set; } = default!;

  }

  public class Light
  {
    [JsonPropertyName("light_level_report")]
    public LightLevelReport LightLevelReport { get; set; } = default!;

  }

  public class LightLevelReport
  {

    [JsonPropertyName("changed")]
    public DateTimeOffset Changed { get; set; }

    /// <summary>
    /// Light level in 10000*log10(lux) +1 measured by sensor. Logarithmic scale used because the human eye adjusts to light levels and small changes at low lux levels are more noticeable than at high lux levels. This allows use of linear scale configuration sliders.
    /// </summary>
    [JsonPropertyName("light_level")]
    public int LightLevel { get; set; } = default!;

    public double LuxLevel
    {
      get
      {
        double lightLevel = LightLevel > 0 ? LightLevel - 1 : 0;
        lightLevel = lightLevel / 10000;
        return Math.Pow(10, lightLevel);
      }
    }
  }
}
