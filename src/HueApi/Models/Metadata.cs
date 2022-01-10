using System.Diagnostics;
using System.Text.Json.Serialization;

namespace HueApi.Models
{
  [DebuggerDisplay("{Name} {Archetype}")]
  public class Metadata
  {
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("archetype")]
    public string? Archetype { get; set; } = default!;

    [JsonPropertyName("image")]
    public ResourceIdentifier? Image { get; set; }
  }
}
