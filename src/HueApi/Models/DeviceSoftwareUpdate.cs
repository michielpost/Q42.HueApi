using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class DeviceSoftwareUpdate : HueResource
  {
    [JsonPropertyName("state")]
    public DeviceSoftwareUpdateState State { get; set; }

    [JsonPropertyName("problems")]
    public List<string>? Problems { get; set; }


  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum DeviceSoftwareUpdateState
  {
    no_update, update_pending, ready_to_install, installing
  }

}
