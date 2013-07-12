using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Q42.HueApi.Extensions;
using Newtonsoft.Json;

namespace Q42.HueApi
{
  /// <summary>
  /// Partial HueClient, contains requests to the /lights/ url
  /// </summary>
  public partial class HueClient
  {
    /// <summary>
    /// Asynchronously retrieves an individual light.
    /// </summary>
    /// <param name="id">The light's Id.</param>
    /// <returns>The <see cref="Light"/> if found, <c>null</c> if not.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="id"/> is empty or a blank string.</exception>
    public async Task<Light> GetLightAsync(string id)
    {
      if (id == null)
        throw new ArgumentNullException("id");
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id can not be empty or a blank string", "id");

      CheckInitialized();

      HttpClient client = new HttpClient();
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}lights/{1}", ApiBase, id))).ConfigureAwait(false);

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Array)
      {
        // Hue gives back errors in an array for this request
        JObject error = (JObject)token.First["error"];
        if (error["type"].Value<int>() == 3) // Light not found
          return null;

        throw new Exception(error["description"].Value<string>());
      }

      return token.ToObject<Light>();
    }

    /// <summary>
    /// Asynchronously gets all lights registered with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="Light"/>s registered with the bridge.</returns>
    public async Task<IEnumerable<Light>> GetLightsAsync()
    {
      CheckInitialized();

      Bridge bridge = await GetBridgeAsync().ConfigureAwait(false);
      return bridge.Lights;
    }

    /// <summary>
    /// Send a lightCommand to a list of lights
    /// </summary>
    /// <param name="command"></param>
    /// <param name="lightList">if null, send command to all lights</param>
    /// <returns></returns>
    public Task SendCommandAsync(LightCommand command, IEnumerable<string> lightList = null)
    {
      if (command == null)
        throw new ArgumentNullException("command");

      string jsonCommand = JsonConvert.SerializeObject(command, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      return SendCommandRawAsync(jsonCommand, lightList);
    }


    /// <summary>
    /// Send a json command to a list of lights
    /// </summary>
    /// <param name="command"></param>
    /// <param name="lightList">if null, send command to all lights</param>
    /// <returns></returns>
    public Task SendCommandRawAsync(string command, IEnumerable<string> lightList = null)
    {
      if (command == null)
        throw new ArgumentNullException("command");

      CheckInitialized();

      if (lightList == null || !lightList.Any())
      {
        return SendGroupCommandAsync(command);
      }
      else if (lightList.Count() == 1 || !_useGroups)
      {
        return lightList.ForEachAsync(_parallelRequests, async (lightId) =>
        {
          HttpClient client = new HttpClient();
          await client.PutAsync(new Uri(ApiBase + string.Format("lights/{0}/state", lightId)), new StringContent(command)).ConfigureAwait(false);

        });
      }
      else
      {
        //Bridge firmware is updated, this should work. Can be disabled by setting _useGroups = false
        //Create a group (1) with the lights you want to set and update that group. most of the time faster than updating all individual lights
        //And lights change color at the same time.
        return Task.Run(async () =>
        {
          string lightString = CreateLightList(lightList);

          HttpClient client = new HttpClient();
          //Delete group 1
          await DeleteGroup("1").ConfigureAwait(false);

          //Create group (will be group 1) with the lights we want to target
          string groupId = await CreateGroup(lightList).ConfigureAwait(false);

          //Wait for 2 seconds so each lamp knows about the group (2 seconds is based on testing, can 
          await Task.Delay(2000);

          //Send command to group 1
          //await client.PutAsync(new Uri(ApiBase + "groups/1/action"), new StringContent(command));
          await SendGroupCommandAsync(command, groupId).ConfigureAwait(false);
        });

      }
    }


    /// <summary>
    /// Set the next Hue color
    /// </summary>
    /// <param name="lightList"></param>
    /// <returns></returns>
    public Task SetNextHueColorAsync(IEnumerable<string> lightList = null)
    {
      //Invalid JSON, but it works
      string command = "{\"hue\":+10000,\"sat\":255}";

      return SendCommandRawAsync(command, lightList);

    }
  }
}
