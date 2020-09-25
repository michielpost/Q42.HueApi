using Newtonsoft.Json.Linq;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using Q42.HueApi.Models.Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
  /// <summary>
  ///  Partial HueClient, contains requests to the /api/ url
  /// </summary>
  public partial class LocalHueClient : ILocalHueClient_Api
  {
    public async Task<string?> RegisterAsync(string applicationName, string deviceName)
    {
      var result = await RegisterAsync(applicationName, deviceName, false);
      return result?.Username;
    }


    /// <summary>
    /// Register your <paramref name="applicationName"/> and <paramref name="deviceName"/> at the Hue Bridge.
    /// </summary>
    /// <param name="applicationName">The name of your app.</param>
    /// <param name="deviceName">The name of the device.</param>
    /// <param name="generateClientKey">Set to true if you want a client key to use the streaming api</param>
    /// <returns>Secret key for the app to communicate with the bridge.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="applicationName"/> or <paramref name="deviceName"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="applicationName"/> or <paramref name="deviceName"/> aren't long enough, are empty or contains spaces.</exception>

    public async Task<RegisterEntertainmentResult?> RegisterAsync(string applicationName, string deviceName, bool generateClientKey)
    {
      var result = await RegisterAsync(_ip, applicationName, deviceName, generateClientKey);

      if (result != null)
      {
        Initialize(result.Username);

        if (!string.IsNullOrWhiteSpace(result.StreamingClientKey))
          InitializeStreaming(result.StreamingClientKey);
      }

      return result;
    }


    /// <summary>
    /// Register your <paramref name="applicationName"/> and <paramref name="deviceName"/> at the Hue Bridge.
    /// </summary>
    /// <param name="ip">ip address of bridge</param>
    /// <param name="applicationName">The name of your app.</param>
    /// <param name="deviceName">The name of the device.</param>
    /// <param name="generateClientKey">Set to true if you want a client key to use the streaming api</param>
    /// <returns>Secret key for the app to communicate with the bridge.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="applicationName"/> or <paramref name="deviceName"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="applicationName"/> or <paramref name="deviceName"/> aren't long enough, are empty or contains spaces.</exception>
    private async Task<RegisterEntertainmentResult?> RegisterAsync(string ip, string applicationName, string deviceName, bool generateClientKey)
    {
      if (applicationName == null)
        throw new ArgumentNullException(nameof(applicationName));
      if (applicationName.Trim() == String.Empty)
        throw new ArgumentException("applicationName must not be empty", nameof(applicationName));
      if (applicationName.Length > 20)
        throw new ArgumentException("applicationName max is 20 characters.", nameof(applicationName));
      if (applicationName.Contains(" "))
        throw new ArgumentException("Cannot contain spaces.", nameof(applicationName));

      if (deviceName == null)
        throw new ArgumentNullException(nameof(deviceName));
      if (deviceName.Length < 0 || deviceName.Trim() == String.Empty)
        throw new ArgumentException("deviceName must be at least 0 characters.", nameof(deviceName));
      if (deviceName.Length > 19)
        throw new ArgumentException("deviceName max is 19 characters.", nameof(deviceName));
      if (deviceName.Contains(" "))
        throw new ArgumentException("Cannot contain spaces.", nameof(deviceName));

      string fullName = string.Format("{0}#{1}", applicationName, deviceName);

      JObject obj = new JObject();
      obj["devicetype"] = fullName;

      if (generateClientKey)
        obj["generateclientkey"] = true;

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var response = await client.PostAsync(new Uri($"{Scheme}://{ip}/api"), new JsonContent(obj.ToString())).ConfigureAwait(false);
      var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      JObject? result;
      try
      {
        JArray jresponse = JArray.Parse(stringResponse);
        result = (JObject?)jresponse.First;
      }
      catch
      {
        //Not an expected response. Return response as exception
        throw new HueException(stringResponse);
      }

      if (result != null)
      {
        JToken? error;
        if (result.TryGetValue("error", out error))
        {
          if (error["type"]?.Value<int>() == 101) // link button not pressed
            throw new LinkButtonNotPressedException("Link button not pressed");
          else
            throw new HueException(error["description"]?.Value<string>());
        }

        var username = result["success"]?["username"]?.Value<string>();
        var streamingClientKey = result["success"]?["clientkey"]?.Value<string>();

        if (username != null)
        {
          return new RegisterEntertainmentResult()
          {
            Ip = ip,
            Username = username,
            StreamingClientKey = streamingClientKey
          };
        }
      }

      return null;
    }

    public async Task<bool> CheckConnection()
    {
      HttpClient client = await GetHttpClient().ConfigureAwait(false);

      try
      {
        //Check if there is a hue bridge on the specified IP by checking the content of description.xml
        var result = await client.GetAsync($"{Scheme}://{_ip}/description.xml").ConfigureAwait(false);
        if (result.IsSuccessStatusCode)
        {
          string res = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
          if (!string.IsNullOrWhiteSpace(res))
          {
            if (!res.ToLower().Contains("philips hue bridge"))
              return false;
          }
        }
        else
        {
          return false;
        }
      }
      catch (Exception)
      {
        return false;
      }

      try
      {
        //Check if app is registered
        var test = await this.GetBridgeAsync().ConfigureAwait(false);
      }
      catch
      {
        return false;
      }

      return true;
    }
  }
}
