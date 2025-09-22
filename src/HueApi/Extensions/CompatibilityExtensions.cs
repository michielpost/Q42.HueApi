using HueApi.Models;
using HueApi.Models.Requests;

namespace HueApi
{
  /// <summary>
  /// Extensions for compatibility with the original Q42.HueApi
  /// </summary>
  public static class CompatibilityExtensions
  {
    /// <summary>
    /// Get bridge info
    /// </summary>
    /// <returns></returns>
    public static Task<HueResponse<Bridge>> GetBridgeAsync(this LocalHueApi api)
    {
      return api.Bridge.GetAllAsync();
    }

    /// <summary>
    /// Get all groups
    /// </summary>
    /// <returns></returns>
    public static Task<HueResponse<GroupedLight>> GetGroupsAsync(this LocalHueApi api)
    {
      return api.GroupedLight.GetAllAsync();
    }

    /// <summary>
    /// Get the state of a single group
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Task<HueResponse<GroupedLight>> GetGroupAsync(this LocalHueApi api, Guid id)
    {
      return api.GroupedLight.GetByIdAsync(id);
    }

    /// <summary>
    /// Returns the entertainment groups
    /// </summary>
    /// <returns></returns>
    public static Task<HueResponse<EntertainmentConfiguration>> GetEntertainmentGroups(this LocalHueApi api)
    {
      return api.EntertainmentConfiguration.GetAllAsync();
    }

    /// <summary>
    /// Sets the light name
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Task<HuePutResponse> SetLightNameAsync(this LocalHueApi api, Guid id, string name)
    {
      UpdateLight updateLight = new UpdateLight()
      {
        Metadata = new Metadata()
        {
          Name = name
        }
      };

      return api.Light.UpdateAsync(id, updateLight);

    }

    public static Task<HuePutResponse> RecallSceneAsync(this LocalHueApi api, Guid id)
    {
      return api.Scene.UpdateAsync(id, new UpdateScene() { Recall = new Recall() { Action = SceneRecallAction.active } });
    }

  }
}
