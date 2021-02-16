using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Q42.HueApi
{
  [Obsolete]
  public class SoftwareUpdate
  {
	  /// <summary>
	  /// 0 means there is no update at all.
	  /// 1 means there is an update to download. The bridge will eventually download it by himself. 
	  /// 2 means there is an update available to apply.
	  /// 3 means apply/applying the update. 
	  /// http://www.everyhue.com/vanilla/discussion/484/firmware-update-triggering
	  /// </summary>
    [JsonProperty("updatestate")]
    public int? UpdateState { get; set; }

    [JsonProperty("url")]
    public string Url { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("notify")]
    public bool? Notify { get; set; }

	/// <summary>
	/// Setting this flag to true lets the bridge search for software updates in the portal. After the search attempt, this flag is set back to false. Requires portal connection to update server
	/// http://www.developers.meethue.com/documentation/software-update
	/// </summary>
	[JsonProperty("checkforupdate")]
    public bool CheckForUpdate { get; set; }

    [JsonProperty("devicetypes")]
    public SoftwareUpdateDevices DeviceTypes { get; set; }
  }

  [Obsolete]
  public class SoftwareUpdateDevices
  {
    [JsonProperty("bridge")]
    public bool Bridge { get; set; }

    [JsonProperty("lights")]
    public IReadOnlyCollection<string> Lights { get; set; }

    [JsonProperty("sensors")]
    public IReadOnlyCollection<string> Sensors { get; set; }

  }
}
