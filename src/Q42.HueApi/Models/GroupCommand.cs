using System;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Q42.HueApi
{

  /// <summary>
  /// Compose a light command to send to a light
  /// </summary>
  [DataContract]
  public class GroupCommand : LightCommand
  {
    /// <summary>
    /// Scene ID to activate
    /// </summary>
    [DataMember (Name = "scene")]
    public string Scene { get; set; }

  }

}
