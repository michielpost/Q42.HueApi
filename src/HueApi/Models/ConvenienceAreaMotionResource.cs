using HueApi.Models.Sensors;
using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class ConvenienceAreaMotionResource : HueResource
  {
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    [JsonPropertyName("motion")]
    public Motion? Motion { get; set; }

    [JsonPropertyName("sensitivity")]
    public Sensitivity? Sensitivity { get; set; }
  }
}
