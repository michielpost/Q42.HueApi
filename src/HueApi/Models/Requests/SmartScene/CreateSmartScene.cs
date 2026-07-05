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

    /// <summary>
    /// Duration of the transition from on one timeslot's scene to the other (defaults to 60000ms)
    /// </summary>
    [JsonPropertyName("transition_duration")]
    public int? TransitionDuration { get; set; }
  }
}
