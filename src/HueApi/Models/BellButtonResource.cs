using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class BellButtonResource : HueResource
  {
    [JsonPropertyName("button")]
    public BellButton? Button { get; set; }
  }

  public class BellButton
  {
    [JsonPropertyName("last_event")]
    public ButtonEvent? LastEvent { get; set; }

    [JsonPropertyName("button_report")]
    public ButtonReport? ButtonReport { get; set; }

    [JsonPropertyName("repeat_interval")]
    public int? RepeatInterval { get; set; }

    [JsonPropertyName("event_values")]
    public List<ButtonEvent>? EventValues { get; set; }
  }


}
