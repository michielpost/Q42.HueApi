using System.Text.Json.Serialization;

namespace HueApi.Models.Requests.SmartScene
{
  public class CreateSmartScene : BaseResourceRequest
  {
    [JsonPropertyName("group")]
    public ResourceIdentifier? Group { get; set; }

    [JsonPropertyName("week_timeslots")]
    public List<SmartSceneDayTimeslot> WeekTimeslots { get; set; } = default!;

    [JsonPropertyName("active_timeslot")]
    public ActiveTimeslot ActiveTimeslot { get; set; } = default!;

    [JsonPropertyName("recall")]
    public SmartSceneRecall Recall { get; set; } = default!;
  }
}
