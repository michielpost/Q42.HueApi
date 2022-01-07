using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class Metadata
  {
    [JsonPropertyName("archetype")]
    public string Archetype { get; set; } = default!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
  }
}
