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
using Q42.HueApi.Models;
using Newtonsoft.Json.Serialization;

namespace Q42.HueApi
{
  /// <summary>
  /// Responsible for communicating with the bridge
  /// </summary>
  public partial class HueClient : IHueClient
  {

    private readonly string _ip;
    private readonly int _parallelRequests = 5;

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

	  CheckValidIp(ip);

      _ip = ip;
    }

	/// <summary>
	/// Check if the provided IP is valid by using it in an URI to the Hue Bridge
	/// </summary>
	/// <param name="ip"></param>
	private void CheckValidIp(string ip)
	{
		Uri uri;
		if (!Uri.TryCreate(string.Format("http://{0}/description.xml", ip), UriKind.Absolute, out uri))
		{
			//Invalid ip or hostname caused Uri creation to fail
			throw new Exception(string.Format("The supplied ip to the HueClient is not a valid ip: {0}", ip));
		}
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

	  CheckValidIp(ip);


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

    [ThreadStatic]
    private static HttpClient _httpClient;
    public static HttpClient GetHttpClient()
    {
      // return per-thread HttpClient
      if (_httpClient == null)
        _httpClient = new HttpClient();
      return _httpClient;
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
    /// Deserialization helper that can also check for errors
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    private static T DeserializeResult<T>(string json) where T : class
    {
      try
      {
        T objResult = JsonConvert.DeserializeObject<T>(json);

        return objResult;

      }
      catch (Exception ex)
      {
        var defaultResult = DeserializeDefaultHueResult(json);

        //We expect an actual object, it was unsuccesful, show error why
        if (defaultResult.HasErrors())
          throw new Exception(defaultResult.Errors.First().Error.Description);
      }

      return null;
    }


    /// <summary>
    /// Checks if the json contains errors
    /// </summary>
    /// <param name="json"></param>
    private static HueResults DeserializeDefaultHueResult(string json)
    {
      HueResults result = null;

      try
      {
        result = JsonConvert.DeserializeObject<HueResults>(json);
      }
      catch (JsonSerializationException ex)
      {
        //Ignore JsonSerializationException
      }

      return result;

    }

    /// <summary>
    /// Checks if the json contains errors
    /// </summary>
    /// <param name="json"></param>
    private static IReadOnlyCollection<T> DeserializeDefaultHueResult<T>(string json)
    {
      List<T> result = null;

      try
      {
        result = JsonConvert.DeserializeObject<List<T>>(json);
      }
      catch (JsonSerializationException ex)
      {
        //Ignore JsonSerializationException
      }

      return result;

    }

  }
}
