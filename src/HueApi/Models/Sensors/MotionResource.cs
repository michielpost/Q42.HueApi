using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Sensors
{
  public class MotionResource : HueResource
  {
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = default!;

    [JsonPropertyName("motion")]
    public Motion Motion { get; set; } = default!;

    [JsonPropertyName("sensitivity")]
    public Sensitivity? Sensitivity { get; set; }
  }

  public class Sensitivity
  {
    [JsonPropertyName("status")]
    public SensitivityStatus Status { get; set; } = default!;

    [JsonPropertyName("sensitivity")]
    public int SensitivityValue { get; set; }

    [JsonPropertyName("sensitivity_max")]
    public int? SensitivityMax { get; set; }
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum SensitivityStatus
  {
    set, changing
  }

  public class Motion
  {
    [JsonPropertyName("motion_report")]
    public MotionReport? MotionReport { get; set; }
  }

  public class MotionReport
  {
    [JsonPropertyName("changed")]
    public DateTimeOffset Changed { get; set; }

    /// <summary>
    /// true if motion is detected
    /// </summary>
    [JsonPropertyName("motion")]
    public bool Motion { get; set; }

  }
}
