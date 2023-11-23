using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Requests
{
  public class UpdateScene : BaseResourceRequest
  {
    [JsonPropertyName("actions")]
    public List<SceneAction>? Actions { get; set; }

    [JsonPropertyName("recall")]
    public Recall? Recall { get; set; }

    [JsonPropertyName("palette")]
    public Palette? Palette { get; set; }

    [JsonPropertyName("speed")]
    public double? Speed { get; set; }

    /// <summary>
    /// Indicates whether to automatically start the scene dynamically on active recall
    /// </summary>
    [JsonPropertyName("auto_dynamic")]
    public bool? AutoDynamic { get; set; }

  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum SceneRecallAction
  {
    active, dynamic_palette
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum SceneRecallStatus
  {
    active, dynamic_palette
  }

  public class Recall
  {
    [JsonPropertyName("action")]
    public SceneRecallAction? Action { get; set; }

    [JsonPropertyName("status")]
    public SceneRecallStatus? Status { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("dimming")]
    public Dimming? Dimming { get; set; }
  }
}
