using HueApi.Models.Requests.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class Gradient
  {
    [JsonPropertyName("points")]
    public List<GradientPoint> Points { get; set; } = new();

    [JsonPropertyName("mode")]
    public GradientMode Mode { get; set; } = new();

    [JsonPropertyName("mode_values")]
    public List<string>? ModeValues { get; set; }

    [JsonPropertyName("points_capable")]
    public int? PointsCapable { get; set; }

    [JsonPropertyName("pixel_count")]
    public int? PixelCount { get; set; }
  }

  public class GradientPoint : IUpdateColor
  {
    [JsonPropertyName("color")]
    public Color? Color { get; set; }
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum GradientMode
  {
    interpolated_palette, interpolated_palette_mirrored, random_pixelated
  }
}
