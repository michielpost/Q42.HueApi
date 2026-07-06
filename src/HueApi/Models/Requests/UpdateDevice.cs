using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdateDevice : BaseResourceRequest
  {
    [JsonPropertyName("identify")]
    public Identify? Identify { get; set; }

    [JsonPropertyName("usertest")]
    public UserTestUpdate? UserTest { get; set; }

    [JsonPropertyName("geometry")]
    public ResourceGeometry? Geometry { get; set; }
  }

  public class Identify
  {
    [JsonPropertyName("action")]
    public string Action { get; set; } = "identify";

    /// <summary>
    /// Duration in milliseconds to perform the identify cycle.
    /// </summary>
    [JsonPropertyName("duration")]
    public int? Duration { get; set; }
  }

  public class UserTestUpdate
  {
    /// <summary>
    /// Activates or extends user usertest mode of device for 120 seconds.
    /// </summary>
    [JsonPropertyName("usertest")]
    public bool UserTest { get; set; }
  }
}
