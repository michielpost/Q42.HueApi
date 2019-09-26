using Q42.HueApi.Models;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  public interface IHueClient_Schedules
  {
    /// <summary>
    /// Get all schedules
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<Schedule>> GetSchedulesAsync();


    /// <summary>
    /// Get a single schedule
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Schedule?> GetScheduleAsync(string id);


    /// <summary>
    /// Create a schedule
    /// </summary>
    /// <param name="schedule"></param>
    /// <returns></returns>
    Task<string?> CreateScheduleAsync(Schedule schedule);


    /// <summary>
    /// Update a schedule
    /// </summary>
    /// <param name="id"></param>
    /// <param name="schedule"></param>
    /// <returns></returns>
    Task<HueResults> UpdateScheduleAsync(string id, Schedule schedule);

    /// <summary>
    /// Delete a schedule
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<DeleteDefaultHueResult>> DeleteScheduleAsync(string id);
  }
}
