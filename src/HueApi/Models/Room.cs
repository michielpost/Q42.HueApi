using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class Room : HueResource
  {
    [JsonPropertyName("children")]
    public List<ResourceIdentifier> Children { get; set; } = new();

    [JsonPropertyName("grouped_services")]
    public List<ResourceIdentifier> GroupedServices { get; set; } = new();

  }
}
