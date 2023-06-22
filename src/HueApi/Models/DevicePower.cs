using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class DevicePower : HueResource
  {
    [JsonPropertyName("power_state")]
    public PowerState? PowerState { get; set; }
  }

  public class PowerState
  {
    /// <summary>
    /// normal – battery level is sufficient – low – battery level low, some features (e.g. software update) might stop working, please change battery soon – critical – battery level critical, device can fail any moment
    /// </summary>
    [JsonPropertyName("battery_state")]
    public BatteryState? BatteryState { get; set; }

    /// <summary>
    /// The current battery state in percent, only for battery powered devices.
    /// </summary>
    [JsonPropertyName("battery_level")]
    public int? BatteryLevel { get; set; }
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum BatteryState
  {
    normal, low, critical
  }
}

