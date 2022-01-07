using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Requests
{
  public class UpdateLight : BaseResourceRequest
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

    //TODO: Gradient https://developers.meethue.com/develop/hue-api-v2/api-reference/#resource_light__id__put

  }

  public class UpdateAlert
  {
    [JsonPropertyName("action")]
    public string Action { get; set; } = "breathe";

  }
}
