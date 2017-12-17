using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Streaming.Connection;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming
{
  public class StreamingHueClient
  {
    //Inspired by https://github.com/jinghongbo/Ssl.Net/tree/master/src/Ssl.Net/Ssl.Net
    private DtlsTransport _dtlsTransport;
    private UdpTransport _udp;
    private bool _simulator;

    private Socket _socket;
    private ILocalHueClient _localHueClient;
    private string _ip, _appKey, _clientKey;

    public ILocalHueClient LocalHueClient => _localHueClient;

    public StreamingHueClient(string ip, string appKey, string clientKey)
    {
      _ip = ip;
      _appKey = appKey;
      _clientKey = clientKey;

      _localHueClient = new LocalHueClient(ip, appKey, clientKey);

      _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      _socket.Bind(new IPEndPoint(IPAddress.Any, 0));
    }

    public void Close()
    {
      _dtlsTransport.Close();
    }


    public async Task Connect(string groupId, bool simulator = false)
    {
      _simulator = simulator;
      var enableResult = await _localHueClient.SetStreamingAsync(groupId);

      byte[] psk = FromHex(_clientKey);
      BasicTlsPskIdentity pskIdentity = new BasicTlsPskIdentity(_appKey, psk);

      var dtlsClient = new DtlsClient(null, pskIdentity);

      DtlsClientProtocol clientProtocol = new DtlsClientProtocol(new SecureRandom());

      await _socket.ConnectAsync(IPAddress.Parse(_ip), 2100);
      _udp = new UdpTransport(_socket);

      if(!simulator)
        _dtlsTransport = clientProtocol.Connect(dtlsClient, _udp);

    }

    public void AutoUpdate(StreamingGroup entGroup, int frequency, CancellationToken cancellationToken)
    {
      if (!this._simulator)
      {
        int groupCount = (entGroup.Count / 10) + 1;
        frequency = frequency / groupCount;
      }

      var waitTime = (int)TimeSpan.FromSeconds(1).TotalMilliseconds / frequency;

      Task.Run(async () =>
      {
        while(!cancellationToken.IsCancellationRequested)
        {
          Send(entGroup.GetCurrentState());

          await Task.Delay(waitTime);
        }

      });

    }

    public void Send(List<byte[]> states)
    {
      foreach(var state in states)
      {
        Send(state, 0, state.Length);
      }
    }

    public static byte[] FromHex(string hex)
    {
      hex = hex.Replace("-", "");
      byte[] raw = new byte[hex.Length / 2];
      for (int i = 0; i < raw.Length; i++)
      {
        raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
      }
      return raw;
    }


    public int Send(byte[] buffer, int offset, int count)
    {
      if(!_simulator)
        _dtlsTransport.Send(buffer, offset, count);
      else
        _udp.Send(buffer, offset, count);

      return count;
    }

  }
}
