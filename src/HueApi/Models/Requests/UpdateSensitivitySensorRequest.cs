using HueApi.Models.Sensors;
using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdateSensitivitySensorRequest : UpdateSensorRequest
  {
    [JsonPropertyName("sensitivity")]
    public Sensitivity? Sensitivity { get; set; }

  }
}
