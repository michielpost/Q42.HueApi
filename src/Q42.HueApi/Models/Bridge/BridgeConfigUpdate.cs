using Newtonsoft.Json;
using System;

namespace Q42.HueApi
{
  /// <summary>
  /// Allowed properties to update the BridgeConfig
  /// </summary>
  public class BridgeConfigUpdate
  {

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("dhcp")]
    public bool? Dhcp { get; set; }

    [JsonProperty("ipaddress")]
    public string IpAddress { get; set; }

    [JsonProperty("netmask")]
    public string NetMask { get; set; }

    [JsonProperty("gateway")]
    public string Gateway { get; set; }

    [Obsolete]
    [JsonProperty("swupdate")]
    public SoftwareUpdate SoftwareUpdate { get; set; }

    [JsonProperty("swupdate2")]
    public SoftwareUpdate2 SoftwareUpdate2 { get; set; }

    [JsonProperty("linkbutton")]
    public bool? LinkButton { get; set; }

    [JsonProperty("portalservices")]
    public bool? PortalServices { get; set; }

    [JsonProperty("timezone")]
    public string TimeZone { get; set; }

	/// <summary>
	/// As of 1.9. If set to true performs a touchlink action.
	/// </summary>
	[JsonProperty("touchlink")]
	public bool? TouchLink { get; set; }

    /// <summary>
    /// The current wireless frequency channel used by the bridge. It can take values of 11, 15, 20,25 or 0 if undefined (factory new).
    /// </summary>
    [JsonProperty("zigbeechannel")]
    public int? ZigbeeChannel { get; set; }
  }
}
