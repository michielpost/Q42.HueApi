using Q42.HueApi.Models;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  public interface IHueClient_ResourceLinks
  {
    Task<IReadOnlyCollection<DeleteDefaultHueResult>> DeleteResourceLinkAsync(string resourceLinkId);
    Task<IReadOnlyCollection<ResourceLink>> GetResourceLinksAsync();
    Task<ResourceLink?> GetResourceLinkAsync(string id);
    Task<string?> CreateResourceLinkAsync(ResourceLink resourceLink);
    Task<HueResults> UpdateResourceLinkAsync(string id, ResourceLink resourceLink);

  }
}
