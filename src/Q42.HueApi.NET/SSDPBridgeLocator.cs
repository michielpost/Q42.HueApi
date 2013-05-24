using Q42.HueApi.Interfaces;
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



    public async Task<IEnumerable<string>> LocateBridgesAsync(TimeSpan timeout)
    {
      if (timeout <= TimeSpan.Zero)
        throw new ArgumentException("Timeout value must be greater than zero.", "timeout");

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

      return await FilterBridges(_discoveredDevices);
    
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
          var receivedString = Encoding.UTF8.GetString(response);

          var location = receivedString.Substring(receivedString.ToLower().IndexOf("location:", System.StringComparison.Ordinal) + 9);
          receivedString = location.Substring(0, location.IndexOf("\r", System.StringComparison.Ordinal)).Trim();

          _discoveredDevices.Add(receivedString);
        }
      }
      catch
      {
        //TODO handle exception for when connection closes
      }


    }


    private async Task<IEnumerable<string>> FilterBridges(List<string> discoveredDevices)
    {
      List<string> bridgeIps = new List<string>();


      var endpoints = discoveredDevices.Where(s => s.EndsWith("/description.xml")).ToList();

      foreach (var endpoint in endpoints)
      {
        var ip = endpoint.Replace("http://", "").Replace("/description.xml", "");

        //Not in the list yet?
        if (!bridgeIps.Contains(ip))
        {
          //Check if it is Hue Bridge
          if (await IsHue(endpoint))
          {
            //Add ip
            bridgeIps.Add(ip);
          }
        }
      }

      return bridgeIps;
    }

    // http://www.nerdblog.com/2012/10/a-day-with-philips-hue.html - description.xml retrieval
    private async Task<bool> IsHue(string discoveryUrl)
    {
      var http = new HttpClient { Timeout = TimeSpan.FromMilliseconds(2000) };
      var res = await http.GetStringAsync(discoveryUrl);
      if (!string.IsNullOrWhiteSpace(res))
      {
        if (res.ToLower().Contains("philips hue bridge"))
          return true;
      }
      return false;
    }

  }
}
