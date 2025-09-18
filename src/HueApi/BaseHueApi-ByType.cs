using HueApi.Extensions;
using HueApi.Models;
using HueApi.Models.Sensors;
using System.Net.Http.Json;

namespace HueApi
{
  public abstract partial class BaseHueApi
  {
    protected Dictionary<string, Type> _resourceTypes = new Dictionary<string, Type>()
    {
      { "resource", typeof(HueResponse<HueResource>) },
      { "light", typeof(HueResponse<Light>) },
      { "scene", typeof(HueResponse<Scene>) },
      { "room", typeof(HueResponse<Room>) },
      { "zone", typeof(HueResponse<Zone>) },
      { "bridge_home", typeof(HueResponse<BridgeHome>) },
      { "grouped_light", typeof(HueResponse<GroupedLight>) },
      { "device", typeof(HueResponse<Device>) },
      { "bridge", typeof(HueResponse<Bridge>) },
      { "device_software_update", typeof(HueResponse<DeviceSoftwareUpdate>) },
      { "device_power", typeof(HueResponse<DevicePower>) },
      { "zigbee_connectivity", typeof(HueResponse<ZigbeeConnectivity>) },
      { "zgp_connectivity", typeof(HueResponse<ZgpConnectivity>) },
      { "zigbee_device_discovery", typeof(HueResponse<ZigbeeDeviceDiscovery>) },
      { "motion", typeof(HueResponse<MotionResource>) },
      { "service_group", typeof(HueResponse<ServiceGroupResource>) },
      { "grouped_motion", typeof(HueResponse<GroupedMotionResource>) },
      { "grouped_light_level", typeof(HueResponse<GroupedLightLevelResource>) },
      { "camera_motion", typeof(HueResponse<CameraMotionResource>) },
      { "temperature", typeof(HueResponse<TemperatureResource>) },
      { "light_level", typeof(HueResponse<LightLevel>) },
      { "button", typeof(HueResponse<ButtonResource>) },
      { "bell_button", typeof(HueResponse<BellButtonResource>) },
      { "relative_rotary", typeof(HueResponse<RelativeRotaryResource>) },
      { "behavior_script", typeof(HueResponse<BehaviorScript>) },
      { "behavior_instance", typeof(HueResponse<BehaviorInstance>) },
      { "geofence_client", typeof(HueResponse<GeofenceClient>) },
      { "geolocation", typeof(HueResponse<Geolocation>) },
      { "entertainment_configuration", typeof(HueResponse<EntertainmentConfiguration>) },
      { "entertainment", typeof(HueResponse<Entertainment>) },
      { "homekit", typeof(HueResponse<Homekit>) },
      { "matter", typeof(HueResponse<MatterItem>) },
      { "matter_fabric", typeof(HueResponse<MatterFabric>) },
      { "smart_scene", typeof(HueResponse<SmartScene>) },
      { "contact", typeof(HueResponse<ContactSensor>) },
      { "tamper", typeof(HueResponse<TamperSensor>) },
      { "motion_area_configuration", typeof(HueResponse<MotionAreaConfigResource>) },
      { "motion_area_candidate", typeof(HueResponse<MotionAreaCandidateResource>) },
      { "convenience_area_motion", typeof(HueResponse<ConvenienceAreaMotionResource>) },
      { "security_area_motion", typeof(HueResponse<SecurityAreaMotionResource>) },
      { "speaker", typeof(HueResponse<SpeakerResource>) },
      { "clip", typeof(HueResponse<ClipResource>) },
      { "wifi_connectivity", typeof(HueResponse<WifiConnectivityResource>) }
    };


    public Task<HueResponse<HueResource>> GetResourceTypedAsync(HueResource res) => GetResourceTypedAsync(res.Type, res.Id);


    /// <summary>
    /// Generic method to get any resource by type and id
    /// </summary>
    /// <param name="rtype"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<HueResponse<HueResource>> GetResourceTypedAsync(string rtype, Guid? id = null)
    {
      var url = ResourceTypeIdUrl(rtype, id);
      var type = typeof(HueResponse<HueResource>);
      if (_resourceTypes.ContainsKey(rtype))
        type = _resourceTypes[rtype];

      var response = await client.GetAsync(url);

      var result = await ProcessResponseAsync(response, type);

      return result;
    }

    protected async Task<HueResponse<HueResource>> ProcessResponseAsync(HttpResponseMessage? response, Type type)
    {
      if (response == null)
        return new();

      if (response.IsSuccessStatusCode)
      {
        var data = await response.Content.ReadAsStringAsync();
        var json = System.Text.Json.JsonSerializer.Deserialize(data, type);

        if (json == null)
          return new();

        // If it's already HueResponse<HueResource>, return it directly
        if (json is HueResponse<HueResource> resourceResponse)
          return resourceResponse;

        // Otherwise, try to convert from HueResponse<TDerived>
        var jsonType = json.GetType();
        if (jsonType.IsGenericType && jsonType.GetGenericTypeDefinition() == typeof(HueResponse<>))
        {
          var derivedType = jsonType.GetGenericArguments()[0];
          if (typeof(HueResource).IsAssignableFrom(derivedType))
          {
            // Call ConvertToBase<TDerived, HueResource> via reflection
            var method = typeof(HueResponseExtensions)
                .GetMethod(nameof(HueResponseExtensions.ConvertToBase))?
                .MakeGenericMethod(derivedType);

            var converted = method?.Invoke(null, new[] { json });
            return converted as HueResponse<HueResource> ?? new HueResponse<HueResource>();
          }
        }

        return new HueResponse<HueResource>();
      }
      else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
      {
        throw new UnauthorizedAccessException();
      }
      else
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<HueErrorResponse>();

        var result = new HueResponse<HueResource>();
        if (errorResponse != null)
          result.Errors = errorResponse.Errors;

        return result;
      }
    }
  }

}
