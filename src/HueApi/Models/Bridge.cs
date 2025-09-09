using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class Bridge : HueResource
  {
    [JsonPropertyName("bridge_id")]
    public string? BridgeId { get; set; }

    [JsonPropertyName("time_zone")]
    public TimeZoneConfig? TimeZone { get; set; }

    /// <summary>
    /// Available when the bridge is migrated from a previous generation bridge.
    /// </summary>
    [JsonPropertyName("import")]
    public ImportData? Import { get; set; }
  }

  public class TimeZoneConfig
  {
    /// <summary>
    /// Time zone where the user's home is located (as Olson ID).
    /// </summary>
    [JsonPropertyName("time_zone")]
    public string? TimeZone { get; set; }
  }

  public class ImportData
  {
    /// <summary>
    /// Bridge ID (in lower case) where the imported data originates from.
    /// </summary>
    [JsonPropertyName("origin")]
    public string? Origin { get; set; }

    /// <summary>
    /// UTC date and time when the import took place.
    /// </summary>
    [JsonPropertyName("time")]
    public DateTimeOffset? Time { get; set; }
  }
}
