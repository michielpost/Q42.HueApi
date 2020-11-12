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
  public partial class LocalHueClient : HueClient, ILocalHueClient
  {

    private readonly string _ip;

    /// <summary>
    /// Client Key for streaming api
    /// </summary>
    protected string _clientKey;

    public bool IsStreamingInitialized { get; protected set; }

    public bool UseHttps { get; set; }


    /// <summary>
    /// Base URL for the API
    /// </summary>
    protected override string ApiBase
    {
      get
      {
        if (!string.IsNullOrWhiteSpace(_appKey))
          return $"{Scheme}://{_ip}/api/{_appKey}/";
        else
          return $"{Scheme}://{_ip}/api/";
      }
    }

    private string Scheme
    {
      get
      {
        if (UseHttps)
          return "https";
        else
          return "http";
      }
    }

    /// <summary>
    /// Initialize with Bridge IP
    /// </summary>
    /// <param name="ip"></param>
    public LocalHueClient(string ip)
    {
      if (ip == null)
        throw new ArgumentNullException(nameof(ip));

      CheckValidIp(ip);

      _ip = ip;
    }

    /// <summary>
    /// Constructor which allows you to provide your own HttpClient
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="httpClient"></param>
    public LocalHueClient(string ip, HttpClient httpClient)
      : base(httpClient)
    {
      if (ip == null)
        throw new ArgumentNullException(nameof(ip));

      CheckValidIp(ip);

      _ip = ip;
    }

    /// <summary>
    /// Check if the provided IP is valid by using it in an URI to the Hue Bridge
    /// </summary>
    /// <param name="ip"></param>
    private void CheckValidIp(string ip)
    {
      Uri? uri;
      if (!Uri.TryCreate($"{Scheme}://{ip}/description.xml", UriKind.Absolute, out uri))
      {
        //Invalid ip or hostname caused Uri creation to fail
        throw new HueException(string.Format("The supplied ip to the HueClient is not a valid ip: {0}", ip));
      }
    }

    /// <summary>
    /// Initialize with Bridge IP and AppKey
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="appKey"></param>
    public LocalHueClient(string ip, string appKey)
    {
      if (ip == null)
        throw new ArgumentNullException(nameof(ip));

      CheckValidIp(ip);


      _ip = ip;

      //Direct initialization
      Initialize(appKey);
    }

    public LocalHueClient(string ip, string appKey, string clientKey)
      : this(ip, appKey)
    {
      InitializeStreaming(clientKey);
    }

  }
}
