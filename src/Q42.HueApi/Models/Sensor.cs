using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{

  [DataContract]
  public class Sensor
  {
    [DataMember]
    public string Id { get; set; }

    [DataMember(Name = "state")]
    public SensorState State { get; set; }
    [DataMember(Name = "config")]
    public SensorConfig Config { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "type")]
    public string Type { get; set; }

    [DataMember(Name = "modelid")]
    public string ModelId { get; set; }

    [DataMember(Name = "manufacturername")]
    public string ManufacturerName { get; set; }

    [DataMember(Name = "swversion")]
    public string SwVersion { get; set; }

    [DataMember(Name = "uniqueid")]
    public string UniqueId { get; set; }

	[DataMember(Name = "productid")]
	public string ProductId { get; set; }

	[DataMember(Name = "swconfigid")]
	public string SwConfigId { get; set; }
  }

  [DataContract]
  public class SensorState
  {
	//Documentation says Daylight is not nullable, but in the real world they can be null
    [DataMember(Name = "daylight")]
    public bool? Daylight { get; set; }

    [DataMember(Name = "lastupdated")]
    public string Lastupdated { get; set; }

    [DataMember(Name = "presence")]
    public bool Presence { get; set; }

    [DataMember(Name = "buttonevent")]
    public int? ButtonEvent { get; set; }
    
   [DataMember(Name = "status")]
    public int? Status { get; set; }

	[DataMember(Name = "open")]
	public bool? Open { get; set; }

	[DataMember(Name = "temperature")]
	public int? Temperature { get; set; }

	[DataMember(Name = "humidity")]
	public int? Humidity { get; set; }

	[DataMember(Name = "flag")]
	public bool? Flag { get; set; }
	}

  [DataContract]
  public class SensorConfig
  {
    [DataMember(Name = "on")]
    public bool? On { get; set; }

    [DataMember(Name = "long")]
    public string Long { get; set; }

    [DataMember(Name = "lat")]
    public string Lat { get; set; }

    [DataMember(Name = "sunriseoffset")]
    public int? SunriseOffset { get; set; }

    [DataMember(Name = "sunsetoffset")]
    public int? SunsetOffset { get; set; }

    [DataMember(Name = "url")]
    public string Url { get; set; }

    [DataMember(Name = "reachable")]
    public bool? Reachable { get; set; }

    [DataMember(Name = "battery")]
    public int? Battery { get; set; }
  }

  
}
