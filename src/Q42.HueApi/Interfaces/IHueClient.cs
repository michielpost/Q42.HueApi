using Q42.HueApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  public interface IHueClient
  {
    string ApiBase { get; }

    /// <summary>
    /// Initialize the client with your app key
    /// </summary>
    /// <param name="appKey"></param>
    void Initialize(string appKey);

    /// <summary>
    /// Register application name at the bridge to be able to send commands
    /// </summary>
    /// <param name="appName"></param>
    /// <returns></returns>
    Task<bool> RegisterAsync(string appName);

    /// <summary>
    /// Set the next Hue color
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    Task SetNextHueColorAsync(IEnumerable<string> lampList = null);

    /// <summary>
    /// Get all lamps known to the bridge
    /// </summary>
    /// <returns></returns>
    Task<List<Lamp>> GetLampsAsync();

    /// <summary>
    /// Get bridge info
    /// </summary>
    /// <returns></returns>
    Task<Bridge> GetBridgeAsync();

    /// <summary>
    /// Send a raw string / json command
    /// </summary>
    /// <param name="command">json</param>
    /// <param name="lampList">if null, send to all lamps</param>
    /// <returns></returns>
    Task SendCommandRawAsync(string command, IEnumerable<string> lampList = null);

    /// <summary>
    /// Send a lamp command
    /// </summary>
    /// <param name="command">Compose a new LampCommand()</param>
    /// <param name="lampList">if null, send to all lamps</param>
    /// <returns></returns>
    Task SendCommandAsync(LampCommand command, IEnumerable<string> lampList = null);

    /// <summary>
    /// Send command to a group
    /// </summary>
    /// <param name="command"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    Task SendGroupCommandAsync(LampCommand command, int group = 0);

  }
}
