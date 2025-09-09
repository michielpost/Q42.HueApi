using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class MotionAreaConfigResource : HueResource
  {
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("group")]
    public ResourceIdentifier? Group { get; set; }

    /// <summary>
    /// A list of motion area services used in this configuration, with each service including its health status. Supported types “MotionAreaCandidate”
    /// </summary>
    [JsonPropertyName("participants")]
    public List<ResourceIdentifier> Participants { get; set; } = new();

    [JsonPropertyName("health")]
    public MotionAreaConfigHealth Health { get; set; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum MotionAreaConfigHealth
  {
    healthy, degraded, recovering, unrecoverable, not_running
  }
}
