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

  }

  public class Recall
  {
    [JsonPropertyName("action")]
    public string? Action { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("duration")]
    public int? Duration { get; set; }

    [JsonPropertyName("dimming")]
    public Dimming? Dimming { get; set; }
  }
}
