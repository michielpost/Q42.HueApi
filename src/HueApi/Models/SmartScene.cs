using HueApi.Models.Requests.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{
  public class SmartScene : HueResource
  {
    [JsonPropertyName("group")]
    public ResourceIdentifier? Group { get; set; }

    [JsonPropertyName("week_timeslots")]
    public List<SmartSceneDayTimeslot> WeekTimeslots { get; set; } = default!;

    [JsonPropertyName("active_timeslot")]
    public ActiveTimeslot ActiveTimeslot { get; set; } = default!;

    [JsonPropertyName("state")]
    public SmartSceneState State { get; set; }
  }

  public class SmartSceneDayTimeslot
  {
    [JsonPropertyName("timeslots")]
    public List<SmartSceneTimeslot> Timeslots { get; set; } = default!;

    [JsonPropertyName("recurrence")]
    public List<Weekday> Recurrence { get; set; } = default!;
  }

  public class SmartSceneTimeslot
  {
    [JsonPropertyName("start_time")]
    public TimeslotStartTime StartTime { get; set; } = default!;

    [JsonPropertyName("target")]
    public ResourceIdentifier Target { get; set; } = default!;

    
  }

  public class TimeslotStartTime
  {
    [JsonPropertyName("kind")]
    public string Kind { get; set; } = "time";

    [JsonPropertyName("time")]
    public TimeslotStartTimeTime Time { get; set; } = default!;
  }

  public class TimeslotStartTimeTime
  {
    [JsonPropertyName("hour")]
    public int Hour { get; set; } = default!;

    [JsonPropertyName("minute")]
    public int Minute { get; set; } = default!;

    [JsonPropertyName("second")]
    public int Second { get; set; } = default!;
  }



  public class ActiveTimeslot
  {
    [JsonPropertyName("timeslot_id")]
    public int TimeslotId { get; set; }

    [JsonPropertyName("weekday")]
    public Weekday Weekday { get; set; }
  }


  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum Weekday
  {
    monday, tuesday, wednesday, thursday, friday, saturday, sunday
  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum SmartSceneState
  {
    inactive, active
  }
}
