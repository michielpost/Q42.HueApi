using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum EntertainmentConfigurationAction
  {
    start, stop
  }

  public class UpdateEntertainmentConfiguration : BaseResourceRequest
  {
    [JsonPropertyName("action")]
    public EntertainmentConfigurationAction? Action { get; set; }

    /// <summary>
    /// one of screen, monitor, music, 3dspace, other
    /// </summary>
    [JsonPropertyName("configuration_type")]
    public string? ConfigurationType { get; set; }

    [JsonPropertyName("locations")]
    public Locations? Locations { get; set; }

    [JsonPropertyName("stream_proxy")]
    public StreamProxy? StreamProxy { get; set; }

  }
}
