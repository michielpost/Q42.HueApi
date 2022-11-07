using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class Light : HueResource
  {
    [JsonPropertyName("alert")]
    public Alert? Alert { get; set; }

    [JsonPropertyName("dynamics")]
    public Dynamics? Dynamics { get; set; }

    [JsonPropertyName("mode")]
    public string Mode { get; set; } = default!;

    [JsonPropertyName("on")]
    public On On { get; set; } = default!;

    [JsonPropertyName("owner")]
    public ResourceIdentifier Owner { get; set; } = default!;

    [JsonPropertyName("color")]
    public Color? Color { get; set; }

    [JsonPropertyName("color_temperature")]
    public ColorTemperature? ColorTemperature { get; set; }

    [JsonPropertyName("dimming")]
    public Dimming? Dimming { get; set; }

    [JsonPropertyName("effects")]
    public Effects? Effects { get; set; }

    [JsonPropertyName("timed_effects")]
    public TimedEffects? TimedEffects { get; set; }
  }

  public class Alert
  {
    [JsonPropertyName("action_values")]
    public List<string> ActionValues { get; set; } = new List<string>();
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
    public bool SpeedValid { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = default!;

    [JsonPropertyName("status_values")]
    public List<string> StatusValues { get; set; } = new();
  }

  public class On
  {
    [JsonPropertyName("on")]
    public bool IsOn { get; set; }
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
    [JsonPropertyName("mirek")]
    public int? Mirek { get; set; }

    [JsonPropertyName("mirek_schema")]
    public MirekSchema MirekSchema { get; set; } = default!;

    [JsonPropertyName("mirek_valid")]
    public bool MirekValid { get; set; }
  }

  public class Dimming
  {
    [JsonPropertyName("brightness")]
    public double Brightness { get; set; } = 100;

    [JsonPropertyName("min_dim_level")]
    public double? MinDimLevel { get; set; }
  }

  public class Effects
  {
    [JsonPropertyName("effect")]
    public Effect Effect { get; set; } = new();

    [JsonPropertyName("effect_values")]
    public List<string> EffectValues { get; set; } = new();

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("status_values")]
    public List<string> StatusValues { get; set; } = new();
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum Effect
  {
    no_effect, fire, candle
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
    public List<string> EffectValues { get; set; } = new();

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("status_values")]
    public List<string> StatusValues { get; set; } = new();

  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum TimedEffect
  {
    no_effect, sunrise
  }
}
