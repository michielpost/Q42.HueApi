using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace HueApi.Models.Requests
{
  public class UpdateBehaviorInstance : BaseResourceRequest
  {
    [JsonPropertyName("script_id")]
    public string? ScriptId { get; set; }

    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("configuration")]
    public JsonElement? Configuration { get; set; }

    [JsonPropertyName("trigger")]
    public JsonElement? Trigger { get; set; }

    [JsonPropertyName("migrated_from")]
    public string? MigratedFrom { get; set; }
  }
}
