using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Q42.HueApi.Extensions;
using Newtonsoft.Json;
using Q42.HueApi.Models.Groups;
using System.Dynamic;
using Q42.HueApi.Models;
using Q42.HueApi.Interfaces;

namespace Q42.HueApi
{
  /// <summary>
  /// Partial HueClient, contains requests to the /lights/ url
  /// </summary>
  public partial class HueClient : IHueClient_Lights
  {
    /// <summary>
    /// Asynchronously retrieves an individual light.
    /// </summary>
    /// <param name="id">The light's Id.</param>
    /// <returns>The <see cref="Light"/> if found, <c>null</c> if not.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="id"/> is empty or a blank string.</exception>
    public async Task<Light?> GetLightAsync(string id)
    {
      if (id == null)
        throw new ArgumentNullException(nameof(id));
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id can not be empty or a blank string", nameof(id));

      CheckInitialized();

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}lights/{1}", ApiBase, id))).ConfigureAwait(false);

      //#if DEBUG
      //      //Normal result example
      //      stringResult = "{    \"state\": {        \"hue\": 50000,        \"on\": true,        \"effect\": \"none\",        \"alert\": \"none\",       \"bri\": 200,        \"sat\": 200,        \"ct\": 500,        \"xy\": [0.5, 0.5],        \"reachable\": true,       \"colormode\": \"hs\"    },    \"type\": \"Living Colors\",    \"name\": \"LC 1\",    \"modelid\": \"LC0015\",    \"swversion\": \"1.0.3\",    \"pointsymbol\": {        \"1\": \"none\",        \"2\": \"none\",        \"3\": \"none\",        \"4\": \"none\",        \"5\": \"none\",        \"6\": \"none\",        \"7\": \"none\",        \"8\": \"none\"    }}";

      //      //Lux result
      //      stringResult = "{    \"state\": {       \"on\": true,        \"effect\": \"none\",        \"alert\": \"none\",       \"bri\": 200,           \"reachable\": true,       \"colormode\": \"hs\"    },    \"type\": \"Living Colors\",    \"name\": \"LC 1\",    \"modelid\": \"LC0015\",    \"swversion\": \"1.0.3\",    \"pointsymbol\": {        \"1\": \"none\",        \"2\": \"none\",        \"3\": \"none\",        \"4\": \"none\",        \"5\": \"none\",        \"6\": \"none\",        \"7\": \"none\",        \"8\": \"none\"    }}";
      //#endif

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Array)
      {
        // Hue gives back errors in an array for this request
        var error = token.First?["error"];
        if (error?["type"]?.Value<int>() == 3) // Light not found
          return null;

        throw new HueException(error?["description"]?.Value<string>());
      }

      var light = token.ToObject<Light>();
      if (light != null)
        light.Id = id;

