using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using Q42.HueApi.Models;
using System;

namespace Q42.HueApi
{
  [DataContract]
  public class BridgeConfig
  {

    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "mac")]
    public string MacAddress { get; set; }

    [DataMember(Name = "dhcp")]
    public bool Dhcp { get; set; }

    [DataMember(Name = "ipaddress")]
    public string IpAddress { get; set; }

    [DataMember(Name = "netmask")]
    public string NetMask { get; set; }

    [DataMember(Name = "gateway")]
    public string Gateway { get; set; }

    [DataMember(Name = "proxyaddress")]
    public string ProxyAddress { get; set; }

    [DataMember(Name = "proxyport")]
    public int ProxyPort { get; set; }

    //Cant be a DateTime? because when value is not available, HueBridge sends value "none"
    //TODO: Create custom json deserializer
    [DataMember(Name = "UTC")]
    public string Utc { get; set; }

    [DataMember(Name = "swversion")]
    public string SoftwareVersion { get; set; }

    [DataMember(Name = "swupdate")]
    public SoftwareUpdate SoftwareUpdate { get; set; }

    [DataMember(Name = "whitelist")]
    public IDictionary<string, WhiteList> WhiteList { get; set; }

    [DataMember(Name = "linkbutton")]
    public bool LinkButton { get; set; }

    [DataMember(Name = "portalservices")]
    public bool PortalServices { get; set; }

    [DataMember(Name = "portalconnection")]
    public string PortalConnection { get; set; }

    [DataMember(Name = "apiversion")]
    public string ApiVersion { get; set; }

    //Cant be a DateTime? because when value is not available, HueBridge sends value "none"
    //TODO: Create custom json deserializer
    [DataMember(Name = "localtime")]
    public string LocalTime { get; set; }

    [DataMember(Name = "timezone")]
    public string TimeZone { get; set; }

    [DataMember(Name = "portalstate")]
    public PortalState PortalState { get; set; }

    [DataMember(Name = "zigbeechannel")]
    public int ZigbeeChannel { get; set; }
  }

}