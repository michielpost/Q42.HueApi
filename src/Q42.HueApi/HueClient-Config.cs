using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using Q42.HueApi.Models.Groups;
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
  public partial class HueClient : IHueClient_Config
  {
    /// <summary>
    /// Deletes a whitelist entry
    /// </summary>
    /// <returns></returns>
    public async Task<bool> DeleteWhiteListEntryAsync(string entry)
    {
      CheckInitialized();

      HttpClient client = await GetHttpClient().ConfigureAwait(false);

      var response = await client.DeleteAsync(new Uri(string.Format("{0}config/whitelist/{1}", ApiBase, entry))).ConfigureAwait(false);
      var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      JArray jresponse = JArray.Parse(stringResponse);
      JObject? result = (JObject?)jresponse.First;

      if (result != null && result.TryGetValue("error", out JToken? error))
      {
        if (error?["type"]?.Value<int>() == 3) // entry not available
          return false;
        else
          throw new HueException(error?["description"]?.Value<string>());
      }

      return true;

    }



    /// <summary>
    /// Asynchronously gets the whitelist with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="WhiteList"/>s registered with the bridge.</returns>
    public async Task<IEnumerable<WhiteList>?> GetWhiteListAsync()
    {
      //Not needed to check if initialized, can be used without API key

      BridgeConfig? config = await GetConfigAsync().ConfigureAwait(false);
      if (config == null)
        return null;
      
      return config.WhiteList.Select(l => l.Value).ToList();
    }


    /// <summary>
    /// Get bridge info
    /// </summary>
    /// <returns></returns>
    public async Task<Bridge?> GetBridgeAsync()
    {
      CheckInitialized();

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var stringResult = await client.GetStringAsync(new Uri(ApiBase)).ConfigureAwait(false);

      BridgeState? jsonResult = DeserializeResult<BridgeState>(stringResult);
      if (jsonResult == null)
        return null;

      return new Bridge(jsonResult);
    }
    
    
    /// <summary>
    /// Get bridge config
    /// </summary>
    /// <returns>BridgeConfig object</returns>
    public async Task<BridgeConfig?> GetConfigAsync()
    {
        //Not needed to check if initialized, can be used without API key

        HttpClient client = await GetHttpClient().ConfigureAwait(false);
        string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}config", ApiBase))).ConfigureAwait(false);
        JToken token = JToken.Parse(stringResult);
        BridgeConfig? config = null;
        if (token.Type == JTokenType.Object)
        {
            var jsonResult = (JObject)token;
            config = JsonConvert.DeserializeObject<BridgeConfig>(jsonResult.ToString());

            //Fix whitelist IDs
            foreach (var whitelist in config.WhiteList)
              whitelist.Value.Id = whitelist.Key;
        }
        return config;
    }

    /// <summary>
    /// Update bridge config
    /// </summary>
    /// <param name="update"></param>
    /// <returns></returns>
    public async Task<HueResults> UpdateBridgeConfigAsync(BridgeConfigUpdate update)
    {
      CheckInitialized();

      string command = JsonConvert.SerializeObject(update, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var result = await client.PutAsync(new Uri(string.Format("{0}config", ApiBase)), new JsonContent(command)).ConfigureAwait(false);

      string jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);
    }
  }
}
