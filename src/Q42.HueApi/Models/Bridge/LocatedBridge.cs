using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Bridge
{
	public class LocatedBridge
	{
		public string BridgeId { get; set; }
		public string IpAddress { get; set; }

    /// <summary>
    /// Overrides ToString() to give something more useful than object name.
    /// </summary>
    /// <returns>A string like "Bridge 021788FFFE6E28D4: 192.168.12.34"</returns>
    public override string ToString()
    {
      return String.Format("Bridge {0}: {1}", BridgeId, IpAddress);
    }
  }
}
