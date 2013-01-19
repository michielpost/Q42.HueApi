using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Q42.HueApi.Models
{
  /// <summary>
  /// Status data returned from the bridge
  /// </summary>
  public class BridgeBridge
  {
    public Dictionary<string, Light> lights { get; set; }
    public Config config { get; set; }
  }

  public class Bridge
  {
    public Bridge(BridgeBridge bridge)
    {
      if (bridge == null)
        throw new ArgumentNullException ("bridge");

      config = bridge.config;
      foreach (var light in bridge.lights)
        light.Value.id = light.Key;
      lights = bridge.lights.Select(l => l.Value).ToList();
    }

    public List<Light> lights { get; set; }
    public Config config { get; set; }
  }

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

  [DataContract]
  public class BridgeConfig
  {
    [DataMember (Name = "name")]
    public string Name { get; set; }

    [DataMember (Name = "mac")]
    public string MacAddress { get; set; }

    [DataMember (Name = "dhcp")]
    public bool Dhcp { get; set; }

    [DataMember (Name = "ipaddress")]
    public string IpAddress { get; set; }

    [DataMember (Name = "netmask")]
    public string NetMask { get; set; }

    [DataMember (Name = "gateway")]
    public string Gateway { get; set; }

    [DataMember (Name = "proxyaddress")]
    public string ProxyAddress { get; set; }

    [DataMember (Name = "proxyport")]
    public int ProxyPort { get; set; }

    [DataMember (Name = "UTC")]
    public string Utc { get; set; }

    [DataMember (Name = "swversion")]
    public string SoftwareVersion { get; set; }

    [DataMember (Name = "swupdate")]
    public SoftwareUpdate SoftwareUpdate { get; set; }

    [DataMember (Name = "linkbutton")]
    public bool LinkButton { get; set; }

    [DataMember (Name = "portalservices")]
    public bool PortalServices { get; set; }
  }

  [DataContract]
  public class State
  {
    [DataMember (Name = "on")]
    public bool On { get; set; }

    [DataMember (Name = "bri")]
    public byte Brightness { get; set; }

    [DataMember (Name = "hue")]
    public int Hue { get; set; }

    [DataMember (Name = "sat")]
    public int Saturation { get; set; }

    [DataMember (Name = "xy")]
    public double[] ColorCoordinates { get; set; }

    [DataMember (Name = "ct")]
    public int ColorTemperature { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    [DataMember (Name = "alert")]
    public Alerts Alert { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    [DataMember (Name = "effect")]
    public Effects Effect { get; set; }

    [DataMember (Name = "colormode")]
    public string ColorMode { get; set; }

    [DataMember (Name = "reachable")]
    public bool IsReachable { get; set; }

    public string ToHex()
    {
      return HueColorConverter.HexFromState(this);
    }
  }

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
