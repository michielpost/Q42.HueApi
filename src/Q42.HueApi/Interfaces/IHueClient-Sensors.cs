using Q42.HueApi.Models;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  public interface IHueClient_Sensors
  {
    Task<IReadOnlyCollection<Sensor>> GetSensorsAsync();
    Task<string?> CreateSensorAsync(Sensor sensor);
    Task<HueResults> FindNewSensorsAsync();
    Task<IReadOnlyCollection<Sensor>> GetNewSensorsAsync();
    Task<Sensor?> GetSensorAsync(string id);
    Task<HueResults> UpdateSensorAsync(string id, string newName);
    Task<HueResults> ChangeSensorConfigAsync(string id, SensorConfig config);
    Task<HueResults> ChangeSensorStateAsync(string id, SensorState state);

    /// <summary>
    /// Delete Sensor
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<DeleteDefaultHueResult>> DeleteSensorAsync(string id);


  }
}
