using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
  /// <summary>
  /// Lamp object returned from bridge
  /// </summary>
  public class BridgeLamp
  {
    /// <summary>
    /// Lamp name
    /// </summary>
    public string Name { get; set; }
  }

  /// <summary>
  /// Local lamp object
  /// </summary>
  public class Lamp
  {
    /// <summary>
    /// LampId
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// ToString
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return this.Id + ": " + this.Name;
    }
  }
}
