using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class RelativeRotaryResource : HueResource
  {
    [JsonPropertyName("relative_rotary")]
    public RelativeRotary? RelativeRotary { get; set; }
  }

  public class RelativeRotary
  {
    /// <summary>
    /// Deprecated: renamed to rotary_report
    /// </summary>
    [JsonPropertyName("last_event")]
    public RelativeRotaryLastEvent? LastEvent { get; set; }

    [JsonPropertyName("rotary_report")]
    public RelativeRotaryReport? RotaryReport { get; set; }
  }

  public class RelativeRotaryReport
  {
    /// <summary>
    /// Last time the value of this property is updated.
    /// </summary>
    [JsonPropertyName("updated")]
    public DateTimeOffset Updated { get; set; }

    /// <summary>
    /// Indicate which type of rotary event is received
    /// </summary>
    [JsonPropertyName("action")]
    public RelativeRotaryLastEventAction? Action { get; set; }

    [JsonPropertyName("rotation")]
    public RelativeRotaryLastEventRotation? Rotation { get; set; }
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
