using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class GeofenceClient : HueResource
  {
    /// <summary>
    /// (string – minLength: 1 – maxLength: 32)
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }


    [JsonPropertyName("is_at_home")]
    public bool? IsAtHome { get; set; }
  }
}
