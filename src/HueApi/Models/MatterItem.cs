using System.Diagnostics;
using System.Text.Json.Serialization;

namespace HueApi.Models
{
  [DebuggerDisplay("{Type} | {IdV1} | {Id}")]
  public class MatterItem : HueResource
  {
    [JsonPropertyName("max_fabrics")]
    public int MaxFabrics { get; set; } = default!;

    [JsonPropertyName("has_qr_code")]
    public bool HasQrCode { get; set; }

    [JsonPropertyName("software_version_string")]
    public string? SoftwareVersionString { get; set; }

  }
}
