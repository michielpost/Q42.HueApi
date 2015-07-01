using Newtonsoft.Json.Linq;
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
  public partial class LocalHueClient
  {
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
        throw new ArgumentNullException("appName");
      if (appName.Trim() == String.Empty)
        throw new ArgumentException("appName must not be empty", "appName");
      if (appName.Length > 40)
        throw new ArgumentException("appName max is 40 characters.", "appKey");

      if (appKey == null)
        throw new ArgumentNullException("appKey");
      if (appKey.Length < 10 || appKey.Trim() == String.Empty)
        throw new ArgumentException("appKey must be at least 10 characters.", "appKey");
      if (appKey.Length > 40)
        throw new ArgumentException("appKey max is 40 characters.", "appKey");
      if (appKey.Contains(" "))
        throw new ArgumentException("Cannot contain spaces.", "appName");

      JObject obj = new JObject();
      obj["username"] = appKey;
      obj["devicetype"] = appName;

      HttpClient client = HueClient.GetHttpClient();
      var response = await client.PostAsync(new Uri(string.Format("http://{0}/api", _ip)), new StringContent(obj.ToString())).ConfigureAwait(false);
      var stringResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      JObject result;
      try
      {
        JArray jresponse = JArray.Parse(stringResponse);
        result = (JObject)jresponse.First;
      }
      catch
      {
        //Not an expected response. Return response as exception
        throw new Exception(stringResponse);
      }

      JToken error;
      if (result.TryGetValue("error", out error))
      {
        if (error["type"].Value<int>() == 101) // link button not pressed
          return false;
        else
          throw new Exception(error["description"].Value<string>());
      }

      Initialize(result["success"]["username"].Value<string>());
      return true;
    }

    public async Task<bool> CheckConnection()
    {
      HttpClient client = HueClient.GetHttpClient();

      try
      {
        //Check if there is a hue bridge on the specified IP by checking the content of description.xml
        var result = await client.GetAsync(string.Format("http://{0}/description.xml", _ip)).ConfigureAwait(false);
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
      catch(Exception e)
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
