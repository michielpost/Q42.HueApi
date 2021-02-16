using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Q42.HueApi.Models.Groups
{
  internal class CreateGroupRequest : UpdateGroupRequest
  {

    /// <summary>
    /// Luminaire / Lightsource / LightGroup
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [JsonProperty("type")]
    public GroupType Type { get; set; }

  }
}
