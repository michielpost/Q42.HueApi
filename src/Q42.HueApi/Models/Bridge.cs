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
  public class BridgeBridge
  {
    public Dictionary<string, Light> lights { get; set; }
    public Config config { get; set; }
  }

  public class Bridge
  {
    public Bridge(BridgeBridge bridge)
    {
      config = bridge.config;
      foreach (var light in bridge.lights)
        light.Value.id = light.Key;
      lights = bridge.lights.Select(l => l.Value).ToList();
    }

    public List<Light> lights { get; set; }
    public Config config { get; set; }
  }

  public class Light
  {
    public string id { get; set; }
    public State state { get; set; }
    public string type { get; set; }
    public string name { get; set; }
    public string modelid { get; set; }
    public string swversion { get; set; }
  }

  public class Config
  {
    public string name { get; set; }
    public string mac { get; set; }
    public bool dhcp { get; set; }
    public string ipaddress { get; set; }
    public string netmask { get; set; }
    public string gateway { get; set; }
    public string proxyaddress { get; set; }
    public int proxyport { get; set; }
    public string UTC { get; set; }
    public string swversion { get; set; }
    public Swupdate swupdate { get; set; }
    public bool linkbutton { get; set; }
    public bool portalservices { get; set; }
  }

  public class State
  {
    public bool on { get; set; }
    public int bri { get; set; }
    public int hue { get; set; }
    public int sat { get; set; }
    public List<double> xy { get; set; }
    public int ct { get; set; }
    public string alert { get; set; }
    public string effect { get; set; }
    public string colormode { get; set; }
    public bool reachable { get; set; }

    public string ToHex()
    {
      return ColorService.HexFromState(this);
    }
  }

  public class Swupdate
  {
    public int updatestate { get; set; }
    public string url { get; set; }
    public string text { get; set; }
    public bool notify { get; set; }
  }

}
