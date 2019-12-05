using System.Net;
using System.Net.Sockets;

namespace Q42.HueApi.Extensions
{
  /// <summary>
  /// IPAddress Helpers
  /// </summary>
  internal static class IPAddressExtensions
  {
    /// <summary>
    /// Check if given IPv4 is a link-local (auto configuration) address (according to RFC3927)
    /// </summary>
    /// <remarks>https://tools.ietf.org/html/rfc3927</remarks>
    /// <param name="ip">The IPv4</param>
    /// <returns>True if link-local, false otherwise</returns>
    public static bool IsIPv4LinkLocal(this IPAddress ip)
    {
      if (ip.AddressFamily != AddressFamily.InterNetwork)
      {
        // Not an IPv4, simply return false
        return false;
      }

      return ip.ToString().StartsWith("169.254.");
    }

    /// <summary>
    /// Check if given IP is a loopback
    /// <para>This is just a helper extension around the static method IPAddress.IsLoopback</para>
    /// </summary>
    /// <param name="ip">The IP</param>
    /// <returns>True if loopback, False otherwise</returns>
    public static bool IsLoopback(this IPAddress ip)
    {
      return IPAddress.IsLoopback(ip);
    }

    /// <summary>
    /// Check if given IPv4 is in private range (according to RFC1918)
    /// </summary>
    /// <remarks>https://tools.ietf.org/html/rfc1918</remarks>
    /// <param name="ip">The IPv4</param>
    /// <returns>True if private, false otherwise</returns>
    public static bool IsIPv4Private(this IPAddress ip)
    {
      if (ip.AddressFamily != AddressFamily.InterNetwork)
      {
        // Not an IPv4, simply return false
        return false;
      }

      byte[] bytes = ip.GetAddressBytes();

      switch (bytes[0])
      {
        // 10.0.0.0 - 10.255.255.255 (10/8 prefix)
        case 10:
          return true;

        // 172.16.0.0 - 172.31.255.255 (172.16/12 prefix)
        case 172:
          return bytes[1] < 32 && bytes[1] >= 16;

        // 192.168.0.0 - 192.168.255.255 (192.168/16 prefix)
        case 192:
          return bytes[1] == 168;

        // Others
        default:
          return false;
      }
    }
  }
}
