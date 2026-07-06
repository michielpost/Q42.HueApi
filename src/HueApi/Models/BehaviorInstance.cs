using System.Text.Json;
using System.Text.Json.Serialization;

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
    public List<ResourceDependee>? Dependees { get; set; }

    [JsonPropertyName("status")]
    public BehaviorInstanceStatus Status { get; set; } = default!;

    [JsonPropertyName("last_error")]
    public string LastError { get; set; } = default!;

    [JsonPropertyName("migrated_from")]
    public string? MigratedFrom { get; set; }
  }



  public class ResourceDependee
  {
    /// <summary>
    /// Id of the dependency resource (target)
    /// </summary>
    [JsonPropertyName("target")]
    public ResourceIdentifier Target { get; set; } = new();

    /// <summary>
    /// critical: The source cannot function without the target – non_critical: The source can function without the target
    /// </summary>
    [JsonPropertyName("level")]
    public string? Level { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum BehaviorInstanceStatus
  {
    initializing, running, disabled, errored
  }
}
