using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
