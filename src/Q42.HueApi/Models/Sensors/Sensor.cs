using Newtonsoft.Json;
using Q42.HueApi.Converters;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Sensors;
using Q42.HueApi.Models.Sensors.CLIP;
using Q42.HueApi.Models.Sensors.ZigBee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  /// <summary>
  /// Based on http://www.developers.meethue.com/documentation/supported-sensors
  /// </summary>
  public class Sensor :
      CLIPGenericFlag,
      CLIPGenericStatus,
      CLIPHumidity,
      CLIPLightlevel,
      CLIPOpenClose,
      CLIPPresence,
      CLIPSwitch,
      CLIPTemperature,
      DaylightSensor,
      ZGPSwitch,
      ZLLPresence,
      ZLLSwitch,
      ZLLTemperature
  {
    [JsonProperty("state")]
    public SensorState State { get; set; }
    [JsonProperty("config")]
    public SensorConfig Config { get; set; }

    [JsonProperty("capabilities")]
    public SensorCapabilities Capabilities { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("manufacturername")]
    public string ManufacturerName { get; set; }

    [JsonProperty("modelid")]
    public string ModelId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("productid")]
    public string ProductId { get; set; }

    [JsonProperty("swconfigid")]
    public string SwConfigId { get; set; }

    [JsonProperty("swversion")]
    public string SwVersion { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("uniqueid")]
    public string UniqueId { get; set; }
  }

  public class SensorState : ICommandBody,
      CLIPGenericFlagState,
      CLIPGenericStatusState,
      CLIPHumidityState,
      CLIPLightlevelState,
      CLIPOpenCloseState,
      CLIPPresenceState,
      CLIPSwitchState,
      CLIPTemperatureState,
      DaylightSensorState,
      ZGPSwitchState,
      ZLLPresenceState,
      ZLLSwitchState,
      ZLLTemperatureState
  {
    [JsonProperty("buttonevent")]
    public int? ButtonEvent { get; set; }

    [JsonProperty("dark")]
    public bool? Dark { get; set; }
    [JsonProperty("daylight")]
    public bool? Daylight { get; set; }

    [JsonProperty("flag")]
    public bool? Flag { get; set; }

    [JsonProperty("humidity")]
    public int? Humidity { get; set; }

    [JsonProperty("lastupdated")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? Lastupdated { get; set; }

    [JsonProperty("lightlevel")]
    public long? LightLevel { get; set; }

    [JsonProperty("open")]
    public bool? Open { get; set; }

    [JsonProperty("presence")]
    public bool? Presence { get; set; }

    [JsonProperty("status")]
    public int? Status { get; set; }

    [JsonProperty("temperature")]
    public int? Temperature { get; set; }
  }

  public class SensorConfig :
      CLIPGenericFlagConfig,
      CLIPGenericStatusConfig,
      CLIPHumidityConfig,
      CLIPLightlevelConfig,
      CLIPOpenCloseConfig,
      CLIPPresenceConfig,
      CLIPSwitchConfig,
      CLIPTemperatureConfig,
      DaylightSensorConfig,
      ZGPSwitchConfig,
      ZLLPresenceConfig,
      ZLLSwitchConfig,
      ZLLTemperatureConfig
  {
    [JsonProperty("alert")]
    public string Alert { get; set; }

    [JsonProperty("battery")]
    public int? Battery { get; set; }

    [JsonProperty("configured")]
    public bool? Configured { get; }

    [JsonProperty("lat")]
    public string Lat { get; set; }

    [JsonProperty("long")]
    public string Long { get; set; }

    [JsonProperty("ledindication")]
    public bool? LedIndication { get; set; }

    [JsonProperty("on")]
    public bool? On { get; set; }

    [JsonProperty("pending")]
    public List<string> Pending { get; set; }

    [JsonProperty("reachable")]
    public bool? Reachable { get; set; }

    [JsonProperty("sunriseoffset")]
    public int? SunriseOffset { get; set; }

    [JsonProperty("sunsetoffset")]
    public int? SunsetOffset { get; set; }

    [JsonProperty("tholddark")]
    public long? TholdDark { get; set; }

    [JsonProperty("tholdoffset")]
    public long? TholdOffset { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("usertest")]
    public bool? Usertest { get; set; }

    [JsonProperty("sensitivity")]
    public int? Sensitivity { get; set; }

    [JsonProperty("sensitivitymax")]
    public int? SensitivityMax { get; set; }
  }

  public class SensorCapabilities :
       GeneralSensorCapabilities
  {
    [JsonProperty("certified")]
    public bool? Certified { get; set; }

    [JsonProperty("primary")]
    public bool? Primary { get; set; }

    [JsonProperty("inputs")]
    public SensorInput[]? Inputs { get; set; }
  }

  public class SensorInput
  {
	  [JsonProperty("repeatintervals")]
	  public int[] RepeatIntervals { get; set; } = default!;
	
	  [JsonProperty("events")]
	  public SensorEvent[] Events { get; set; } = default!;
  }

  public class SensorEvent
  {
    [JsonProperty("buttonevent")]
    public int ButtonEvent { get; set; }

    [JsonProperty("eventtype")]
    public string EventType { get; set; } = default!;
  }
}
