using System.Runtime.Serialization;

namespace Q42.HueApi.Models
{
  [DataContract]
  public class Light
  {
    [DataMember (Name = "id")]
    public string Id { get; set; }

    [DataMember (Name = "state")]
    public State State { get; set; }

    [DataMember (Name = "type")]
    public string Type { get; set; }

    [DataMember (Name = "name")]
    public string Name { get; set; }

    [DataMember (Name = "modelid")]
    public string ModelId { get; set; }

    [DataMember (Name = "swversion")]
    public string SoftwareVersion { get; set; }
  }
}