using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.Extensions;
using Q42.HueApi.Models.Bridge;

namespace Q42.HueApi
{
  /// <summary>
  /// Base class for bridge locator based on Multicast UDP discovery
  /// </summary>
  public abstract class MUdpBasedBridgeLocator : BridgeLocator
  {
    /// <summary>
    /// Locate bridges by sending a specific multicast packet and listening to any reply
    /// <para>
    /// The multicast packet is sent every second and any endpoint responding is investigated to see if it is a Hue Bridge</para>
    /// </summary>
    /// <param name="multicastAddress">Multicast address to send to</param>
    /// <param name="multicastPort">Multicast port to send to</param>
    /// <param name="localPort">Local port to send from and listen to</param>
    /// <param name="discoveryMessageContent">The content to send</param>
    /// <param name="cancellationToken">Token to cancel the search</param>
    /// <returns>List of bridge IPs found</returns>
    protected async Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(
      IPAddress multicastAddress, int multicastPort, int localPort, byte[] discoveryMessageContent, CancellationToken cancellationToken)
    {
      var discoveredBridges = new ConcurrentDictionary<string, LocatedBridge>();

      // We will bind to all network interfaces having a private IPv4
      List<Socket> socketList = NetworkInterfaceExtensions.GetAllUpNetworkInterfacesFirstPrivateIPv4()
        // Create socket for each address remaining (and asking for a random available port)
        .Select(info => CreateSocketForMulticastUDPIPv4(new IPEndPoint(info.Address, localPort), multicastAddress))
        .ToList();

      try
      {
        foreach (Socket socket in socketList)
        {
          // Spin up a new thread to listen to this socket
          new Thread(() => ListenSocketAndCheckEveryEndpoint(socket, discoveredBridges)).Start();
        }

        do
        {
          // Send multicast discovery message for each socket
          foreach (Socket socket in socketList)
          {
            socket.SendTo(discoveryMessageContent, SocketFlags.None, new IPEndPoint(multicastAddress, multicastPort));
          }

          // Wait 1 seconds (can be shorter if cancelled)
          await Task.Delay(TimeSpan.FromMilliseconds(1000), cancellationToken);
        }
        while (!cancellationToken.IsCancellationRequested);
      }
      catch (TaskCanceledException)
      {
        // Cancellation requested
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

      return discoveredBridges.Select(x => x.Value).ToList();
    }

    /// <summary>
    /// Create a socket for multicast send/receive
    /// </summary>
    /// <param name="localEndpoint">the local endpoint to use</param>
    /// <param name="multicastGroupAddress">the multicast group</param>
    private static Socket CreateSocketForMulticastUDPIPv4(IPEndPoint localEndpoint, IPAddress multicastGroupAddress)
    {
      // Create an IPv4 UDP socket
      Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

      // Allow address reuse
      socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

      // Bind to all interface and to a port (if 0, ask for a free one)
      socket.Bind(localEndpoint);

      // Set TTL to 1: it will stays on the local network
      socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 1);

      // Join Multicast group
      socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
        new MulticastOption(multicastGroupAddress, localEndpoint.Address));

      return socket;
    }

    /// <summary>
    /// Listen to any response on a socket and check if responding IP is a Hue Bridge
    /// </summary>
    /// <param name="socket">The socket to listen to</param>
    /// <param name="discoveredBridges">The dictionary to fill with located bridges</param>
    private static void ListenSocketAndCheckEveryEndpoint(Socket socket, ConcurrentDictionary<string, LocatedBridge> discoveredBridges)
    {
      try
      {
        var ipSeen = new List<string>();
        var socketAddress = ((IPEndPoint?)socket.LocalEndPoint)?.Address;

        while (true)
        {
          var responseRawBuffer = new byte[8000];
          EndPoint responseEndPoint = new IPEndPoint(IPAddress.Any, 0);

          // Blocking call until something is received
          socket.ReceiveFrom(responseRawBuffer, ref responseEndPoint);

          // We received a response
          try
          {
            // Get IP address
            IPAddress responseIpAddress = ((IPEndPoint)responseEndPoint).Address;

            if (!(socketAddress?.Equals(responseIpAddress) ?? false) && !ipSeen.Contains(responseIpAddress.ToString()))
            {
              // Try decode response as UTF8
              var responseBody = Encoding.UTF8.GetString(responseRawBuffer);

              if (!string.IsNullOrWhiteSpace(responseBody))
              {
                // Spin up a new thread to handle this specific response so we can continue waiting for response
                new Thread(() =>
                {
                  try
                  {
                    // Check if it's a Hue Bridge
                    string serialNumber = CheckHueDescriptor(responseIpAddress, TimeSpan.FromMilliseconds(1000)).Result;

                    if (!string.IsNullOrWhiteSpace(serialNumber))
                    {
                      discoveredBridges.TryAdd(responseIpAddress.ToString(), new LocatedBridge()
                      {
                        IpAddress = responseIpAddress.ToString(),
                        BridgeId = serialNumber,
                      });
                    }
                    else
                    {
                      // Not a valid S/N
                    }
                  }
                  catch
                  {
                    // Something went wrong, ignore...
                  }
                }).Start();

                // Add this ip to local list, so we won't start other thread for it
                ipSeen.Add(responseIpAddress.ToString());
              }
              else
              {
                // Not a valid response
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
  }
}
