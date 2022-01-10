using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Requests
{
  public class BaseResourceRequest
  {
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata? Metadata { get; set; }
  }
}
