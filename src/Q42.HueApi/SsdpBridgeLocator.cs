using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.Extensions;
using Q42.HueApi.Models.Bridge;

namespace Q42.HueApi
{
  /// <summary>
  /// Uses SSDP protocol to locate all Hue Bridge accross the network
  /// </summary>
  /// <remarks>https://developers.meethue.com/develop/application-design-guidance/hue-bridge-discovery</remarks>
  public class SsdpBridgeLocator : BridgeLocator
  {
    private readonly IPAddress multicastAddress = IPAddress.Parse("239.255.255.250");
    private const int multicastPort = 1900;

    private const string messageHeader = "M-SEARCH * HTTP/1.1";
    private const string messageHost = "HOST: 239.255.255.250:1900";
    private const string messageMan = "MAN: \"ssdp:discover\"";
    private const string messageMx = "MX: 1";
    private const string messageSt = "ST: SsdpSearch:all";

    private readonly Regex ssdpResponseLocationRegex = new Regex(@"LOCATION: *(http.+?/description\.xml)\r", RegexOptions.IgnoreCase);
    private static readonly Regex xmlResponseCheckHueRegex = new Regex(@"Philips hue bridge", RegexOptions.IgnoreCase);
    private static readonly Regex xmlResponseSerialNumberRegex = new Regex(@"<serialnumber>(.+?)</serialnumber>", RegexOptions.IgnoreCase);

    private readonly byte[] ssdpDiscoveryMessage = Encoding.UTF8.GetBytes(
        string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{0}",
                      "\r\n",
                      messageHeader,
                      messageHost,
                      messageMan,
                      messageMx,
                      messageSt));

    private ConcurrentDictionary<string, LocatedBridge> _discoveredBridges;

    /// <summary>
    /// Locate bridges
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the search</param>
    /// <returns>List of bridge IPs found</returns>
    public override async Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(CancellationToken cancellationToken)
    {
      _discoveredBridges = new ConcurrentDictionary<string, LocatedBridge>();

      List<Socket> socketList = new List<Socket>();

      try
      {
#if !NET45
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
#endif
          // On Windows, using INADDR_ANY could lead to issue due to the Windows SSDP Discovery service throttling multicast packets
          // See https://stackoverflow.com/q/32682969/8861729
          // So we will bind directly to all network interface having a private IPs
          socketList = NetworkInterface.GetAllNetworkInterfaces()
            // Keep only connected interfaces
            .Where(itf => itf.OperationalStatus == OperationalStatus.Up)
            // Retrieve first unicast address on each interface
            .Select(itf => itf.GetIPProperties().UnicastAddresses.First())
            // Keep only private IPv4
            .Where(info => !info.Address.IsLoopback() && !info.Address.IsIPv4LinkLocal() && info.Address.IsIPv4Private())
            // Create socket for each address remaining (and asking for a random available port)
            .Select(info => CreateSocketForSSDP(new IPEndPoint(info.Address, 0)))
            .ToList();
#if !NET45
        }
        else
        {
          // On other plateform, simply use INADDR_ANY (and asking for a random available port)
          socketList.Add(CreateSocketForSSDP(new IPEndPoint(IPAddress.Any, 0)));
        }
#endif

        foreach (Socket socket in socketList)
        {
          // Send SSDP discovery message
          socket.SendTo(ssdpDiscoveryMessage, SocketFlags.None, new IPEndPoint(multicastAddress, multicastPort));
          // Spin up a new thread to listen to this socket
          new Thread(() => ListenSocket(socket)).Start();
        }

        try
        {
          // Wait until cancellation requested
          await Task.Delay(Timeout.Infinite, cancellationToken);
        }
        catch (TaskCanceledException)
        {
          // Cancellation requested
        }
      }
      finally
      {
        // Close socket (which will end thread) then dispose unmanaged resource
        foreach (Socket socket in socketList)
        {
          socket.Close();
          socket.Dispose();
        }
      }

