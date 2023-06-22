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
  }

  public class Motion
  {
    [JsonPropertyName("motion")]
    public bool MotionState { get; set; }

    [JsonPropertyName("motion_valid")]
    public bool MotionValid { get; set; }
  
  }
}
