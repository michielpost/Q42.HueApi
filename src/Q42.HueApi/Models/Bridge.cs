using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  /// <summary>
  /// Status data returned from the bridge
  /// </summary>
  internal class BridgeBridge
  {
    public Dictionary<string, Light> lights { get; set; }
    public BridgeConfig config { get; set; }
  }

  public class Bridge
  {
    internal Bridge(BridgeBridge bridge)
    {
      if (bridge == null)
        throw new ArgumentNullException ("bridge");

      Config = bridge.config;
      foreach (var light in bridge.lights)
        light.Value.Id = light.Key;
      Lights = bridge.lights.Select(l => l.Value).ToList();
    }

    public IEnumerable<Light> Lights { get; private set; }
    public BridgeConfig Config { get; private set; }
  }
}
