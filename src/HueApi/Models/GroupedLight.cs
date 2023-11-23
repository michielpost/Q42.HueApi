using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class GroupedLight : HueResource
  {
    [JsonPropertyName("on")]
    public On On { get; set; } = default!;

    [JsonPropertyName("dimming")]
    public Dimming? Dimming { get; set; }

    [JsonPropertyName("alert")]
    public Alert? Alert { get; set; }

    [JsonPropertyName("signaling")]
    public Signaling? Signaling { get; set; }
  }
}
