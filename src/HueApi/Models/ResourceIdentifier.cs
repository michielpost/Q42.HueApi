using System.Diagnostics;
using System.Text.Json.Serialization;

namespace HueApi.Models
{
  [DebuggerDisplay("{Rtype} | {Rid}")]
  public record ResourceIdentifier
  {
    [JsonPropertyName("rid")]
    public Guid Rid { get; set; }

    [JsonPropertyName("rtype")]
    public string Rtype { get; set; } = default!;
  }
}
