using Q42.HueApi.Models;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  public interface IHueClient_Lights
  {
    /// <summary>
    /// Asynchronously gets all lights registered with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="Light"/>s registered with the bridge.</returns>
    Task<IEnumerable<Light>> GetLightsAsync();

    /// <summary>
    /// Asynchronously retrieves an individual light.
    /// </summary>
    /// <param name="id">The light's Id.</param>
    /// <returns>The <see cref="Light"/> if found, <c>null</c> if not.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="id"/> is empty or a blank string.</exception>
    Task<Light?> GetLightAsync(string id);

    /// <summary>
    /// Delete Light
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<DeleteDefaultHueResult>> DeleteLightAsync(string id);

    /// <summary>
    /// Sets the light name
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<HueResults> SetLightNameAsync(string id, string name);

    /// <summary>
    /// Update Light Config
    /// </summary>
    /// <param name="id"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    Task<HueResults> LightConfigUpdate(string id, LightConfigUpdate config);

    /// <summary>
    /// Send a raw string / json command
    /// </summary>
    /// <param name="command">json</param>
    /// <param name="lightList">if null, send to all lights</param>
    /// <returns></returns>
    Task<HueResults> SendCommandRawAsync(string command, IEnumerable<string>? lightList = null);

    /// <summary>
    /// Send a light command
    /// </summary>
    /// <param name="command">Compose a new lightCommand()</param>
    /// <param name="lightList">if null, send to all lights</param>
    /// <returns></returns>
    Task<HueResults> SendCommandAsync(LightCommand command, IEnumerable<string>? lightList = null);

    /// <summary>
    /// Start searching for new lights
    /// </summary>
    /// <returns></returns>
    Task<HueResults> SearchNewLightsAsync(IEnumerable<string>? deviceIds = null);

    /// <summary>
    /// Gets a list of lights that were discovered the last time a search for new lights was performed.
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<Light>> GetNewLightsAsync();
  }
}
