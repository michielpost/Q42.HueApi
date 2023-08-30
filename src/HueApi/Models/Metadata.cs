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
    public string? Archetype { get; set; }

    [JsonPropertyName("image")]
    public ResourceIdentifier? Image { get; set; }

    /// <summary>
    /// Used with Button
    /// control identifier of the switch which is unique per device. Meaning in combination with type – dots Number of dots – number Number printed on device – other a logical order of controls in switch
    /// </summary>
    [JsonPropertyName("control_id")]
    public int? ControlId { get; set; }
  }
}
