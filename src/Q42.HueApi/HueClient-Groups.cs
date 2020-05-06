using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
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
  public partial class HueClient : IHueClient_Groups
  {
    /// <summary>
    /// Create a group for a list of lights
    /// </summary>
    /// <param name="lights">List of lights in the group</param>
    /// <param name="name">Optional name</param>
    /// <param name="roomClass">for room creation the room class has to be passed, without class it will get the default: "Other" class.</param>
    /// <returns></returns>
    public async Task<string?> CreateGroupAsync(IEnumerable<string> lights, string? name = null, RoomClass? roomClass = null, GroupType groupType = GroupType.Room)
    {
      CheckInitialized();

      if (lights == null)
        throw new ArgumentNullException(nameof(lights));

      CreateGroupRequest jsonObj = new CreateGroupRequest();
      jsonObj.Lights = lights;

      if (!string.IsNullOrEmpty(name))
        jsonObj.Name = name!;

      if (roomClass.HasValue)
        jsonObj.Class = roomClass.Value;

      jsonObj.Type = groupType;

      string jsonString = JsonConvert.SerializeObject(jsonObj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      //Create group with the lights we want to target
      var response = await client.PostAsync(new Uri(String.Format("{0}groups", ApiBase)), new JsonContent(jsonString)).ConfigureAwait(false);
      var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      HueResults groupResult = DeserializeDefaultHueResult(jsonResult);

      if (groupResult.Count > 0 && groupResult[0].Success != null && !string.IsNullOrEmpty(groupResult[0].Success.Id))
      {
        return groupResult[0].Success.Id.Replace("/groups/", string.Empty);
      }

      if (groupResult.HasErrors())
        throw new HueException(groupResult.Errors.First().Error.Description);

      return null;

    }

    /// <summary>
    /// Deletes a single group
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public async Task<IReadOnlyCollection<DeleteDefaultHueResult>> DeleteGroupAsync(string groupId)
    {
      CheckInitialized();

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      //Delete group 1
      var result =  await client.DeleteAsync(new Uri(ApiBase + string.Format("groups/{0}", groupId))).ConfigureAwait(false);

      string jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult<DeleteDefaultHueResult>(jsonResult);

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
        throw new ArgumentNullException(nameof(command));

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
        throw new ArgumentNullException(nameof(command));

      CheckInitialized();

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var result = await client.PutAsync(new Uri(ApiBase + string.Format("groups/{0}/action", group)), new JsonContent(command)).ConfigureAwait(false);

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

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}groups", ApiBase))).ConfigureAwait(false);

      List<Group> results = new List<Group>();

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Object)
      {
        //Each property is a light
        var jsonResult = (JObject)token;

        foreach (var prop in jsonResult.Properties())
        {
          Group newGroup = JsonConvert.DeserializeObject<Group>(prop.Value.ToString());
          newGroup.Id = prop.Name;

          results.Add(newGroup);
        }

      }

      return results;

    }

    public async Task<IReadOnlyList<Group>> GetEntertainmentGroups()
    {
      var allGroups = await GetGroupsAsync();
      return allGroups.Where(x => x.Type == Models.Groups.GroupType.Entertainment).ToList();
    }

    /// <summary>
    /// Get the state of a single group
    /// </summary>
    /// <returns></returns>
    public async Task<Group?> GetGroupAsync(string id)
    {
      CheckInitialized();

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}groups/{1}", ApiBase, id))).ConfigureAwait(false);

//#if DEBUG
//      stringResult = "{ \"type\": null,  \"action\": {        \"on\": true,        \"xy\": [0.5, 0.5]    },    \"lights\": [        \"1\",        \"2\"    ],    \"name\": \"bedroom\",}";
//#endif

      Group? group = DeserializeResult<Group>(stringResult);

      if (group != null && string.IsNullOrEmpty(group.Id))
        group.Id = id;

      return group;


    }

    /// <summary>
    /// Update a group
    /// </summary>
    /// <param name="id">Group ID</param>
    /// <param name="lights">List of light IDs</param>
    /// <param name="name">Group Name</param>
	  /// <param name="roomClass">for room creation the room class has to be passed, without class it will get the default: "Other" class.</param>
    /// <returns></returns>
    public async Task<HueResults> UpdateGroupAsync(string id, IEnumerable<string> lights, string? name = null, RoomClass? roomClass = null)
    {
      if (id == null)
        throw new ArgumentNullException(nameof(id));
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id must not be empty", nameof(id));
      if (lights == null)
        throw new ArgumentNullException(nameof(lights));

      UpdateGroupRequest jsonObj = new UpdateGroupRequest();
      jsonObj.Lights = lights;

      if(!string.IsNullOrEmpty(name))
        jsonObj.Name = name!;

      if (roomClass.HasValue)
        jsonObj.Class = roomClass.Value;

      string jsonString = JsonConvert.SerializeObject(jsonObj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var response = await client.PutAsync(new Uri(String.Format("{0}groups/{1}", ApiBase, id)), new JsonContent(jsonString)).ConfigureAwait(false);
      var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }

    public async Task<HueResults> UpdateGroupLocationsAsync(string id, Dictionary<string, LightLocation> locations)
    {
      if (id == null)
        throw new ArgumentNullException(nameof(id));
      if (id.Trim() == String.Empty)
        throw new ArgumentException("id must not be empty", nameof(id));
      if (locations == null || !locations.Any())
        throw new ArgumentNullException(nameof(locations));

      JObject jsonObj = new JObject();
      jsonObj.Add("locations", JToken.FromObject(locations));

      string jsonString = JsonConvert.SerializeObject(jsonObj, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      HttpClient client = await GetHttpClient().ConfigureAwait(false);
      var response = await client.PutAsync(new Uri(String.Format("{0}groups/{1}", ApiBase, id)), new JsonContent(jsonString)).ConfigureAwait(false);
      var jsonResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }

  
  }
}