      return light;
    }

    /// <summary>
    /// Sets the light name
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<HueResults> SetLightNameAsync(string id, string name)
    {
      if (id == null)
        throw new ArgumentNullException(nameof(id));
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id can not be empty or a blank string", nameof(id));

      CheckInitialized();

      string command = JsonConvert.SerializeObject(new { name = name });

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var result = await client.PutAsync(new Uri(String.Format("{0}lights/{1}", ApiBase, id)), new JsonContent(command)).ConfigureAwait(false);

      var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);
    }

    /// <summary>
    /// Sets the light name
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public async Task<HueResults> LightConfigUpdate(string id, LightConfigUpdate config)
    {
      if (id == null)
        throw new ArgumentNullException(nameof(id));
      if (config == null)
        throw new ArgumentNullException(nameof(config));

      CheckInitialized();

      string jsonCommand = JsonConvert.SerializeObject(config, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var result = await client.PutAsync(new Uri(String.Format("{0}lights/{1}/config", ApiBase, id)), new JsonContent(jsonCommand)).ConfigureAwait(false);

      var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);
    }

    /// <summary>
    /// Asynchronously gets all lights registered with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="Light"/>s registered with the bridge.</returns>
    public async Task<IEnumerable<Light>> GetLightsAsync()
    {
      CheckInitialized();

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}lights", ApiBase))).ConfigureAwait(false);

      List<Light> results = new List<Light>();

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Object)
      {
        //Each property is a light
        var jsonResult = (JObject)token;

        foreach (var prop in jsonResult.Properties())
        {
          Light newLight = JsonConvert.DeserializeObject<Light>(prop.Value.ToString());
          newLight.Id = prop.Name;
          results.Add(newLight);
        }
      }
      return results;
    }

    /// <summary>
    /// Send a lightCommand to a list of lights
    /// </summary>
    /// <param name="command"></param>
    /// <param name="lightList">if null, send command to all lights</param>
    /// <returns></returns>
    public Task<HueResults> SendCommandAsync(LightCommand command, IEnumerable<string>? lightList = null)
    {
      if (command == null)
        throw new ArgumentNullException(nameof(command));

      string jsonCommand = JsonConvert.SerializeObject(command, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      return SendCommandRawAsync(jsonCommand, lightList);
    }


    /// <summary>
    /// Send a json command to a list of lights
    /// </summary>
    /// <param name="command"></param>
    /// <param name="lightList">if null, send command to all lights</param>
    /// <returns></returns>
    public async Task<HueResults> SendCommandRawAsync(string command, IEnumerable<string>? lightList = null)
    {
      if (command == null)
        throw new ArgumentNullException(nameof(command));

      CheckInitialized();

      if (lightList == null || !lightList.Any())
      {
        //Group 0 always contains all the lights
        return await SendGroupCommandAsync(command).ConfigureAwait(false);
      }
      else
      {
        HueResults results = new HueResults();
        HttpClient client = await GetHttpClient().ConfigureAwait(false);

        await lightList.ForEachAsync(_parallelRequests, async (lightId) =>
        {
          try
          {
            var result = await client.PutAsync(new Uri(ApiBase + $"lights/{lightId}/state"), new JsonContent(command)).ConfigureAwait(false);

            string jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            results.AddRange(DeserializeDefaultHueResult(jsonResult));
          }
          catch(Exception ex)
          {
            results.Add(new DefaultHueResult()
            {
              Error = new ErrorResult()
              {
                Address = $"lights/{lightId}/state",
                Description = ex.ToString()
              }
            }); ;
          }

        }).ConfigureAwait(false);

        return results;
      }
    }

    /// <summary>
    /// Start searching for new lights
    /// </summary>
    /// <param name="deviceIds">The maxiumum number of serial numbers in any request is 10.</param>
    /// <returns></returns>
    public async Task<HueResults> SearchNewLightsAsync(IEnumerable<string>? deviceIds = null)
    {
      CheckInitialized();

      StringContent? jsonStringContent = null;

      if (deviceIds != null)
      {
        JObject jsonObj = new JObject();
        jsonObj.Add("deviceid", JToken.FromObject(deviceIds.Take(10)));

        string jsonString = JsonConvert.SerializeObject(jsonObj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

        jsonStringContent = new JsonContent(jsonString);

      }

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var response = await client.PostAsync(new Uri(String.Format("{0}lights", ApiBase)), jsonStringContent).ConfigureAwait(false);

      var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }

    /// <summary>
    /// Gets a list of lights that were discovered the last time a search for new lights was performed. The list of new lights is always deleted when a new search is started.
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyCollection<Light>> GetNewLightsAsync()
    {
      CheckInitialized();

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}lights/new", ApiBase))).ConfigureAwait(false);

      //#if DEBUG
      //      //stringResult = "{\"7\": {\"name\": \"Hue Lamp 7\"},   \"8\": {\"name\": \"Hue Lamp 8\"},    \"lastscan\": \"2012-10-29T12:00:00\"}";
      //#endif

      List<Light> results = new List<Light>();

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Object)
      {
        //Each property is a light
        var jsonResult = (JObject)token;

        foreach (var prop in jsonResult.Properties())
        {
          if (prop.Name != "lastscan")
          {
            Light newLight = JsonConvert.DeserializeObject<Light>(prop.Value.ToString());
            newLight.Id = prop.Name;

            results.Add(newLight);

          }
        }

      }

      return results;

    }

    /// <summary>
    /// Deletes a single light
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public async Task<IReadOnlyCollection<DeleteDefaultHueResult>> DeleteLightAsync(string id)
    {
      CheckInitialized();

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      //Delete light
      var result = await client.DeleteAsync(new Uri(ApiBase + string.Format("lights/{0}", id))).ConfigureAwait(false);

      string jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      //#if DEBUG
      //      jsonResult = "[{\"success\":\"/lights/" + id + " deleted\"}]";
      //#endif

      return DeserializeDefaultHueResult<DeleteDefaultHueResult>(jsonResult);

    }
  }
}
