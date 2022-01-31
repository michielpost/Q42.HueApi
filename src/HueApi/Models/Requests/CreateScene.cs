using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Requests
{
  public class CreateScene : BaseResourceRequest
  {
    public CreateScene(Metadata metadata, ResourceIdentifier group)
    {
      Metadata = metadata;
      Group = group;
    }

    [JsonPropertyName("group")]
    public ResourceIdentifier Group { get; set; }

    [JsonPropertyName("actions")]
    public List<SceneAction> Actions { get; set; } = new();

    [JsonPropertyName("palette")]
    public Palette? Palette { get; set; }

    [JsonPropertyName("speed")]
    public double? Speed { get; set; }

  }
 
}
