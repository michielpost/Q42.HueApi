using System.Text.Json.Serialization;

namespace HueApi.Models.Requests.SmartScene
{
  public class SmartSceneRecall
  {
    [JsonPropertyName("action")]
    public SmartSceneRecallAction Action { get; set; } = default!;
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum SmartSceneRecallAction
  {
    activate, deactivate
  }
}
