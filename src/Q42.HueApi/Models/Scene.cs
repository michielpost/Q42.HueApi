using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Converters;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Q42.HueApi.Models
{
  public class Scene
  {
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("lights")]
    public IEnumerable<string> Lights { get; set; }

    /// <summary>
    /// Whitelist user that created or modified the content of the scene. Note that changing name does not change the owner.
    /// </summary>
    [JsonProperty("owner")]
    public string Owner { get; set; }

    /// <summary>
    /// App specific data linked to the scene.  Each individual application should take responsibility for the data written in this field.
    /// </summary>
    [JsonProperty("appdata")]
    public SceneAppData? AppData { get; set; }

    /// <summary>
    /// Only available on a GET of an individual scene resource (/api/<username>/scenes/<id>). Not available for scenes created via a PUT in version 1. . Reserved for future use.
    /// </summary>
    [JsonProperty("picture")]
    public string? Picture { get; set; }

    /// <summary>
    /// Indicates whether the scene can be automatically deleted by the bridge. Only available by POSTSet to 'false' when omitted. Legacy scenes created by PUT are defaulted to true. When set to 'false' the bridge keeps the scene until deleted by an application.
    /// </summary>
    [JsonProperty("recycle")]
    public bool? Recycle { get; set; }


    /// <summary>
    /// Indicates that the scene is locked by a rule or a schedule and cannot be deleted until all resources requiring or that reference the scene are deleted.
    /// </summary>
    [JsonProperty("locked")]
    public bool? Locked { get; set; }

    [JsonProperty("version")]
    public int? Version { get; set; }

    [JsonProperty("lastupdated")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? LastUpdated { get; set; }

    [JsonProperty("storelightstate")]
    public bool? StoreLightState { get; set; }

    [JsonProperty("transitiontime")]
    [JsonConverter(typeof(TransitionTimeConverter))]
    public TimeSpan? TransitionTime { get; set; }

    [JsonProperty("lightstates")]
    public Dictionary<string, State> LightStates { get; set; }

    /// <summary>
    /// null defaults to LightScene
    /// </summary>
    [JsonProperty("type")]
    [JsonConverter(typeof(StringEnumConverter))]
    public SceneType? Type { get; set; }

    /// <summary>
    /// When using SceneType.GroupScene: group ID that a scene is linked to.
    /// </summary>
    [JsonProperty("group")]
    public string? Group { get; set; }

    /// <summary>
    /// 1.36 Unique ID for an image representing the scene. Only available for scenes create from Signify images by Hue application.
    /// </summary>
    [JsonProperty("image")]
    public string? Image { get; set; }

    /// <summary>
    /// Overrides ToString() to give something more useful than object name.
    /// </summary>
    /// <returns>A string like "Scene 1: Brightest"</returns>
    public override string ToString()
    {
      return String.Format("Scene {0}: {1}", Id, Name);
    }

  }

  public class SceneAppData
  {
    [JsonProperty("version")]
    public int? Version { get; set; }
    [JsonProperty("data")]
    public string Data { get; set; }
  }

  [JsonConverter(typeof(StringEnumConverter))]
  public enum SceneType
  {
    [EnumMember(Value = "LightScene")]
    LightScene,
    [EnumMember(Value = "GroupScene")]
    GroupScene,
  }
}
