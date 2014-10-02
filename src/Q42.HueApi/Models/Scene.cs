using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  public class Scene
  {
    [IgnoreDataMember]
    public string Id { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "lights")]
    public IEnumerable<string> Lights { get; set; }

    [DataMember(Name = "active")]
    public bool Active { get; set; }

  }
}
