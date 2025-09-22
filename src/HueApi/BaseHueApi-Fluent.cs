using HueApi.HueEndpoints;
using HueApi.Models;
using HueApi.Models.Requests;
using HueApi.Models.Requests.SmartScene;
using HueApi.Models.Sensors;

namespace HueApi
{
  public abstract partial class BaseHueApi
  {
    public ResourceEndpoint<HueResource> Resource { get; }
    public ReadEditEndpoint<Light, UpdateLight> Light { get; }
    public CrudEndpoint<Scene, CreateScene, UpdateScene> Scene { get; }
    public CrudEndpoint<Room, BaseResourceRequest, BaseResourceRequest> Room { get; }
    public CrudEndpoint<Zone, CreateZone, UpdateZone> Zone { get; }
    public ReadOnlyEndpoint<BridgeHome> BridgeHome { get; }
    public ReadEditEndpoint<GroupedLight, UpdateGroupedLight> GroupedLight { get; }
    public CrudWithoutCreateEndpoint<Device, UpdateDevice> Device { get; }
    public ReadEditEndpoint<Bridge, BaseResourceRequest> Bridge { get; }
    public ReadEditEndpoint<DeviceSoftwareUpdate, BaseResourceRequest> DeviceSoftwareUpdate { get; }
    public ReadEditEndpoint<DevicePower, BaseResourceRequest> DevicePower { get; }
    public ReadEditEndpoint<ZigbeeConnectivity, BaseResourceRequest> ZigbeeConnectivity { get; }
    public ReadEditEndpoint<ZgpConnectivity, BaseResourceRequest> ZgpConnectivity { get; }
    public ReadEditEndpoint<ZigbeeDeviceDiscovery, BaseResourceRequest> ZigbeeDeviceDiscovery { get; }
    public ReadEditEndpoint<MotionResource, UpdateSensitivitySensorRequest> Motion { get; }
    public CrudEndpoint<ServiceGroupResource, CreateUpdateServiceGroup, CreateUpdateServiceGroup> ServiceGroup { get; }
    public ReadEditEndpoint<GroupedMotionResource, UpdateGroupedMotionRequest> GroupedMotion { get; }
    public ReadEditEndpoint<GroupedLightLevelResource, UpdateGroupedLightLevelRequest> GroupedLightLevel { get; }
    public ReadEditEndpoint<CameraMotionResource, UpdateSensitivitySensorRequest> CameraMotion { get; }
    public ReadEditEndpoint<TemperatureResource, UpdateSensorRequest> Temperature { get; }
    public ReadEditEndpoint<LightLevelResource, UpdateSensorRequest> LightLevel { get; }
    public ReadEditEndpoint<ButtonResource, BaseResourceRequest> Button { get; }
    public ReadEditEndpoint<BellButtonResource, BaseResourceRequest> BellButton { get; }
    public ReadEditEndpoint<RelativeRotaryResource, BaseResourceRequest> RelativeRotary { get; }
    public ReadEditEndpoint<BehaviorScript, BaseResourceRequest> BehaviorScript { get; }
    public CrudEndpoint<BehaviorInstance, UpdateBehaviorInstance, UpdateBehaviorInstance> BehaviorInstance { get; }
    public CrudEndpoint<GeofenceClient, UpdateGeofenceClient, UpdateGeofenceClient> GeofenceClient { get; }
    public ReadEditEndpoint<Geolocation, UpdateGeolocation> Geolocation { get; }
    public CrudEndpoint<EntertainmentConfiguration, UpdateEntertainmentConfiguration, UpdateEntertainmentConfiguration> EntertainmentConfiguration { get; }
    public ReadEditEndpoint<Entertainment, UpdateEntertainment> Entertainment { get; }
    public ReadEditEndpoint<Homekit, BaseResourceRequest> Homekit { get; }
    public ReadEditEndpoint<MatterItem, MatterItemUpdate> Matter { get; }
    public ReadDeleteEndpoint<MatterFabric> MatterFabric { get; }
    public CrudEndpoint<SmartScene, CreateSmartScene, UpdateSmartScene> SmartScene { get; }
    public ReadEditEndpoint<ContactSensor, UpdateSensorRequest> Contact { get; }
    public ReadEditEndpoint<TamperSensor, BaseResourceRequest> Tamper { get; }
    public CrudEndpoint<MotionAreaConfigResource, CreateUpdateMotionAreaConfig, CreateUpdateMotionAreaConfig> MotionAreaConfiguration { get; }
    public ReadEditEndpoint<MotionAreaCandidateResource, BaseResourceRequest> MotionAreaCandidate { get; }
    public ReadEditEndpoint<ConvenienceAreaMotionResource, BaseResourceRequest> ConvenienceAreaMotion { get; }
    public ReadEditEndpoint<SecurityAreaMotionResource, BaseResourceRequest> SecurityAreaMotion { get; }
    public ReadEditEndpoint<SpeakerResource, BaseResourceRequest> Speaker { get; }
    public ReadEditEndpoint<ClipResource, BaseResourceRequest> Clip { get; }
    public ReadEditEndpoint<WifiConnectivityResource, BaseResourceRequest> WifiConnectivity { get; }


