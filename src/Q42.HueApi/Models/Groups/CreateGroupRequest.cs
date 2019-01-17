using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Groups
{
  [DataContract]
  internal class CreateGroupRequest : UpdateGroupRequest
  {

    /// <summary>
    /// Luminaire / Lightsource / LightGroup
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [DataMember(Name = "type")]
    public GroupType Type { get; set; }

  }
}
