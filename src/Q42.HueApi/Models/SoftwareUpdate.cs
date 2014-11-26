using System.Runtime.Serialization;

namespace Q42.HueApi
{
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
    public int UpdateState { get; set; }

    [DataMember (Name = "url")]
    public string Url { get; set; }

    [DataMember (Name = "text")]
    public string Text { get; set; }

    [DataMember (Name = "notify")]
    public bool Notify { get; set; }
  }
}