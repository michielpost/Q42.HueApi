using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Requests
{
  public class UpdateEntertainment : BaseResourceRequest
  {
    [JsonPropertyName("proxy")]
    public bool Proxy { get; set; }

    [JsonPropertyName("renderer")]
    public bool Renderer { get; set; }

    [JsonPropertyName("segments")]
    public Segment? Segments { get; set; }

    [JsonPropertyName("max_streams")]
    public int? MaxStreams { get; set; }
  }
}
