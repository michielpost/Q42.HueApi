using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdateGeolocation : BaseResourceRequest
  {
    /// <summary>
    /// minimum: -180 – maximum: 180
    /// </summary>
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    /// <summary>
    /// minimum: -90 – maximum: 90
    /// </summary>
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }
  }
}
