using Newtonsoft.Json;
using Q42.HueApi.Interfaces;

namespace Q42.HueApi
{

  /// <summary>
  /// Send a SceneID as command
  /// </summary>
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
    [JsonProperty("scene")]
    public string Scene { get; set; }

    [JsonProperty("storelightstate")]
    public bool? StoreLightState { get; set; }

  }

}
