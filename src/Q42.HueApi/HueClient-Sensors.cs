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
using Q42.HueApi.Interfaces;

namespace Q42.HueApi
{
	/// <summary>
	/// Partial HueClient, contains requests to the /scenes/ url
	/// </summary>
	public partial class HueClient : IHueClient_Sensors
  {
		/// <summary>
		/// Asynchronously gets all sensors registered with the bridge.
		/// </summary>
		/// <returns>An enumerable of <see cref="Sensor"/>s registered with the bridge.</returns>
		public async Task<IReadOnlyCollection<Sensor>> GetSensorsAsync()
		{
			CheckInitialized();

			HttpClient client = await GetHttpClient().ConfigureAwait(false);
			string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}sensors", ApiBase))).ConfigureAwait(false);

//#if DEBUG
//			stringResult = "{    \"1\": {        \"state\": {            \"daylight\": false,            \"lastupdated\": \"2014-06-27T07:38:51\"        },        \"config\": {            \"on\": true,            \"long\": \"none\",            \"lat\": \"none\",            \"sunriseoffset\": 50,            \"sunsetoffset\": 50        },        \"name\": \"Daylight\",        \"type\": \"Daylight\",        \"modelid\": \"PHDL00\",        \"manufacturername\": \"Philips\",        \"swversion\": \"1.0\"    },    \"2\": {        \"state\": {            \"buttonevent\": 0,            \"lastupdated\": \"none\"        },        \"config\": {            \"on\": true        },        \"name\": \"Tap Switch 2\",        \"type\": \"ZGPSwitch\",        \"modelid\": \"ZGPSWITCH\",        \"manufacturername\": \"Philips\",        \"uniqueid\": \"00:00:00:00:00:40:03:50-f2\"    }}";
//#endif


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

