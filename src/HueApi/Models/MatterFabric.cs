using System.Diagnostics;
using System.Text.Json.Serialization;

namespace HueApi.Models
{
  [DebuggerDisplay("{Type} | {IdV1} | {Id}")]
  public class MatterFabric : MatterItem
  {
    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;

    [JsonPropertyName("fabric_data")]
    public FabricData? FabricData { get; set; } = default!;

  }
  public class FabricData
  {
    [JsonPropertyName("label")]
    public string Label { get; set; } = default!;

    [JsonPropertyName("vendor_id")]
    public int VendorId { get; set; } = default!;
  }
}
