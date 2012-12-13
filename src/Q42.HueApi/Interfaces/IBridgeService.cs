using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  public interface IBridgeService
  {
    /// <summary>
    /// Register application name at the bridge to be able to send commands
    /// </summary>
    /// <param name="appName"></param>
    /// <returns></returns>
    Task<bool> Register(string appName);
  }
}