      return _discoveredBridges.Select(x => x.Value).ToList();
    }

    /// <summary>
    /// Create a multicast socket for SSDP
    /// </summary>
    /// <param name="endpointToBind">the endpoint to bind to</param>
    private Socket CreateSocketForSSDP(IPEndPoint endpointToBind)
    {
      Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

      // Socket config : allow address reuse
      socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

      // Socket config : bind to all interface and to a port (if 0, ask for a free one)
      socket.Bind(endpointToBind);

      // Socket config : Add Multicast address membership to be able to send to it
      socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastAddress, IPAddress.Any));

      return socket;
    }

    /// <summary>
    /// Listen to response on socket
    /// </summary>
    /// <param name="socket">The socket to listen to</param>
    private void ListenSocket(Socket socket)
    {
      try
      {
        var ipSeen = new List<string>();
        while (true)
        {
          var responseRawBuffer = new byte[8000];
          EndPoint responseEndPoint = new IPEndPoint(IPAddress.Any, multicastPort);
          socket.ReceiveFrom(responseRawBuffer, ref responseEndPoint);

          // We received a response
          try
          {
            // Get IP address
            string responseIpAddress = (responseEndPoint as IPEndPoint)?.Address.ToString() ?? "";

            if (!ipSeen.Contains(responseIpAddress))
            {
              // Try decode response as UTF8
              var responseBody = Encoding.UTF8.GetString(responseRawBuffer);

              if (!string.IsNullOrWhiteSpace(responseIpAddress) && !string.IsNullOrWhiteSpace(responseBody))
              {
                // Spin up a new thread to handle this specific response so we can continue waiting for response
                new Thread(async () => await HandleSSDPResponseAsync(responseIpAddress, responseBody).ConfigureAwait(false)).Start();

                // Add this ip to local list, so we won't start other thread for it
                ipSeen.Add(responseIpAddress);
              }
              else
              {
                // Not a valid IP or response
              }
            }
            else
            {
              // IP already seen
            }
          }
          catch
          {
            // Something went wrong when parsing the response. Ignore it.
          }
        }
      }
      catch
      {
        // Socket connection closed, this will terminate the thread
      }
    }

    /// <summary>
    /// Handle a SSDP response
    /// </summary>
    /// <param name="ipAddress">The IP that responded</param>
    /// <param name="response">The response received</param>
    private async Task HandleSSDPResponseAsync(string ipAddress, string response)
    {
      try
      {
        var location = ssdpResponseLocationRegex.Match(response);
        if (location.Success)
        {
          // Check if it's a Hue Bridge
          string serialNumber = await IsHue(location.Groups[1].Value).ConfigureAwait(false);

          if (!string.IsNullOrWhiteSpace(serialNumber))
          {
            _discoveredBridges.TryAdd(ipAddress, new LocatedBridge()
            {
              IpAddress = ipAddress,
              BridgeId = serialNumber,
            });
          }
          else
          {
            // Not a valid S/N
          }
        }
        else
        {
          // Not a valid location
        }
      }
      catch
      {
        // Something went wrong, ignore...
      }
    }

    /// <summary>
    /// Check if the endpoint is a Hue Bridge
    /// </summary>
    /// <param name="discoveryUrl">Endpoint URL</param>
    /// <returns>The Serial Number, or empty if not a hue bridge</returns>
    private async Task<string> IsHue(string discoveryUrl)
    {
      try
      {
        string xmlResponse = "";
        using (var client = new HttpClient { Timeout = TimeSpan.FromMilliseconds(2000) })
        {
          xmlResponse = await client.GetStringAsync(discoveryUrl).ConfigureAwait(false);
        }

        if (xmlResponseCheckHueRegex.IsMatch(xmlResponse))
        {
          var serialNumberMatch = xmlResponseSerialNumberRegex.Match(xmlResponse);

          if (serialNumberMatch.Success)
          {
            return serialNumberMatch.Groups[1].Value;
          }
          else
          {
            // No S/N found
          }
        }
        else
        {
          // Not a Hue Bridge
        }
      }
      catch
      {
        // Something went wrong, ignore...
      }

      return "";
    }
  }
}
