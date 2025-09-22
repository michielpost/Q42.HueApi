using System.Text.Json.Serialization;

namespace HueApi.Models.Responses
{
  public class EventStreamData : HueResource
  {

  }

  public class EventStreamResponse : HueResource
  {
    [JsonPropertyName("creationtime")]
    public new DateTimeOffset CreationTime { get; set; }

    [JsonPropertyName("data")]
    public List<EventStreamData> Data { get; set; } = new();

  }

}
