using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class Homekit : HueResource
  {
    /// <summary>
    /// Read only field indicating whether homekit is already paired, currently open for pairing, or unpaired. Transitions: – unpaired > pairing – pushlink button press or power cycle – pairing > paired – through HAP – pairing > unpaired – 10 minutes – paired > unpaired – homekit reset.
    /// </summary>
    [JsonPropertyName("status")]
    public HomekitStatus? Status { get; set; }
  
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum HomekitStatus
  {
    paired, pairing, unpaired
  }
}
