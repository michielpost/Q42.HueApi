using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
  public partial class RemoteHueClient : HueClient, IRemoteHueClient
  {
    private readonly string _apiBase = "https://api.meethue.com/v2/bridges/";
    private static Func<Task<string>> _getAccessToken;
    private string _bridgeId;


    public RemoteHueClient(Func<Task<string>> getAccessToken)
    {
      _getAccessToken = getAccessToken;
    }

    /// <summary>
    /// Initialize client with your app key
    /// </summary>
    /// <param name="appKey"></param>
    public void Initialize(string bridgeId, string appKey)
    {
      if (bridgeId == null)
        throw new ArgumentNullException(nameof(bridgeId));

      _bridgeId = bridgeId;

      Initialize(appKey);
    }

    /// <summary>
    /// Get bridge info
    /// </summary>
    /// <returns></returns>
    public async Task<List<RemoteBridge>?> GetBridgesAsync()
    {
      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var stringResult = await client.GetStringAsync(new Uri(_apiBase)).ConfigureAwait(false);

      var bridges = DeserializeResult<List<RemoteBridge>>(stringResult);

      return bridges;
    }


    public new async Task<HttpClient> GetHttpClient()
    {
      // return per-thread HttpClient
      if (_httpClient == null)
      {
        _httpClient = new HttpClient();

      }

      if (_getAccessToken != null)
      {
        var remoteAccessToken = await _getAccessToken().ConfigureAwait(false);
        if (!string.IsNullOrEmpty(remoteAccessToken))
          _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", remoteAccessToken);
      }

      return _httpClient;
    }


    /// <summary>
    /// Base URL for the API
    /// </summary>
    protected override string ApiBase
    {
      get
      {
        return $"{_apiBase}{_bridgeId}/{_appKey}/";
      }
    }
  }
}
