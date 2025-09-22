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

  public  abstract partial class BaseHueApi
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

    protected string ResourceIdUrl(string resourceUrl, Guid id) => $"{resourceUrl}/{id}";


    #region Light
    [Obsolete("Use Light.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Light>> GetLightsAsync() => HueGetRequestAsync<Light>(LightUrl);
    [Obsolete("Use Light.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Light>> GetLightAsync(Guid id) => HueGetRequestAsync<Light>(ResourceIdUrl(LightUrl, id));
    [Obsolete("Use Light.UpdateAsync. This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateLightAsync(Guid id, UpdateLight data) => HuePutRequestAsync(ResourceIdUrl(LightUrl, id), data);
    #endregion

    #region Scene
    [Obsolete("Use Scene.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Scene>> GetScenesAsync() => HueGetRequestAsync<Scene>(SceneUrl);
    [Obsolete("Use Scene.CreateAsync(). This method will be removed in the future.")]
    public Task<HuePostResponse> CreateSceneAsync(CreateScene data) => HuePostRequestAsync(SceneUrl, data);
    [Obsolete("Use Scene.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Scene>> GetSceneAsync(Guid id) => HueGetRequestAsync<Scene>(ResourceIdUrl(SceneUrl, id));
    [Obsolete("Use Scene.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateSceneAsync(Guid id, UpdateScene data) => HuePutRequestAsync(ResourceIdUrl(SceneUrl, id), data);
    [Obsolete("Use Scene.DeleteAsync(). This method will be removed in the future.")]
    public Task<HueDeleteResponse> DeleteSceneAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(SceneUrl, id));
    #endregion

    #region SmartScene
    [Obsolete("Use SmartScene.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<SmartScene>> GetSmartScenesAsync() => HueGetRequestAsync<SmartScene>(SmartSceneUrl);
    [Obsolete("Use SmartScene.CreateAsync(). This method will be removed in the future.")]
    public Task<HuePostResponse> CreateSmartSceneAsync(CreateSmartScene data) => HuePostRequestAsync(SmartSceneUrl, data);
    [Obsolete("Use SmartScene.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<SmartScene>> GetSmartSceneAsync(Guid id) => HueGetRequestAsync<SmartScene>(ResourceIdUrl(SmartSceneUrl, id));
    [Obsolete("Use SmartScene.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateSmartSceneAsync(Guid id, UpdateSmartScene data) => HuePutRequestAsync(ResourceIdUrl(SmartSceneUrl, id), data);
    [Obsolete("Use SmartScene.DeleteAsync(). This method will be removed in the future.")]
    public Task<HueDeleteResponse> DeleteSmartSceneAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(SmartSceneUrl, id));
    #endregion

    #region Room
    [Obsolete("Use Room.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Room>> GetRoomsAsync() => HueGetRequestAsync<Room>(RoomUrl);
    [Obsolete("Use Room.CreateAsync(). This method will be removed in the future.")]
    public Task<HuePostResponse> CreateRoomAsync(BaseResourceRequest data) => HuePostRequestAsync(RoomUrl, data);
    [Obsolete("Use Room.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Room>> GetRoomAsync(Guid id) => HueGetRequestAsync<Room>(ResourceIdUrl(RoomUrl, id));
    [Obsolete("Use Room.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateRoomAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(RoomUrl, id), data);
    [Obsolete("Use Room.DeleteAsync(). This method will be removed in the future.")]
    public Task<HueDeleteResponse> DeleteRoomAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(RoomUrl, id));
    #endregion

    #region Zone
    [Obsolete("Use Zone.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Zone>> GetZonesAsync() => HueGetRequestAsync<Zone>(ZoneUrl);
    [Obsolete("Use Zone.CreateAsync(). This method will be removed in the future.")]
    public Task<HuePostResponse> CreateZoneAsync(CreateZone data) => HuePostRequestAsync(ZoneUrl, data);
    [Obsolete("Use Zone.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Zone>> GetZoneAsync(Guid id) => HueGetRequestAsync<Zone>(ResourceIdUrl(ZoneUrl, id));
    [Obsolete("Use Zone.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateZoneAsync(Guid id, UpdateZone data) => HuePutRequestAsync(ResourceIdUrl(ZoneUrl, id), data);
    [Obsolete("Use Zone.DeleteAsync(). This method will be removed in the future.")]
    public Task<HueDeleteResponse> DeleteZoneAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(ZoneUrl, id));
    #endregion

    #region BridgeHome
    [Obsolete("Use BridgeHome.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<BridgeHome>> GetBridgeHomesAsync() => HueGetRequestAsync<BridgeHome>(BridgeHomeUrl);
    [Obsolete("Use BridgeHome.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<BridgeHome>> GetBridgeHomeAsync(Guid id) => HueGetRequestAsync<BridgeHome>(ResourceIdUrl(BridgeHomeUrl, id));
    [Obsolete("Use BridgeHome.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateBridgeHomeAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(BridgeHomeUrl, id), data);
    #endregion

    #region GroupedLight
    [Obsolete("Use GroupedLight.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<GroupedLight>> GetGroupedLightsAsync() => HueGetRequestAsync<GroupedLight>(GroupedLightUrl);
    [Obsolete("Use GroupedLight.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<GroupedLight>> GetGroupedLightAsync(Guid id) => HueGetRequestAsync<GroupedLight>(ResourceIdUrl(GroupedLightUrl, id));
    [Obsolete("Use GroupedLight.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateGroupedLightAsync(Guid id, UpdateGroupedLight data) => HuePutRequestAsync(ResourceIdUrl(GroupedLightUrl, id), data);
    #endregion

    #region Device
    [Obsolete("Use Device.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Device>> GetDevicesAsync() => HueGetRequestAsync<Device>(DeviceUrl);
    [Obsolete("Use Device.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Device>> GetDeviceAsync(Guid id) => HueGetRequestAsync<Device>(ResourceIdUrl(DeviceUrl, id));
    [Obsolete("Use Device.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateDeviceAsync(Guid id, UpdateDevice data) => HuePutRequestAsync(ResourceIdUrl(DeviceUrl, id), data);
    #endregion

    #region Bridge
    [Obsolete("Use Bridge.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Bridge>> GetBridgesAsync() => HueGetRequestAsync<Bridge>(BridgeUrl);
    [Obsolete("Use Bridge.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Bridge>> GetBridgeAsync(Guid id) => HueGetRequestAsync<Bridge>(ResourceIdUrl(BridgeUrl, id));
    [Obsolete("Use Bridge.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateBridgeAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(BridgeUrl, id), data);
    #endregion

    #region DeviceSoftwareUpdate
    [Obsolete("Use DeviceSoftwareUpdate.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<DeviceSoftwareUpdate>> GetDeviceSoftwareUpdatesAsync() => HueGetRequestAsync<DeviceSoftwareUpdate>(DeviceSoftwareUpdateUrl);
    [Obsolete("Use DeviceSoftwareUpdate.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<DeviceSoftwareUpdate>> GetDeviceSoftwareUpdateAsync(Guid id) => HueGetRequestAsync<DeviceSoftwareUpdate>(ResourceIdUrl(DeviceSoftwareUpdateUrl, id));
    [Obsolete("Use DeviceSoftwareUpdate.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateDeviceSoftwareUpdateAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(DeviceSoftwareUpdateUrl, id), data);
    #endregion

    #region DevicePower
    [Obsolete("Use DevicePower.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<DevicePower>> GetDevicePowersAsync() => HueGetRequestAsync<DevicePower>(DevicePowerUrl);
    [Obsolete("Use DevicePower.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<DevicePower>> GetDevicePowerAsync(Guid id) => HueGetRequestAsync<DevicePower>(ResourceIdUrl(DevicePowerUrl, id));
    [Obsolete("Use DevicePower.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateDevicePowerAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(DevicePowerUrl, id), data);
    #endregion

    #region ZigbeeConnectivity
    [Obsolete("Use ZigbeeConnectivity.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ZigbeeConnectivity>> GetZigbeeConnectivityAsync() => HueGetRequestAsync<ZigbeeConnectivity>(ZigbeeConnectivityUrl);
    [Obsolete("Use ZigbeeConnectivity.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ZigbeeConnectivity>> GetZigbeeConnectivityAsync(Guid id) => HueGetRequestAsync<ZigbeeConnectivity>(ResourceIdUrl(ZigbeeConnectivityUrl, id));
    [Obsolete("Use ZigbeeConnectivity.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateZigbeeConnectivityAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(ZigbeeConnectivityUrl, id), data);
    #endregion

    #region ZgpConnectivity
    [Obsolete("Use ZgpConnectivity.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ZgpConnectivity>> GetZgpConnectivityAsync() => HueGetRequestAsync<ZgpConnectivity>(ZgpConnectivityUrl);
    [Obsolete("Use ZgpConnectivity.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ZgpConnectivity>> GetZgpConnectivityAsync(Guid id) => HueGetRequestAsync<ZgpConnectivity>(ResourceIdUrl(ZgpConnectivityUrl, id));
    [Obsolete("Use ZgpConnectivity.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateZgpConnectivityAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(ZgpConnectivityUrl, id), data);
    #endregion

    #region ZigbeeDeviceDiscovery
    [Obsolete("Use ZigbeeDeviceDiscovery.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ZigbeeDeviceDiscovery>> GetZigbeeDeviceDiscoveryAsync() => HueGetRequestAsync<ZigbeeDeviceDiscovery>(ZigbeeDeviceDiscoveryUrl);
    [Obsolete("Use ZigbeeDeviceDiscovery.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ZigbeeDeviceDiscovery>> GetZigbeeDeviceDiscoveryAsync(Guid id) => HueGetRequestAsync<ZigbeeDeviceDiscovery>(ResourceIdUrl(ZigbeeDeviceDiscoveryUrl, id));
    [Obsolete("Use ZigbeeDeviceDiscovery.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateZigbeeDeviceDiscoveryAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(ZigbeeDeviceDiscoveryUrl, id), data);
    #endregion

    #region Motion
    [Obsolete("Use Motion.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<MotionResource>> GetMotionsAsync() => HueGetRequestAsync<MotionResource>(MotionUrl);
    [Obsolete("Use Motion.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<MotionResource>> GetMotionAsync(Guid id) => HueGetRequestAsync<MotionResource>(ResourceIdUrl(MotionUrl, id));
    [Obsolete("Use Motion.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateMotionAsync(Guid id, UpdateSensitivitySensorRequest data) => HuePutRequestAsync(ResourceIdUrl(MotionUrl, id), data);
    #endregion

    #region ServiceGroup
    [Obsolete("Use ServiceGroup.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ServiceGroupResource>> GetServiceGroupsAsync() => HueGetRequestAsync<ServiceGroupResource>(ServiceGroupUrl);
    [Obsolete("Use ServiceGroup.CreateAsync(). This method will be removed in the future.")]
    public Task<HuePostResponse> CreateServiceGroupAsync(CreateUpdateServiceGroup data) => HuePostRequestAsync(ServiceGroupUrl, data);
    [Obsolete("Use ServiceGroup.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ServiceGroupResource>> GetServiceGroupAsync(Guid id) => HueGetRequestAsync<ServiceGroupResource>(ResourceIdUrl(ServiceGroupUrl, id));
    [Obsolete("Use ServiceGroup.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateServiceGroupAsync(Guid id, CreateUpdateServiceGroup data) => HuePutRequestAsync(ResourceIdUrl(ServiceGroupUrl, id), data);
    [Obsolete("Use ServiceGroup.DeleteAsync(). This method will be removed in the future.")]
    public Task<HueDeleteResponse> DeleteServiceGroupAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(ServiceGroupUrl, id));
    #endregion

    #region GroupedMotion
    [Obsolete("Use GroupedMotion.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<GroupedMotionResource>> GetGroupedMotionsAsync() => HueGetRequestAsync<GroupedMotionResource>(GroupedMotionUrl);
    [Obsolete("Use GroupedMotion.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<GroupedMotionResource>> GetGroupedMotionAsync(Guid id) => HueGetRequestAsync<GroupedMotionResource>(ResourceIdUrl(GroupedMotionUrl, id));
    [Obsolete("Use GroupedMotion.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateGroupedMotionAsync(Guid id, UpdateGroupedMotionRequest data) => HuePutRequestAsync(ResourceIdUrl(GroupedMotionUrl, id), data);
    #endregion

    #region GroupedLightLevel
    [Obsolete("Use GroupedLightLevel.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<GroupedLightLevelResource>> GetGroupedLightLevelsAsync() => HueGetRequestAsync<GroupedLightLevelResource>(GroupedLightLevelUrl);
    [Obsolete("Use GroupedLightLevel.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<GroupedLightLevelResource>> GetGroupedLightLevelAsync(Guid id) => HueGetRequestAsync<GroupedLightLevelResource>(ResourceIdUrl(GroupedLightLevelUrl, id));
    [Obsolete("Use GroupedLightLevel.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateGroupedLightLevelAsync(Guid id, UpdateGroupedLightLevelRequest data) => HuePutRequestAsync(ResourceIdUrl(GroupedLightLevelUrl, id), data);
    #endregion

    #region CameraMotion
    [Obsolete("Use CameraMotion.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<CameraMotionResource>> GetCameraMotionsAsync() => HueGetRequestAsync<CameraMotionResource>(CameraMotionUrl);
    [Obsolete("Use CameraMotion.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<CameraMotionResource>> GetCameraMotionAsync(Guid id) => HueGetRequestAsync<CameraMotionResource>(ResourceIdUrl(CameraMotionUrl, id));
    [Obsolete("Use CameraMotion.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateCameraMotionAsync(Guid id, UpdateSensitivitySensorRequest data) => HuePutRequestAsync(ResourceIdUrl(CameraMotionUrl, id), data);
    #endregion

    #region Temperature
    [Obsolete("Use Temperature.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<TemperatureResource>> GetTemperaturesAsync() => HueGetRequestAsync<TemperatureResource>(TemperatureUrl);
    [Obsolete("Use Temperature.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<TemperatureResource>> GetTemperatureAsync(Guid id) => HueGetRequestAsync<TemperatureResource>(ResourceIdUrl(TemperatureUrl, id));
    [Obsolete("Use Temperature.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateTemperatureAsync(Guid id, UpdateSensorRequest data) => HuePutRequestAsync(ResourceIdUrl(TemperatureUrl, id), data);
    #endregion

    #region LightLevel
    [Obsolete("Use LightLevel.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<LightLevelResource>> GetLightLevelsAsync() => HueGetRequestAsync<LightLevelResource>(LightLevelUrl);
    [Obsolete("Use LightLevel.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<LightLevelResource>> GetLightLevelAsync(Guid id) => HueGetRequestAsync<LightLevelResource>(ResourceIdUrl(LightLevelUrl, id));
    [Obsolete("Use LightLevel.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateLightLevelAsync(Guid id, UpdateSensorRequest data) => HuePutRequestAsync(ResourceIdUrl(LightLevelUrl, id), data);
    #endregion

    #region Button
    [Obsolete("Use Button.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ButtonResource>> GetButtonsAsync() => HueGetRequestAsync<ButtonResource>(ButtonUrl);
    [Obsolete("Use Button.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ButtonResource>> GetButtonAsync(Guid id) => HueGetRequestAsync<ButtonResource>(ResourceIdUrl(ButtonUrl, id));
    [Obsolete("Use Button.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateButtonAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(ButtonUrl, id), data);
    #endregion

    #region BellButton
    [Obsolete("Use BellButton.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<BellButtonResource>> GetBellButtonsAsync() => HueGetRequestAsync<BellButtonResource>(BellButtonUrl);
    [Obsolete("Use BellButton.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<BellButtonResource>> GetBellButtonAsync(Guid id) => HueGetRequestAsync<BellButtonResource>(ResourceIdUrl(BellButtonUrl, id));
    [Obsolete("Use BellButton.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateBellButtonAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(BellButtonUrl, id), data);
    #endregion

    #region RelativeRotary
    [Obsolete("Use RelativeRotary.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<RelativeRotaryResource>> GetRelativeRotaryAsync() => HueGetRequestAsync<RelativeRotaryResource>(RelativeRotaryUrl);
    [Obsolete("Use RelativeRotary.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<RelativeRotaryResource>> GetRelativeRotaryAsync(Guid id) => HueGetRequestAsync<RelativeRotaryResource>(ResourceIdUrl(RelativeRotaryUrl, id));
    [Obsolete("Use RelativeRotary.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateRelativeRotaryAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(RelativeRotaryUrl, id), data);
    #endregion

    #region BehaviorScript
    [Obsolete("Use BehaviorScript.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<BehaviorScript>> GetBehaviorScriptsAsync() => HueGetRequestAsync<BehaviorScript>(BehaviorScriptUrl);
    [Obsolete("Use BehaviorScript.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<BehaviorScript>> GetBehaviorScriptAsync(Guid id) => HueGetRequestAsync<BehaviorScript>(ResourceIdUrl(BehaviorScriptUrl, id));
    [Obsolete("Use BehaviorScript.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateBehaviorScriptAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(BehaviorScriptUrl, id), data);
    #endregion

    #region BehaviorInstance
    [Obsolete("Use BehaviorInstance.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<BehaviorInstance>> GetBehaviorInstancesAsync() => HueGetRequestAsync<BehaviorInstance>(BehaviorInstanceUrl);
    [Obsolete("Use BehaviorInstance.CreateAsync(). This method will be removed in the future.")]
    public Task<HuePostResponse> CreateBehaviorInstanceAsync(UpdateBehaviorInstance data) => HuePostRequestAsync(BehaviorInstanceUrl, data);
    [Obsolete("Use BehaviorInstance.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<BehaviorInstance>> GetBehaviorInstanceAsync(Guid id) => HueGetRequestAsync<BehaviorInstance>(ResourceIdUrl(BehaviorInstanceUrl, id));
    [Obsolete("Use BehaviorInstance.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateBehaviorInstanceAsync(Guid id, UpdateBehaviorInstance data) => HuePutRequestAsync(ResourceIdUrl(BehaviorInstanceUrl, id), data);
    [Obsolete("Use BehaviorInstance.DeleteAsync(). This method will be removed in the future.")]
    public Task<HueDeleteResponse> DeleteBehaviorInstanceAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(BehaviorInstanceUrl, id));
    #endregion

    #region GeofenceClient
    [Obsolete("Use GeofenceClient.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<GeofenceClient>> GetGeofenceClientsAsync() => HueGetRequestAsync<GeofenceClient>(GeofenceClientUrl);
    [Obsolete("Use GeofenceClient.CreateAsync(). This method will be removed in the future.")]
    public Task<HuePostResponse> CreateGeofenceClientAsync(UpdateGeofenceClient data) => HuePostRequestAsync(GeofenceClientUrl, data);
    [Obsolete("Use GeofenceClient.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<GeofenceClient>> GetGeofenceClientAsync(Guid id) => HueGetRequestAsync<GeofenceClient>(ResourceIdUrl(GeofenceClientUrl, id));
    [Obsolete("Use GeofenceClient.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateGeofenceClientAsync(Guid id, UpdateGeofenceClient data) => HuePutRequestAsync(ResourceIdUrl(GeofenceClientUrl, id), data);
    [Obsolete("Use GeofenceClient.DeleteAsync(). This method will be removed in the future.")]
    public Task<HueDeleteResponse> DeleteGeofenceClientAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(GeofenceClientUrl, id));
    #endregion

    #region Geolocation
    [Obsolete("Use Geolocation.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Geolocation>> GetGeolocationsAsync() => HueGetRequestAsync<Geolocation>(GeolocationUrl);
    [Obsolete("Use Geolocation.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Geolocation>> GetGeolocationAsync(Guid id) => HueGetRequestAsync<Geolocation>(ResourceIdUrl(GeolocationUrl, id));
    [Obsolete("Use Geolocation.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateGeolocationAsync(Guid id, UpdateGeolocation data) => HuePutRequestAsync(ResourceIdUrl(GeolocationUrl, id), data);
    #endregion

    #region EntertainmentConfiguration
    [Obsolete("Use EntertainmentConfiguration.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<EntertainmentConfiguration>> GetEntertainmentConfigurationsAsync() => HueGetRequestAsync<EntertainmentConfiguration>(EntertainmentConfigurationUrl);
    [Obsolete("Use EntertainmentConfiguration.CreateAsync(). This method will be removed in the future.")]
    public Task<HuePostResponse> CreateEntertainmentConfigurationAsync(UpdateEntertainmentConfiguration data) => HuePostRequestAsync(EntertainmentConfigurationUrl, data);
    [Obsolete("Use EntertainmentConfiguration.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<EntertainmentConfiguration>> GetEntertainmentConfigurationAsync(Guid id) => HueGetRequestAsync<EntertainmentConfiguration>(ResourceIdUrl(EntertainmentConfigurationUrl, id));
    [Obsolete("Use EntertainmentConfiguration.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateEntertainmentConfigurationAsync(Guid id, UpdateEntertainmentConfiguration data) => HuePutRequestAsync(ResourceIdUrl(EntertainmentConfigurationUrl, id), data);
    [Obsolete("Use EntertainmentConfiguration.DeleteAsync(). This method will be removed in the future.")]
    public Task<HueDeleteResponse> DeleteEntertainmentConfigurationAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(EntertainmentConfigurationUrl, id));
    #endregion

    #region Entertainment
    [Obsolete("Use Entertainment.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Entertainment>> GetEntertainmentServicesAsync() => HueGetRequestAsync<Entertainment>(EntertainmentUrl);
    [Obsolete("Use Entertainment.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Entertainment>> GetEntertainmentServiceAsync(Guid id) => HueGetRequestAsync<Entertainment>(ResourceIdUrl(EntertainmentUrl, id));
    [Obsolete("Use Entertainment.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateEntertainmentServiceAsync(Guid id, UpdateEntertainment data) => HuePutRequestAsync(ResourceIdUrl(EntertainmentUrl, id), data);
    #endregion

    #region Homekit
    [Obsolete("Use Homekit.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Homekit>> GetHomekitsAsync() => HueGetRequestAsync<Homekit>(HomekitUrl);
    [Obsolete("Use Homekit.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Homekit>> GetHomekitAsync(Guid id) => HueGetRequestAsync<Homekit>(ResourceIdUrl(HomekitUrl, id));
    [Obsolete("Use Homekit.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateHomekitAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(HomekitUrl, id), data);
    #endregion

    #region Resource
    [Obsolete("Use Resource.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<HueResource>> GetResourcesAsync() => HueGetRequestAsync<HueResource>(ResourceUrl);
    #endregion

    #region Matter
    [Obsolete("Use Matter.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Models.MatterItem>> GetMatterItemsAsync() => HueGetRequestAsync<Models.MatterItem>(MatterUrl);
    [Obsolete("Use Matter.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Models.MatterItem>> GetMatterItemAsync(Guid id) => HueGetRequestAsync<Models.MatterItem>(ResourceIdUrl(MatterUrl, id));
    [Obsolete("Use Matter.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateMatterItemAsync(Guid id, MatterItemUpdate data) => HuePutRequestAsync(ResourceIdUrl(MatterUrl, id), data);
    #endregion

    #region MatterFabric
    [Obsolete("Use MatterFabric.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Models.MatterFabric>> GetMatterFabricsAsync() => HueGetRequestAsync<Models.MatterFabric>(MatterFabricUrl);
    [Obsolete("Use MatterFabric.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<Models.MatterFabric>> GetMatterFabricAsync(Guid id) => HueGetRequestAsync<Models.MatterFabric>(ResourceIdUrl(MatterFabricUrl, id));
    [Obsolete("Use MatterFabric.DeleteAsync(). This method will be removed in the future.")]
    public Task<HueDeleteResponse> DeleteMatterFabricAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(MatterFabricUrl, id));
    #endregion

    #region Contact
    [Obsolete("Use ContactSensor.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ContactSensor>> GetContactSensorsAsync() => HueGetRequestAsync<ContactSensor>(ContactUrl);
    [Obsolete("Use ContactSensor.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ContactSensor>> GetContactSensorAsync(Guid id) => HueGetRequestAsync<ContactSensor>(ResourceIdUrl(ContactUrl, id));
    [Obsolete("Use ContactSensor.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateContactSensorsAsync(Guid id, UpdateSensorRequest data) => HuePutRequestAsync(ResourceIdUrl(ContactUrl, id), data);
    #endregion

    #region Tamper
    [Obsolete("Use TamperSensor.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<TamperSensor>> GetTamperSensorsAsync() => HueGetRequestAsync<TamperSensor>(TamperUrl);
    [Obsolete("Use TamperSensor.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<TamperSensor>> GetTamperSensorAsync(Guid id) => HueGetRequestAsync<TamperSensor>(ResourceIdUrl(TamperUrl, id));
    [Obsolete("Use TamperSensor.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateTamperSensorAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(TamperUrl, id), data);
    #endregion

    #region MotionAreaConfiguration
    [Obsolete("Use MotionAreaConfig.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<MotionAreaConfigResource>> GetMotionAreaConfigsAsync() => HueGetRequestAsync<MotionAreaConfigResource>(MotionAreaConfigUrl);
    [Obsolete("Use MotionAreaConfig.CreateAsync(). This method will be removed in the future.")]
    public Task<HuePostResponse> CreateMotionAreaConfigAsync(CreateUpdateMotionAreaConfig data) => HuePostRequestAsync(MotionAreaConfigUrl, data);
    [Obsolete("Use MotionAreaConfig.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<MotionAreaConfigResource>> GetMotionAreaConfigAsync(Guid id) => HueGetRequestAsync<MotionAreaConfigResource>(ResourceIdUrl(MotionAreaConfigUrl, id));
    [Obsolete("Use MotionAreaConfig.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateMotionAreaConfigAsync(Guid id, CreateUpdateMotionAreaConfig data) => HuePutRequestAsync(ResourceIdUrl(MotionAreaConfigUrl, id), data);
    [Obsolete("Use MotionAreaConfig.DeleteAsync(). This method will be removed in the future.")]
    public Task<HueDeleteResponse> DeleteMotionAreaConfigAsync(Guid id) => HueDeleteRequestAsync(ResourceIdUrl(MotionAreaConfigUrl, id));
    #endregion

    #region MotionAreaCandidate
    [Obsolete("Use MotionAreaCandidate.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<MotionAreaCandidateResource>> GetMotionAreaCandidatesAsync() => HueGetRequestAsync<MotionAreaCandidateResource>(MotionAreaCandidateUrl);
    [Obsolete("Use MotionAreaCandidate.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<MotionAreaCandidateResource>> GetMotionAreaCandidateAsync(Guid id) => HueGetRequestAsync<MotionAreaCandidateResource>(ResourceIdUrl(MotionAreaCandidateUrl, id));
    [Obsolete("Use MotionAreaCandidate.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateMotionAreaCandidateAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(MotionAreaCandidateUrl, id), data);
    #endregion

    #region ConvenienceAreaMotion
    [Obsolete("Use ConvenienceAreaMotion.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ConvenienceAreaMotionResource>> GetConvenienceAreaMotionsAsync() => HueGetRequestAsync<ConvenienceAreaMotionResource>(ConvenienceAreaMotionUrl);
    [Obsolete("Use ConvenienceAreaMotion.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ConvenienceAreaMotionResource>> GetConvenienceAreaMotionAsync(Guid id) => HueGetRequestAsync<ConvenienceAreaMotionResource>(ResourceIdUrl(ConvenienceAreaMotionUrl, id));
    [Obsolete("Use ConvenienceAreaMotion.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateConvenienceAreaMotionAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(ConvenienceAreaMotionUrl, id), data);
    #endregion

    #region SecurityAreaMotion
    [Obsolete("Use SecurityAreaMotion.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<SecurityAreaMotionResource>> GetSecurityAreaMotionsAsync() => HueGetRequestAsync<SecurityAreaMotionResource>(SecurityAreaMotionUrl);
    [Obsolete("Use SecurityAreaMotion.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<SecurityAreaMotionResource>> GetSecurityAreaMotionAsync(Guid id) => HueGetRequestAsync<SecurityAreaMotionResource>(ResourceIdUrl(SecurityAreaMotionUrl, id));
    [Obsolete("Use SecurityAreaMotion.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateSecurityAreaMotionAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(SecurityAreaMotionUrl, id), data);
    #endregion

    #region Speaker
    [Obsolete("Use Speaker.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<SpeakerResource>> GetSpeakersAsync() => HueGetRequestAsync<SpeakerResource>(SpeakerUrl);
    [Obsolete("Use Speaker.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<SpeakerResource>> GetSpeakerAsync(Guid id) => HueGetRequestAsync<SpeakerResource>(ResourceIdUrl(SpeakerUrl, id));
    [Obsolete("Use Speaker.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateSpeakerAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(SpeakerUrl, id), data);
    #endregion

    #region Clip
    [Obsolete("Use Clip.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ClipResource>> GetClipsAsync() => HueGetRequestAsync<ClipResource>(ClipUrl);
    [Obsolete("Use Clip.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<ClipResource>> GetClipAsync(Guid id) => HueGetRequestAsync<ClipResource>(ResourceIdUrl(ClipUrl, id));
    [Obsolete("Use Clip.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateClipAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(ClipUrl, id), data);
    #endregion

    #region WifiConnectivity
    [Obsolete("Use WifiConnectivity.GetAllAsync(). This method will be removed in the future.")]
    public Task<HueResponse<WifiConnectivityResource>> GetWifiConnectivitysAsync() => HueGetRequestAsync<WifiConnectivityResource>(WifiConnectivityUrl);
    [Obsolete("Use WifiConnectivity.GetByIdAsync(). This method will be removed in the future.")]
    public Task<HueResponse<WifiConnectivityResource>> GetWifiConnectivityAsync(Guid id) => HueGetRequestAsync<WifiConnectivityResource>(ResourceIdUrl(WifiConnectivityUrl, id));
    [Obsolete("Use WifiConnectivity.UpdateAsync(). This method will be removed in the future.")]
    public Task<HuePutResponse> UpdateWifiConnectivityAsync(Guid id, BaseResourceRequest data) => HuePutRequestAsync(ResourceIdUrl(WifiConnectivityUrl, id), data);
    #endregion


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
      else if(response.StatusCode == System.Net.HttpStatusCode.Forbidden)
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
