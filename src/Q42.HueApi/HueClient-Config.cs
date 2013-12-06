using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
  /// <summary>
  /// Partial HueClient, contains requests to the /config/ url
  /// </summary>
  public partial class HueClient
  {
    /// <summary>
    /// Deletes a whitelist entry
    /// </summary>
    /// <returns></returns>
    public async Task<bool> DeleteWhiteListEntryAsync(string entry)
    {
      CheckInitialized();

      HttpClient client = new HttpClient();

      var response = await client.DeleteAsync(new Uri(string.Format("{0}config/whitelist/{1}", ApiBase, entry))).ConfigureAwait(false);
      var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      JArray jresponse = JArray.Parse(stringResponse);
      JObject result = (JObject)jresponse.First;

      JToken error;
      if (result.TryGetValue("error", out error))
      {
        if (error["type"].Value<int>() == 3) // entry not available
          return false;
        else
          throw new Exception(error["description"].Value<string>());
      }

      return true;

    }



    /// <summary>
    /// Asynchronously gets the whitelist with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="WhiteList"/>s registered with the bridge.</returns>
    public async Task<IEnumerable<WhiteList>> GetWhiteListAsync()
    {
      CheckInitialized();

      Bridge bridge = await GetBridgeAsync().ConfigureAwait(false);
      return bridge.WhiteList;
    }


    /// <summary>
    /// Get bridge info
    /// </summary>
    /// <returns></returns>
    public async Task<Bridge> GetBridgeAsync()
    {
      CheckInitialized();

      HttpClient client = new HttpClient();
      var stringResult = await client.GetStringAsync(new Uri(ApiBase)).ConfigureAwait(false);

      BridgeState jsonResult = JsonConvert.DeserializeObject<BridgeState>(stringResult);

      return new Bridge(jsonResult);
    }

    /// <summary>
    /// Update bridge config
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public async Task UpdateBridgeConfigAsync(BridgeConfigUpdate update)
    {
      CheckInitialized();

      string command = JsonConvert.SerializeObject(update, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      HttpClient client = new HttpClient();
      var stringResult = await client.PutAsync(new Uri(string.Format("{0}config", ApiBase)), new StringContent(command)).ConfigureAwait(false);
    }
  }
}
