using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
  /// <summary>
  /// Partial HueClient, contains requests to the /Groups/ url
  /// </summary>
  public partial class HueClient
  {
    /// <summary>
    /// Create a group for a list of lights
    /// </summary>
    /// <param name="lights"></param>
    /// <returns></returns>
    public async Task<string> CreateGroupAsync(IEnumerable<string> lights, string name = null)
    {
      CheckInitialized();

      if (lights == null)
        throw new ArgumentNullException("lights");
      
      dynamic jsonObj = new ExpandoObject();
      jsonObj.lights = lights;

      if (!string.IsNullOrEmpty(name))
        jsonObj.name = name;

      string jsonString = JsonConvert.SerializeObject(jsonObj);

      HttpClient client = HueClient.GetHttpClient();
      //Create group with the lights we want to target
      var response = await client.PostAsync(new Uri(String.Format("{0}groups", ApiBase)), new StringContent(jsonString)).ConfigureAwait(false);
      var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      HueResults groupResult = DeserializeDefaultHueResult(jsonResult);

      if (groupResult.Count > 0 && groupResult[0].Success != null && !string.IsNullOrEmpty(groupResult[0].Success.Id))
      {
        return groupResult[0].Success.Id.Replace("/groups/", string.Empty);
      }

      if (groupResult.HasErrors())
        throw new Exception(groupResult.Errors.First().Error.Description);

      return null;

    }

    /// <summary>
    /// Deletes a single group
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public async Task<HueResults> DeleteGroupAsync(string groupId)
    {
      CheckInitialized();

      HttpClient client = HueClient.GetHttpClient();
      //Delete group 1
      var result =  await client.DeleteAsync(new Uri(ApiBase + string.Format("groups/{0}", groupId))).ConfigureAwait(false);

      string jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }

    /// <summary>
    /// Send command to a group
    /// </summary>
    /// <param name="command"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    public Task<HueResults> SendGroupCommandAsync(ICommandBody command, string group = "0")
    {
      if (command == null)
        throw new ArgumentNullException("command");

      string jsonCommand = JsonConvert.SerializeObject(command, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      return SendGroupCommandAsync(jsonCommand, group);
    }

    /// <summary>
    /// Send command to a group
    /// </summary>
    /// <param name="command"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    private async Task<HueResults> SendGroupCommandAsync(string command, string group = "0") //Group 0 contains all the lights
    {
      if (command == null)
        throw new ArgumentNullException("command");

      CheckInitialized();

      HttpClient client = HueClient.GetHttpClient();
      var result = await client.PutAsync(new Uri(ApiBase + string.Format("groups/{0}/action", group)), new StringContent(command)).ConfigureAwait(false);

      string jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }

    /// <summary>
    /// Get all groups
    /// </summary>
    /// <returns></returns>
    public async Task<IReadOnlyCollection<Group>> GetGroupsAsync()
    {
      CheckInitialized();

      HttpClient client = HueClient.GetHttpClient();
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}groups", ApiBase))).ConfigureAwait(false);

      List<Group> results = new List<Group>();

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Object)
      {
        //Each property is a light
        var jsonResult = (JObject)token;

        foreach (var prop in jsonResult.Properties())
        {
          Group newGroup = new Group();
          newGroup.Id = prop.Name;
          newGroup.Name = prop.First["name"].ToString();

          results.Add(newGroup);
        }

      }

      return results;

    }

    /// <summary>
    /// Get the state of a single group
    /// </summary>
    /// <returns></returns>
    public async Task<Group> GetGroupAsync(string id)
    {
      CheckInitialized();

      HttpClient client = HueClient.GetHttpClient();
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}groups/{1}", ApiBase, id))).ConfigureAwait(false);

#if DEBUG
      //stringResult = "{    \"action\": {        \"on\": true,        \"xy\": [0.5, 0.5]    },    \"lights\": [        \"1\",        \"2\"    ],    \"name\": \"bedroom\",}";
#endif

      Group group = DeserializeResult<Group>(stringResult);

      if (string.IsNullOrEmpty(group.Id))
        group.Id = id;

      return group;


    }

    /// <summary>
    /// Update a group
    /// </summary>
    /// <param name="id">Group ID</param>
    /// <param name="lights">List of light IDs</param>
    /// <param name="name">Group Name</param>
    /// <returns></returns>
    public async Task<HueResults> UpdateGroupAsync(string id, IEnumerable<string> lights, string name = null)
    {
      if (id == null)
        throw new ArgumentNullException("id");
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id must not be empty", "id");
      if (lights == null)
        throw new ArgumentNullException("lights");      

      dynamic jsonObj = new ExpandoObject();
      jsonObj.lights = lights;

      if(!string.IsNullOrEmpty(name))
        jsonObj.name = name;

      string jsonString = JsonConvert.SerializeObject(jsonObj);

      HttpClient client = HueClient.GetHttpClient();
      var response = await client.PutAsync(new Uri(String.Format("{0}groups/{1}", ApiBase, id)), new StringContent(jsonString)).ConfigureAwait(false);
      var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }
  }
}
