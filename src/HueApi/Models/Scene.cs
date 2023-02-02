using HueApi.Models.Requests.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
    public List<ColorTemperature> ColorTemperature { get; set; } = new();

    //[MaxLength(1)]
    [JsonPropertyName("dimming")]
    public List<Dimming> Dimming { get; set; } = new();
  }

  public class ColorPalette
  {
    [JsonPropertyName("color")]
    public Color Color { get; set; } = new();

    [JsonPropertyName("dimming")]
    public Dimming Dimming { get; set; } = new();
  }
 
}
