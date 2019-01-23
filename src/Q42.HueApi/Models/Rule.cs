using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  [DataContract]
  public class Rule
  {
    public string Id { get; set; }

    [DataMember(Name = "Name")]
    public string Name { get; set; }

    [DataMember(Name = "Lasttriggered")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? LastTriggered { get; set; } //Can be "none", so don't convert to DateTime

    [DataMember(Name = "creationtime")]
    [JsonConverter(typeof(NullableDateTimeConverter))]
    public DateTime? CreationTime { get; set; }

    [DataMember(Name = "timestriggered")]
    public int TimesTriggered { get; set; }

    [DataMember(Name = "owner")]
    public string Owner { get; set; }

    [DataMember(Name = "status")]
    public string Status { get; set; }

    [DataMember(Name = "conditions")]
    public List<RuleCondition> Conditions { get; set; }

    [DataMember(Name = "actions")]
    public List<InternalBridgeCommand> Actions { get; set; }
  }

  [DataContract]
  public class RuleCondition
  {
    [DataMember(Name = "address")]
    public string Address { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    [DataMember(Name = "operator")]
    public RuleOperator Operator { get; set; }

    [DataMember(Name = "value")]
    public string Value { get; set; }
  }

  /// <summary>
  /// Possible light alerts
  /// </summary>
  public enum RuleOperator
  {
    /// <summary>
    /// Equal, Used for bool and int.
    /// </summary>
    [EnumMember(Value = "eq")]
    Equal,

    /// <summary>
    /// Time (timestamps) int and bool values. Only dx or ddx is allowed, but not both. Triggers when value of button event is changed or change of presence is detected.ddx is introduced in 1.13
    /// </summary>
    [EnumMember(Value = "dx")]
    Dx,

    /// <summary>
    /// Time (timestamps) int and bool values. Only dx or ddx is allowed, but not both. Triggers when value of button event is changed or change of presence is detected.ddx is introduced in 1.13
    /// </summary>
    [EnumMember(Value = "ddx")]
    Ddx,

    /// <summary>
    /// Time (timestamps) int and bool values.  An attribute has or has not changed for a given time.  Does not trigger a rule change.  Not allowed on /config/utc and /config/localtime. Introduced in 1.13
    /// </summary>
    [EnumMember(Value = "stable")]
    Stable,

    /// <summary>
    /// Time (timestamps) int and bool values.  An attribute has or has not changed for a given time.  Does not trigger a rule change.  Not allowed on /config/utc and /config/localtime. Introduced in 1.13
    /// </summary>
    [EnumMember(Value = "not stable")]
    NotStable,

    /// <summary>
    /// Current time is in or not in given time interval.  "in" rule will be triggered on starttime and "not in" rule will be triggered on endtime.  Only one "in" operator is allowed in a rule.  Multiple "not in" operators are allowed in a rule.  Not allowed to be combined with "not in". Introduced in  1.14
    /// </summary>
    [EnumMember(Value = "in")]
    In,

    /// <summary>
    /// Current time is in or not in given time interval.  "in" rule will be triggered on starttime and "not in" rule will be triggered on endtime.  Only one "in" operator is allowed in a rule.  Multiple "not in" operators are allowed in a rule.  Not allowed to be combined with "not in". Introduced in  1.14
    /// </summary>
    [EnumMember(Value = "not in")]
    NotIn,

    /// <summary>
    /// LessThan, Allowed on int values
    /// </summary>
    [EnumMember(Value = "lt")]
    LessThan,

    /// <summary>
    /// GreaterThan, Allowed on int values
    /// </summary>
    [EnumMember(Value = "gt")]
    GreaterThan
  }
}
