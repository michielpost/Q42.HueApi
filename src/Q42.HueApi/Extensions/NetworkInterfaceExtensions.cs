using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace Q42.HueApi.Extensions
{
  /// <summary>
  /// NetworkInterface Helpers
  /// </summary>
  internal static class NetworkInterfaceExtensions
  {
    /// <summary>
    /// Retrieve the list of all first private IPv4 of each interfaces that are up
    /// </summary>
    /// <returns>List of private IPv4 information</returns>
    public static List<UnicastIPAddressInformation> GetAllUpNetworkInterfacesFirstPrivateIPv4()
    {
      return NetworkInterface.GetAllNetworkInterfaces()
            // Keep only connected interfaces
            .Where(itf => itf.OperationalStatus == OperationalStatus.Up)
            // Retrieve all unicast addresses of each interface
            .SelectMany(itf => itf.GetIPProperties().UnicastAddresses)
            // Keep only private IPv4
            .Where(info => !info.Address.IsLoopback() && !info.Address.IsIPv4LinkLocal() && info.Address.IsIPv4Private())
            .ToList();
    }
  }
}
