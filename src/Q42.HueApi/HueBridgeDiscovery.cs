using Q42.HueApi.Models.Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.Extensions;

namespace Q42.HueApi
{
  /// <summary>
  /// Some Hue Bridge Discovery approaches using the Q42.HueApi
  /// See https://developers.meethue.com/develop/application-design-guidance/hue-bridge-discovery/
  /// Based on code by @Indigo744 https://github.com/Q42/Q42.HueApi/issues/198#issuecomment-564011445
  /// </summary>
  public static class HueBridgeDiscovery
  {
    /// <summary>
    /// Discovery of Hue Bridges:
    /// - Run all locators (N-UPNP, MDNS, SSDP, network scan) in parallel
    /// - Check first with N-UPNP, then MDNS/SSDP
    /// - If nothing found, continue with network scan
    /// </summary>
    /// <remarks>General purpose approach for comprehensive search</remarks>
    /// <remarks>Max awaited time if nothing found is maxNetwScanTimeout</remarks>
    /// <param name="fastTimeout">Timeout for the fast locators (at least a few seconds, usually around 5 seconds)</param>
    /// <param name="maxNetwScanTimeout">Timeout for the slow network scan (at least a 30 seconds, to few minutes)</param>
    /// <returns>List of bridges found</returns>
    public async static Task<List<LocatedBridge>> CompleteDiscoveryAsync(TimeSpan fastTimeout, TimeSpan maxNetwScanTimeout)
    {
      using (var fastLocatorsCancelSrc = new CancellationTokenSource(fastTimeout))
      using (var slowNetwScanCancelSrc = new CancellationTokenSource(maxNetwScanTimeout))
      {
        // Start all tasks in parallel without awaiting

        // Pack all fast locators in an array so we can await them in order
        var fastLocateTask = new Task<IEnumerable<LocatedBridge>>[] {
                // N-UPNP is the fastest (only one endpoint to check)
                (new HttpBridgeLocator()).LocateBridgesAsync(fastLocatorsCancelSrc.Token),
                // MDNS is the most reliable for bridge V2
                (new MdnsBridgeLocator()).LocateBridgesAsync(fastLocatorsCancelSrc.Token),
                // SSDP is older but works for bridge V1 & V2
                (new SsdpBridgeLocator()).LocateBridgesAsync(fastLocatorsCancelSrc.Token),
            };

        // The network scan locator is clearly the slowest
        var slowNetwScanTask = (new LocalNetworkScanBridgeLocator()).LocateBridgesAsync(slowNetwScanCancelSrc.Token);

        IEnumerable<LocatedBridge> result;

        // We will loop through the fast locators and await each one in order
        foreach (var fastTask in fastLocateTask)
        {
          // Await this task to get its result
          result = await fastTask;

          // Check if it contains anything
          if (result.Any())
          {
            // Cancel all remaining tasks and return
            fastLocatorsCancelSrc.CancelWithBackgroundContinuations();
            slowNetwScanCancelSrc.CancelWithBackgroundContinuations();

            return result.ToList();
          }
          else
          {
            // Nothing found using this locator
          }
        }

        // All fast locators failed, we wait for the network scan to complete and return whatever we found
        result = await slowNetwScanTask;
        return result.ToList();
      }
    }

    /// <summary>
    /// Discovery of Hue Bridges:
    /// - Run all fast locators (N-UPNP, MDNS, SSDP) in parallel
    /// - Check first with N-UPNP, then MDNS/SSDP after 5 seconds
    /// - If nothing found, run network scan up to 1 minute
    /// </summary>
    /// <remarks>Better approach for comprehensive search for smartphone environment</remarks>
    /// <remarks>Max awaited time if nothing found is fastTimeout+maxNetwScanTimeout</remarks>
    /// <param name="fastTimeout">Timeout for the fast locators (at least a few seconds, usually around 5 seconds)</param>
    /// <param name="maxNetwScanTimeout">Timeout for the slow network scan (at least a 30 seconds, to few minutes)</param>
    /// <returns>List of bridges found</returns>
    public async static Task<List<LocatedBridge>> FastDiscoveryWithNetworkScanFallbackAsync(TimeSpan fastTimeout, TimeSpan maxNetwScanTimeout)
    {
      using (var fastLocatorsCancelSrc = new CancellationTokenSource(fastTimeout))
      {
        // Start all tasks in parallel without awaiting

        // Pack all fast locators in an array so we can await them in order
        var fastLocateTask = new Task<IEnumerable<LocatedBridge>>[] {
                // N-UPNP is the fastest (only one endpoint to check)
                (new HttpBridgeLocator()).LocateBridgesAsync(fastLocatorsCancelSrc.Token),
                // MDNS is the most reliable for bridge V2
                (new MdnsBridgeLocator()).LocateBridgesAsync(fastLocatorsCancelSrc.Token),
                // SSDP is older but works for bridge V1 & V2
                (new SsdpBridgeLocator()).LocateBridgesAsync(fastLocatorsCancelSrc.Token),
            };

        IEnumerable<LocatedBridge> result;

        // We will loop through the fast locators and await each one in order
        foreach (var fastTask in fastLocateTask)
        {
          // Await this task to get its result
          result = await fastTask;

          // Check if it contains anything
          if (result.Any())
          {
            // Cancel all remaining tasks and return
            fastLocatorsCancelSrc.CancelWithBackgroundContinuations();

            return result.ToList();
          }
          else
          {
            // Nothing found using this locator
          }
        }

        // All fast locators failed, let's try the network scan and return whatever we found
        result = await (new LocalNetworkScanBridgeLocator()).LocateBridgesAsync(maxNetwScanTimeout);
        return result.ToList();
      }
    }

    /// <summary>
    /// Discovery of Hue Bridges:
    /// - Run only the fast locators (N-UPNP, MDNS, SSDP) in parallel
    /// - Check first with N-UPNP, then MDNS/SSDP
    /// </summary>
    /// <remarks>Best approach if network scan is not desirable</remarks>
    /// <param name="timeout">Timeout for the search (at least a few seconds, usually around 5 seconds)</param>
    /// <returns>List of bridges found</returns>
    public async static Task<List<LocatedBridge>> FastDiscoveryAsync(TimeSpan timeout)
    {
      using (var fastLocatorsCancelSrc = new CancellationTokenSource(timeout))
      {
        // Start all tasks in parallel without awaiting

        // Pack all fast locators in an array so we can await them in order
        var fastLocateTask = new Task<IEnumerable<LocatedBridge>>[] {
                // N-UPNP is the fastest (only one endpoint to check)
                (new HttpBridgeLocator()).LocateBridgesAsync(fastLocatorsCancelSrc.Token),
                // MDNS is the most reliable for bridge V2
                (new MdnsBridgeLocator()).LocateBridgesAsync(fastLocatorsCancelSrc.Token),
                // SSDP is older but works for bridge V1 & V2
                (new SsdpBridgeLocator()).LocateBridgesAsync(fastLocatorsCancelSrc.Token),
            };

        IEnumerable<LocatedBridge> result = new List<LocatedBridge>();

        // We will loop through the fast locators and await each one in order
        foreach (var fastTask in fastLocateTask)
        {
          // Await this task to get its result
          result = await fastTask;

          // Check if it contains anything
          if (result.Any())
          {
            // Cancel all remaining tasks and break
            fastLocatorsCancelSrc.CancelWithBackgroundContinuations();

            break;
          }
          else
          {
            // Nothing found using this locator
          }
        }

        return result.ToList();
      }
    }

  }
}
