using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class HueErrors : List<HueError>
  {
  }

  public class HueError
  {
    [JsonPropertyName("description")]
    public string? Description { get; set; }
  }
}
