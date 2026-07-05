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

    /// <summary>
    /// Used with Light
    /// Function of the lightservice (one of functional, decorative, mixed, unknown)
    /// </summary>
    [JsonPropertyName("function")]
    public string? Function { get; set; }

    /// <summary>
    /// Used with Light
    /// A fixed mired value of the white lamp (minimum: 50 – maximum: 1000)
    /// </summary>
    [JsonPropertyName("fixed_mired")]
    public int? FixedMired { get; set; }

    /// <summary>
    /// Used with Scene and SmartScene
    /// Application specific data. Free format string (minLength: 1 – maxLength: 16)
    /// </summary>
    [JsonPropertyName("appdata")]
    public string? Appdata { get; set; }

    /// <summary>
    /// Used with BehaviorScript
    /// Category of the behavior script (one of automation, entertainment, accessory, other)
    /// </summary>
    [JsonPropertyName("category")]
    public string? Category { get; set; }
  }
}
