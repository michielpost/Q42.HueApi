using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class WifiConnectivityResource : HueResource
  {

    [JsonPropertyName("status")]
    public WifiConnectivityStatus Status { get; set; }

    [JsonPropertyName("has_ssid")]
    public bool HasSSID { get; set; }
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum WifiConnectivityStatus
  {
    connected, disconnected
  }

}
