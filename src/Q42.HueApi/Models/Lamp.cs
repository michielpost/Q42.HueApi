using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  /// <summary>
  /// Lamp object returned from bridge
  /// </summary>
  public class BridgeLamp
  {
    public string Name { get; set; }
  }

  /// <summary>
  /// Local lamp object
  /// </summary>
  public class Lamp
  {
    public string Id { get; set; }
    public string Name { get; set; }
    public string ColorHex { get; set; }

    public override string ToString()
    {
      return this.Id + ": " + this.Name;
    }
  }
}
