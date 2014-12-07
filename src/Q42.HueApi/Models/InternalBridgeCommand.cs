using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  [DataContract]
  public class InternalBridgeCommand
  {
    [DataMember(Name = "address")]
    public string Address { get; set; }

    [JsonConverter(typeof(HttpMethodConverter))]
    [DataMember(Name = "method")]
    public HttpMethod Method { get; set; }

    [DataMember(Name = "body")]
    public ICommandBody Body { get; set; }
  }
}
