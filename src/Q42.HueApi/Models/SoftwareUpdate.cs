using System.Runtime.Serialization;

namespace Q42.HueApi
{
  [DataContract]
  public class SoftwareUpdate
  {
    [DataMember (Name = "updatestate")]
    public int UpdateState { get; set; }

    [DataMember (Name = "url")]
    public string Url { get; set; }

    [DataMember (Name = "text")]
    public string Text { get; set; }

    [DataMember (Name = "notify")]
    public bool Notify { get; set; }
  }
}