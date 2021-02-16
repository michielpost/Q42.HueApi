using Newtonsoft.Json;

namespace Q42.HueApi.Models
{
  public class LightConfigUpdate
  {
    [JsonProperty("startup")]
    public LightStartup Startup { get; set; }

  }
}
