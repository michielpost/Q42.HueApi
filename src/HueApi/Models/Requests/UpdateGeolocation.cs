using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Requests
{
  public class UpdateGeolocation : BaseResourceRequest
  {
    /// <summary>
    /// minimum: -180 – maximum: 180
    /// </summary>
    [JsonPropertyName("longitude")]
    public int Longitude { get; set; }

    /// <summary>
    /// minimum: -90 – maximum: 90
    /// </summary>
    [JsonPropertyName("latitude")]
    public int Latitude { get; set; }
  }
}
