using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class RegisterRequest
  {
    public RegisterRequest(string deviceType)
    {
      DeviceType = deviceType;
    }

    [JsonPropertyName("devicetype")]
    public string DeviceType { get; }

    [JsonPropertyName("generateclientkey")]
    public bool GenerateClientKey { get; set; } = true;
  }
}
