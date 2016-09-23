using System.Runtime.Serialization;

namespace Q42.HueApi
{
  [DataContract]
  public class PortalState
  {
    [DataMember(Name = "signedon")]
    public bool SignedOn { get; set; }

    [DataMember(Name = "incoming")]
    public bool Incoming { get; set; }

    [DataMember(Name = "outgoing")]
    public bool Outgoing { get; set; }

    [DataMember(Name = "communication")]
    public string Communication { get; set; }
  }
}