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

    /// <summary>
    /// True if the valid GPS coordinates have been set.
    /// </summary>
    [DataMember(Name = "configured")]
    public bool? Configured { get; set; }

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

    [DataMember(Name = "battery")]
    public int? Sensitivity { get; set; }

    [DataMember(Name = "sensitivitymax")]
    public int? SensitivityMax { get; set; }

    /// <summary>
    /// Activates or extends user usertest mode of device for 120 seconds.  False deactivates usertest mode.
    /// </summary>
    [DataMember(Name = "usertest")]
    public string UserTest { get; set; }

    /// <summary>
    /// Array of config parameters which is not yet committed to sensor.  As long as the atrribute is listed here, the configuration attribute value listed on GET does not take effect and might return to previous value.  A subsequent PUT on listed atrribute might return error 10.
    /// </summary>
    [DataMember(Name = "pending")]
    public List<string> Pending { get; set; }

    /// <summary>
    /// Turns device LED during normal operation on or off.  Devices might still indicate exceptional operation (Reset, SW Update, Battery Low)
    /// Optional, only used for ZLL sensors.
    /// </summary>
    [DataMember(Name = "ledindication")]
    public bool? LedIndication { get; set; }
    }
}
