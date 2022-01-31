using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models.Requests
{
  public class CreateZone : BaseResourceRequest
  {
    //[JsonPropertyName("children")]
    //public List<ResourceIdentifier> Children { get; set; } = new();
  }

  public class UpdateZone : CreateZone
  {
   
  }
}
