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

    [DataMember(Name = "swupdate")]
    public SwUpdate SwUpdate { get; set; }

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
    [DataMember(Name = "streaming")]
    public StreamingLightCapabilities Streaming { get; set; }
  }
}
