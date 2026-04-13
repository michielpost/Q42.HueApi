using HueApi.Models;
using HueApi.Models.Responses;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HueApi
{
  public delegate void EventStreamMessage(string bridgeIp, List<EventStreamResponse> events);

  public abstract partial class BaseHueApi
  {
    protected HttpClient client = default!;

    protected const string ResourceUrl = "clip/v2/resource";
    protected const string LightUrl = $"{ResourceUrl}/light";
    protected const string SceneUrl = $"{ResourceUrl}/scene";
    protected const string RoomUrl = $"{ResourceUrl}/room";
    protected const string ZoneUrl = $"{ResourceUrl}/zone";
    protected const string BridgeHomeUrl = $"{ResourceUrl}/bridge_home";
    protected const string GroupedLightUrl = $"{ResourceUrl}/grouped_light";
    protected const string DeviceUrl = $"{ResourceUrl}/device";
    protected const string BridgeUrl = $"{ResourceUrl}/bridge";
    protected const string DeviceSoftwareUpdateUrl = $"{ResourceUrl}/device_software_update";
    protected const string DevicePowerUrl = $"{ResourceUrl}/device_power";
    protected const string ZigbeeConnectivityUrl = $"{ResourceUrl}/zigbee_connectivity";
    protected const string ZgpConnectivityUrl = $"{ResourceUrl}/zgp_connectivity";
    protected const string ZigbeeDeviceDiscoveryUrl = $"{ResourceUrl}/zigbee_device_discovery";
    protected const string MotionUrl = $"{ResourceUrl}/motion";
    protected const string ServiceGroupUrl = $"{ResourceUrl}/service_group";
    protected const string GroupedMotionUrl = $"{ResourceUrl}/grouped_motion";
    protected const string GroupedLightLevelUrl = $"{ResourceUrl}/grouped_light_level";
    protected const string CameraMotionUrl = $"{ResourceUrl}/camera_motion";
    protected const string TemperatureUrl = $"{ResourceUrl}/temperature";
    protected const string LightLevelUrl = $"{ResourceUrl}/light_level";
    protected const string ButtonUrl = $"{ResourceUrl}/button";
    protected const string BellButtonUrl = $"{ResourceUrl}/bell_button";
    protected const string RelativeRotaryUrl = $"{ResourceUrl}/relative_rotary";
    protected const string BehaviorScriptUrl = $"{ResourceUrl}/behavior_script";
    protected const string BehaviorInstanceUrl = $"{ResourceUrl}/behavior_instance";
    protected const string GeofenceClientUrl = $"{ResourceUrl}/geofence_client";
    protected const string GeolocationUrl = $"{ResourceUrl}/geolocation";
    protected const string EntertainmentConfigurationUrl = $"{ResourceUrl}/entertainment_configuration";
    protected const string EntertainmentUrl = $"{ResourceUrl}/entertainment";
    protected const string HomekitUrl = $"{ResourceUrl}/homekit";
    protected const string MatterUrl = $"{ResourceUrl}/matter";
    protected const string MatterFabricUrl = $"{ResourceUrl}/matter_fabric";
    protected const string SmartSceneUrl = $"{ResourceUrl}/smart_scene";
    protected const string ContactUrl = $"{ResourceUrl}/contact";
    protected const string TamperUrl = $"{ResourceUrl}/tamper";

    protected const string MotionAreaConfigUrl = $"{ResourceUrl}/motion_area_configuration";
    protected const string MotionAreaCandidateUrl = $"{ResourceUrl}/motion_area_candidate";
    protected const string ConvenienceAreaMotionUrl = $"{ResourceUrl}/convenience_area_motion";
    protected const string SecurityAreaMotionUrl = $"{ResourceUrl}/security_area_motion";
    protected const string SpeakerUrl = $"{ResourceUrl}/speaker";
    protected const string ClipUrl = $"{ResourceUrl}/clip";
    protected const string WifiConnectivityUrl = $"{ResourceUrl}/wifi_connectivity";



    internal string ResourceTypeIdUrl(string? rtype, Guid? id = null)
    {
      var url = ResourceUrl;
      if (!string.IsNullOrEmpty(rtype))
      {
        url += $"/{rtype}";

        if (id.HasValue)
          url += $"/{id.Value}";
      }

      return url;
    }

    /// <summary>
    /// Generic method to get any resource by type and id
    /// </summary>
    /// <param name="rtype"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task<HueResponse<HueResource>> GetResourceAsync(string rtype, Guid? id = null) => HueGetRequestAsync<HueResource>(ResourceTypeIdUrl(rtype, id));
    public Task<HueResponse<HueResource>> GetResourceAsync(HueResource res) => HueGetRequestAsync<HueResource>(ResourceTypeIdUrl(res.Type, res.Id));


    internal async Task<HueResponse<T>> HueGetRequestAsync<T>(string url)
    {
      var response = await client.GetAsync(url).ConfigureAwait(false);

      return await ProcessResponseAsync<HueResponse<T>>(response).ConfigureAwait(false);
    }

    internal async Task<HueDeleteResponse> HueDeleteRequestAsync(string url)
    {
      var response = await client.DeleteAsync(url).ConfigureAwait(false);

      return await ProcessResponseAsync<HueDeleteResponse>(response).ConfigureAwait(false);
    }

    internal async Task<HuePutResponse> HuePutRequestAsync<D>(string url, D data)
    {
      JsonSerializerOptions options = new()
      {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
      };

      var response = await client.PutAsJsonAsync(url, data, options).ConfigureAwait(false);

      return await ProcessResponseAsync<HuePutResponse>(response).ConfigureAwait(false);
    }

    internal async Task<HuePostResponse> HuePostRequestAsync<D>(string url, D data)
    {
      JsonSerializerOptions options = new()
      {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
      };

      var response = await client.PostAsJsonAsync(url, data, options).ConfigureAwait(false);

      return await ProcessResponseAsync<HuePostResponse>(response).ConfigureAwait(false);
    }


    protected async Task<T> ProcessResponseAsync<T>(HttpResponseMessage? response) where T : HueErrorResponse, new()
    {
      if (response == null)
        return new T();

      if (response.IsSuccessStatusCode)
      {
        return (await response.Content.ReadFromJsonAsync<T>().ConfigureAwait(false)) ?? new();
      }
      else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
      {
        throw new UnauthorizedAccessException();
      }
      else
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<HueErrorResponse>().ConfigureAwait(false);

        var result = new T();
        if (errorResponse != null)
          result.Errors = errorResponse.Errors;

        return result;
      }
    }

  }
}
