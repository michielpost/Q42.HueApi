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
    /// Asynchronously gets all scenes registered with the bridge.
    /// </summary>
    /// <returns>An enumerable of <see cref="Scene"/>s registered with the bridge.</returns>
    public async Task<IEnumerable<Scene>> GetScenesAsync()
    {
      CheckInitialized();

      HttpClient client = HueClient.GetHttpClient();
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}scenes", ApiBase))).ConfigureAwait(false);

#if DEBUG
      stringResult = "{    \"1\": {        \"name\": \"My Scene 1\",        \"lights\": [            \"1\",            \"2\",            \"3\"        ],        \"active\": true    },    \"2\": {        \"name\": \"My Scene 2\",        \"lights\": [            \"1\",            \"2\",            \"3\"        ],        \"active\": true    }}";
#endif


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

    public async Task<HueResults> CreateOrUpdateSceneAsync(string id, string name, IEnumerable<string> lights)
    {
      CheckInitialized();

      if (id == null)
        throw new ArgumentNullException("id");
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id must not be empty", "id");
      if (lights == null)
        throw new ArgumentNullException("lights");

      dynamic jsonObj = new ExpandoObject();
      jsonObj.lights = lights;

      if (!string.IsNullOrEmpty(name))
        jsonObj.name = name;

      string jsonString = JsonConvert.SerializeObject(jsonObj);

      HttpClient client = HueClient.GetHttpClient();
      var response = await client.PutAsync(new Uri(String.Format("{0}scenes/{1}", ApiBase, id)), new StringContent(jsonString)).ConfigureAwait(false);

      var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }

    public async Task<HueResults> ModifySceneAsync(string sceneId, string lightId, LightCommand command)
    {
      CheckInitialized();

      if (sceneId == null)
        throw new ArgumentNullException("sceneId");
      if (sceneId.Trim() == String.Empty)
        throw new ArgumentException("sceneId must not be empty", "sceneId");
      if (lightId == null)
        throw new ArgumentNullException("lightId");
      if (lightId.Trim() == String.Empty)
        throw new ArgumentException("lightId must not be empty", "lightId");

      if (command == null)
        throw new ArgumentNullException("command");

      string jsonCommand = JsonConvert.SerializeObject(command, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      HttpClient client = HueClient.GetHttpClient();
      var response = await client.PutAsync(new Uri(String.Format("{0}scenes/{1}/lights/{1}/state", ApiBase, sceneId, lightId)), new StringContent(jsonCommand)).ConfigureAwait(false);

      var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);
    }


    public Task<HueResults> RecallSceneAsync(string sceneId, string groupId = "0")
    {
      if (sceneId == null)
        throw new ArgumentNullException("sceneId");

      var groupCommand = new GroupCommand() { Scene = sceneId };

      return this.SendGroupCommandAsync(groupCommand, groupId);

    }
  }
}
