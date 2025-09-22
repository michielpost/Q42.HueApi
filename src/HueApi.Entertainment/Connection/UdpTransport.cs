using HueApi.Entertainment.Models;
using Org.BouncyCastle.Crypto.Tls;
using System.Net.Sockets;
using System.Text;

namespace HueApi.Entertainment.Connection
{
  /// <summary>
  /// Based on https://github.com/jinghongbo/Ssl.Net/tree/master/src/Ssl.Net/Ssl.Net
  /// </summary>
  internal class UdpTransport : DatagramTransport, IDisposable
  {
    private Socket _socket;
    private bool _disposed;

    public UdpTransport(Socket socket)
    {
      _socket = socket ?? throw new ArgumentNullException(nameof(socket));
    }

    public void Close()
    {
      if (_socket != null && _socket.IsBound)
      {
        try
        {
          _socket.Shutdown(SocketShutdown.Both);
        }
        catch (SocketException) { /* Log or ignore if already closed */ }
        _socket.Close();
      }
    }

    public void Dispose()
    {
      if (!_disposed)
      {
        Close();
        _disposed = true;
      }
    }

    public int GetReceiveLimit()
    {
      return 1024 * 4;
    }

    public int GetSendLimit()
    {
      return 1024 * 4;
    }

    public int Receive(byte[] buf, int off, int len, int waitMillis)
    {
      //string converted = Encoding.UTF8.GetString(buf, 0, buf.Length);

      if (_socket.Connected)
      {
        if (waitMillis == 0 && _socket.Available == 0)
        {
          return -1;
        }

        if (SpinWait.SpinUntil(() => _socket.Available > 0, waitMillis))
        {
          return _socket.Receive(buf, off, len, SocketFlags.None);
        }
        else
        {
          if (waitMillis == 60000) // 1 min
          {
            throw new TimeoutException();
          }

          return -1;
        }

      }

      throw new HueEntertainmentException("Receiving data but socket not connected");
    }

    public void Send(byte[] buf, int off, int len)
    {
      //string converted = Encoding.UTF8.GetString(buf, 0, buf.Length);

      try
      {
        _socket.Send(buf, off, len, SocketFlags.None);
      }
      catch (SocketException ex)
      {
        throw new HueEntertainmentException("Failed to send data over UDP", ex);
      }
    }
  }
}
