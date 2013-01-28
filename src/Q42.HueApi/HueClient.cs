using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Extensions;
using Q42.HueApi.Interfaces;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;

namespace Q42.HueApi
{
  /// <summary>
  /// Responsible for communicating with the bridge
  /// </summary>
  public class HueClient : IHueClient
  {

    private readonly string _ip;
    private readonly int _parallelRequests = 5;
    private readonly bool _useGroups = false;

    private string _appKey;

    /// <summary>
    /// Indicates the HueClient is initialized with an AppKey
    /// </summary>
    public bool IsInitialized { get; private set; }


    /// <summary>
    /// Base URL for the API
    /// </summary>
    public string ApiBase
    {
      get
      {
        return string.Format("http://{0}/api/{1}/", _ip, _appKey);
      }
    }

    /// <summary>
    /// Initialize with Bridge IP
    /// </summary>
    /// <param name="ip"></param>
    public HueClient(string ip)
    {
      if (ip == null)
        throw new ArgumentNullException ("ip");

      _ip = ip;
    }

    /// <summary>
    /// Initialize with Bridge IP and AppKey
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="appKey"></param>
    public HueClient(string ip, string appKey)
    {
      if (ip == null)
        throw new ArgumentNullException ("ip");

      _ip = ip;

      //Direct initialization
      Initialize(appKey);
    }


    /// <summary>
    /// Initialize client with your app key
    /// </summary>
    /// <param name="appKey"></param>
    public void Initialize(string appKey)
    {
      if (appKey == null)
        throw new ArgumentNullException ("appKey");

      _appKey = appKey;

      IsInitialized = true;
    }

    /// <summary>
    /// Check if the HueClient is initialized
    /// </summary>
    private void CheckInitialized()
    {
      if (!IsInitialized)
        throw new InvalidOperationException ("HueClient is not initialized. First call RegisterAsync or Initialize.");
    }

    /// <summary>
    /// Register your <paramref name="appName"/> and <paramref name="appKey"/> at the Hue Bridge.
    /// </summary>
    /// <param name="appKey">Secret key for your app. Must be at least 10 characters.</param>
    /// <param name="appName">The name of your app or device.</param>
    /// <returns><c>true</c> if success, <c>false</c> if the link button hasn't been pressed.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="appName"/> or <paramref name="appKey"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="appName"/> or <paramref name="appKey"/> aren't long enough, are empty or contains spaces.</exception>
    public async Task<bool> RegisterAsync(string appName, string appKey)
    {
      if (appName == null)
        throw new ArgumentNullException ("appName");
      if (appName.Trim() == String.Empty)
        throw new ArgumentException("appName must not be empty", "appName");
      if (appKey == null)
        throw new ArgumentNullException ("appKey");
      if (appKey.Length < 10 || appKey.Trim() == String.Empty)
        throw new ArgumentException("appKey must be at least 10 characters.", "appKey");
      if (appKey.Contains (" "))
        throw new ArgumentException("Cannot contain spaces.", "appName");

      JObject obj = new JObject();
      obj["username"] = appKey;
      obj["devicetype"] = appName;

      HttpClient client = new HttpClient();
      var response = await client.PostAsync(new Uri(string.Format("http://{0}/api", _ip)), new StringContent(obj.ToString())).ConfigureAwait(false);
      var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      JArray jresponse = JArray.Parse (stringResponse);
      JObject result = (JObject)jresponse.First;

      JToken error;
      if (result.TryGetValue ("error", out error))
      {
        if (error["type"].Value<int>() == 101) // link button not pressed
          return false;
        else
          throw new Exception (error["description"].Value<string>());
      }

      Initialize (result["success"]["username"].Value<string>());
      return true;
    }


    /// <summary>
    /// Set the next Hue color
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    public Task SetNextHueColorAsync(IEnumerable<string> lampList = null)
    {
      //Invalid JSON, but it works
      string command = "{\"hue\":+10000,\"sat\":255}";

      return SendCommandRawAsync(command, lampList);

    }

