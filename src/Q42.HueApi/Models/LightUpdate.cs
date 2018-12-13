using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Q42.HueApi.Models
{
  [DataContract]
  public class LightConfigUpdate
  {
    [DataMember(Name = "startup")]
    public LightStartup Startup { get; set; }

  }
}
