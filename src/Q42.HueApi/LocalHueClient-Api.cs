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
    /// Register your <paramref name="applicationName"/> and <paramref name="deviceName"/> at the Hue Bridge.
    /// </summary>
    /// <param name="applicationName">The name of your app.</param>
    /// <param name="deviceName">The name of the device.</param>
    /// <returns>Secret key for the app to communicate with the bridge.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="applicationName"/> or <paramref name="deviceName"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="applicationName"/> or <paramref name="deviceName"/> aren't long enough, are empty or contains spaces.</exception>
    public async Task<string> RegisterAsync(string applicationName, string deviceName)
    {
      if (applicationName == null)
        throw new ArgumentNullException("applicationName");
      if (applicationName.Trim() == String.Empty)
        throw new ArgumentException("applicationName must not be empty", "applicationName");
      if (applicationName.Length > 20)
        throw new ArgumentException("applicationName max is 20 characters.", "applicationName");
      if(applicationName.Contains(" "))
        throw new ArgumentException("Cannot contain spaces.", "applicationName");

      if (deviceName == null)
        throw new ArgumentNullException("deviceName");
      if (deviceName.Length < 0 || deviceName.Trim() == String.Empty)
        throw new ArgumentException("deviceName must be at least 0 characters.", "deviceName");
      if (deviceName.Length > 19)
        throw new ArgumentException("deviceName max is 19 characters.", "deviceName");
      if (deviceName.Contains(" "))
        throw new ArgumentException("Cannot contain spaces.", "deviceName");

      string fullName = string.Format("{0}#{1}", applicationName, deviceName);

      JObject obj = new JObject();
      obj["devicetype"] = fullName;

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
          throw new Exception("Link button not pressed");
        else
          throw new Exception(error["description"].Value<string>());
      }

      var key = result["success"]["username"].Value<string>();
      Initialize(key);

      return key;
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
