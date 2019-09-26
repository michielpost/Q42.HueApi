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
  public interface ILocalHueClient_Api
  {
    /// <summary>
    /// Register your <paramref name="appName"/> and <paramref name="deviceName"/> at the Hue Bridge.
    /// </summary>
    /// <param name="deviceName">The name of the device.</param>
    /// <param name="appName">The name of your app.</param>
    /// <returns><c>true</c> if success, <c>false</c> if the link button hasn't been pressed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="appName"/> or <paramref name="deviceName"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="appName"/> or <paramref name="deviceName"/> aren't long enough, are empty or contains spaces.</exception>
    Task<string?> RegisterAsync(string appName, string deviceName);

    Task<RegisterEntertainmentResult?> RegisterAsync(string applicationName, string deviceName, bool generateClientKey);

    /// <summary>
    /// Initialize the client with your app key
    /// </summary>
    /// <param name="appKey"></param>
    void Initialize(string appKey);
  
  }
}
