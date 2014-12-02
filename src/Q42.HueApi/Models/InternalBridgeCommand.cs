using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

    [DataMember(Name = "method")]
    public string Method { get; set; }

    [DataMember(Name = "body")]
    public ICommandBody Body { get; set; }
  }
}
