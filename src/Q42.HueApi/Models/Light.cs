using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Models.Gamut;
using System;
using System.Runtime.Serialization;

namespace Q42.HueApi
{
  public class Light
  {
    public string Id { get; set; }

    [JsonProperty("state")]
    public State State { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("modelid")]
    public string ModelId { get; set; }

    [JsonProperty("productid")]
    public string ProductId { get; set; }

    [JsonProperty("swconfigid")]
    public string SwConfigId { get; set; }

    /// <summary>
    /// Unique id of the device. The MAC address of the device with a unique endpoint id in the form: AA:BB:CC:DD:EE:FF:00:11-XX
    /// </summary>
    [JsonProperty("uniqueid")]
    public string UniqueId { get; set; }

    /// <summary>
    /// As of 1.9. Unique ID of the luminaire the light is a part of in the format: AA:BB:CC:DD-XX-YY.  AA:BB:, ... represents the hex of the luminaireid, XX the lightsource position (incremental but may contain gaps) and YY the lightpoint position (index of light in luminaire group).  A gap in the lightpoint position indicates an incomplete luminaire (light search required to discover missing light points in this case).
    /// </summary>
    [JsonProperty("luminaireuniqueid")]
    public string LuminaireUniqueId { get; set; }

    [JsonProperty("manufacturername")]
    public string ManufacturerName { get; set; }

    [JsonProperty("swversion")]
    public string SoftwareVersion { get; set; }

    [JsonProperty("capabilities")]
    public LightCapabilities Capabilities { get; set; }

    [JsonProperty("config")]
    public LightConfig Config { get; set; }

    [JsonProperty("swupdate")]
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

  public class SwUpdate
  {
    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("lastinstall")]
    public DateTime? Lastinstall { get; set; }
  }

  public class StreamingLightCapabilities
  {
    [JsonProperty("renderer")]
    public bool Renderer { get; set; }
    [JsonProperty("proxy")]
    public bool Proxy { get; set; }
  }

  public class LightCapabilities
  {
    [JsonProperty("certified")]
    public bool Certified { get; set; }

    [JsonProperty("control")]
    public Control Control { get; set; }

    [JsonProperty("streaming")]
    public StreamingLightCapabilities Streaming { get; set; }
  }

  public class Control
  {
    [JsonProperty("mindimlevel")]
    public int? MinDimLevel { get; set; }

    [JsonProperty("maxlumen")]
    public int? MaxLumen { get; set; }

    /// <summary>
    /// A, B or C
    /// </summary>
    [JsonProperty("colorgamuttype")]
    public string ColorGamutType { get; set; }

    [JsonProperty("colorgamut")]
    public CIE1931Gamut? ColorGamut { get; set; }

    [JsonProperty("ct")]
    public ColorTemperature ColorTemperature { get; set; }
  }

  public class ColorTemperature
  {
    [JsonProperty("min")]
    public int Min { get; set; }
    [JsonProperty("max")]
    public int Max { get; set; }
  }

  public class LightConfig
  {
    [JsonProperty("archetype")]
    public string ArcheType { get; set; }

    [JsonProperty("function")]
    public string Function { get; set; }

    [JsonProperty("direction")]
    public string Direction { get; set; }

    [JsonProperty("startup")]
    public LightStartup Startup { get; set; }
  }

  public class LightStartup
  {
    [JsonProperty("mode")]
    [JsonConverter(typeof(StringEnumConverter))]
    public StartupMode? Mode { get; set; }

    [JsonProperty("configured")]
    public bool? Configured { get; set; }

    /// <summary>
    /// Only bri, xy, ct properties are used
    /// </summary>
    [JsonProperty("customsettings")]
    public LightCommand CustomSettings { get; set; }
  }

  /// <summary>
  /// Defined on https://developers.meethue.com/develop/hue-api/supported-devices/
  /// </summary>
  [JsonConverter(typeof(StringEnumConverter))]
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
