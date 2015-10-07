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

    private readonly int _parallelRequests = 5;

    /// <summary>
    /// Indicates the HueClient is initialized with an AppKey
    /// </summary>
    public bool IsInitialized { get; protected set; }

	protected HueClient()
	{

	}

	protected virtual string ApiBase { get; private set; }

    [ThreadStatic]
    protected static HttpClient _httpClient;
    
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
