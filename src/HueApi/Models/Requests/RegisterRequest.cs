using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
