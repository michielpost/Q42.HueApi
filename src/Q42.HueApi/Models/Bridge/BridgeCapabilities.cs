using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  public class BridgeCapabilities
  {
    public LightsCapability Lights { get; set; }
    public SensorsCapability Sensors { get; set; }
    public GroupsCapability Groups { get; set; }
    public ScenesCapability Scenes { get; set; }
    public SchedulesCapability Schedules { get; set; }
    public RulesCapability Rules { get; set; }
    public ResourceLinksCapability Resourcelinks { get; set; }
    public Timezones Timezones { get; set; }

  }

  public class LightsCapability : Capability
  {
  }

  public class ClipCapability : Capability
  {
  }

  public class ZllCapability : Capability
  {
  }

  public class ZgpCapability : Capability
  {
  }

  public class SensorsCapability : Capability
  {
    /// <summary>
    /// Capability information of resources which are directly created by POST
    /// </summary>
    public ClipCapability Clip { get; set; }

    /// <summary>
    /// Capability information of Zigbee resources which are discovered by POST
    /// </summary>
    public ZllCapability Zll { get; set; }

    /// <summary>
    /// Capability information of ZGP resources which are discovered by POST
    /// </summary>
    public ZgpCapability Zgp { get; set; }
  }

  public class GroupsCapability : Capability
  {
  }

  public class LightstatesCapability : Capability
  {
  }

  public class ScenesCapability : Capability
  {
    /// <summary>
    /// Represents the total pool of individual lightsstates (scene setting per lamp) which can be used across all scenes in /scenes/lightstates
    /// </summary>
    public LightstatesCapability Lightstates { get; set; }
  }

  public class SchedulesCapability : Capability
  {
  }

  public class ConditionsCapability : Capability
  {
  }

  public class ActionsCapability : Capability
  {
  }

  public class RulesCapability : Capability
  {
    public ConditionsCapability Conditions { get; set; }

    /// <summary>
    /// Represents the total pool of individual actions which can be used across all rules in /rules/actions
    /// </summary>
    public ActionsCapability Actions { get; set; }
  }

  public class ResourceLinksCapability : Capability
  {

  }

  public class Timezones
  {
    /// <summary>
    /// List of supported time zones represented as tz database strings. Each value can be set in /config/timezone. Other values are not supported.
    /// </summary>
    public List<string> Values { get; set; }
  }

  public abstract class Capability
  {
    /// <summary>
    /// Total (maximum) number of resources which still can be created by POST on this resource path. The number of creatable resources for a specific subresource type might be lower.
    /// </summary>
    public int Available { get; set; }

    /// <summary>
    /// Total number of resources which can be read on this resource path. Apply to all below.
    /// </summary>
    public int Total { get; set; }
  }

}
