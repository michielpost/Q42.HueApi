using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.NET
{
  public class SSDPBridgeLocator : IBridgeLocator
  {
    readonly IPAddress multicastAddress = IPAddress.Parse("239.255.255.250");
    const int multicastPort = 1900;
    const int unicastPort = 1901;

    const string messageHeader = "M-SEARCH * HTTP/1.1";
    const string messageHost = "HOST: 239.255.255.250:1900";
    const string messageMan = "MAN: \"ssdp:discover\"";
    const string messageMx = "MX: 8";
    const string messageSt = "ST:SsdpSearch:all";


    readonly byte[] broadcastMessage = Encoding.UTF8.GetBytes(
        string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{0}",
                      "\r\n",
                      messageHeader,
                      messageHost,
                      messageMan,
                      messageMx,
                      messageSt));

    private List<string> _discoveredDevices = new List<string>();



    public async Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(TimeSpan timeout)
    {
      if (timeout <= TimeSpan.Zero)
        throw new ArgumentException("Timeout value must be greater than zero.", nameof(timeout));

      _discoveredDevices = new List<string>();


      using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
      {
        socket.Bind(new IPEndPoint(IPAddress.Any, unicastPort));
        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastAddress, IPAddress.Any));
        var thd = new Thread(() => GetSocketResponse(socket));
        socket.SendTo(broadcastMessage, 0, broadcastMessage.Length, SocketFlags.None, new IPEndPoint(multicastAddress, multicastPort));
        thd.Start();
        Thread.Sleep(timeout);
        socket.Close();
      }

      return await FilterBridges(_discoveredDevices).ConfigureAwait(false);
    
    }

    public void GetSocketResponse(Socket socket)
    {
      try
      {
        while (true)
        {
          var response = new byte[8000];
          EndPoint ep = new IPEndPoint(IPAddress.Any, multicastPort);
          socket.ReceiveFrom(response, ref ep);

          try
          {
            var receivedString = Encoding.UTF8.GetString(response);

            var location = receivedString.Substring(receivedString.IndexOf("LOCATION:", System.StringComparison.Ordinal) + 9);
            receivedString = location.Substring(0, location.IndexOf("\r", System.StringComparison.Ordinal)).Trim();

            _discoveredDevices.Add(receivedString);
          }
          catch
          {
            //Not a UTF8 string, ignore this response.
          }
        }
      }
      catch
      {
        //TODO handle exception for when connection closes
      }


    }


    private async Task<IEnumerable<LocatedBridge>> FilterBridges(List<string> discoveredDevices)
    {
      List<LocatedBridge> bridges = new List<LocatedBridge>();


      var endpoints = discoveredDevices.Where(s => s.EndsWith("/description.xml")).ToList();

      var filteredEndpoints = endpoints.Distinct();

      foreach (var endpoint in filteredEndpoints)
      {
        var ip = endpoint.Replace("http://", "").Replace("/description.xml", "");

        //Not in the list yet?
        if (!bridges.Where(x => x.IpAddress == ip).Any())
        {
		  //Check if it is Hue Bridge
		  string serialNumber = await IsHue(endpoint).ConfigureAwait(false);
		  if (!string.IsNullOrWhiteSpace(serialNumber))
          {
            //Add ip
            bridges.Add(new LocatedBridge() { IpAddress = ip, BridgeId = serialNumber });
          }
        }
      }

      return bridges;
    }

    // http://www.nerdblog.com/2012/10/a-day-with-philips-hue.html - description.xml retrieval
    private async Task<string> IsHue(string discoveryUrl)
    {
      // since this specifies timeout (and probably isn't called much), don't use shared client
      var http = new HttpClient { Timeout = TimeSpan.FromMilliseconds(2000) };
      try
      {
        var res = await http.GetStringAsync(discoveryUrl).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(res))
        {
			res = res.ToLower();

			if (res.Contains("philips hue bridge"))
			{
				int startSerial = res.IndexOf("<serialnumber>");
				if (startSerial > 0)
				{
					int endSerial = res.IndexOf("</", startSerial);

					int startPoint = startSerial + 14;
					return res.Substring(startPoint, endSerial - startPoint);
				}
			}
		}
      }
      catch
      {
        //Not a UTF8 string, ignore this response.
      }
      return null;
    }

  }
}
