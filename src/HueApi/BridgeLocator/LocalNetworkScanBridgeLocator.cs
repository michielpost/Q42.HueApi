using HueApi.Extensions;
using System.Collections.Concurrent;
using System.Net;

namespace HueApi.BridgeLocator
{
  /// <summary>
  /// Try to locate the bridge by performing an IP scan
  /// </summary>
  /// <remarks>https://developers.meethue.com/develop/application-design-guidance/hue-bridge-discovery</remarks>
  public class LocalNetworkScanBridgeLocator : BridgeLocator
  {
    /// <summary>
    /// Locate bridges
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the search</param>
    /// <returns>List of bridge IPs found</returns>
    public override async Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(CancellationToken cancellationToken)
    {
      var discoveredBridges = new ConcurrentDictionary<string, LocatedBridge>();

      // Get all IPv4 private unicast addresses from all network interfaces that are up
      List<IPAddress> networkIps = NetworkInterfaceExtensions.GetAllUpNetworkInterfacesFirstPrivateIPv4()
        // For each interface, get all IP from this network
        .SelectMany(info => info.Address.GetAllIPv4FromNetwork(info.IPv4Mask))
        // Eventually filter common ones
        .Distinct()
        .ToList();

      if (networkIps.Count > 0)
      {
        // Run in another thread so we won't block the UI
        await Task.Run(() =>
        {
          try
          {
            // We'll use the Parallel.ForEach methods so the work will be distributed on all core
            ParallelOptions parallelOptions = new ParallelOptions
            {
              CancellationToken = cancellationToken,
              MaxDegreeOfParallelism = Environment.ProcessorCount,
            };

            Parallel.ForEach(networkIps, parallelOptions, async (ip) =>
            {
              // Check if an IP is a Hue Bridge by checking its descriptor
              // Note that the timeout here is important:
              // - if small, can speedup significantly the searching, but may miss an answer if the Hue Bridge took too much time to answer
              // - if big, will be sure to check thoroughly each IP, but the search can be slower
              //var config = GetBridgeConfigAsync(ip, TimeSpan.FromMilliseconds(1000), cancellationToken).Result;
              //string? serialNumber = config?.BridgeId;

              string serialNumber = CheckHueDescriptor(ip, TimeSpan.FromMilliseconds(1000), cancellationToken).Result;

              if (!string.IsNullOrEmpty(serialNumber))
              {
                var locatedBridge = new LocatedBridge(serialNumber!, ip.ToString(), null);

                if (discoveredBridges.TryAdd(ip.ToString(), locatedBridge))
                {
                  OnBridgeFound(locatedBridge);
                }
              }
              else
              {
                // Not a hue bridge
              }
            });
          }
          catch (OperationCanceledException)
          {
            // Cancellation requested
          }
        });
      }
      else
      {
        // No IP found
      }

      return discoveredBridges.Select(x => x.Value).ToList();
    }
  }
}
