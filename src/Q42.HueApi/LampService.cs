using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Q42.HueApi.Extensions;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http;

namespace Q42.HueApi
{
  /// <summary>
  /// Responsible for communicating with the lamps
  /// </summary>
  public class LampService : ILampService
  {

    private readonly string _ip;
    private readonly string _appKey;

    private readonly int _parallelRequests = 5;

    private readonly bool _useGroups = false;

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
    /// Initialize with Bridge IP and AppKey
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="appKey"></param>
    public LampService(string ip, string appKey)
    {
      _ip = ip;
      _appKey = appKey;
    }


    /// <summary>
    /// Turn lamps on (all if lamplist is null)
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    public Task SetOn(IEnumerable<string> lampList = null)
    {
      LampCommand command = new LampCommand();
      command.on = true;

      return SendCommand(command, lampList);
    }

    /// <summary>
    /// Turn lamps off (all if lamplist is null)
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    public Task SetOff(IEnumerable<string> lampList = null)
    {
      LampCommand command = new LampCommand();
      command.on = false;
      command.effect = Effects.none;

      return SendCommand(command, lampList);
    }



    /// <summary>
    /// Send alert
    /// </summary>
    /// <param name="lampList"></param>
    /// <param name="alert"></param>
    /// <returns></returns>
    public Task SendAlert(string alert, IEnumerable<string> lampList = null)
    {
      //string command = "{\"alert\":\"" + alert + "\"}";

      LampCommand command = new LampCommand();
      command.alert = Alerts.lselect;

      return SendCommand(command, lampList);
    }


    /// <summary>
    /// Get all lamps registered at the bridge
    /// </summary>
    /// <returns></returns>
    public async Task<List<Lamp>> GetLamps()
    {
      HttpClient client = new HttpClient();
      var stringResult = await client.GetStringAsync(new Uri(ApiBase + "lights"));

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
    public async Task<Bridge> GetBridge()
    {
      HttpClient client = new HttpClient();
      var stringResult = await client.GetStringAsync(new Uri(ApiBase));

      BridgeBridge jsonResult = JsonConvert.DeserializeObject<BridgeBridge>(stringResult);

      return new Bridge(jsonResult);
    }


    /// <summary>
    /// Send a LampCommand to a list of lamps
    /// </summary>
    /// <param name="command"></param>
    /// <param name="lampList">if null, send command to all lamps</param>
    /// <returns></returns>
    public Task SendCommand(LampCommand command, IEnumerable<string> lampList = null)
    {
      string jsonCommand = JsonConvert.SerializeObject(command, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      return SendCommand(jsonCommand, lampList);
    }

    /// <summary>
    /// Send a json command to a list of lamps
    /// </summary>
    /// <param name="command"></param>
    /// <param name="lampList">if null, send command to all lamps</param>
    /// <returns></returns>
    public Task SendCommand(string command, IEnumerable<string> lampList = null)
    {
      if (lampList == null || lampList.Count() == 0)
      {
        return Task.Run(async () =>
        {
          HttpClient client = new HttpClient();
          await client.PutAsync(new Uri(ApiBase + "groups/0/action"), new StringContent(command));

        });
      }
      else if (lampList.Count() == 1 || !_useGroups)
      {
        return lampList.ForEachAsync(_parallelRequests, async (lampId) =>
        {
          HttpClient client = new HttpClient();
          await client.PutAsync(new Uri(ApiBase + string.Format("lights/{0}/state", lampId)), new StringContent(command));

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
          await client.DeleteAsync(new Uri(ApiBase + "groups/1"));

          //Create group (will be group 1) with the lamps we want to target
          await client.PostAsync(new Uri(ApiBase + "groups"), new StringContent("{\"lights\":" + lampString + "}"));

          //Send command to group 1
          await client.PutAsync(new Uri(ApiBase + "groups/1/action"), new StringContent(command));
        });

      }
    }


    
  }
}
