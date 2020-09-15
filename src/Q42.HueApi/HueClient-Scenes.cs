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
  public partial class HueClient : IHueClient_Scenes
  {


    /// <summary>
    /// Asynchronously gets all scenes registered with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="Scene"/>s registered with the bridge.</returns>
    public async Task<IReadOnlyCollection<Scene>> GetScenesAsync()
    {
      CheckInitialized();

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}scenes", ApiBase))).ConfigureAwait(false);

      List<Scene> results = new List<Scene>();

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Object)
      {
        //Each property is a scene
        var jsonResult = (JObject)token;

        foreach (var prop in jsonResult.Properties())
        {
          Scene scene = JsonConvert.DeserializeObject<Scene>(prop.Value.ToString());
          scene.Id = prop.Name;
          
          results.Add(scene);
        }

      }

      return results;

    }

		public async Task<string?> CreateSceneAsync(Scene scene)
		{
			CheckInitialized();

			if (scene == null)
				throw new ArgumentNullException(nameof(scene));
			if ((scene.Type == null || scene.Type == SceneType.LightScene) && (scene.Lights == null || !scene.Lights.Any()))
				throw new ArgumentNullException(nameof(scene.Lights));
      if (scene.Type == SceneType.GroupScene && string.IsNullOrEmpty(scene.Group))
        throw new ArgumentNullException(nameof(scene.Group));
      if (scene.Name == null)
				throw new ArgumentNullException(nameof(scene.Name));

			//It defaults to false, but fails when omitted
			//https://github.com/Q42/Q42.HueApi/issues/56
			if (!scene.Recycle.HasValue)
				scene.Recycle = false;

			//Filter non updatable properties
			//Set these fields to null
			var sceneJson = JObject.FromObject(scene, new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore });
			sceneJson.Remove("Id");
			sceneJson.Remove("version");
			sceneJson.Remove("lastupdated");
			sceneJson.Remove("locked");
			sceneJson.Remove("owner");
			sceneJson.Remove("lightstates");

			string jsonString = JsonConvert.SerializeObject(sceneJson, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

			HttpClient client = await GetHttpClient().ConfigureAwait(false);
			var response = await client.PostAsync(new Uri(String.Format("{0}scenes", ApiBase)), new JsonContent(jsonString)).ConfigureAwait(false);

			var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			HueResults sceneResult = DeserializeDefaultHueResult(jsonResult);

			if (sceneResult.Count > 0 && sceneResult[0].Success != null && !string.IsNullOrEmpty(sceneResult[0].Success.Id))
			{
				return sceneResult[0].Success.Id;
			}

			if (sceneResult.HasErrors())
				throw new HueException(sceneResult.Errors.First().Error.Description);

			return null;

		}

		/// <summary>
		/// UpdateSceneAsync
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="lights"></param>
		/// <param name="storeLightState">If set, the lightstates of the lights in the scene will be overwritten by the current state of the lights. Can also be used in combination with transitiontime to update the transition time of a scene.</param>
		/// <param name="transitionTime">Can be used in combination with storeLightState</param>
		/// <returns></returns>
		public async Task<HueResults> UpdateSceneAsync(string id, string name, IEnumerable<string> lights, bool? storeLightState = null, TimeSpan? transitionTime = null)
	{
		CheckInitialized();

		if (id == null)
			throw new ArgumentNullException(nameof(id));
		if (id.Trim() == String.Empty)
			throw new ArgumentException("id must not be empty", nameof(id));
		if (lights == null)
			throw new ArgumentNullException(nameof(lights));

      JObject jsonObj = new JObject();
      jsonObj.Add("lights", JToken.FromObject(lights));

      if (storeLightState.HasValue)
		{
        jsonObj.Add("storelightstate", storeLightState.Value);

			//Transitiontime can only be used in combination with storeLightState
			if (transitionTime.HasValue)
			{
          jsonObj.Add("transitiontime", (uint)transitionTime.Value.TotalSeconds * 10);
			}
		}

		if (!string.IsNullOrEmpty(name))
        jsonObj.Add("name", name);


      string jsonString = JsonConvert.SerializeObject(jsonObj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

		HttpClient client = await GetHttpClient().ConfigureAwait(false);
		var response = await client.PutAsync(new Uri(String.Format("{0}scenes/{1}", ApiBase, id)), new JsonContent(jsonString)).ConfigureAwait(false);

		var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

		return DeserializeDefaultHueResult(jsonResult);

	}

		/// <summary>
		/// UpdateSceneAsync
		/// </summary>
		/// <param name="id"></param>
		/// <param name="scene"></param>
		/// <returns></returns>
		public async Task<HueResults> UpdateSceneAsync(string id, Scene scene)
		{
			CheckInitialized();

			if (id == null)
				throw new ArgumentNullException(nameof(id));
			if (id.Trim() == String.Empty)
				throw new ArgumentException("id must not be empty", nameof(id));
			if (scene == null)
				throw new ArgumentNullException(nameof(scene));

			//Set these fields to null
			var sceneJson = JObject.FromObject(scene, new JsonSerializer() { NullValueHandling = NullValueHandling.Ignore });
			sceneJson.Remove("Id");
			sceneJson.Remove("recycle");
			sceneJson.Remove("version");
			sceneJson.Remove("lastupdated");
			sceneJson.Remove("locked");
			sceneJson.Remove("owner");
			sceneJson.Remove("lightstates");

			string jsonString = JsonConvert.SerializeObject(sceneJson, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

			HttpClient client = await GetHttpClient().ConfigureAwait(false);
			var response = await client.PutAsync(new Uri(String.Format("{0}scenes/{1}", ApiBase, id)), new JsonContent(jsonString)).ConfigureAwait(false);

			var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

			return DeserializeDefaultHueResult(jsonResult);

		}


		public async Task<HueResults> ModifySceneAsync(string sceneId, string lightId, LightCommand command)
    {
      CheckInitialized();

      if (sceneId == null)
        throw new ArgumentNullException(nameof(sceneId));
      if (sceneId.Trim() == String.Empty)
        throw new ArgumentException("sceneId must not be empty", nameof(sceneId));
      if (lightId == null)
        throw new ArgumentNullException(nameof(lightId));
      if (lightId.Trim() == String.Empty)
        throw new ArgumentException("lightId must not be empty", nameof(lightId));

      if (command == null)
        throw new ArgumentNullException(nameof(command));

      string jsonCommand = JsonConvert.SerializeObject(command, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var response = await client.PutAsync(new Uri(String.Format("{0}scenes/{1}/lights/{2}/state", ApiBase, sceneId, lightId)), new JsonContent(jsonCommand)).ConfigureAwait(false);

      var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);
    }


    public Task<HueResults> RecallSceneAsync(string sceneId, string groupId = "0")
    {
      if (sceneId == null)
        throw new ArgumentNullException(nameof(sceneId));

      var groupCommand = new SceneCommand() { Scene = sceneId };

      return this.SendGroupCommandAsync(groupCommand, groupId);

    }

		/// <summary>
		/// Deletes a scene
		/// </summary>
		/// <param name="sceneId"></param>
		/// <returns></returns>
		public async Task<IReadOnlyCollection<DeleteDefaultHueResult>> DeleteSceneAsync(string sceneId)
	{
		CheckInitialized();

		HttpClient client = await GetHttpClient().ConfigureAwait(false);
		var result = await client.DeleteAsync(new Uri(String.Format("{0}scenes/{1}", ApiBase, sceneId))).ConfigureAwait(false);

		string jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

		return DeserializeDefaultHueResult<DeleteDefaultHueResult>(jsonResult);

	}

		/// <summary>
		/// Get a single scene
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<Scene?> GetSceneAsync(string id)
		{
			CheckInitialized();

			HttpClient client = await GetHttpClient().ConfigureAwait(false);
			string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}scenes/{1}", ApiBase, id))).ConfigureAwait(false);

			Scene? scene = DeserializeResult<Scene>(stringResult);

			if (scene != null && string.IsNullOrEmpty(scene.Id))
				scene.Id = id;

			return scene;

		}
	}
}
