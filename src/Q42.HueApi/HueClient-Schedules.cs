using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Models;
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
  /// Partial HueClient, contains requests to the /schedules/ url
  /// </summary>
  public partial class HueClient
  {
    /// <summary>
    /// Get all schedules
    /// </summary>
    /// <returns></returns>
    public async Task<List<Schedule>> GetSchedulesAsync()
    {
      CheckInitialized();

      HttpClient client = HueClient.GetHttpClient();
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}schedules", ApiBase))).ConfigureAwait(false);

      List<Schedule> results = new List<Schedule>();

      JToken token = JToken.Parse(stringResult);
      if (token.Type == JTokenType.Object)
      {
        //Each property is a light
        var jsonResult = (JObject)token;

        foreach (var prop in jsonResult.Properties())
        {
          Schedule newSchedule = new Schedule();
          newSchedule.Id = prop.Name;
          newSchedule.Name = prop.First["name"].ToString();

          results.Add(newSchedule);
        }

      }

      return results;
    }

    /// <summary>
    /// Get a single schedule
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Schedule> GetScheduleAsync(string id)
    {
      CheckInitialized();

      HttpClient client = HueClient.GetHttpClient();
      string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}schedules/{1}", ApiBase, id))).ConfigureAwait(false);

      Schedule schedule = DeserializeResult<Schedule>(stringResult);

      if (string.IsNullOrEmpty(schedule.Id))
        schedule.Id = id;

      return schedule;

    }

    /// <summary>
    /// Create a schedule
    /// </summary>
    /// <param name="schedule"></param>
    /// <returns></returns>
    public async Task<string> CreateScheduleAsync(Schedule schedule)
    {
      CheckInitialized();

      string command = JsonConvert.SerializeObject(schedule, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
     
      HttpClient client = HueClient.GetHttpClient();

      //Create schedule
      var result = await client.PostAsync(new Uri(ApiBase + "schedules"), new StringContent(command)).ConfigureAwait(false);

      var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      DefaultHueResult[] scheduleResult = JsonConvert.DeserializeObject<DefaultHueResult[]>(jsonResult);

      if (scheduleResult.Length > 0 && scheduleResult[0].Success != null && !string.IsNullOrEmpty(scheduleResult[0].Success.Id))
      {
        return scheduleResult[0].Success.Id;
      }

      return null;
    }

    /// <summary>
    /// Update a schedule
    /// </summary>
    /// <param name="id"></param>
    /// <param name="schedule"></param>
    /// <returns></returns>
    public async Task<HueResults> UpdateScheduleAsync(string id, Schedule schedule)
    {
      CheckInitialized();

      string command = JsonConvert.SerializeObject(schedule, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

      HttpClient client = HueClient.GetHttpClient();

      //Create schedule
      var result = await client.PutAsync(new Uri(string.Format("{0}schedules/{1}", ApiBase, id)), new StringContent(command)).ConfigureAwait(false);

      var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);

    }

    /// <summary>
    /// Delete a schedule
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<HueResults> DeleteScheduleAsync(string id)
    {
      HttpClient client = HueClient.GetHttpClient();
      //Delete schedule
      var result = await client.DeleteAsync(new Uri(ApiBase + string.Format("schedules/{0}", id))).ConfigureAwait(false);

      string jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

      return DeserializeDefaultHueResult(jsonResult);
    }
  }
}
