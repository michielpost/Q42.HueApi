using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Q42.HueApi
{
  /// <summary>
  /// Allowed properties to update the BridgeConfig
  /// </summary>
  [DataContract]
  public class BridgeConfigUpdate
  {

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "dhcp")]
    public bool? Dhcp { get; set; }

    [DataMember(Name = "ipaddress")]
    public string IpAddress { get; set; }

    [DataMember(Name = "netmask")]
    public string NetMask { get; set; }

    [DataMember(Name = "gateway")]
    public string Gateway { get; set; }

    [DataMember(Name = "proxyaddress")]
    public string ProxyAddress { get; set; }

    [DataMember(Name = "proxyport")]
    public int? ProxyPort { get; set; }

    [DataMember(Name = "swupdate")]
    public SoftwareUpdate SoftwareUpdate { get; set; }

    [DataMember(Name = "linkbutton")]
    public bool? LinkButton { get; set; }

    [DataMember(Name = "portalservices")]
    public bool? PortalServices { get; set; }
  }
}