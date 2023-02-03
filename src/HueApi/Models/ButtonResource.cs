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
    [JsonPropertyName("last_event")]
    public ButtonLastEvent? LastEvent { get; set; }
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum ButtonLastEvent
  {
    initial_press, repeat, short_release, long_release, double_short_release, long_press
  }
}
