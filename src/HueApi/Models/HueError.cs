using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class HueErrors : List<HueError>
  {
  }

  public class HueError
  {
    [JsonPropertyName("description")]
    public string? Description { get; set; }
  }
}
