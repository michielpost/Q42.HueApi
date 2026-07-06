using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class Scene : HueResource
  {
    [JsonPropertyName("actions")]
    public List<SceneAction> Actions { get; set; } = new();

    [JsonPropertyName("group")]
    public ResourceIdentifier? Group { get; set; }

    [JsonPropertyName("palette")]
    public Palette? Palette { get; set; }

    [JsonPropertyName("speed")]
    public double Speed { get; set; }

    /// <summary>
    /// Indicates whether to automatically start the scene dynamically on active recall
    /// </summary>
    [JsonPropertyName("auto_dynamic")]
    public bool? AutoDynamic { get; set; }

    /// <summary>
    /// Consists the information about the current status and last time it is recalled
    /// </summary>
    [JsonPropertyName("status")]
    public SceneStatus? Status { get; set; }

    /// <summary>
    /// Details about the used scene mapping. Only available with SpatialAware.
    /// </summary>
    [JsonPropertyName("mapping")]
    public SceneMapping? Mapping { get; set; }

    [JsonPropertyName("last_actions_update")]
    public SceneLastActionsUpdate? LastActionsUpdate { get; set; }
  }

  public class SceneStatus
  {
    /// <summary>
    /// One of inactive, static, dynamic_palette
    /// </summary>
    [JsonPropertyName("active")]
    public string? Active { get; set; }

    [JsonPropertyName("last_recall")]
    public DateTimeOffset? LastRecall { get; set; }
  }

  public class SceneMapping
  {
    /// <summary>
    /// One of classic, spatial
    /// </summary>
    [JsonPropertyName("algorithm")]
    public string? Algorithm { get; set; }
  }

  public class SceneLastActionsUpdate
  {
    /// <summary>
    /// Source of most recent actions list update. One of clip, autogrow
    /// </summary>
    [JsonPropertyName("source")]
    public string? Source { get; set; }
  }

  public class SceneAction
  {
    [JsonPropertyName("action")]
    public LightAction Action { get; set; } = default!;

    [JsonPropertyName("target")]
    public ResourceIdentifier Target { get; set; } = default!;
  }

  public class Palette
  {
    [JsonPropertyName("color")]
    public List<ColorPalette> Color { get; set; } = new();

    [JsonPropertyName("color_temperature")]
    public List<ColorTemperaturePalette> ColorTemperature { get; set; } = new();

    //[MaxLength(1)]
    [JsonPropertyName("dimming")]
    public List<Dimming> Dimming { get; set; } = new();

    //[MaxLength(3)]
    [JsonPropertyName("effects_v2")]
    public List<EffectsV2Palette>? EffectsV2 { get; set; }
  }

  public class ColorPalette
  {
    [JsonPropertyName("color")]
    public Color Color { get; set; } = new();

    [JsonPropertyName("dimming")]
    public Dimming Dimming { get; set; } = new();
  }

  public class ColorTemperaturePalette
  {
    [JsonPropertyName("color_temperature")]
    public ColorTemperature ColorTemperature { get; set; } = new();

    [JsonPropertyName("dimming")]
    public Dimming Dimming { get; set; } = new();
  }

  public class EffectsV2Palette
  {
    [JsonPropertyName("action")]
    public EffectAction Action { get; set; } = new();
  }

}
