using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  [DataContract]
  public class Schedule
  {
    [IgnoreDataMember]
    public string Id { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "description")]
    public string Description { get; set; }

    [DataMember(Name = "command")]
    public ScheduleCommand Command { get; set; }

    [DataMember(Name = "time")]
		[JsonConverter(typeof(HueDateTimeConverter))]
		public HueDateTime Time { get; set; }
    
    [DataMember(Name = "created")]
    public DateTime? Created { get; set; }

    [DataMember(Name = "status")]
    public string Status { get; set; }

    [DataMember(Name = "autodelete")]
    public bool Autodelete { get; set; }

    [DataMember(Name = "starttime")]
    public DateTime? Starttime { get; set; }
  }

  [DataContract]
  public class ScheduleCommand
  {
    [DataMember(Name = "address")]
    public string Address { get; set; }

    [DataMember(Name = "method")]
    public string Method { get; set; }

    [DataMember(Name = "body")]
    public LightCommand Body { get; set; }
  }

}
