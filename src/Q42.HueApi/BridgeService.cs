using Newtonsoft.Json;
using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
  /// <summary>
  /// Responsible for registering app with the bridge
  /// </summary>
  public class BridgeService : IBridgeService
  {

    private readonly string _ip;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="ip"></param>
    public BridgeService(string ip)
    {
      _ip = ip;

    }


    /// <summary>
    /// Register your appName at the Hue Bridge
    /// </summary>
    /// <param name="appName">Secret key for your app. Must be at least 10 characters.</param>
    /// <returns>Exception if the link button is not pressed. True if ok. False for other errors</returns>
    public async Task<bool> Register(string appName)
    {

      if (appName.Length < 10)
        throw new ArgumentException("Must be at least 10 characters.", "appName");

      if(appName.Contains(" "))
        throw new ArgumentException("Cannot contain spaces.", "appName");


      var jsonRequest = "{\"username\": \"" + appName + "\", \"devicetype\":\"" + appName + "\"}";

      HttpClient client = new HttpClient();
      var response = await client.PostAsync(new Uri(string.Format("http://{0}/api", _ip)), new StringContent(jsonRequest));
      var stringResponse = await response.Content.ReadAsStringAsync();

      if (stringResponse.Contains("link button not pressed"))
      {
        throw new Exception("Press the link button");
      }
      else if (response.IsSuccessStatusCode && stringResponse.Contains(appName))
      {
          return true;
      }

      return false;

    }

  }

}
