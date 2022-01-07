using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class HueResponse<T> : HueErrorResponse
  {
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();
  }

  public class HuePostResponse : HueResponse<ResourceIdentifier>
  {

  }

  public class HuePutResponse : HueResponse<ResourceIdentifier>
  {

  }

  public class HueDeleteResponse : HueResponse<ResourceIdentifier>
  {

  }

  public class HueErrorResponse
  {
    [JsonPropertyName("errors")]
    public HueErrors Errors { get; set; } = new();

    public bool HasErrors => Errors.Any();

  }
}
