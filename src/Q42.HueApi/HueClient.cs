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
using Q42.HueApi.Models.Groups;

namespace Q42.HueApi
{
  /// <summary>
  /// Responsible for communicating with the bridge
  /// </summary>
  public partial class HueClient : IHueClient
  {

    private readonly string _ip;
    private readonly int _parallelRequests = 5;
    private readonly bool _useGroups = true;

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
        throw new ArgumentNullException("ip");

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
        throw new ArgumentNullException("ip");

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
        throw new ArgumentNullException("appKey");

      _appKey = appKey;

      IsInitialized = true;
    }

    /// <summary>
    /// Check if the HueClient is initialized
    /// </summary>
    private void CheckInitialized()
    {
      if (!IsInitialized)
        throw new InvalidOperationException("HueClient is not initialized. First call RegisterAsync or Initialize.");
    }


    /// <summary>
    /// Create string of all the lights
    /// </summary>
    /// <param name="lights"></param>
    /// <returns></returns>
    private static string CreateLightList(IEnumerable<string> lights)
    {
      //TODO: Can this be replaced by json.net serializer?

      string lightString = string.Empty;
      lightString += "[";
      foreach (var light in lights)
      {
        lightString += "\"" + light + "\",";
      }
      lightString = lightString.Substring(0, lightString.Length - 1) + "]";
      return lightString;
    }

  }
}
