using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using Q42.HueApi.Models.Bridge;
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
  ///  Partial HueClient, contains requests to the /api/ url
  /// </summary>
  public partial class LocalHueClient : ILocalHueClient_Streaming
  {

    public void InitializeStreaming(string clientKey)
    {
      IsStreamingInitialized = true;
      this._clientKey = clientKey;
    }

    public async Task<HueResults> SetStreamingAsync(string id, bool active = true)
    {
      if (id == null)
        throw new ArgumentNullException(nameof(id));
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id must not be empty", nameof(id));

      JObject jsonObj = new JObject();
      jsonObj.Add("stream", JToken.FromObject(new { active = active }));

      string jsonString = JsonConvert.SerializeObject(jsonObj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var response = await client.PutAsync(new Uri(String.Format("{0}groups/{1}", ApiBase, id)), new JsonContent(jsonString)).ConfigureAwait(false);
      var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }
  }
}
