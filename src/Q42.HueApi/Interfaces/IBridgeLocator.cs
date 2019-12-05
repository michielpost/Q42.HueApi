using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.Models.Bridge;

namespace Q42.HueApi.Interfaces
{
  /// <summary>
  /// Different platforms can make specific implementations of this interface
  /// </summary>
  public interface IBridgeLocator
  {
    /// <summary>
    /// Locate bridges
    /// </summary>
    /// <param name="timeout">Timeout before stopping the search</param>
    /// <returns>List of bridge IPs found</returns>
    Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(TimeSpan timeout);

    /// <summary>
    /// Locate bridges
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the search</param>
    /// <returns>List of bridge IPs found</returns>
    Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(CancellationToken cancellationToken);
  }
}
