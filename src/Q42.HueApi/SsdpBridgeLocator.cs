using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;

namespace Q42.HueApi
{
  /// <summary>
  /// Uses SSDP protocol to locate all Hue Bridge accross the network
  /// </summary>
  /// <remarks>https://developers.meethue.com/develop/application-design-guidance/hue-bridge-discovery</remarks>
  public class SsdpBridgeLocator : IBridgeLocator
  {
    private readonly IPAddress multicastAddress = IPAddress.Parse("239.255.255.250");
    private const int multicastPort = 1900;
    private const int unicastPort = 1901;

    private const string messageHeader = "M-SEARCH * HTTP/1.1";
    private const string messageHost = "HOST: 239.255.255.250:1900";
    private const string messageMan = "MAN: \"ssdp:discover\"";
    private const string messageMx = "MX: 8";
    private const string messageSt = "ST:SsdpSearch:all";

    private readonly Regex ssdpResponseLocationRegex = new Regex(@"LOCATION: *(http.+?/description\.xml)\r", RegexOptions.IgnoreCase);
    private static readonly Regex xmlResponseCheckHueRegex = new Regex(@"Philips hue bridge", RegexOptions.IgnoreCase);
    private static readonly Regex xmlResponseSerialNumberRegex = new Regex(@"<serialnumber>(.+?)</serialnumber>", RegexOptions.IgnoreCase);

    private readonly byte[] broadcastMessage = Encoding.UTF8.GetBytes(
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
    /// <para>Note that it will take at least the amount of time specified in the timeout parameter</para>
    /// </summary>
    /// <param name="timeout">Timeout before stopping the search (best practice is waiting at least 5 seconds)</param>
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
    public async Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(CancellationToken cancellationToken)
    {
      _discoveredBridges = new ConcurrentDictionary<string, LocatedBridge>();

      using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
      {
        // Configure socket
        socket.Bind(new IPEndPoint(IPAddress.Any, unicastPort));
        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastAddress, IPAddress.Any));
        socket.SendTo(broadcastMessage, 0, broadcastMessage.Length, SocketFlags.None, new IPEndPoint(multicastAddress, multicastPort));

        // Spin up a new thread to handle socket responses
        new Thread(() => ListenSocketResponseAsync(socket)).Start();

        try
        {
          // Wait until cancellation requested
          await Task.Delay(Timeout.Infinite, cancellationToken);
        }
        catch (TaskCanceledException)
        {
          // Cancellation requested
        }

        // Force close the connection (will end the thread)
        socket.Close();
      }

      return _discoveredBridges.Select(x => x.Value).ToList();
    }

    /// <summary>
    /// Listen to response on socket
    /// </summary>
    /// <param name="socket">The socket to listen to</param>
    private void ListenSocketResponseAsync(Socket socket)
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
