using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.Models.Bridge;

namespace Q42.HueApi
{
  /// <summary>
  /// Uses SSDP protocol to locate all Hue Bridge accross the network
  /// </summary>
  /// <remarks>https://developers.meethue.com/develop/application-design-guidance/hue-bridge-discovery</remarks>
  public class SsdpBridgeLocator : MUdpBasedBridgeLocator
  {
    /// <summary>
    /// Multicast group for SSDP Protocol
    /// </summary>
    private readonly IPAddress ssdpMulticastAddress = IPAddress.Parse("239.255.255.250");

    /// <summary>
    /// Multicast port for sending SSDP discovery message
    /// </summary>
    private const int ssdpMulticastPort = 1900;

    /// <summary>
    /// Local port to use to listen to response (0 = use any free port available)
    /// </summary>
    private const int ssdpLocalPort = 0;

    /// <summary>
    /// SSDP discovery message to send
    /// </summary>
    private readonly byte[] ssdpDiscoveryMessage = Encoding.UTF8.GetBytes(
        string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{0}",
                      "\r\n",
                      "M-SEARCH * HTTP/1.1",
                      $"HOST: 239.255.255.250:1900",
                      "MAN: \"ssdp:discover\"",
                      "MX: 1",
                      "ST: SsdpSearch:all"));

    /// <summary>
    /// Locate bridges
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the search</param>
    /// <returns>List of bridge IPs found</returns>
    public override async Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(CancellationToken cancellationToken)
    {
      return await LocateBridgesAsync(ssdpMulticastAddress, ssdpMulticastPort, ssdpLocalPort, ssdpDiscoveryMessage, cancellationToken);
    }
  }
}
