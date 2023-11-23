using HueApi.Models.Requests.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Requests
{
  public class UpdateLight : BaseResourceRequest, IUpdateColor, IUpdateColorTemperature, IUpdateOn, IUpdateDimmingDelta, IUpdateDimming, IUpdateDynamics
  {
    [JsonPropertyName("on")]
    public On? On { get; set; }

    [JsonPropertyName("dimming")]
    public Dimming? Dimming { get; set; }

    [JsonPropertyName("dimming_delta")]
    public DimmingDelta? DimmingDelta { get; set; }

    [JsonPropertyName("color_temperature")]
    public ColorTemperature? ColorTemperature { get; set; }

    [JsonPropertyName("color_temperature_delta")]
    public ColorTemperatureDelta? ColorTemperatureDelta { get; set; }

    [JsonPropertyName("color")]
    public Color? Color { get; set; }

    [JsonPropertyName("dynamics")]
    public Dynamics? Dynamics { get; set; }

    [JsonPropertyName("alert")]
    public UpdateAlert? Alert { get; set; }

    [JsonPropertyName("signaling")]
    public SignalingUpdate? Signaling { get; set; }

    [JsonPropertyName("gradient")]
    public Gradient? Gradient { get; set; }

    [JsonPropertyName("effects")]
    public Effects? Effects { get; set; }

    [JsonPropertyName("timed_effects")]
    public TimedEffects? TimedEffects { get; set; }

    [JsonPropertyName("powerup")]
    public PowerUp? PowerUp { get; set; }

  }

  public class UpdateAlert
  {
    [JsonPropertyName("action")]
    public string Action { get; set; } = "breathe";

  }
}
