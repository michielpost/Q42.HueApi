using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
