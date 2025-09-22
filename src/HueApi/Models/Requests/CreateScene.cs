using System.Text.Json.Serialization;

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
