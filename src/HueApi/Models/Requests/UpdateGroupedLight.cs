using HueApi.Models.Requests.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Requests
{
  public class UpdateGroupedLight : BaseResourceRequest, IUpdateColor, IUpdateColorTemperature, IUpdateOn
  {
    [JsonPropertyName("on")]
    public On? On { get; set; }

    [JsonPropertyName("color")]
    public Color? Color { get; set; }

    [JsonPropertyName("color_temperature")]
    public ColorTemperature? ColorTemperature { get; set; }

    [JsonPropertyName("dimming")]
    public Dimming? Dimming { get; set; }

    [JsonPropertyName("alert")]
    public UpdateAlert? Alert { get; set; }

    [JsonPropertyName("dynamics")]
    public Dynamics? Dynamics { get; set; }

  }
}
