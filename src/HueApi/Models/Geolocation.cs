using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class Geolocation : HueResource
  {
    /// <summary>
    /// minimum: -180 – maximum: 180
    /// </summary>
    [JsonPropertyName("longitude")]
    public int? Longitude { get; set; }

    /// <summary>
    /// minimum: -90 – maximum: 90
    /// </summary>
    [JsonPropertyName("latitude")]
    public int? Latitude { get; set; }
  }
}
