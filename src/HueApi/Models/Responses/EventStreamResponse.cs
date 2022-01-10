using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Responses
{
  public class EventStreamData : HueResource
  {
    [JsonPropertyName("owner")]
    public ResourceIdentifier? Owner { get; set; }

  }

  public class EventStreamResponse : HueResource
  {
    [JsonPropertyName("creationtime")]
    public new DateTimeOffset CreationTime { get; set; }

    [JsonPropertyName("data")]
    public List<EventStreamData> Data { get; set; } = new();

  }

}
