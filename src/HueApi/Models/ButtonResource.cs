using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class ButtonResource : HueResource
  {
    [JsonPropertyName("button")]
    public Button? Button { get; set; }
  }

  public class Button
  {
    [JsonPropertyName("button_report")]
    public ButtonReport? ButtonReport { get; set; }

    [JsonPropertyName("repeat_interval")]
    public int? RepeatInterval { get; set; }

    [JsonPropertyName("event_values")]
    public List<ButtonEvent>? EventValues { get; set; }
  }

  public class ButtonReport
  {
    [JsonPropertyName("updated")]
    public DateTimeOffset? Updated { get; set; }

    [JsonPropertyName("event")]
    public ButtonEvent? Event { get; set; }
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum ButtonEvent
  {
    initial_press, repeat, short_release, long_release, double_short_release, long_press
  }
}
