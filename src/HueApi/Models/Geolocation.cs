using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class Geolocation : HueResource
  {
    /// <summary>
    /// minimum: -180 – maximum: 180
    /// </summary>
    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }

    /// <summary>
    /// minimum: -90 – maximum: 90
    /// </summary>
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    /// <summary>
    /// Is the geolocation configured
    /// </summary>
    [JsonPropertyName("is_configured")]
    public bool IsConfigured { get; set; }

    /// <summary>
    /// Info related to today's sun (only available when geolocation has been configured)
    /// </summary>
    [JsonPropertyName("sun_today")]
    public SunToday? SunToday { get; set; }
  }

  public class SunToday
  {
    /// <summary>
    /// Time of day at sunset (in local time, and only valid on normal days, see property day_type)
    /// </summary>
    [JsonPropertyName("sunset_time")]
    public string? SunsetTime { get; set; }

    /// <summary>
    /// One of normal_day, polar_day, polar_night
    /// </summary>
    [JsonPropertyName("day_type")]
    public string? DayType { get; set; }
  }
}
