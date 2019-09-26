using Q42.HueApi.Models;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  public interface IHueClient_Scenes
  {
    Task<IReadOnlyCollection<Scene>> GetScenesAsync();
    /// <summary>
    /// Creates a new scene
    /// </summary>
    /// <param name="scene"></param>
    /// <returns>ID of the new scene</returns>
    Task<string?> CreateSceneAsync(Scene scene);
    Task<HueResults> UpdateSceneAsync(string sceneId, Scene scene);
    Task<HueResults> UpdateSceneAsync(string sceneId, string name, IEnumerable<string> lights, bool? storeLightState = null, TimeSpan? transitionTime = null);
    Task<HueResults> ModifySceneAsync(string sceneId, string lightId, LightCommand command);
    Task<HueResults> RecallSceneAsync(string sceneId, string groupId = "0");
    Task<IReadOnlyCollection<DeleteDefaultHueResult>> DeleteSceneAsync(string sceneId);

    Task<Scene?> GetSceneAsync(string id);
  }
}
