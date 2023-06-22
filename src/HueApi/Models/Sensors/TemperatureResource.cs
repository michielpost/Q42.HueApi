using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Sensors
{
  public class TemperatureResource : HueResource
  {
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = default!;

    [JsonPropertyName("motion")]
    public Temperature Temperature { get; set; } = default!;
  }

  public class Temperature
  {
    /// <summary>
    /// Temperature in 1.00 degrees Celsius
    /// </summary>
    [JsonPropertyName("temperature")]
    public int TemperatureValue { get; set; }

    [JsonPropertyName("temperature_valid")]
    public bool Temperature_valid { get; set; }

  }
}
