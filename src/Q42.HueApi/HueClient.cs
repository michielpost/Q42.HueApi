using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
    public bool IsInitialized { get; set; }


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
        throw new Exception("HueClient not Initialized. First call Register or Initialize.");
    }

    /// <summary>
    /// Register your appName at the Hue Bridge
    /// </summary>
    /// <param name="appKey">Secret key for your app. Must be at least 10 characters.</param>
    /// <returns>Exception if the link button is not pressed. True if ok. False for other errors</returns>
    public async Task<bool> RegisterAsync(string appKey)
    {
      if (appKey == null)
        throw new ArgumentNullException ("appKey");
      if (appKey.Length < 10 || appKey.Trim() == String.Empty)
        throw new ArgumentException("Must be at least 10 characters.", "appName");

      if (appKey.Contains(" "))
        throw new ArgumentException("Cannot contain spaces.", "appName");


      var jsonRequest = "{\"username\": \"" + appKey + "\", \"devicetype\":\"" + appKey + "\"}";

      HttpClient client = new HttpClient();
      var response = await client.PostAsync(new Uri(string.Format("http://{0}/api", _ip)), new StringContent(jsonRequest)).ConfigureAwait(false);
      var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      if (stringResponse.Contains("link button not pressed"))
      {
        throw new Exception("Press the link button");
      }
      else if (response.IsSuccessStatusCode && stringResponse.Contains(appKey))
      {
        Initialize(appKey);

        return true;
      }

      return false;

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
    /// Get all lamps registered at the bridge
    /// </summary>
    /// <returns></returns>
    public async Task<List<Lamp>> GetLampsAsync()
    {
      CheckInitialized();

      HttpClient client = new HttpClient();
      var stringResult = await client.GetStringAsync(new Uri(ApiBase + "lights")).ConfigureAwait(false);

      Dictionary<string, BridgeLamp> jsonResult = JsonConvert.DeserializeObject<Dictionary<string, BridgeLamp>>(stringResult);

      List<Lamp> lampList = new List<Lamp>();

      foreach (var dic in jsonResult)
      {
        Lamp newLamp = new Lamp();
        newLamp.Id = dic.Key;
        newLamp.Name = dic.Value.Name;

        lampList.Add(newLamp);
      }

      return lampList;
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

      if (lampList == null || lampList.Count() == 0)
      {
        return SendGroupCommandAsync(command);
      }
      else if (lampList.Count() == 1 || !_useGroups)
      {
        return lampList.ForEachAsync(_parallelRequests, async (lampId) =>
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
          foreach (var lamp in lampList)
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
