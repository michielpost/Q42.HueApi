using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Groups
{
  [DataContract]
  public class Group
  {

	[DataMember]
	public string Id { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Luminaire / Lightsource / LightGroup
    /// </summary>
    [DataMember(Name = "type")]
    public string Type { get; set; }

	/// <summary>
	/// Category of the Room type. Default is "Other".
	/// </summary>
	[DataMember(Name = "class")]
	public string Class { get; set; }

	/// <summary>
	/// As of 1.4. Uniquely identifies the hardware model of the luminaire. Only present for automatically created Luminaires.
	/// </summary>
	[DataMember(Name = "modelid")]
    public string ModelId { get; set; }

    /// <summary>
    /// Lights property only filled when getting a single group
    /// </summary>
    [DataMember(Name = "lights")]
    public List<string> Lights { get; set; }
    
    /// <summary>
    /// Action property only filled when getting a single group
    /// </summary>
    [DataMember(Name = "action")]
    public State Action { get; set; }
  }
}
