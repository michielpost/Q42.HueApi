using HueApi.Models.Requests.Interface;
using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdateLight : BaseResourceRequest, IUpdateColor, IUpdateColorTemperature, IUpdateOn, IUpdateDimmingDelta, IUpdateDimming, IUpdateDynamics
  {
    [JsonPropertyName("identify")]
    public Identify? Identify { get; set; }

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

    [Obsolete("Use EffectsV2")]
    [JsonPropertyName("effects")]
    public Effects? Effects { get; set; }

    [JsonPropertyName("effects_v2")]
    public EffectsV2? EffectsV2 { get; set; }

    [JsonPropertyName("timed_effects")]
    public TimedEffects? TimedEffects { get; set; }

    [JsonPropertyName("powerup")]
    public PowerUp? PowerUp { get; set; }

    [JsonPropertyName("content_configuration")]
    public ContentConfiguration? ContentConfiguration { get; set; }

  }

  public class UpdateAlert
  {
    [JsonPropertyName("action")]
    public string Action { get; set; } = "breathe";

  }
}
