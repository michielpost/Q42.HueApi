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
  internal class BridgeBridge
  {
    public Dictionary<string, Light> lights { get; set; }
    public BridgeConfig config { get; set; }
    public Dictionary<string, WhiteList> whitelist { get; set; }
  }

  /// <summary>
  /// Hue Bridge
  /// </summary>
  public class Bridge
  {
    internal Bridge(BridgeBridge bridge)
    {
      if (bridge == null)
        throw new ArgumentNullException("bridge");

      Config = bridge.config;

      foreach (var light in bridge.lights)
        light.Value.Id = light.Key;
      Lights = bridge.lights.Select(l => l.Value).ToList();

      foreach (var whitelist in bridge.config.WhiteList)
        whitelist.Value.Id = whitelist.Key;
      WhiteList = bridge.config.WhiteList.Select(l => l.Value).ToList();
    }

    /// <summary>
    /// Light info from the bridge
    /// </summary>
    public IEnumerable<Light> Lights { get; private set; }

    /// <summary>
    /// Bridge config info
    /// </summary>
    public BridgeConfig Config { get; private set; }

    /// <summary>
    /// Light info from the bridge
    /// </summary>
    public IEnumerable<WhiteList> WhiteList { get; private set; }

  }
}
