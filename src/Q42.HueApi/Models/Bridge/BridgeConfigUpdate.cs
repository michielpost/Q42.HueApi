using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System;

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

    [Obsolete]
    [DataMember(Name = "swupdate")]
    public SoftwareUpdate SoftwareUpdate { get; set; }

    [DataMember(Name = "swupdate2")]
    public SoftwareUpdate2 SoftwareUpdate2 { get; set; }

    [DataMember(Name = "linkbutton")]
    public bool? LinkButton { get; set; }

    [DataMember(Name = "portalservices")]
    public bool? PortalServices { get; set; }

    [DataMember(Name = "timezone")]
    public string TimeZone { get; set; }

	/// <summary>
	/// As of 1.9. If set to true performs a touchlink action.
	/// </summary>
	[DataMember(Name = "touchlink")]
	public bool? TouchLink { get; set; }

    /// <summary>
    /// The current wireless frequency channel used by the bridge. It can take values of 11, 15, 20,25 or 0 if undefined (factory new).
    /// </summary>
    [DataMember(Name = "zigbeechannel")]
    public int? ZigbeeChannel { get; set; }
  }
}