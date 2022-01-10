using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Requests
{
  public class UpdateDevice : BaseResourceRequest
  {
    [JsonPropertyName("identify")]
    public Identify? Identify { get; set; }
  }

  public class Identify
  {
    [JsonPropertyName("action")]
    public string Action { get; set; } = "identify";
  }
}
