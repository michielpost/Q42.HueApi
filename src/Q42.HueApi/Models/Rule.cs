using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  public class Rule
  {
    public string Id { get; set; }

    [DataMember(Name = "Name")]
    public string Name { get; set; }

    [DataMember(Name = "Lasttriggered")]
    public DateTime? LastTriggered { get; set; }

    [DataMember(Name = "creationtime")]
    public DateTime? CreationTime { get; set; }

    [DataMember(Name = "timestriggered")]
    public int TimesTriggered { get; set; }

    [DataMember(Name = "owner")]
    public string Owner { get; set; }

    [DataMember(Name = "status")]
    public string Status { get; set; }

    [DataMember(Name = "conditions")]
    public List<Condition> Conditions { get; set; }

    [DataMember(Name = "actions")]
    public List<Action> Actions { get; set; }
  }

  public class Condition
  {
    [DataMember(Name = "address")]
    public string Address { get; set; }

    [DataMember(Name = "operator")]
    public string Operator { get; set; }

    [DataMember(Name = "value")]
    public string Value { get; set; }
  }

  public class Body
  {
    [DataMember(Name = "scene")]
    public string Scene { get; set; }
  }

  public class Action
  {
    [DataMember(Name = "address")]
    public string Address { get; set; }

    [DataMember(Name = "method")]
    public string Method { get; set; }

    [DataMember(Name = "body")]
    public Body Body { get; set; }
  }

  
}