		public async Task<string?> CreateSensorAsync(Sensor sensor)
		{
			if (sensor == null)
				throw new ArgumentNullException(nameof(sensor));

			CheckInitialized();

			//Set fields to null
			sensor.Id = null!; 

			string sensorJson = JsonConvert.SerializeObject(sensor, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

			HttpClient client = await GetHttpClient().ConfigureAwait(false);

			//Create sensor
			var result = await client.PostAsync(new Uri(String.Format("{0}sensors", ApiBase)), new JsonContent(sensorJson)).ConfigureAwait(false);

			var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

			DefaultHueResult[] sensorResult = JsonConvert.DeserializeObject<DefaultHueResult[]>(jsonResult);

			if (sensorResult.Length > 0 && sensorResult[0].Success != null && !string.IsNullOrEmpty(sensorResult[0].Success.Id))
			{
				var id = sensorResult[0].Success.Id;
				sensor.Id = id;
				return id;
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

			HttpClient client = await GetHttpClient().ConfigureAwait(false);
			var response = await client.PostAsync(new Uri(String.Format("{0}sensors", ApiBase)), new StringContent(string.Empty)).ConfigureAwait(false);

			var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			return DeserializeDefaultHueResult(jsonResult);

		}


		/// <summary>
		/// Gets a list of sensors that were discovered the last time a search for new sensors was performed. The list of new sensors is always deleted when a new search is started.
		/// </summary>
		/// <returns></returns>
		public async Task<IReadOnlyCollection<Sensor>> GetNewSensorsAsync()
		{
			CheckInitialized();

			HttpClient client = await GetHttpClient().ConfigureAwait(false);
			string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}sensors/new", ApiBase))).ConfigureAwait(false);

//#if DEBUG
//			//stringResult = "{\"7\": {\"name\": \"Hue Lamp 7\"},   \"8\": {\"name\": \"Hue Lamp 8\"},    \"lastscan\": \"2012-10-29T12:00:00\"}";
//#endif

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
						Sensor newSensor = JsonConvert.DeserializeObject<Sensor>(prop.Value.ToString());
						newSensor.Id = prop.Name;

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
		public async Task<Sensor?> GetSensorAsync(string id)
		{
			if (id == null)
				throw new ArgumentNullException(nameof(id));
			if (id.Trim() == String.Empty)
				throw new ArgumentException("id can not be empty or a blank string", nameof(id));

			CheckInitialized();

			HttpClient client = await GetHttpClient().ConfigureAwait(false);
			string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}sensors/{1}", ApiBase, id))).ConfigureAwait(false);

//#if DEBUG
//			stringResult = "{\"state\":{         \"buttonevent\": 34,         \"lastupdated\":\"2013-03-25T13:32:34\", },\"name\": \"Wall tap 1\", \"modelid\":\"ZGPSWITCH\",  \"uniqueid\":\"01:23:45:67:89:AB-12\",\"manufacturername\": \"Philips\",\"swversion\":\"1.0\", \"type\":  \"ZGPSwitch\"}";
//#endif

			JToken token = JToken.Parse(stringResult);
			if (token.Type == JTokenType.Array)
			{
				// Hue gives back errors in an array for this request
				JToken? error = token.First?["error"];
        if (error != null)
        {
          if (error["type"]?.Value<int>() == 3) // Rule not found
            return null;

          throw new HueException(error["description"]?.Value<string>());
        }
			}

			var sensor = token.ToObject<Sensor>();
      if(sensor != null)
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
				throw new ArgumentNullException(nameof(id));
			if (id.Trim() == String.Empty)
				throw new ArgumentException("id must not be empty", nameof(id));
			if (string.IsNullOrEmpty(newName))
				throw new ArgumentNullException(nameof(newName));

      JObject jsonObj = new JObject();
      jsonObj.Add("name", newName);

			string jsonString = JsonConvert.SerializeObject(jsonObj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

			HttpClient client = await GetHttpClient().ConfigureAwait(false);

			//Update sensor
			var result = await client.PutAsync(new Uri(string.Format("{0}sensors/{1}", ApiBase, id)), new JsonContent(jsonString)).ConfigureAwait(false);

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
				throw new ArgumentNullException(nameof(id));
			if (id.Trim() == String.Empty)
				throw new ArgumentException("id must not be empty", nameof(id));
			if (config == null)
				throw new ArgumentNullException(nameof(config));

			var updateJson = JObject.FromObject(config, new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore });

			//Remove properties from json that are readonly
			updateJson.Remove("battery");
			updateJson.Remove("reachable");
			updateJson.Remove("configured");
			updateJson.Remove("pending");
			updateJson.Remove("sensitivitymax");

			string jsonString = JsonConvert.SerializeObject(updateJson, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

			HttpClient client = await GetHttpClient().ConfigureAwait(false);

			//Change sensor config
			var result = await client.PutAsync(new Uri(string.Format("{0}sensors/{1}/config", ApiBase, id)), new JsonContent(jsonString)).ConfigureAwait(false);

			var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

			return DeserializeDefaultHueResult(jsonResult);

		}

		public async Task<HueResults> ChangeSensorStateAsync(string id, SensorState state)
		{
			CheckInitialized();

			if (id == null)
				throw new ArgumentNullException(nameof(id));
			if (id.Trim() == String.Empty)
				throw new ArgumentException("id must not be empty", nameof(id));
			if (state == null)
				throw new ArgumentNullException(nameof(state));

			string jsonString = JsonConvert.SerializeObject(state, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

			HttpClient client = await GetHttpClient().ConfigureAwait(false);

			//Change sensor state
			var result = await client.PutAsync(new Uri(string.Format("{0}sensors/{1}/state", ApiBase, id)), new JsonContent(jsonString)).ConfigureAwait(false);

			var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

			return DeserializeDefaultHueResult(jsonResult);

		}

		/// <summary>
		/// Deletes a single sensor
		/// </summary>
		/// <param name="groupId"></param>
		/// <returns></returns>
		public async Task<IReadOnlyCollection<DeleteDefaultHueResult>> DeleteSensorAsync(string id)
		{
			CheckInitialized();

			if (id == null)
				throw new ArgumentNullException(nameof(id));
			if (id.Trim() == String.Empty)
				throw new ArgumentException("id must not be empty", nameof(id));

			HttpClient client = await GetHttpClient().ConfigureAwait(false);
			//Delete sensor
			var result = await client.DeleteAsync(new Uri(ApiBase + string.Format("sensors/{0}", id))).ConfigureAwait(false);

			string jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

//#if DEBUG
//			jsonResult = "[{\"success\":\"/sensors/" + id + " deleted\"}]";
//#endif

			return DeserializeDefaultHueResult<DeleteDefaultHueResult>(jsonResult);

		}
	}
}
