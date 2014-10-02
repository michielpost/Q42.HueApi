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

    public async Task<string> CreateSensorAsync(Sensor sensor)
    {
      CheckInitialized();

      string sensorJson = JsonConvert.SerializeObject(sensor, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      HttpClient client = new HttpClient();

      //Create schedule
      var result = await client.PostAsync(new Uri(String.Format("{0}sensors", ApiBase)), new StringContent(sensorJson)).ConfigureAwait(false);

      var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      DefaultHueResult[] sensorResult = JsonConvert.DeserializeObject<DefaultHueResult[]>(jsonResult);

      if (sensorResult.Length > 0 && sensorResult[0].Success != null && !string.IsNullOrEmpty(sensorResult[0].Success.Id))
      {
        return sensorResult[0].Success.Id;
      }

      return null;
    }

    /// <summary>
    /// Starts a search for new sensors.
    /// </summary>
    /// <returns></returns>
    public async Task<HueResults> FindNewSensorsAsync()
    {
      CheckInitialized();

      HttpClient client = new HttpClient();
      var response = await client.PostAsync(new Uri(String.Format("{0}sensors", ApiBase)), null).ConfigureAwait(false);

      var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }


    /// <summary>
    /// Gets a list of sensors that were discovered the last time a search for new sensors was performed. The list of new sensors is always deleted when a new search is started.
    /// </summary>
    /// <returns></returns>
    public async Task<List<Sensor>> GetNewSensorsAsync()
    {
      CheckInitialized();

      HttpClient client = new HttpClient();
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}sensors/new", ApiBase))).ConfigureAwait(false);

#if DEBUG
      //stringResult = "{\"7\": {\"name\": \"Hue Lamp 7\"},   \"8\": {\"name\": \"Hue Lamp 8\"},    \"lastscan\": \"2012-10-29T12:00:00\"}";
#endif

      List<Sensor> results = new List<Sensor>();

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Object)
      {
        //Each property is a light
        var jsonResult = (JObject)token;

        foreach (var prop in jsonResult.Properties())
        {
          if (prop.Name != "lastscan")
          {
            Sensor newSensor = new Sensor();
            newSensor.Id = prop.Name;
            newSensor.Name = prop.First["name"].ToString();

            results.Add(newSensor);

          }
        }

      }

      return results;

    }


    /// <summary>
    /// Asynchronously gets single sensor
    /// </summary>
    /// <returns><see cref="Sensor"/></returns>
    public async Task<Sensor> GetSensorAsync(string id)
    {
      if (id == null)
        throw new ArgumentNullException("id");
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id can not be empty or a blank string", "id");

      CheckInitialized();

      HttpClient client = new HttpClient();
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}sensors/{1}", ApiBase, id))).ConfigureAwait(false);

#if DEBUG
      stringResult = "{\"state\":{         \"buttonevent\": 34,         \"lastupdated\":\"2013-03-25T13:32:34\", },\"name\": \"Wall tap 1\", \"modelid\":\"ZGPSWITCH\",  \"uniqueid\":\"01:23:45:67:89:AB-12\",\"manufacturername\": \"Philips\",\"swversion\":\"1.0\", \"type\":  \"ZGPSwitch\"}";
#endif

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Array)
      {
        // Hue gives back errors in an array for this request
        JObject error = (JObject)token.First["error"];
        if (error["type"].Value<int>() == 3) // Rule not found
          return null;

        throw new Exception(error["description"].Value<string>());
      }

      var sensor = token.ToObject<Sensor>();
      sensor.Id = id;

      return sensor;
    }

    /// <summary>
    /// Update a sensor
    /// </summary>
    /// <param name="id"></param>
    /// <param name="newName"></param>
    /// <returns></returns>
    public async Task<HueResults> UpdateSensorAsync(string id, string newName)
    {
      CheckInitialized();

      if (id == null)
        throw new ArgumentNullException("id");
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id must not be empty", "id");
      if (string.IsNullOrEmpty(newName))
        throw new ArgumentNullException("newName");

      dynamic jsonObj = new ExpandoObject();
      jsonObj.name = newName;

      string jsonString = JsonConvert.SerializeObject(jsonObj);

      HttpClient client = new HttpClient();

      //Create schedule
      var result = await client.PutAsync(new Uri(string.Format("{0}sensors/{1}", ApiBase, id)), new StringContent(jsonString)).ConfigureAwait(false);

      var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }

    /// <summary>
    /// Changes the Sensor configuration
    /// </summary>
    /// <param name="id"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public async Task<HueResults> ChangeSensorConfigAsync(string id, SensorConfig config)
    {
      CheckInitialized();

      if (id == null)
        throw new ArgumentNullException("id");
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id must not be empty", "id");
      if (config == null)
        throw new ArgumentNullException("config");

      string jsonString = JsonConvert.SerializeObject(config);

      HttpClient client = new HttpClient();

      //Create schedule
      var result = await client.PutAsync(new Uri(string.Format("{0}sensors/{1}/config", ApiBase, id)), new StringContent(jsonString)).ConfigureAwait(false);

      var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }

    public async Task<HueResults> ChangeSensorStateAsync(string id, SensorState state)
    {
      CheckInitialized();

      if (id == null)
        throw new ArgumentNullException("id");
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id must not be empty", "id");
      if (state == null)
        throw new ArgumentNullException("state");

      string jsonString = JsonConvert.SerializeObject(state);

      HttpClient client = new HttpClient();

      //Create schedule
      var result = await client.PutAsync(new Uri(string.Format("{0}sensors/{1}/state", ApiBase, id)), new StringContent(jsonString)).ConfigureAwait(false);

      var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }
  }
}
