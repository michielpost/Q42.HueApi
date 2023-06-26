using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class BehaviorInstance : HueResource
  {
    [JsonPropertyName("script_id")]
    public string ScriptId { get; set; } = default!;

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = default!;

    [JsonPropertyName("state")]
    public JsonElement State { get; set; } = default!;

    [JsonPropertyName("configuration")]
    public JsonElement Configuration { get; set; } = default!;

    [JsonPropertyName("dependees")]
    public List<HueResource>? Dependees { get; set; }

    [JsonPropertyName("status")]
    public BehaviorInstanceStatus Status { get; set; } = default!;

    [JsonPropertyName("last_error")]
    public string LastError { get; set; } = default!;

    [JsonPropertyName("migrated_from")]
    public string? MigratedFrom { get; set; }
  }

  

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum BehaviorInstanceStatus
  {
    initializing, running, disabled, errore
  }
}
