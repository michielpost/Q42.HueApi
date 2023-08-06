using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class Entertainment : HueResource
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

  public class Segment
  {
    [JsonPropertyName("configurable")]
    public bool Configurable { get; set; }

    [JsonPropertyName("max_segments")]
    public int MaxSegments { get; set; }

    [JsonPropertyName("segments")]
    public List<SegmentItem> Segments { get; set; } = new();
  }

  public class SegmentItem
  {
    [JsonPropertyName("length")]
    public int Length { get; set; }

    [JsonPropertyName("start")]
    public int Start { get; set; }
  }
}
