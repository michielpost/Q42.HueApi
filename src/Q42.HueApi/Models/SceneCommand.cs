using System;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Interfaces;

namespace Q42.HueApi
{

  /// <summary>
  /// Send a SceneID as command
  /// </summary>
  [DataContract]
  public class SceneCommand : ICommandBody
  {
    public SceneCommand()
    {

    }

    public SceneCommand(string sceneId)
    {
      this.Scene = sceneId;
    }

    /// <summary>
    /// Scene ID to activate
    /// </summary>
    [DataMember(Name = "scene")]
    public string Scene { get; set; }

    [DataMember(Name = "storelightstate")]
    public bool? StoreLightState { get; set; }

  }

}
