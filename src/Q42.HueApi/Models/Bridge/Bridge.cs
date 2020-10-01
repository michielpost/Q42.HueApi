using Q42.HueApi.Models;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
  /// <summary>
  /// Status data returned from the bridge
  /// </summary>
  internal class BridgeState
  {
    public Dictionary<string, Light> Lights { get; set; }
    public Dictionary<string, Group> Groups { get; set; }
    public BridgeConfig? config { get; set; }
    public Dictionary<string, WhiteList> whitelist { get; set; }
  }

  /// <summary>
  /// Hue Bridge
  /// </summary>
  public class Bridge
  {
    internal Bridge(BridgeState bridge)
    {
      if (bridge == null)
        throw new ArgumentNullException(nameof(bridge));

      Config = bridge.config;

      foreach (var light in bridge.Lights)
        light.Value.Id = light.Key;
      Lights = bridge.Lights.Select(l => l.Value).ToList();

      foreach (var group in bridge.Groups)
        group.Value.Id = group.Key;
      Groups = bridge.Groups.Select(l => l.Value).ToList();

      //Fix whitelist IDs
      if (bridge.config != null)
      {
        foreach (var whitelist in bridge.config.WhiteList ?? new Dictionary<string, WhiteList>())
          whitelist.Value.Id = whitelist.Key;

        WhiteList = bridge.config.WhiteList.Select(l => l.Value).ToList();
      }
    }

    /// <summary>
    /// Light info from the bridge
    /// </summary>
    public IEnumerable<Light> Lights { get; private set; }
    public IEnumerable<Group> Groups { get; private set; } = Enumerable.Empty<Group>();

    /// <summary>
    /// Bridge config info
    /// </summary>
    public BridgeConfig? Config { get; private set; }

    /// <summary>
    /// Light info from the bridge
    /// </summary>
    public IEnumerable<WhiteList> WhiteList { get; private set; } = new List<WhiteList>();

    /// <summary>
    /// Is Hue Entertainment API used on a group right now?
    /// </summary>
    public bool IsStreamingActive
    {
      get
      {
        return Groups.Any(x => x.Stream?.Active ?? false);
      }
    }

    /// <summary>
    /// Overrides ToString() to give something more useful than object name.
    /// </summary>
    /// <returns>A string like "Bridge 021788FFFE6E28D4"</returns>
    public override string ToString()
    {
      return String.Format("Bridge {0}", this.Config?.BridgeId);
    }
  }
}
