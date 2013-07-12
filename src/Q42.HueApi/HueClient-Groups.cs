using Newtonsoft.Json;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
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
    /// <param name="lightList"></param>
    /// <returns></returns>
    public async Task<string> CreateGroup(IEnumerable<string> lightList)
    {
      if (lightList == null)
        throw new ArgumentNullException("lightList");

      string lightString = CreateLightList(lightList);

      HttpClient client = new HttpClient();

      //Create group with the lights we want to target
      var result = await client.PostAsync(new Uri(ApiBase + "groups"), new StringContent("{\"lights\":" + lightString + "}")).ConfigureAwait(false);

      var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      GroupPutResult[] groupResult = JsonConvert.DeserializeObject<GroupPutResult[]>(jsonResult);

      if (groupResult.Length > 0 && groupResult[0].Success != null && !string.IsNullOrEmpty(groupResult[0].Success.Id))
      {
        return groupResult[0].Success.Id.Replace("/groups/", string.Empty);
      }

      return null;

    }

    /// <summary>
    /// Deletes a single group
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public Task DeleteGroup(string groupId)
    {
      HttpClient client = new HttpClient();
      //Delete group 1
      return client.DeleteAsync(new Uri(ApiBase + string.Format("groups/{0}", groupId)));
    }

    /// <summary>
    /// Send command to a group
    /// </summary>
    /// <param name="command"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    public Task SendGroupCommandAsync(LightCommand command, string group = "0")
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
    private Task SendGroupCommandAsync(string command, string group = "0")
    {
      if (command == null)
        throw new ArgumentNullException("command");

      CheckInitialized();

      HttpClient client = new HttpClient();
      return client.PutAsync(new Uri(ApiBase + string.Format("groups/{0}/action", group)), new StringContent(command));
    }
  }
}
