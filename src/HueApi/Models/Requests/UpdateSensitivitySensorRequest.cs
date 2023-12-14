using HueApi.Models.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Requests
{
  public class UpdateSensitivitySensorRequest : UpdateSensorRequest
  {
    [JsonPropertyName("sensitivity")]
    public Sensitivity? Sensitivity { get; set; }

  }
}
