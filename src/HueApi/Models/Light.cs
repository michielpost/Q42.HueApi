using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class Light : HueResource
  {
    [JsonPropertyName("on")]
    public On On { get; set; } = default!;

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
    public Alert? Alert { get; set; }

    [JsonPropertyName("product_data")]
    public LightProductData? ProductData { get; set; }

    [JsonPropertyName("signaling")]
    public Signaling? Signaling { get; set; }

    [JsonPropertyName("mode")]
    public string Mode { get; set; } = default!;

    [JsonPropertyName("gradient")]
    public Gradient? Gradient { get; set; }

    [JsonPropertyName("effects_v2")]
    public EffectsV2? EffectsV2 { get; set; }


    [JsonPropertyName("timed_effects")]
    public TimedEffects? TimedEffects { get; set; }

    [JsonPropertyName("powerup")]
    public PowerUp? PowerUp { get; set; }

    [JsonPropertyName("content_configuration")]
    public ContentConfiguration? ContentConfiguration { get; set; }

    [JsonPropertyName("geometry")]
    public Geometry? Geometry { get; set; }

    [JsonPropertyName("identify")]
    public IdentifyFeature? Identify { get; set; }

    /// <summary>
    /// Service identification number. 0 indicates service of a single instance
    /// </summary>
    [JsonPropertyName("service_id")]
    public int? ServiceId { get; set; }

    [JsonPropertyName("dimming_configuration")]
    public DimmingConfiguration? DimmingConfiguration { get; set; }

  }

  /// <summary>
  /// Empty status object present on resources that support the identify action
  /// </summary>
  public class IdentifyFeature
  {
  }

  public class DimmingConfiguration
  {
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// Brightness percentage. Value 0 is the lowest possible brightness. Maximum: 100
    /// </summary>
    [JsonPropertyName("min_level")]
    public double? MinLevel { get; set; }
  }

  public class LightProductData
  {
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("archetype")]
    public string? ArcheType { get; set; }

    [JsonPropertyName("function")]
    public string? Function { get; set; }
  }

  public class Alert
  {
    [JsonPropertyName("action_values")]
    public List<string> ActionValues { get; set; } = new List<string>();
  }

  public class Signaling
  {
    [JsonPropertyName("signal_values")]
    public List<Signal>? SignalValues { get; set; }

    [JsonPropertyName("status")]
    public SignalingStatus? Status { get; set; }
  }

  public class SignalingStatus
  {
    /// <summary>
    /// Indicates which signal is currently active.
    /// </summary>
    [JsonPropertyName("signal")]
    public Signal Signal { get; set; }

    /// <summary>
    /// Timestamp indicating when the active signal is expected to end. Value is not set if there is no_signal
    /// </summary>
    [JsonPropertyName("estimated_end")]
    public DateTimeOffset? EstimatedEnd { get; set; }

    [JsonPropertyName("colors")]
    public List<Color>? Colors { get; set; }
  }

  public class SignalingUpdate
  {
    [JsonPropertyName("signal")]
    public Signal Signal { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    /// <summary>
    /// List of colors to apply to the signal (not supported by all signals)
    /// </summary>
    [JsonPropertyName("colors")]
    public List<Color>? Colors { get; set; }

  }

  public class Dynamics
  {
    /// <summary>
    /// Duration of a light transition or timed effects in ms.
    /// </summary>
    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("speed")]
    public double Speed { get; set; }

    [JsonPropertyName("speed_valid")]
    public bool? SpeedValid { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("status_values")]
    public List<string>? StatusValues { get; set; }
  }

  public class On
  {
    [JsonPropertyName("on")]
    public bool IsOn { get; set; }
  }

  public class PowerUpOn
  {
    /// <summary>
    /// State to activate after powerup. On will use the value specified in the “on” property. When setting mode “on”, the on property must be included. Toggle will alternate between on and off on each subsequent power toggle. Previous will return to the state it was in before powering off.
    /// </summary>
    [JsonPropertyName("mode")]
    public PowerUpOnMode? Mode { get; set; }

    [JsonPropertyName("on")]
    public On? On { get; set; }
  }

  public class XyPosition
  {
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }
  }

  public class Gamut
  {
    [JsonPropertyName("blue")]
    public XyPosition Blue { get; set; } = new();

    [JsonPropertyName("green")]
    public XyPosition Green { get; set; } = new();

    [JsonPropertyName("red")]
    public XyPosition Red { get; set; } = new();
  }


  public class Color
  {
    [JsonPropertyName("gamut")]
    public Gamut? Gamut { get; set; }

    [JsonPropertyName("gamut_type")]
    public string? GamutType { get; set; }

    [JsonPropertyName("xy")]
    public XyPosition Xy { get; set; } = new();
  }

  public class MirekSchema
  {
    [JsonPropertyName("mirek_maximum")]
    public int MirekMaximum { get; set; }

    [JsonPropertyName("mirek_minimum")]
    public int MirekMinimum { get; set; }
  }

  public class ColorTemperature
  {
    /// <summary>
    /// minimum: 153 – maximum: 500
    /// </summary>
    [JsonPropertyName("mirek")]
    public int? Mirek { get; set; }

    [JsonPropertyName("mirek_schema")]
    public MirekSchema MirekSchema { get; set; } = default!;

    [JsonPropertyName("mirek_valid")]
    public bool MirekValid { get; set; }
  }

  public class ColorTemperatureDelta
  {
    [JsonPropertyName("action")]
    public DeltaAction Action { get; set; }

    /// <summary>
    ///  maximum: 347
    ///  Mirek delta to current mirek. Clip at mirek_minimum and mirek_maximum of mirek_schema.
    /// </summary>
    [JsonPropertyName("mirek_delta")]
    public int MirekDelta { get; set; }
  }

  public class Dimming
  {
    [JsonPropertyName("brightness")]
    public double Brightness { get; set; } = 100;

    [JsonPropertyName("min_dim_level")]
    public double? MinDimLevel { get; set; }
  }

  public class DimmingDelta
  {
    [JsonPropertyName("action")]
    public DeltaAction Action { get; set; }

    [JsonPropertyName("brightness_delta")]
    public double BrightnessDelta { get; set; }
  }

  public class EffectsV2
  {
    [JsonPropertyName("action")]
    public EffectAction Action { get; set; } = new();

    [JsonPropertyName("status")]
    public EffectsV2Status? Status { get; set; }
  }


  public class EffectAction
  {
    [JsonPropertyName("effect")]
    public Effect Effect { get; set; } = new();

    [JsonPropertyName("parameters")]
    public EffectParams? Parameters { get; set; }

    /// <summary>
    /// Possible effect values you can set in a light.
    /// </summary>
    [JsonPropertyName("effect_values")]
    public List<Effect>? EffectValues { get; set; }

  }

  public class EffectsV2Status
  {
    [JsonPropertyName("effect")]
    public Effect Effect { get; set; } = new();

    [JsonPropertyName("effect_values")]
    public List<Effect>? EffectValues { get; set; }

    [JsonPropertyName("parameters")]
    public EffectParams? Parameters { get; set; }
  }

  public class EffectParams
  {
    [JsonPropertyName("color")]
    public Color? Color { get; set; }

    [JsonPropertyName("color_temperature")]
    public ColorTemperature? ColorTemperature { get; set; }

    /// <summary>
    /// minimum: 0 – maximum: 1)
    /// </summary>
    [JsonPropertyName("speed")]
    public double? Speed { get; set; }

  }


  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum Effect
  {
    prism, opal, glisten, sparkle, fire, candle, underwater, cosmos, sunbeam, enchant, no_effect
  }

  public class TimedEffects
  {
    [JsonPropertyName("effect")]
    public TimedEffect Effect { get; set; } = new();

    /// <summary>
    /// Duration is mandatory when timed effect is set except for no_effect. Resolution decreases for a larger duration. e.g Effects with duration smaller than a minute will be rounded to a resolution of 1s, while effects with duration larger than an hour will be arounded up to a resolution of 300s. Duration has a max of 21600000 ms.
    /// </summary>
    [JsonPropertyName("duration")]
    public int Duration { get; set; } = new();

    [JsonPropertyName("effect_values")]
    public List<string>? EffectValues { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("status_values")]
    public List<string>? StatusValues { get; set; }

  }

  public class PowerUp
  {
    [JsonPropertyName("preset")]
    public PowerUpPreset Preset { get; set; } = new();

    [JsonPropertyName("on")]
    public PowerUpOn? On { get; set; }

    [JsonPropertyName("dimming")]
    public PowerUpDimming? Dimming { get; set; }

    [JsonPropertyName("color")]
    public PowerUpColor? Color { get; set; }

    [JsonPropertyName("configured")]
    public bool? Configured { get; set; }

  }

  public class PowerUpDimming
  {
    /// <summary>
    /// Dimming will set the brightness to the specified value after power up. When setting mode “dimming”, the dimming property must be included. Previous will set brightness to the state it was in before powering off.
    /// </summary>
    [JsonPropertyName("mode")]
    public PowerUpDimmingMode Mode { get; set; }

    [JsonPropertyName("dimming")]
    public Dimming? Dimming { get; set; }
  }

  public class PowerUpColor
  {
    /// <summary>
    /// State to activate after powerup. Availability of “color_temperature” and “color” modes depend on the capabilities of the lamp.
    /// </summary>
    [JsonPropertyName("mode")]
    public PowerUpColorMode Mode { get; set; }

    [JsonPropertyName("color_temperature")]
    public ColorTemperature? ColorTemperature { get; set; }

    [JsonPropertyName("color")]
    public Color? Color { get; set; }
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum PowerUpDimmingMode
  {
    dimming, previous
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum PowerUpColorMode
  {
    color_temperature, color, previous
  }

  public class ContentConfiguration
  {
    [JsonPropertyName("orientation")]
    public ContentConfigurationOrientation? Orientation { get; set; }

    [JsonPropertyName("order")]
    public ContentConfigurationOrder? Order { get; set; }

    [JsonPropertyName("association")]
    public ContentConfigurationAssociation? Association { get; set; }
  }

  public class ContentConfigurationAssociation
  {
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("configurable")]
    public bool? Configurable { get; set; }

    /// <summary>
    /// Defines what object the service is associated with. not_associated: Not associated – screen: Associated with a screen by placing it behind or around the screen following an arc
    /// </summary>
    [JsonPropertyName("association")]
    public ContentAssociation Association { get; set; }
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum ContentAssociation
  {
    not_associated, screen
  }

  public class ContentConfigurationOrientation
  {
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("configurable")]
    public bool? Configurable { get; set; }

    [JsonPropertyName("orientation")]
    public Orientation Orientation { get; set; }
  }

  public class ContentConfigurationOrder
  {
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("configurable")]
    public bool? Configurable { get; set; }

    [JsonPropertyName("order")]
    public Order Order { get; set; }
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum Orientation
  {
    horizontal, vertical
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum Order
  {
    forward, reversed
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum TimedEffect
  {
    no_effect, sunrise, sunset
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum DeltaAction
  {
    up, down, stop
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum PowerUpPreset
  {
    safety, powerfail, last_on_state, custom
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum PowerUpOnMode
  {
    on, toggle, previous
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum Signal
  {
    no_signal, on_off, on_off_color, alternating
  }
}
