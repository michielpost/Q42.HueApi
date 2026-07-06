using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdateZigbeeConnectivity : BaseResourceRequest
  {
    /// <summary>
    /// Requested zigbee channel. Only the value property should be set.
    /// </summary>
    [JsonPropertyName("channel")]
    public ZigbeeChannel? Channel { get; set; }
  }
}
