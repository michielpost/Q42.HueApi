using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class SpeakerResource : HueResource
  {
    [JsonPropertyName("alarm")]
    public SoundData? Alarm { get; set; }

    [JsonPropertyName("chime")]
    public SoundData? Chime { get; set; }

    [JsonPropertyName("alert")]
    public SoundData? Alert { get; set; }

    [JsonPropertyName("mute")]
    public MuteData? Mute { get; set; }

  }

  public class MuteData
  {
    [JsonPropertyName("mute")]
    public MuteValue? Mute { get; set; }

  }

  public class SoundData
  {
    [JsonPropertyName("sound_values")]
    public List<SupportedSounds>? SoundValues { get; set; }

    [JsonPropertyName("status")]
    public SoundStatus? Status { get; set; }

  }

  public class SoundStatus
  {
    [JsonPropertyName("sound")]
    public SupportedSounds? Sound { get; set; }

    [JsonPropertyName("sound_values")]
    public List<SupportedSounds>? SoundValues { get; set; }

    [JsonPropertyName("estimated_end")]
    public SoundStatusEstimatedEnd? EstimatedEnd { get; set; }

  }

  public class SoundStatusEstimatedEnd
  {
    [JsonPropertyName("estimate")]
    public DateTimeOffset? Estimate { get; set; }

  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum SupportedSounds
  {
    no_sound, alert, bleep, ding_dong_classic, ding_dong_modern, rise, siren, westminster_classic, westminster_modern, ding_dong_xylo, hue_default, sonar, swing, bright, glow, bounce, reveal, welcome, bright_modern, fairy, galaxy, echo
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum MuteValue
  {
    mute, unmute
  }
}
