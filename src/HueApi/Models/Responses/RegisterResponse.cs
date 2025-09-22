using System.Text.Json.Serialization;

namespace HueApi.Models.Responses
{
  public class RegisterResponse
  {
    [JsonPropertyName("success")]
    public RegisterResult Success { get; } = default!;
  }

  public class RegisterResult
  {
    [JsonPropertyName("username")]
    public string Username { get; } = default!;

    [JsonPropertyName("clientkey")]
    public string? ClientKey { get; set; }
  }
}
