using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;

namespace Q42.HueApi
{
  /// <summary>
  /// Uses the special nupnp url from meethue.com to find registered bridges based on your external IP
  /// </summary>
  public abstract class BridgeLocator : IBridgeLocator
  {
    /// <summary>
    /// Locate bridges
    /// </summary>
    /// <param name="timeout">Timeout before stopping the search</param>
    /// <returns>List of bridge IPs found</returns>
    public async Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(TimeSpan timeout)
    {
      if (timeout <= TimeSpan.Zero)
      {
        throw new ArgumentException("Timeout value must be greater than zero.", nameof(timeout));
      }

      using (CancellationTokenSource cancelSource = new CancellationTokenSource(timeout))
      {
        return await LocateBridgesAsync(cancelSource.Token);
      }
    }

    /// <summary>
    /// Locate bridges
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the search</param>
    /// <returns>List of bridge IPs found</returns>
    public abstract Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(CancellationToken cancellationToken);
  }
}
