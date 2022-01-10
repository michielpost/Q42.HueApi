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
    [JsonPropertyName("alert")]
    public Alert? Alert { get; set; }

    [JsonPropertyName("on")]
    public On? On { get; set; }
  }
}