    protected BaseHueApi()
    {
      Resource = new ResourceEndpoint<HueResource>(this);
      Light = new ReadEditEndpoint<Light, UpdateLight>(this, "light");
      Scene = new CrudEndpoint<Scene, CreateScene, UpdateScene>(this, "scene");
      Room = new CrudEndpoint<Room, BaseResourceRequest, BaseResourceRequest>(this, "room");
      Zone = new CrudEndpoint<Zone, CreateZone, UpdateZone>(this, "zone");
      BridgeHome = new ReadOnlyEndpoint<BridgeHome>(this, "bridge_home");
      GroupedLight = new ReadEditEndpoint<GroupedLight, UpdateGroupedLight>(this, "grouped_light");
      Device = new CrudWithoutCreateEndpoint<Device, UpdateDevice>(this, "device");
      Bridge = new ReadEditEndpoint<Bridge, BaseResourceRequest>(this, "bridge");
      DeviceSoftwareUpdate = new ReadEditEndpoint<DeviceSoftwareUpdate, BaseResourceRequest>(this, "device_software_update");
      DevicePower = new ReadEditEndpoint<DevicePower, BaseResourceRequest>(this, "device_power");
      ZigbeeConnectivity = new ReadEditEndpoint<ZigbeeConnectivity, BaseResourceRequest>(this, "zigbee_connectivity");
      ZgpConnectivity = new ReadEditEndpoint<ZgpConnectivity, BaseResourceRequest>(this, "zgp_connectivity");
      ZigbeeDeviceDiscovery = new ReadEditEndpoint<ZigbeeDeviceDiscovery, BaseResourceRequest>(this, "zigbee_device_discovery");
      Motion = new ReadEditEndpoint<MotionResource, UpdateSensitivitySensorRequest>(this, "motion");
      ServiceGroup = new CrudEndpoint<ServiceGroupResource, CreateUpdateServiceGroup, CreateUpdateServiceGroup>(this, "service_group");
      GroupedMotion = new ReadEditEndpoint<GroupedMotionResource, UpdateGroupedMotionRequest>(this, "grouped_motion");
      GroupedLightLevel = new ReadEditEndpoint<GroupedLightLevelResource, UpdateGroupedLightLevelRequest>(this, "grouped_light_level");
      CameraMotion = new ReadEditEndpoint<CameraMotionResource, UpdateSensitivitySensorRequest>(this, "camera_motion");
      Temperature = new ReadEditEndpoint<TemperatureResource, UpdateSensorRequest>(this, "temperature");
      LightLevel = new ReadEditEndpoint<LightLevelResource, UpdateSensorRequest>(this, "light_level");
      Button = new ReadEditEndpoint<ButtonResource, BaseResourceRequest>(this, "button");
      BellButton = new ReadEditEndpoint<BellButtonResource, BaseResourceRequest>(this, "bell_button");
      RelativeRotary = new ReadEditEndpoint<RelativeRotaryResource, BaseResourceRequest>(this, "relative_rotary");
      BehaviorScript = new ReadEditEndpoint<BehaviorScript, BaseResourceRequest>(this, "behavior_script");
      BehaviorInstance = new CrudEndpoint<BehaviorInstance, UpdateBehaviorInstance, UpdateBehaviorInstance>(this, "behavior_instance");
      GeofenceClient = new CrudEndpoint<GeofenceClient, UpdateGeofenceClient, UpdateGeofenceClient>(this, "geofence_client");
      Geolocation = new ReadEditEndpoint<Geolocation, UpdateGeolocation>(this, "geolocation");
      EntertainmentConfiguration = new CrudEndpoint<EntertainmentConfiguration, UpdateEntertainmentConfiguration, UpdateEntertainmentConfiguration>(this, "entertainment_configuration");
      Entertainment = new ReadEditEndpoint<Entertainment, UpdateEntertainment>(this, "entertainment");
      Homekit = new ReadEditEndpoint<Homekit, BaseResourceRequest>(this, "homekit");
      Matter = new ReadEditEndpoint<MatterItem, MatterItemUpdate>(this, "matter");
      MatterFabric = new ReadDeleteEndpoint<MatterFabric>(this, "matter_fabric");
      SmartScene = new CrudEndpoint<SmartScene, CreateSmartScene, UpdateSmartScene>(this, "smart_scene");
      Contact = new ReadEditEndpoint<ContactSensor, UpdateSensorRequest>(this, "contact");
      Tamper = new ReadEditEndpoint<TamperSensor, BaseResourceRequest>(this, "tamper");
      MotionAreaConfiguration = new CrudEndpoint<MotionAreaConfigResource, CreateUpdateMotionAreaConfig, CreateUpdateMotionAreaConfig>(this, "motion_area_configuration");
      MotionAreaCandidate = new ReadEditEndpoint<MotionAreaCandidateResource, BaseResourceRequest>(this, "motion_area_candidate");
      ConvenienceAreaMotion = new ReadEditEndpoint<ConvenienceAreaMotionResource, BaseResourceRequest>(this, "convenience_area_motion");
      SecurityAreaMotion = new ReadEditEndpoint<SecurityAreaMotionResource, BaseResourceRequest>(this, "security_area_motion");
      Speaker = new ReadEditEndpoint<SpeakerResource, BaseResourceRequest>(this, "speaker");
      Clip = new ReadEditEndpoint<ClipResource, BaseResourceRequest>(this, "clip");
      WifiConnectivity = new ReadEditEndpoint<WifiConnectivityResource, BaseResourceRequest>(this, "wifi_connectivity");
    }



  }

}