    /// <summary>
    /// Asynchronously retrieves an individual light.
    /// </summary>
    /// <param name="id">The light's Id.</param>
    /// <returns>The <see cref="Light"/> if found, <c>null</c> if not.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="id"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="id"/> is empty or a blank string.</exception>
    public async Task<Light> GetLightAsync (string id)
    {
      if (id == null)
        throw new ArgumentNullException ("id");
      if (id.Trim() == String.Empty)
        throw new ArgumentException ("id can not be empty or a blank string", "id");

      CheckInitialized();

      HttpClient client = new HttpClient();
      string stringResult = await client.GetStringAsync (new Uri (String.Format ("{0}lights/{1}", ApiBase, id))).ConfigureAwait (false);

      JToken token = JToken.Parse (stringResult);
      if (token.Type == JTokenType.Array)
      {
        // Hue gives back errors in an array for this request
        JObject error = (JObject)token.First["error"];
        if (error["type"].Value<int>() == 3) // Light not found
          return null;

        throw new Exception (error["description"].Value<string>());
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

      Bridge bridge = await GetBridgeAsync().ConfigureAwait (false);
      return bridge.Lights;
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

      BridgeBridge jsonResult = JsonConvert.DeserializeObject<BridgeBridge>(stringResult);

      return new Bridge(jsonResult);
    }


    /// <summary>
    /// Send a LampCommand to a list of lamps
    /// </summary>
    /// <param name="command"></param>
    /// <param name="lampList">if null, send command to all lamps</param>
    /// <returns></returns>
    public Task SendCommandAsync(LampCommand command, IEnumerable<string> lampList = null)
    {
      if (command == null)
        throw new ArgumentNullException ("command");

      string jsonCommand = JsonConvert.SerializeObject(command, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      return SendCommandRawAsync(jsonCommand, lampList);
    }

    /// <summary>
    /// Send a json command to a list of lamps
    /// </summary>
    /// <param name="command"></param>
    /// <param name="lampList">if null, send command to all lamps</param>
    /// <returns></returns>
    public Task SendCommandRawAsync(string command, IEnumerable<string> lampList = null)
    {
      if (command == null)
        throw new ArgumentNullException ("command");

      CheckInitialized();

      string[] lamps = null;
      if (lampList != null)
        lamps = lampList.ToArray();

      if (lampList == null || lamps.Length == 0)
      {
        return SendGroupCommandAsync(command);
      }
      else if (lamps.Length == 1 || !_useGroups)
      {
        return lamps.ForEachAsync(_parallelRequests, async (lampId) =>
        {
          HttpClient client = new HttpClient();
          await client.PutAsync(new Uri(ApiBase + string.Format("lights/{0}/state", lampId)), new StringContent(command)).ConfigureAwait(false);

        });
      }
      else
      {
        //This does not always work
        //Most of the time it does not, that's why _useGroups = false by default, so this code is not used yet.
        //Maybe when bridge firmware is updated
        return Task.Run(async () =>
        {
          //Create string of all the lamps 
          string lampString = string.Empty;
          lampString += "[";
          foreach (var lamp in lamps)
          {
            lampString += "\"" + lamp + "\",";
          }
          lampString = lampString.Substring(0, lampString.Length - 1) + "]";

          HttpClient client = new HttpClient();
          //Delete group 1
          await client.DeleteAsync(new Uri(ApiBase + "groups/1")).ConfigureAwait(false);

          //Create group (will be group 1) with the lamps we want to target
          await client.PostAsync(new Uri(ApiBase + "groups"), new StringContent("{\"lights\":" + lampString + "}")).ConfigureAwait(false);

          //Send command to group 1
          //await client.PutAsync(new Uri(ApiBase + "groups/1/action"), new StringContent(command));
          await SendGroupCommandAsync(command, 1).ConfigureAwait(false);
        });

      }
    }

    /// <summary>
    /// Send command to a group
    /// </summary>
    /// <param name="command"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    public Task SendGroupCommandAsync(LampCommand command, int group = 0)
    {
      if (command == null)
        throw new ArgumentNullException ("command");

      string jsonCommand = JsonConvert.SerializeObject(command, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      return SendGroupCommandAsync(jsonCommand, group);
    }

    /// <summary>
    /// Send command to a group
    /// </summary>
    /// <param name="command"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    private Task SendGroupCommandAsync(string command, int group = 0)
    {
      if (command == null)
        throw new ArgumentNullException ("command");

      CheckInitialized();

      HttpClient client = new HttpClient();
      return client.PutAsync(new Uri(ApiBase + string.Format("groups/{0}/action", group)), new StringContent(command));
    }

    
  }
}
