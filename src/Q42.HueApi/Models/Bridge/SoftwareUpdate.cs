using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Q42.HueApi
{
  [Obsolete]
  [DataContract]
  public class SoftwareUpdate
  {
	  /// <summary>
	  /// 0 means there is no update at all.
	  /// 1 means there is an update to download. The bridge will eventually download it by himself. 
	  /// 2 means there is an update available to apply.
	  /// 3 means apply/applying the update. 
	  /// http://www.everyhue.com/vanilla/discussion/484/firmware-update-triggering
	  /// </summary>
    [DataMember (Name = "updatestate")]
    public int? UpdateState { get; set; }

    [DataMember (Name = "url")]
    public string Url { get; set; }

    [DataMember (Name = "text")]
    public string Text { get; set; }

    [DataMember (Name = "notify")]
    public bool? Notify { get; set; }

	/// <summary>
	/// Setting this flag to true lets the bridge search for software updates in the portal. After the search attempt, this flag is set back to false. Requires portal connection to update server
	/// http://www.developers.meethue.com/documentation/software-update
	/// </summary>
	[DataMember(Name = "checkforupdate")]
    public bool CheckForUpdate { get; set; }

    [DataMember(Name = "devicetypes")]
    public SoftwareUpdateDevices DeviceTypes { get; set; }
  }

  [Obsolete]
  [DataContract]
  public class SoftwareUpdateDevices
  {
    [DataMember(Name = "bridge")]
    public bool Bridge { get; set; }

    [DataMember(Name = "lights")]
    public IReadOnlyCollection<string> Lights { get; set; }

    [DataMember(Name = "sensors")]
    public IReadOnlyCollection<string> Sensors { get; set; }

  }
}