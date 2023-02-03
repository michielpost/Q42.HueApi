using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class RelativeRotaryResource : HueResource
  {
    [JsonPropertyName("relative_rotary")]
    public RelativeRotary? RelativeRotary { get; set; }
  }

  public class RelativeRotary
  {
    [JsonPropertyName("last_event")]
    public RelativeRotaryLastEvent? LastEvent { get; set; }
  }

  public class RelativeRotaryLastEvent
  {
    [JsonPropertyName("action")]
    public RelativeRotaryLastEventAction? Action { get; set; }

    [JsonPropertyName("rotation")]
    public RelativeRotaryLastEventRotation? Rotation { get; set; }
  }

  public class RelativeRotaryLastEventRotation
  {
    /// <summary>
    /// A rotation opposite to the previous rotation, will always start with new start command.
    /// </summary>
    [JsonPropertyName("direction")]
    public RelativeRotaryDirection? Direction { get; set; }

    /// <summary>
    /// amount of rotation since previous event in case of repeat, amount of rotation since start in case of a start_event. Resolution = 1000 steps / 360 degree rotation.
    /// </summary>
    [JsonPropertyName("steps")]
    public int? Steps { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum RelativeRotaryDirection
  {
    clock_wise, counter_clock_wise
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum RelativeRotaryLastEventAction
  {
    start, repeat
  }
}
