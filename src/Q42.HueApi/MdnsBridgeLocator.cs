using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.Models.Bridge;

namespace Q42.HueApi
{
  /// <summary>
  /// Uses M-DNS protocol to locate all Hue Bridge accross the network
  /// </summary>
  /// <remarks>https://developers.meethue.com/develop/application-design-guidance/hue-bridge-discovery</remarks>
  public class MdnsBridgeLocator : MUdpBasedBridgeLocator
  {
    /// <summary>
    /// Multicast group for MDNS Protocol
    /// </summary>
    private readonly IPAddress ssdpMulticastAddress = IPAddress.Parse("224.0.0.251");

    /// <summary>
    /// Multicast port for sending MDNS discovery message
    /// </summary>
    private const int mdnsMulticastPort = 5353;

    /// <summary>
    /// Local port to use to listen to response (same as sending)
    /// </summary>
    private const int mdnsLocalPort = mdnsMulticastPort;

    /// <summary>
    /// MDNS discovery message to send
    /// </summary>
    private readonly byte[] mdnsDiscoveryMessage = BuildMDNSMessage(
      "_hue._tcp.local",  // Standard official service name
      "_hap._tcp.local",  // Old service name
      "Philips-hue.local" // Old service name
    );

    /// <summary>
    /// Locate bridges
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the search</param>
    /// <returns>List of bridge IPs found</returns>
    public override async Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(CancellationToken cancellationToken)
    {
      return await LocateBridgesAsync(ssdpMulticastAddress, mdnsMulticastPort, mdnsLocalPort, mdnsDiscoveryMessage, cancellationToken);
    }

    /// <summary>
    /// Build a MDNS message with a list of queries (= services) to search for
    /// </summary>
    /// <remarks>MDNS specs: https://tools.ietf.org/html/rfc6762 </remarks>
    /// <remarks>DNS specs: https://tools.ietf.org/html/rfc1035 </remarks>
    /// <param name="queries">List of query to search for</param>
    /// <returns>The raw message content (bytes)</returns>
    private static byte[] BuildMDNSMessage(params string[] queries)
    {
      var bytes = new List<byte>();

      // Build M-DNS Header
      bytes.AddRange(new byte[] { 0x00, 0x00 });  // Transaction ID = None
      bytes.AddRange(new byte[] { 0x01, 0x00 });  // Standard query with Recursion
      bytes.AddRange(new byte[] { 0x00, (byte)queries.Length });  // Number of Queries
      bytes.AddRange(new byte[] { 0x00, 0x00 });  // Answer Resource Records = None
      bytes.AddRange(new byte[] { 0x00, 0x00 });  // Authority Resource Records = None
      bytes.AddRange(new byte[] { 0x00, 0x00 });  // Additional Resource Records = None

      // Build M-DNS Queries
      foreach (var query in queries)
      {
        // Each part of the query FQDN is preceded with a byte specifying the length
        // The dot is not actually written
        bytes.AddRange(query.Split('.')
          .Select(part => Encoding.UTF8.GetBytes(part).ToList())
          .SelectMany(partBytes =>
          {
            // Insert the length in front
            partBytes.Insert(0, (byte)partBytes.Count);
            return partBytes;
          }));

        // Add NULL terminator at the end
        bytes.Add(0x00);

        // Add query configuration
        bytes.AddRange(new byte[] { 0x00, 0xFF }); // QTYPE = ANY
        bytes.AddRange(new byte[] { 0x80, 0xFF }); // UNICAST-RESPONSE + QCLASS = ANY
      }

      return bytes.ToArray();
    }
  }
}
