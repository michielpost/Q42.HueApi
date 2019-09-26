using Q42.HueApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  /// <summary>
  /// Remote Hue Client responsible for interacting with the bridge using the remote API
  /// </summary>
  public interface IRemoteHueClient : IHueClient
  {
    /// <summary>
    /// Initialize the client with a bridgeId and appKey (whitelist identifier)
    /// </summary>
    /// <param name="bridgeId"></param>
    /// <param name="appKey"></param>
    void Initialize(string bridgeId, string appKey);

    /// <summary>
    /// Registers bridge for remote communication. Returns appKey and Initialized the client with this appkey
    /// </summary>
    /// <param name="bridgeId"></param>
    /// <returns></returns>
    Task<string?> RegisterAsync(string bridgeId, string appId);

    /// <summary>
    /// Gets the bridge ID registered by the user. When a user has linked a bridge to an account on www.meethue.com the bridge will appear on this interface.  
    /// </summary>
    /// <returns></returns>
    Task<List<RemoteBridge>?> GetBridgesAsync();
  }
}
