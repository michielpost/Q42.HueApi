using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum EntertainmentConfigurationType
  {
    screen,
    monitor,
    music,
    [EnumMember(Value = "3dspace")] //TODO: Make this work using https://stackoverflow.com/questions/59059989/system-text-json-how-do-i-specify-a-custom-name-for-an-enum-value
    _3dspace,
    other
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum EntertainmentConfigurationStatus
  {
    inactive, active
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum EntertainmentConfigurationStreamProxyMode
  {
    auto, manual
  }

  public class EntertainmentConfiguration : HueResource
  {
    [JsonPropertyName("configuration_type")]
    public EntertainmentConfigurationType ConfigurationType { get; set; }

    [JsonPropertyName("locations")]
    public Locations Locations { get; set; } = new();

    [JsonPropertyName("stream_proxy")]
    public StreamProxy StreamProxy { get; set; } = new();

    [Obsolete("Deprecated: resolve via entertainment services in locations object")]
    [JsonPropertyName("light_services")]
    public List<ResourceIdentifier> LightServices { get; set; } = new();

    [JsonPropertyName("channels")]
    public List<EntertainmentChannel> Channels { get; set; } = new();

    [Obsolete("Deprecated: use metadata/name")]
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("status")]
    public EntertainmentConfigurationStatus Status { get; set; }

    [JsonPropertyName("active_streamer")]
    public ResourceIdentifier? ActiveStreamer { get; set; }


  }

  [DebuggerDisplay("{ChannelId} | {Position}")]
  public class EntertainmentChannel
  {
    [JsonPropertyName("channel_id")]
    public int ChannelId { get; set; }

    [JsonPropertyName("position")]
    public HuePosition Position { get; set; } = new();

    [JsonPropertyName("members")]
    public List<Member> Members { get; set; } = new();
  }

  [DebuggerDisplay("{Index} | {Service}")]
  public class Member
  {
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("service")]
    public ResourceIdentifier? Service { get; set; }
  }

  public class StreamProxy
  {
    [JsonPropertyName("mode")]
    public EntertainmentConfigurationStreamProxyMode Mode { get; set; }

    [JsonPropertyName("node")]
    public ResourceIdentifier? Node { get; set; }
  }

  public class HueServiceLocation
  {
    [JsonPropertyName("positions")]
    public List<HuePosition> Positions { get; set; } = new();

    [JsonPropertyName("service")]
    public ResourceIdentifier? Service { get; set; }

    [Obsolete("Use Positions")]
    [JsonPropertyName("position")]
    public HuePosition? Position { get; set; }

    /// <summary>
    /// Relative equalization factor applied to the entertainment service, to compensate for differences in brightness in the entertainment configuration. Value cannot be 0, writing 0 changes it to lowest possible value.
    /// </summary>
    [JsonPropertyName("equalization_factor")]
    public double? EqualizationFactor { get; set; }
  }

  public class Locations
  {
    [JsonPropertyName("service_locations")]
    public List<HueServiceLocation> ServiceLocations { get; set; } = new();
  }
}
