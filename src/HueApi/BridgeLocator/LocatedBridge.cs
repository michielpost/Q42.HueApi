using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi.BridgeLocator
{
  public class LocatedBridge
  {
    public LocatedBridge(string bridgeId, string ipAddress, int? port)
    {
      BridgeId = bridgeId;
      IpAddress = ipAddress;
      Port = port;
    }

    public string BridgeId { get; }
    public string IpAddress { get; }
    public int? Port { get; }

    public string Url => Port.HasValue ? $"https://{IpAddress}:{Port}" : $"https://{IpAddress}";

    /// <summary>
    /// Overrides ToString() to give something more useful than object name.
    /// </summary>
    /// <returns>A string like "Bridge 021788FFFE6E28D4: 192.168.12.34"</returns>
    public override string ToString()
    {
      return string.Format("Bridge {0}: {1}", BridgeId, IpAddress);
    }
  }
}
