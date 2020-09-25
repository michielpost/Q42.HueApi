using Q42.HueApi.Models;
using Q42.HueApi.Models.Bridge;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  /// <summary>
  /// Hue Client for interaction with a local bridge
  /// </summary>
  public interface ILocalHueClient :
    IHueClient,
    ILocalHueClient_Api,
    ILocalHueClient_Streaming
  {

    /// <summary>
    /// Option to use https connection to local bridge
    /// </summary>
    bool UseHttps { get; set; }

    /// <summary>
    /// Check if there is a working connection with the bridge
    /// </summary>
    /// <returns></returns>
    Task<bool> CheckConnection();

  }
}
