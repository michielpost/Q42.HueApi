using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Converters;
using System;
using System.Runtime.Serialization;

namespace Q42.HueApi.Models
{
  public class Schedule
  {
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("command")]
    public InternalBridgeCommand Command { get; set; }

    [JsonProperty("localtime")]
    [JsonConverter(typeof(HueDateTimeConverter))]
    public HueDateTime LocalTime { get; set; }
    
    [JsonProperty("created")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? Created { get; set; }

    /// <summary>
    /// UTC time that the timer was started. Only provided for timers.
    /// </summary>
    [JsonProperty("starttime")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// "enabled"  Schedule is enabled
    /// "disabled"  Schedule is disabled by user.
    /// Application is only allowed to set “enabled” or “disabled”. Disabled causes a timer to reset when activated (i.e. stop & reset). “enabled” when not provided on creation.
    /// </summary>
    [JsonProperty("status")]
    [JsonConverter(typeof(StringEnumConverter))]
    public ScheduleStatus? Status { get; set; }

    /// <summary>
    /// If set to true, the schedule will be removed automatically if expired, if set to false it will be disabled. Default is true
    /// </summary>
    [JsonProperty("autodelete")]
    public bool? AutoDelete { get; set; }

  }

  [JsonConverter(typeof(StringEnumConverter))]
  public enum ScheduleStatus
  {
    [EnumMember(Value = "enabled")]
    Enabled,
    [EnumMember(Value = "disabled")]
    Disabled,
  }
}
