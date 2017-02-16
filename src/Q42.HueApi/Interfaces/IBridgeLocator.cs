using Q42.HueApi.Models.Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  /// <summary>
  /// Different platforms can make specific implementations of this interface
  /// </summary>
  public interface IBridgeLocator
  {
    /// <summary>
    /// Returns list of bridge IPs
    /// </summary>
    /// <param name="timeout"></param>
    /// <returns></returns>
    Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(TimeSpan timeout);
  }
}
