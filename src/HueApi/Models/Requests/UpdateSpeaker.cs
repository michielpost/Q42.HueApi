using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdateSpeaker : BaseResourceRequest
  {
    [JsonPropertyName("alarm")]
    public SpeakerSoundRequest? Alarm { get; set; }

    [JsonPropertyName("chime")]
    public SpeakerSoundRequest? Chime { get; set; }

    [JsonPropertyName("alert")]
    public SpeakerSoundRequest? Alert { get; set; }

    [JsonPropertyName("mute")]
    public MuteData? Mute { get; set; }
  }

  public class SpeakerSoundRequest
  {
    [JsonPropertyName("sound")]
    public SupportedSounds Sound { get; set; }

    /// <summary>
    /// Requested volume on play sound request
    /// </summary>
    [JsonPropertyName("volume")]
    public SpeakerVolume? Volume { get; set; }

    /// <summary>
    /// Only supported for alarm. Stepsize of 1000 ms. Values in-between steps will round-up to next multiple of 1000 (minimum: 0 – maximum: 65534000)
    /// </summary>
    [JsonPropertyName("duration")]
    public long? Duration { get; set; }
  }

  public class SpeakerVolume
  {
    /// <summary>
    /// minimum: 0 – maximum: 100
    /// </summary>
    [JsonPropertyName("level")]
    public double Level { get; set; }
  }
}
