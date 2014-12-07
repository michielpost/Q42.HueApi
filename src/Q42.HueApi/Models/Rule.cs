using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    public string LastTriggered { get; set; } //Can be "none", so don't convert to DateTime

    [DataMember(Name = "creationtime")]
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
    /// OnChange, Used for Time (timestamps) int and bool values..
    /// </summary>
    [EnumMember(Value = "dx")]
    OnChange,

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
