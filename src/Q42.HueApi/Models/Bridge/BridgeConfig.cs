using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using Q42.HueApi.Models.Bridge;
using Q42.HueApi.Converters;

namespace Q42.HueApi
{
  public class BridgeConfig
  {

    public BridgeConfig()
    {
      WhiteList = new Dictionary<string, WhiteList>();
    }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("mac")]
    public string MacAddress { get; set; }

    [JsonProperty("dhcp")]
    public bool Dhcp { get; set; }

    [JsonProperty("ipaddress")]
    public string IpAddress { get; set; }

    [JsonProperty("netmask")]
    public string NetMask { get; set; }

    [JsonProperty("gateway")]
    public string Gateway { get; set; }

    [JsonProperty("UTC")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? Utc { get; set; }

    [JsonProperty("swversion")]
    public string SoftwareVersion { get; set; }

    [Obsolete]
    [JsonProperty("swupdate")]
    public SoftwareUpdate SoftwareUpdate { get; set; }

    [JsonProperty("swupdate2")]
    public SoftwareUpdate2 SoftwareUpdate2 { get; set; }

    [JsonProperty("whitelist")]
    public IDictionary<string, WhiteList> WhiteList { get; set; }

    [JsonProperty("linkbutton")]
    public bool LinkButton { get; set; }

    [JsonProperty("portalservices")]
    public bool PortalServices { get; set; }

    [JsonProperty("portalconnection")]
    public string PortalConnection { get; set; }

    [JsonProperty("apiversion")]
    public string ApiVersion { get; set; }

    [JsonProperty("localtime")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? LocalTime { get; set; }

    [JsonProperty("timezone")]
    public string TimeZone { get; set; }

    [JsonProperty("portalstate")]
    public PortalState PortalState { get; set; }

    [JsonProperty("zigbeechannel")]
    public int ZigbeeChannel { get; set; }

    /// <summary>
    /// Perform a touchlink action if set to true, setting to false is ignored. When set to true a touchlink procedure starts which adds the closet lamp (within range) to the ZigBee network.  You can then search for new lights and lamp will show up in the bridge.
    /// </summary>
    [JsonProperty("touchlink")]
    public bool TouchLink { get; set; }

    /// <summary>
    /// Indicates if bridge settings are factory new.
    /// </summary>
    [JsonProperty("factorynew")]
    public bool FactoryNew { get; set; }

    /// <summary>
    ///  If a bridge backup file has been restored on this bridge from a bridge with a different bridgeid, it will indicate that bridge id, otherwise it will be null.
    /// </summary>
    [JsonProperty("replacesbridgeid")]
    public string ReplacesBridgeId { get; set; }

    /// <summary>
    /// This parameter uniquely identifies the hardware model of the bridge (BSB001, BSB002).
    /// </summary>
    [JsonProperty("modelid")]
    public string ModelId { get; set; }

    /// <summary>
    /// The unique bridge id. This is currently generated from the bridge Ethernet mac address.
    /// </summary>
    [JsonProperty("bridgeid")]
    public string BridgeId { get; set; }

    [JsonProperty("datastoreversion")]
    public string DataStoreVersion { get; set; }

    [JsonProperty("starterkitid")]
    public string StarterKitId { get; set; }

    [JsonProperty("internetservices")]
    public InternetServices InternetServices { get; set; }

  }

}
