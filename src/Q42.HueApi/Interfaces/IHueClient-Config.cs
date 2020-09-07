using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  public interface IHueClient_Config
  {
    /// <summary>
    /// Deletes a whitelist entry
    /// </summary>
    /// <returns></returns>
    [Obsolete("Removed from Bridge API. Use https://account.meethue.com/apps")]
    Task<bool> DeleteWhiteListEntryAsync(string entry);

    /// <summary>
    /// Asynchronously gets all lights registered with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="WhiteList"/>s registered with the bridge.</returns>
    Task<IEnumerable<WhiteList>?> GetWhiteListAsync();

    /// <summary>
    /// Get bridge info
    /// </summary>
    /// <returns></returns>
    Task<Bridge?> GetBridgeAsync();

    /// <summary>
    /// Update bridge config
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    Task<HueResults> UpdateBridgeConfigAsync(BridgeConfigUpdate update);
  }
}
