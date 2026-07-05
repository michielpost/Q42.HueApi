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

    /// <summary>
    /// An error code associated with the problem for client handling. One of communication_error, attribute_may_have_no_effect, client_error, internal_error
    /// </summary>
    [JsonPropertyName("error_code")]
    public string? ErrorCode { get; set; }
  }
}
