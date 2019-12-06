using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.Extensions;
using Q42.HueApi.Models.Bridge;

namespace Q42.HueApi
{
  /// <summary>
  /// Try to locate the bridge by performing an IP scan
  /// </summary>
  /// <remarks>https://developers.meethue.com/develop/application-design-guidance/hue-bridge-discovery</remarks>
  public class LocalNetworkScanBridgeLocator : BridgeLocator
  {
    private ConcurrentDictionary<string, LocatedBridge> _discoveredBridges;

    /// <summary>
    /// Locate bridges
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the search</param>
    /// <returns>List of bridge IPs found</returns>
    public override async Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(CancellationToken cancellationToken)
    {
      _discoveredBridges = new ConcurrentDictionary<string, LocatedBridge>();

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

            Parallel.ForEach(networkIps, parallelOptions, (ip) => CheckIP(ip, cancellationToken));
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

      return _discoveredBridges.Select(x => x.Value).ToList();
    }

    /// <summary>
    /// Check if an IP is a Hue Bridge
    /// </summary>
    /// <param name="ip">The IP to check</param>
    /// <param name="cancellationToken">The cancellation token</param>
    private void CheckIP(IPAddress ip, CancellationToken cancellationToken)
    {
      // Add a timeout for the current IP tested so we won't wait too much for a response
      using (var currentIpTimeout = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000)))
      using (var cancellationTokenSrcWithTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, currentIpTimeout.Token))
      {
        // Check if an IP is a Hue Bridge by checking its descriptor
        string serialNumber = CheckHueDescriptor(ip, cancellationTokenSrcWithTimeout.Token).Result;

        if (!string.IsNullOrEmpty(serialNumber))
        {
          _discoveredBridges.TryAdd(ip.ToString(), new LocatedBridge()
          {
            IpAddress = ip.ToString(),
            BridgeId = serialNumber,
          });
        }
        else
        {
          // Not a hue bridge
        }
      }
    }
  }
}
