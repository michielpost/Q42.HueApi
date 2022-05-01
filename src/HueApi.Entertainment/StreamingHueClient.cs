using HueApi.Entertainment.Connection;
using HueApi.Entertainment.Models;
using HueApi.Extensions;
using Org.BouncyCastle.Crypto.Tls;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueApi.Entertainment
{
  /// <summary>
  /// Hue Client that supports streaming / hue entertainment api
  /// </summary>
  public class StreamingHueClient : IDisposable
  {
    //Inspired by https://github.com/jinghongbo/Ssl.Net/tree/master/src/Ssl.Net/Ssl.Net
    private DtlsTransport? _dtlsTransport;
    private UdpTransport? _udp;
    private bool _simulator;

    private Socket _socket;
    private LocalHueApi _localHueClient;
    protected string _ip, _appKey, _clientKey;
    private byte[]? entConfigIdBytes;

    public LocalHueApi LocalHueApi => _localHueClient;

    public StreamingHueClient(string ip, string appKey, string clientKey)
    {
      _ip = ip;
      _appKey = appKey;
      _clientKey = clientKey;

      _localHueClient = new LocalHueApi(ip, appKey);

      _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      _socket.Bind(new IPEndPoint(IPAddress.Any, 0));
    }

    public void Close()
    {
      _dtlsTransport?.Close();
      _socket.Shutdown(SocketShutdown.Both);
      _socket.Close();
      entConfigIdBytes = null;
    }


    /// <summary>
    /// Connect to a group to start streaming
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="simulator"></param>
    /// <returns></returns>
    public async Task ConnectAsync(Guid entertainmentAreaId, bool simulator = false)
    {
      _simulator = simulator;
      var enableResult = await _localHueClient.SetStreamingAsync(entertainmentAreaId).ConfigureAwait(false);
      entConfigIdBytes = Encoding.ASCII.GetBytes(entertainmentAreaId.ToString().ToLowerInvariant());

      byte[] psk = FromHex(_clientKey);
      BasicTlsPskIdentity pskIdentity = new BasicTlsPskIdentity(_appKey, psk);

      var dtlsClient = new DtlsClient(null!, pskIdentity);

      DtlsClientProtocol clientProtocol = new DtlsClientProtocol(new SecureRandom());
#if NET461
      _socket.Connect(IPAddress.Parse(_ip), 2100);
#else
      await _socket.ConnectAsync(IPAddress.Parse(_ip), 2100).ConfigureAwait(false);
#endif
      _udp = new UdpTransport(_socket);

      if (!simulator)
        _dtlsTransport = clientProtocol.Connect(dtlsClient, _udp);

    }

    /// <summary>
    /// Auto update the streamgroup
    /// </summary>
    /// <param name="streamingGroup"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="frequency"></param>
    /// <param name="onlySendDirtyStates">Only send light states that have been changed since last update</param>
    public Task AutoUpdateAsync(StreamingGroup streamingGroup, CancellationToken cancellationToken, int frequency = 50, bool onlySendDirtyStates = false)
    {
      if (!_simulator)
      {
        int groupCount = streamingGroup.Count / 20 + 1;
        frequency = frequency / groupCount;
      }
      else
        onlySendDirtyStates = false; //Simulator does not understand partial updates

      var waitTime = TimeSpan.FromMilliseconds(TimeSpan.FromSeconds(1).TotalMilliseconds / frequency);

      return Task.Run(() =>
      {
        int missedMessages = 0;
#if DEBUG
        int lastSecond = 0;
        int msgPerSecondCount = 0;
#endif
        while (!cancellationToken.IsCancellationRequested)
        {
          var sw = Stopwatch.StartNew();

          IEnumerable<IEnumerable<StreamingChannel>> chunks = streamingGroup.GetChunksForUpdate(forceUpdate: !onlySendDirtyStates);
          if (chunks != null)
          {
            Send(chunks);
          }
          else
          {
            missedMessages++;
            if (missedMessages > frequency)
            {
              //If there are no updates, still send updates to keep connection open
              chunks = streamingGroup.GetChunksForUpdate(forceUpdate: true);
              Send(chunks);
              missedMessages = 0;
            }
          }
          sw.Stop();
#if DEBUG
          //Debug.WriteLine("Elasped: " + sw.ElapsedMilliseconds);
          msgPerSecondCount++;
          if (DateTime.Now.Second != lastSecond)
          {
            Debug.WriteLine("Msg per second: " + msgPerSecondCount);
            msgPerSecondCount = 0;
            lastSecond = DateTime.Now.Second;

          }
#endif
          if (sw.Elapsed < waitTime)
            Thread.Sleep(waitTime - sw.Elapsed); //Better performance than Task.Delay
        }

      });

    }

    /// <summary>
    /// Can be used if you dont want to use the AutoUpdate, but need the same logic for sending updated to the bridge
    /// </summary>
    /// <param name="streamingGroup"></param>
    /// <param name="onlySendDirtyStates"></param>
    public void ManualUpdate(StreamingGroup streamingGroup, bool onlySendDirtyStates = false)
    {
      IEnumerable<IEnumerable<StreamingChannel>> chunks = streamingGroup.GetChunksForUpdate(forceUpdate: !onlySendDirtyStates);
      if (chunks != null)
      {
        Send(chunks);
      }
    }

    protected virtual void Send(IEnumerable<IEnumerable<StreamingChannel>> chunks)
    {
      if (entConfigIdBytes == null)
        throw new HueEntertainmentException("This client is not connected to a group");

      var msg = StreamingGroup.GetCurrentStateAsByteArray(this.entConfigIdBytes, chunks);
      Send(msg);
    }

    /// <summary>
    /// Send a list of states to the Hue Bridge
    /// </summary>
    /// <param name="states"></param>
    protected virtual void Send(List<byte[]>? states)
    {
      if (states != null)
      {
        foreach (var state in states)
        {
          Send(state, 0, state.Length);
        }
      }
    }

    /// <summary>
    /// Hex to byte conversion
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    private static byte[] FromHex(string hex)
    {
      hex = hex.Replace("-", "");
      byte[] raw = new byte[hex.Length / 2];
      for (int i = 0; i < raw.Length; i++)
      {
        raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
      }
      return raw;
    }

    /// <summary>
    /// Send bytes to the hue bridge
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    protected virtual int Send(byte[] buffer, int offset, int count)
    {
      if (!_simulator)
        _dtlsTransport?.Send(buffer, offset, count);
      else
        _udp?.Send(buffer, offset, count);

      return count;
    }

    public void Dispose()
    {
      Close();
    }
  }
}
