using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Models.Gamut;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Q42.HueApi
{
  [DataContract]
  public class Light
  {
    [DataMember(Name = "id")]
    public string Id { get; set; }

    [DataMember(Name = "state")]
    public State State { get; set; }

    [DataMember(Name = "type")]
    public string Type { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "modelid")]
    public string ModelId { get; set; }

    [DataMember(Name = "productid")]
    public string ProductId { get; set; }

    [DataMember(Name = "swconfigid")]
    public string SwConfigId { get; set; }

    /// <summary>
    /// Unique id of the device. The MAC address of the device with a unique endpoint id in the form: AA:BB:CC:DD:EE:FF:00:11-XX
    /// </summary>
    [DataMember(Name = "uniqueid")]
    public string UniqueId { get; set; }

    /// <summary>
    /// As of 1.9. Unique ID of the luminaire the light is a part of in the format: AA:BB:CC:DD-XX-YY.  AA:BB:, ... represents the hex of the luminaireid, XX the lightsource position (incremental but may contain gaps) and YY the lightpoint position (index of light in luminaire group).  A gap in the lightpoint position indicates an incomplete luminaire (light search required to discover missing light points in this case).
    /// </summary>
    [DataMember(Name = "luminaireuniqueid")]
    public string LuminaireUniqueId { get; set; }

    [DataMember(Name = "manufacturername")]
    public string ManufacturerName { get; set; }

    [DataMember(Name = "swversion")]
    public string SoftwareVersion { get; set; }

    [DataMember(Name = "capabilities")]
    public LightCapabilities Capabilities { get; set; }

    [DataMember(Name = "config")]
    public LightConfig Config { get; set; }

    [DataMember(Name = "swupdate")]
    public SwUpdate SwUpdate { get; set; }

    /// <summary>
    /// Overrides ToString() to give something more useful than object name.
    /// </summary>
    /// <returns>A string like "Light 1: Pendant"</returns>
    public override string ToString()
    {
      return String.Format("Light {0}: {1}", Id, Name);
    }
  }

  [DataContract]
  public class SwUpdate
  {
    [DataMember(Name = "state")]
    public string State { get; set; }

    [DataMember(Name = "lastinstall")]
    public DateTime? Lastinstall { get; set; }
  }

  [DataContract]
  public class StreamingLightCapabilities
  {
    [DataMember(Name = "renderer")]
    public bool Renderer { get; set; }
    [DataMember(Name = "proxy")]
    public bool Proxy { get; set; }
  }

  [DataContract]
  public class LightCapabilities
  {
    [DataMember(Name = "certified")]
    public bool Certified { get; set; }

    [DataMember(Name = "control")]
    public Control Control { get; set; }

    [DataMember(Name = "streaming")]
    public StreamingLightCapabilities Streaming { get; set; }
  }

  [DataContract]
  public class Control
  {
    [DataMember(Name = "mindimlevel")]
    public int? MinDimLevel { get; set; }

    [DataMember(Name = "maxlumen")]
    public int? MaxLumen { get; set; }

    /// <summary>
    /// A, B or C
    /// </summary>
    [DataMember(Name = "colorgamuttype")]
    public string ColorGamutType { get; set; }

    [DataMember(Name = "colorgamut")]
    public CIE1931Gamut? ColorGamut { get; set; }

    [DataMember(Name = "ct")]
    public ColorTemperature ColorTemperature { get; set; }
  }

  [DataContract]
  public class ColorTemperature
  {
    [DataMember(Name = "min")]
    public int Min { get; set; }
    [DataMember(Name = "max")]
    public int Max { get; set; }
  }

  [DataContract]
  public class LightConfig
  {
    [DataMember(Name = "archetype")]
    public string ArcheType { get; set; }

    [DataMember(Name = "function")]
    public string Function { get; set; }

    [DataMember(Name = "direction")]
    public string Direction { get; set; }

    [DataMember(Name = "startup")]
    public LightStartup Startup { get; set; }
  }

  [DataContract]
  public class LightStartup
  {
    [DataMember(Name = "mode")]
    [JsonConverter(typeof(StringEnumConverter))]
    public StartupMode? Mode { get; set; }

    [DataMember(Name = "configured")]
    public bool? Configured { get; set; }

    /// <summary>
    /// Only bri, xy, ct properties are used
    /// </summary>
    [DataMember(Name = "customsettings")]
    public LightCommand CustomSettings { get; set; }
  }

  /// <summary>
  /// Defined on https://developers.meethue.com/develop/hue-api/supported-devices/
  /// </summary>
  public enum StartupMode
  {
    [EnumMember(Value = "safety")]
    Safety,
    [EnumMember(Value = "powerfail")]
    Powerfail,
    [EnumMember(Value = "lastonstate")]
    LastOnState,
    [EnumMember(Value = "custom")]
    Custom,
    [EnumMember(Value = "unknown")]
    Unknown
  }
}
