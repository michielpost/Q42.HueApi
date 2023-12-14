using HueApi.Models;
using HueApi.Models.Requests;
using HueApi.Models.Requests.SmartScene;
using HueApi.Models.Responses;
using HueApi.Models.Sensors;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HueApi
{
  public delegate void EventStreamMessage(string bridgeIp, List<EventStreamResponse> events);

  public abstract class BaseHueApi
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
    protected const string DevicePowerUrl = $"{ResourceUrl}/device_power";
    protected const string ZigbeeConnectivityUrl = $"{ResourceUrl}/zigbee_connectivity";
    protected const string ZgpConnectivityUrl = $"{ResourceUrl}/zgp_connectivity";
    protected const string ZigbeeDeviceDiscoveryUrl = $"{ResourceUrl}/zigbee_device_discovery";
    protected const string MotionUrl = $"{ResourceUrl}/motion";
    protected const string CameraMotionUrl = $"{ResourceUrl}/camera_motion";
    protected const string TemperatureUrl = $"{ResourceUrl}/temperature";
    protected const string LightLevelUrl = $"{ResourceUrl}/light_level";
    protected const string ButtonUrl = $"{ResourceUrl}/button";
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
    //recource
    protected const string SmartSceneUrl = $"{ResourceUrl}/smart_scene";
    protected const string ContactUrl = $"{ResourceUrl}/contact";
    protected const string TamperUrl = $"{ResourceUrl}/tamper";


    protected string ResourceIdUrl(string resourceUrl, Guid id) => $"{resourceUrl}/{id}";


    #region Light
    public Task<HueResponse<Models.Light>> GetLightsAsync() => HueGetRequestAsync<Models.Light>(LightUrl);
    public Task<HueResponse<Models.Light>> GetLightAsync(Guid id) => HueGetRequestAsync<Models.Light>(ResourceIdUrl(LightUrl, id));
    public Task<HuePutResponse> UpdateLightAsync(Guid id, UpdateLight data) => HuePutRequestAsync(ResourceIdUrl(LightUrl, id), data);
    #endregion

    #region Scene
    public Task<HueResponse<Scene>> GetScenesAsync() => HueGetRequestAsync<Scene>(SceneUrl);
    public Task<HuePostResponse> CreateSceneAsync(CreateScene data) => HuePostRequestAsync(SceneUrl, data);
    public Task<HueResponse<Scene>> GetSceneAsync(Guid id) => HueGetRequestAsync<Scene>(ResourceIdUrl(SceneUrl, id));
    public Task<HuePutResponse> UpdateSceneAsync(Guid id, UpdateScene data) => HuePutRequestAsync(ResourceIdUrl(SceneUrl, id), data);
    public Task<HueDeleteResponse> DeleteSceneAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(SceneUrl, id));
    #endregion

    #region SmartScene
    public Task<HueResponse<SmartScene>> GetSmartScenesAsync() => HueGetRequestAsync<SmartScene>(SmartSceneUrl);
    public Task<HuePostResponse> CreateSmartSceneAsync(CreateSmartScene data) => HuePostRequestAsync(SmartSceneUrl, data);
    public Task<HueResponse<SmartScene>> GetSmartSceneAsync(Guid id) => HueGetRequestAsync<SmartScene>(ResourceIdUrl(SmartSceneUrl, id));
    public Task<HuePutResponse> UpdateSmartSceneAsync(Guid id, UpdateSmartScene data) => HuePutRequestAsync(ResourceIdUrl(SmartSceneUrl, id), data);
    public Task<HueDeleteResponse> DeleteSmartSceneAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(SmartSceneUrl, id));
    #endregion

    #region Room
    public Task<HueResponse<Room>> GetRoomsAsync() => HueGetRequestAsync<Room>(RoomUrl);
    public Task<HuePostResponse> CreateRoomAsync(BaseResourceRequest data) => HuePostRequestAsync(RoomUrl, data);
    public Task<HueResponse<Room>> GetRoomAsync(Guid id) => HueGetRequestAsync<Room>(ResourceIdUrl(RoomUrl, id));
    public Task<HuePutResponse> UpdateRoomAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(RoomUrl, id), data);
    public Task<HueDeleteResponse> DeleteRoomAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(RoomUrl, id));
    #endregion

    #region Zone
    public Task<HueResponse<Zone>> GetZonesAsync() => HueGetRequestAsync<Zone>(ZoneUrl);
    public Task<HuePostResponse> CreateZoneAsync(CreateZone data) => HuePostRequestAsync(ZoneUrl, data);
    public Task<HueResponse<Zone>> GetZoneAsync(Guid id) => HueGetRequestAsync<Zone>(ResourceIdUrl(ZoneUrl, id));
    public Task<HuePutResponse> UpdateZoneAsync(Guid id, UpdateZone data) => HuePutRequestAsync(ResourceIdUrl(ZoneUrl, id), data);
    public Task<HueDeleteResponse> DeleteZoneAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(ZoneUrl, id));
    #endregion

    #region BridgeHome
    public Task<HueResponse<BridgeHome>> GetBridgeHomesAsync() => HueGetRequestAsync<BridgeHome>(BridgeHomeUrl);
    public Task<HueResponse<BridgeHome>> GetBridgeHomeAsync(Guid id) => HueGetRequestAsync<BridgeHome>(ResourceIdUrl(BridgeHomeUrl, id));
    public Task<HuePutResponse> UpdateBridgeHomeAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(BridgeHomeUrl, id), data);
    #endregion

    #region GroupedLight
    public Task<HueResponse<GroupedLight>> GetGroupedLightsAsync() => HueGetRequestAsync<GroupedLight>(GroupedLightUrl);
    public Task<HueResponse<GroupedLight>> GetGroupedLightAsync(Guid id) => HueGetRequestAsync<GroupedLight>(ResourceIdUrl(GroupedLightUrl, id));
    public Task<HuePutResponse> UpdateGroupedLightAsync(Guid id, UpdateGroupedLight data) => HuePutRequestAsync(ResourceIdUrl(GroupedLightUrl, id), data);
    #endregion

    #region Device
    public Task<HueResponse<Device>> GetDevicesAsync() => HueGetRequestAsync<Device>(DeviceUrl);
    public Task<HueResponse<Device>> GetDeviceAsync(Guid id) => HueGetRequestAsync<Device>(ResourceIdUrl(DeviceUrl, id));
    public Task<HuePutResponse> UpdateDeviceAsync(Guid id, UpdateDevice data) => HuePutRequestAsync(ResourceIdUrl(DeviceUrl, id), data);
    #endregion

    #region Bridge
    public Task<HueResponse<Bridge>> GetBridgesAsync() => HueGetRequestAsync<Bridge>(BridgeUrl);
    public Task<HueResponse<Bridge>> GetBridgeAsync(Guid id) => HueGetRequestAsync<Bridge>(ResourceIdUrl(BridgeUrl, id));
    public Task<HuePutResponse> UpdateBridgeAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(BridgeUrl, id), data);
    #endregion

    #region DevicePower
    public Task<HueResponse<DevicePower>> GetDevicePowersAsync() => HueGetRequestAsync<DevicePower>(DevicePowerUrl);
    public Task<HueResponse<DevicePower>> GetDevicePowerAsync(Guid id) => HueGetRequestAsync<DevicePower>(ResourceIdUrl(DevicePowerUrl, id));
    public Task<HuePutResponse> UpdateDevicePowerAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(DevicePowerUrl, id), data);
    #endregion

    #region ZigbeeConnectivity
    public Task<HueResponse<ZigbeeConnectivity>> GetZigbeeConnectivityAsync() => HueGetRequestAsync<ZigbeeConnectivity>(ZigbeeConnectivityUrl);
    public Task<HueResponse<ZigbeeConnectivity>> GetZigbeeConnectivityAsync(Guid id) => HueGetRequestAsync<ZigbeeConnectivity>(ResourceIdUrl(ZigbeeConnectivityUrl, id));
    public Task<HuePutResponse> UpdateZigbeeConnectivityAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(ZigbeeConnectivityUrl, id), data);
    #endregion

    #region ZgpConnectivity
    public Task<HueResponse<ZgpConnectivity>> GetZgpConnectivityAsync() => HueGetRequestAsync<ZgpConnectivity>(ZgpConnectivityUrl);
    public Task<HueResponse<ZgpConnectivity>> GetZgpConnectivityAsync(Guid id) => HueGetRequestAsync<ZgpConnectivity>(ResourceIdUrl(ZgpConnectivityUrl, id));
    public Task<HuePutResponse> UpdateZgpConnectivityAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(ZgpConnectivityUrl, id), data);
    #endregion

    #region zigbee_device_discovery
    public Task<HueResponse<ZigbeeDeviceDiscovery>> GetZigbeeDeviceDiscoveryAsync() => HueGetRequestAsync<ZigbeeDeviceDiscovery>(ZigbeeDeviceDiscoveryUrl);
    public Task<HueResponse<ZigbeeDeviceDiscovery>> GetZigbeeDeviceDiscoveryAsync(Guid id) => HueGetRequestAsync<ZigbeeDeviceDiscovery>(ResourceIdUrl(ZigbeeDeviceDiscoveryUrl, id));
    public Task<HuePutResponse> UpdateZigbeeDeviceDiscoveryAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(ZigbeeDeviceDiscoveryUrl, id), data);
    #endregion

    #region Motion
    public Task<HueResponse<MotionResource>> GetMotionsAsync() => HueGetRequestAsync<MotionResource>(MotionUrl);
    public Task<HueResponse<MotionResource>> GetMotionAsync(Guid id) => HueGetRequestAsync<MotionResource>(ResourceIdUrl(MotionUrl, id));
    public Task<HuePutResponse> UpdateMotionAsync(Guid id, UpdateSensitivitySensorRequest data) => HuePutRequestAsync(ResourceIdUrl(MotionUrl, id), data);
    #endregion

    #region CameraMotion
    public Task<HueResponse<CameraMotionResource>> GetCameraMotionsAsync() => HueGetRequestAsync<CameraMotionResource>(CameraMotionUrl);
    public Task<HueResponse<CameraMotionResource>> GetCameraMotionAsync(Guid id) => HueGetRequestAsync<CameraMotionResource>(ResourceIdUrl(CameraMotionUrl, id));
    public Task<HuePutResponse> UpdateCameraMotionAsync(Guid id, UpdateSensitivitySensorRequest data) => HuePutRequestAsync(ResourceIdUrl(CameraMotionUrl, id), data);
    #endregion

    #region Temperature
    public Task<HueResponse<TemperatureResource>> GetTemperaturesAsync() => HueGetRequestAsync<TemperatureResource>(TemperatureUrl);
    public Task<HueResponse<TemperatureResource>> GetTemperatureAsync(Guid id) => HueGetRequestAsync<TemperatureResource>(ResourceIdUrl(TemperatureUrl, id));
    public Task<HuePutResponse> UpdateTemperatureAsync(Guid id, UpdateSensorRequest data) => HuePutRequestAsync(ResourceIdUrl(TemperatureUrl, id), data);
    #endregion

    #region LightLevel
    public Task<HueResponse<LightLevel>> GetLightLevelsAsync() => HueGetRequestAsync<LightLevel>(LightLevelUrl);
    public Task<HueResponse<LightLevel>> GetLightLevelAsync(Guid id) => HueGetRequestAsync<LightLevel>(ResourceIdUrl(LightLevelUrl, id));
    public Task<HuePutResponse> UpdateLightLevelAsync(Guid id, UpdateSensorRequest data) => HuePutRequestAsync(ResourceIdUrl(LightLevelUrl, id), data);
    #endregion

    #region Button
    public Task<HueResponse<ButtonResource>> GetButtonsAsync() => HueGetRequestAsync<ButtonResource>(ButtonUrl);
    public Task<HueResponse<ButtonResource>> GetButtonAsync(Guid id) => HueGetRequestAsync<ButtonResource>(ResourceIdUrl(ButtonUrl, id));
    public Task<HuePutResponse> UpdateButtonAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(ButtonUrl, id), data);
    #endregion

    #region Relative Rotary
    public Task<HueResponse<RelativeRotaryResource>> GetRelativeRotaryAsync() => HueGetRequestAsync<RelativeRotaryResource>(RelativeRotaryUrl);
    public Task<HueResponse<RelativeRotaryResource>> GetRelativeRotaryAsync(Guid id) => HueGetRequestAsync<RelativeRotaryResource>(ResourceIdUrl(RelativeRotaryUrl, id));
    public Task<HuePutResponse> UpdateRelativeRotaryAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(RelativeRotaryUrl, id), data);
    #endregion

    #region BehaviorScript
    public Task<HueResponse<BehaviorScript>> GetBehaviorScriptsAsync() => HueGetRequestAsync<BehaviorScript>(BehaviorScriptUrl);
    public Task<HueResponse<BehaviorScript>> GetBehaviorScriptAsync(Guid id) => HueGetRequestAsync<BehaviorScript>(ResourceIdUrl(BehaviorScriptUrl, id));
    public Task<HuePutResponse> UpdateBehaviorScriptAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(BehaviorScriptUrl, id), data);
    #endregion

    #region BehaviorInstance
    public Task<HueResponse<BehaviorInstance>> GetBehaviorInstancesAsync() => HueGetRequestAsync<BehaviorInstance>(BehaviorInstanceUrl);
    public Task<HuePostResponse> CreateBehaviorInstanceAsync(UpdateBehaviorInstance data) => HuePostRequestAsync(BehaviorInstanceUrl, data);
    public Task<HueResponse<BehaviorInstance>> GetBehaviorInstanceAsync(Guid id) => HueGetRequestAsync<BehaviorInstance>(ResourceIdUrl(BehaviorInstanceUrl, id));
    public Task<HuePutResponse> UpdateBehaviorInstanceAsync(Guid id, UpdateBehaviorInstance data) => HuePutRequestAsync(ResourceIdUrl(BehaviorInstanceUrl, id), data);
    public Task<HueDeleteResponse> DeleteBehaviorInstanceAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(BehaviorInstanceUrl, id));
    #endregion

    #region GeofenceClient
    public Task<HueResponse<GeofenceClient>> GetGeofenceClientsAsync() => HueGetRequestAsync<GeofenceClient>(GeofenceClientUrl);
    public Task<HuePostResponse> CreateGeofenceClientAsync(UpdateGeofenceClient data) => HuePostRequestAsync(GeofenceClientUrl, data);
    public Task<HueResponse<GeofenceClient>> GetGeofenceClientAsync(Guid id) => HueGetRequestAsync<GeofenceClient>(ResourceIdUrl(GeofenceClientUrl, id));
    public Task<HuePutResponse> UpdateGeofenceClientAsync(Guid id, UpdateGeofenceClient data) => HuePutRequestAsync(ResourceIdUrl(GeofenceClientUrl, id), data);
    public Task<HueDeleteResponse> DeleteGeofenceClientAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(GeofenceClientUrl, id));
    #endregion

    #region Geolocation
    public Task<HueResponse<Geolocation>> GetGeolocationsAsync() => HueGetRequestAsync<Geolocation>(GeolocationUrl);
    public Task<HueResponse<Geolocation>> GetGeolocationAsync(Guid id) => HueGetRequestAsync<Geolocation>(ResourceIdUrl(GeolocationUrl, id));
    public Task<HuePutResponse> UpdateGeolocationAsync(Guid id, UpdateGeolocation data) => HuePutRequestAsync(ResourceIdUrl(GeolocationUrl, id), data);
    #endregion

    #region EntertainmentConfiguration
    public Task<HueResponse<EntertainmentConfiguration>> GetEntertainmentConfigurationsAsync() => HueGetRequestAsync<EntertainmentConfiguration>(EntertainmentConfigurationUrl);
    public Task<HuePostResponse> CreateEntertainmentConfigurationAsync(UpdateEntertainmentConfiguration data) => HuePostRequestAsync(EntertainmentConfigurationUrl, data);
    public Task<HueResponse<EntertainmentConfiguration>> GetEntertainmentConfigurationAsync(Guid id) => HueGetRequestAsync<EntertainmentConfiguration>(ResourceIdUrl(EntertainmentConfigurationUrl, id));
    public Task<HuePutResponse> UpdateEntertainmentConfigurationAsync(Guid id, UpdateEntertainmentConfiguration data) => HuePutRequestAsync(ResourceIdUrl(EntertainmentConfigurationUrl, id), data);
    public Task<HueDeleteResponse> DeleteEntertainmentConfigurationAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(EntertainmentConfigurationUrl, id));
    #endregion

    #region Entertainment
    public Task<HueResponse<Entertainment>> GetEntertainmentServicesAsync() => HueGetRequestAsync<Entertainment>(EntertainmentUrl);
    public Task<HueResponse<Entertainment>> GetEntertainmentServiceAsync(Guid id) => HueGetRequestAsync<Entertainment>(ResourceIdUrl(EntertainmentUrl, id));
    public Task<HuePutResponse> UpdateEntertainmentServiceAsync(Guid id, UpdateEntertainment data) => HuePutRequestAsync(ResourceIdUrl(EntertainmentUrl, id), data);
    #endregion

    #region Homekit
    public Task<HueResponse<Homekit>> GetHomekitsAsync() => HueGetRequestAsync<Homekit>(HomekitUrl);
    public Task<HueResponse<Homekit>> GetHomekitAsync(Guid id) => HueGetRequestAsync<Homekit>(ResourceIdUrl(HomekitUrl, id));
    public Task<HuePutResponse> UpdateHomekitAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(HomekitUrl, id), data);
    #endregion

    #region Resource
    public Task<HueResponse<HueResource>> GetResourcesAsync() => HueGetRequestAsync<HueResource>(ResourceUrl);
    #endregion

    #region Matter
    public Task<HueResponse<Models.MatterItem>> GetMatterItemsAsync() => HueGetRequestAsync<Models.MatterItem>(MatterUrl);
    public Task<HueResponse<Models.MatterItem>> GetMatterItemAsync(Guid id) => HueGetRequestAsync<Models.MatterItem>(ResourceIdUrl(MatterUrl, id));
    public Task<HuePutResponse> UpdateMatterItemAsync(Guid id, MatterItemUpdate data) => HuePutRequestAsync(ResourceIdUrl(MatterUrl, id), data);
    #endregion

    #region MatterFabric
    public Task<HueResponse<Models.MatterItem>> GetMatterFabricsAsync() => HueGetRequestAsync<Models.MatterItem>(MatterFabricUrl);
    public Task<HueResponse<Models.MatterItem>> GetMatterFabricAsync(Guid id) => HueGetRequestAsync<Models.MatterItem>(ResourceIdUrl(MatterFabricUrl, id));
    public Task<HueDeleteResponse> DeleteMatterFabricAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(MatterFabricUrl, id));
    #endregion

    #region Contact
    public Task<HueResponse<ContactSensor>> GetContactSensorsAsync() => HueGetRequestAsync<ContactSensor>(ContactUrl);
    public Task<HueResponse<ContactSensor>> GetContactSensorAsync(Guid id) => HueGetRequestAsync<ContactSensor>(ResourceIdUrl(ContactUrl, id));
    public Task<HuePutResponse> UpdateContactSensorsAsync(Guid id, UpdateSensorRequest data) => HuePutRequestAsync(ResourceIdUrl(ContactUrl, id), data);
    #endregion

    #region Tamper
    public Task<HueResponse<TamperSensor>> GetTamperSensorsAsync() => HueGetRequestAsync<TamperSensor>(TamperUrl);
    public Task<HueResponse<TamperSensor>> GetTamperSensorAsync(Guid id) => HueGetRequestAsync<TamperSensor>(ResourceIdUrl(TamperUrl, id));
    public Task<HuePutResponse> UpdateTamperSensorAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(TamperUrl, id), data);
    #endregion

    protected async Task<HueResponse<T>> HueGetRequestAsync<T>(string url)
    {
      var response = await client.GetAsync(url);

      return await ProcessResponseAsync<HueResponse<T>>(response);
    }

    protected async Task<HueDeleteResponse> HueDeleteRequestAsync(string url)
    {
      var response = await client.DeleteAsync(url);

      return await ProcessResponseAsync<HueDeleteResponse>(response);
    }

    protected async Task<HuePutResponse> HuePutRequestAsync<D>(string url, D data)
    {
      JsonSerializerOptions options = new()
      {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
      };

      var response = await client.PutAsJsonAsync(url, data, options);

      return await ProcessResponseAsync<HuePutResponse>(response);
    }

    protected async Task<HuePostResponse> HuePostRequestAsync<D>(string url, D data)
    {
      JsonSerializerOptions options = new()
      {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
      };

      var response = await client.PostAsJsonAsync(url, data, options);

      return await ProcessResponseAsync<HuePostResponse>(response);
    }


    protected async Task<T> ProcessResponseAsync<T>(HttpResponseMessage? response) where T : HueErrorResponse, new()
    {
      if (response == null)
        return new T();

      if (response.IsSuccessStatusCode)
      {
        return (await response.Content.ReadFromJsonAsync<T>()) ?? new();
      }
      else if(response.StatusCode == System.Net.HttpStatusCode.Forbidden)
      {
        throw new UnauthorizedAccessException();
      }
      else
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<HueErrorResponse>();

        var result = new T();
        if (errorResponse != null)
          result.Errors = errorResponse.Errors;

        return result;
      }
    }

   

  }
}
