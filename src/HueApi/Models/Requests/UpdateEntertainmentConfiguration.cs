using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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

    [JsonPropertyName("configuration_type")]
    public EntertainmentConfigurationType? ConfigurationType { get; set; }

    [JsonPropertyName("locations")]
    public Locations? Locations { get; set; }

    [JsonPropertyName("stream_proxy")]
    public StreamProxy? StreamProxy { get; set; }

  }
}
