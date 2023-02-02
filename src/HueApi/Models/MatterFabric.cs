using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace HueApi.Models
{
  [DebuggerDisplay("{Type} | {IdV1} | {Id}")]
  public class MatterFabric : MatterItem
  {
    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;

    [JsonPropertyName("fabric_data")]
    public FabricData? FabricData { get; set; } = default!;

    [JsonPropertyName("creation_time")]
    public DateTimeOffset CreationTime { get; set; } = default!;

  }
  public class FabricData
  {
    [JsonPropertyName("label")]
    public string Label { get; set; } = default!;

    [JsonPropertyName("vendor_id")]
    public int VendorId { get; set; } = default!;
  }
}
