using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Q42.HueApi.Models.Bridge
{
  [DataContract]
  public class InternetServices
  {
    [DataMember(Name = "remoteaccess")]
    public string RemoteAccess { get; set; }
  }
}
