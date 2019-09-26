using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  public interface IHueClient_Groups
  {

    /// <summary>
    /// Send command to a group
    /// </summary>
    /// <param name="command"></param>
    /// <param name="group"></param>
    /// <returns></returns>
    Task<HueResults> SendGroupCommandAsync(ICommandBody command, string group = "0");

    /// <summary>
    /// Create a group for a list of lights
    /// </summary>
    /// <param name="lights">List of lights in the group</param>
    /// <param name="name">Optional name</param>
    /// <param name="roomClass">for room creation the room class has to be passed, without class it will get the default: "Other" class.</param>
    /// <param name="groupType">GroupType</param>
    /// <returns></returns>
    Task<string?> CreateGroupAsync(IEnumerable<string> lights, string? name = null, RoomClass? roomClass = null, GroupType groupType = GroupType.Room);

    /// <summary>
    /// Deletes a single group
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<DeleteDefaultHueResult>> DeleteGroupAsync(string groupId);

    /// <summary>
    /// Get all groups
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<Group>> GetGroupsAsync();

    /// <summary>
    /// Get the state of a single group
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Group?> GetGroupAsync(string id);

    /// <summary>
    /// Returns the entertainment group
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<Group>> GetEntertainmentGroups();

    /// <summary>
    /// Update a group
    /// </summary>
    /// <param name="id">Group ID</param>
    /// <param name="lights">List of light IDs</param>
    /// <param name="name">Group Name (optional)</param>
    /// <param name="roomClass">for room creation the room class has to be passed, without class it will get the default: "Other" class.</param>
    /// <returns></returns>
    Task<HueResults> UpdateGroupAsync(string id, IEnumerable<string> lights, string? name = null, RoomClass? roomClass = null);

    Task<HueResults> UpdateGroupLocationsAsync(string id, Dictionary<string, LightLocation> locations);
  }
}
