using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Q42.HueApi.Extensions;
using Newtonsoft.Json;
using Q42.HueApi.Models.Groups;
using Q42.HueApi.Models;
using System.Dynamic;

namespace Q42.HueApi
{
  /// <summary>
  /// Partial HueClient, contains requests to the /scenes/ url
  /// </summary>
  public partial class HueClient
  {


    /// <summary>
    /// Asynchronously gets all sensors registered with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="Sensor"/>s registered with the bridge.</returns>
    public async Task<IEnumerable<Sensor>> GetSensorsAsync()
    {
      CheckInitialized();

      HttpClient client = new HttpClient();
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}sensors", ApiBase))).ConfigureAwait(false);

#if DEBUG
      stringResult = "{    \"1\": {        \"state\": {            \"daylight\": false,            \"lastupdated\": \"2014-06-27T07:38:51\"        },        \"config\": {            \"on\": true,            \"long\": \"none\",            \"lat\": \"none\",            \"sunriseoffset\": 50,            \"sunsetoffset\": 50        },        \"name\": \"Daylight\",        \"type\": \"Daylight\",        \"modelid\": \"PHDL00\",        \"manufacturername\": \"Philips\",        \"swversion\": \"1.0\"    },    \"2\": {        \"state\": {            \"buttonevent\": 0,            \"lastupdated\": \"none\"        },        \"config\": {            \"on\": true        },        \"name\": \"Tap Switch 2\",        \"type\": \"ZGPSwitch\",        \"modelid\": \"ZGPSWITCH\",        \"manufacturername\": \"Philips\",        \"uniqueid\": \"00:00:00:00:00:40:03:50-f2\"    }}";
#endif


      List<Sensor> results = new List<Sensor>();

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Object)
      {
        //Each property is a scene
        var jsonResult = (JObject)token;

        foreach (var prop in jsonResult.Properties())
        {
          Sensor scene = JsonConvert.DeserializeObject<Sensor>(prop.Value.ToString());
          scene.Id = prop.Name;
          
          results.Add(scene);
        }

      }

      return results;

    }

   
  }
}
