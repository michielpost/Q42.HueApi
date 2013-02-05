using System.Runtime.Serialization;

namespace Q42.HueApi
{
  [DataContract]
  public class WhiteList
  {
    [DataMember(Name = "id")]
    public string Id { get; set; }

    [DataMember(Name = "last use date")]
    public string LastUsedDate { get; set; }

    [DataMember(Name = "create date")]
    public string CreateDate { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

  }
}