using JeffWilcox.Utilities.Silverlight;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
  /// <summary>
  /// http://www.developers.meethue.com/documentation/remote-api-authentication
  /// </summary>
  public partial class RemoteHueClient
  {

    public async Task<string?> RegisterAsync(string bridgeId, string appId)
    {
      if (string.IsNullOrEmpty(bridgeId))
        throw new ArgumentNullException(nameof(bridgeId));
      if (string.IsNullOrEmpty(appId))
        throw new ArgumentNullException(nameof(appId));

      JObject obj = new JObject();
      obj["linkbutton"] = true;

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var configResponse = await client.PutAsync(new Uri($"{_apiBase}{bridgeId}/0/config"), new JsonContent(obj.ToString())).ConfigureAwait(false);

      JObject bridge = new JObject();
      bridge["devicetype"] = appId;

      var response = await client.PostAsync(new Uri($"{_apiBase}{bridgeId}/"), new JsonContent(bridge.ToString())).ConfigureAwait(false);
      var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);


      JObject? result;
      try
      {
        JArray jresponse = JArray.Parse(stringResponse);
        result = (JObject?)jresponse.First;
      }
      catch
      {
        //Not an expected response. Return response as exception
        throw new HueException(stringResponse);
      }

      if (result != null)
      {
        if (result.TryGetValue("error", out JToken? error))
        {
          if (error?["type"]?.Value<int>() == 101) // link button not pressed
            throw new LinkButtonNotPressedException("Link button not pressed");
          else
            throw new HueException(error?["description"]?.Value<string>());
        }

        var key = result["success"]?["username"]?.Value<string>();
        if (!string.IsNullOrEmpty(key))
        {
          Initialize(key!);

          return key;
        }
      }

      return null;
    }

  }
}
