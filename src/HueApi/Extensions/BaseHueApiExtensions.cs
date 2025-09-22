using HueApi.Models;
using HueApi.Models.Requests;

namespace HueApi.Extensions
{
  public static class BaseHueApiExtensions
  {
    public static Task<HuePutResponse> SetStreamingAsync(this BaseHueApi hueApi, Guid entertainmentAreaId, bool active = true)
    {
      UpdateEntertainmentConfiguration req = new UpdateEntertainmentConfiguration()
      {
        Action = active ? EntertainmentConfigurationAction.start : EntertainmentConfigurationAction.stop
      };

      return hueApi.EntertainmentConfiguration.UpdateAsync(entertainmentAreaId, req);
    }
  }
}
