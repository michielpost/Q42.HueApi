using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Q42.HueApi
{
  [DataContract]
  public class SoftwareUpdate2
  {
	/// <summary>
	/// Setting this flag to true lets the bridge search for software updates in the portal. After the search attempt, this flag is set back to false. Requires portal connection to update server
	/// http://www.developers.meethue.com/documentation/software-update
	/// </summary>
	[DataMember(Name = "checkforupdate")]
    public bool CheckForUpdate { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    [DataMember(Name = "state")]
    public SoftwareUpdateState State { get; set; }

    /// <summary>
    /// Writing “true” triggers installation of software updates when in state anyreadytoinstall or allreadytoinstall.
    /// </summary>
    [DataMember(Name = "install")]
    public bool? Install { get; set; }

    /// <summary>
    /// Timestamp of last change in system configuration
    /// </summary>
    [DataMember(Name = "lastchange")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTimeOffset? LastChange { get; set; }

    [DataMember(Name = "autoinstall")]
    public SoftwareUpdateAutoInstall AutoInstall { get; set; }

    [DataMember(Name = "bridge")]
    public SoftwareUpdateBridge Bridge { get; set; }

  }

  [DataContract]
  public class SoftwareUpdateBridge
  {
    /// <summary>
    /// Time of last software update.
    /// </summary>
    [DataMember(Name = "lastinstall")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTimeOffset? LastInstall { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    [DataMember(Name = "state")]
    public SoftwareUpdateState State { get; set; }
  }

  [DataContract]
  public class SoftwareUpdateAutoInstall
  {
    /// <summary>
    /// Indicates if automatic update is activated. Default is false
    /// </summary>
    [DataMember(Name = "on")]
    public bool? On { get; set; }

    /// <summary>
    /// T[hh]:[mm]:[ss] Local time of day. 
    /// The bridge auto. updates for bridge and zigbee devices. The installation time will be randomized between updatetime and updatetime+T01:00:00. Default is T14:00:00.
    /// </summary>
    [DataMember(Name = "updatetime")]
    public string UpdateTime { get; set; }
  }

  /// <summary>
  /// Possible SoftwareUpdate States
  /// </summary>
  public enum SoftwareUpdateState
  {
    [EnumMember(Value = "unknown")]
    Unknown,
    [EnumMember(Value = "notupdatable")]
    NotUpdatable,
    [EnumMember(Value = "noupdates")]
    NoUpdates,
    [EnumMember(Value = "transferring")]
    Transferring,
    [EnumMember(Value = "anyreadytoinstall")]
    AnyReadytToInstall,
    [EnumMember(Value = "allreadytoinstall")]
    AllReadyToInstall,
    [EnumMember(Value = "installing")]
    Installing,
    [EnumMember(Value = "readytoinstall")]
    ReadyToInstall,
  }

}
