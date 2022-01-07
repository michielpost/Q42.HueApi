using HueApi.Models;
using HueApi.Models.Requests;
using HueApi.Models.Responses;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HueApi
{
  public class LocalHueClient
  {
    protected const string KeyHeaderName = "hue-application-key";

    protected const string RegisterUrl = "/api";
    protected const string ResourceUrl = "/clip/v2/resource";
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
    protected const string MotionUrl = $"{ResourceUrl}/motion";
    protected const string TemperatureUrl = $"{ResourceUrl}/temperature";
    protected const string LightLevelUrl = $"{ResourceUrl}/light_level";
    protected const string ButtonUrl = $"{ResourceUrl}/button";
    protected const string BehaviorScriptUrl = $"{ResourceUrl}/behavior_script";
    protected const string BehaviorInstanceUrl = $"{ResourceUrl}/behavior_instance";
    protected const string GeofenceClientUrl = $"{ResourceUrl}/geofence_client";
    protected const string GeolocationUrl = $"{ResourceUrl}/geolocation";
    protected const string EntertainmentConfigurationUrl = $"{ResourceUrl}/entertainment_configuration";
    protected const string EntertainmentUrl = $"{ResourceUrl}/entertainment";
    protected const string HomekitUrl = $"{ResourceUrl}/homekit";

    protected string ResourceIdUrl(string resourceUrl, Guid id) => $"{resourceUrl}/{id}";

    public Task<HueResponse<List<RegisterResponse>>> Register(RegisterRequest registerRequest) => HueRegisterPostRequest<List<RegisterResponse>, RegisterRequest>(RegisterUrl, registerRequest);

    #region Light
    public Task<HueResponse<Light>> GetLights() => HueGetRequest<Light>(LightUrl);
    public Task<HueResponse<Light>> GetLight(Guid id) => HueGetRequest<Light>(ResourceIdUrl(LightUrl, id));
    public Task<HuePutResponse> UpdateLight(Guid id, UpdateLight data) => HuePutRequest(ResourceIdUrl(LightUrl, id), data);
    #endregion

    #region Scene
    public Task<HueResponse<Scene>> GetScenes() => HueGetRequest<Scene>(SceneUrl);
    public Task<HuePostResponse> CreateScene(CreateScene data) => HuePostRequest(SceneUrl, data);
    public Task<HueResponse<Scene>> GetScene(Guid id) => HueGetRequest<Scene>(ResourceIdUrl(SceneUrl, id));
    public Task<HuePutResponse> UpdateScene(Guid id, UpdateScene data) => HuePutRequest(ResourceIdUrl(SceneUrl, id), data);
    public Task<HueDeleteResponse> DeleteScene(Guid id) => HueDeleteRequest(ResourceIdUrl(SceneUrl, id));
    #endregion

    #region Room
    public Task<HueResponse<Room>> GetRooms() => HueGetRequest<Room>(RoomUrl);
    public Task<HuePostResponse> CreateRoom(BaseResourceRequest data) => HuePostRequest(RoomUrl, data);
    public Task<HueResponse<Room>> GetRoom(Guid id) => HueGetRequest<Room>(ResourceIdUrl(RoomUrl, id));
    public Task<HuePutResponse> UpdateRoom(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(RoomUrl, id), data);
    public Task<HueDeleteResponse> DeleteRoom(Guid id) => HueDeleteRequest(ResourceIdUrl(RoomUrl, id));
    #endregion

    #region Zone
    public Task<HueResponse<Zone>> GetZones() => HueGetRequest<Zone>(ZoneUrl);
    public Task<HuePostResponse> CreateZone(BaseResourceRequest data) => HuePostRequest(ZoneUrl, data);
    public Task<HueResponse<Zone>> GetZone(Guid id) => HueGetRequest<Zone>(ResourceIdUrl(ZoneUrl, id));
    public Task<HuePutResponse> UpdateZone(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(ZoneUrl, id), data);
    public Task<HueDeleteResponse> DeleteZone(Guid id) => HueDeleteRequest(ResourceIdUrl(ZoneUrl, id));
    #endregion

    #region BridgeHome
    public Task<HueResponse<BridgeHome>> GetBridgeHomes() => HueGetRequest<BridgeHome>(BridgeHomeUrl);
    public Task<HueResponse<BridgeHome>> GetBridgeHome(Guid id) => HueGetRequest<BridgeHome>(ResourceIdUrl(BridgeHomeUrl, id));
    public Task<HuePutResponse> UpdateBridgeHome(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(BridgeHomeUrl, id), data);
    #endregion

    #region GroupedLight
    public Task<HueResponse<GroupedLight>> GetGroupedLights() => HueGetRequest<GroupedLight>(GroupedLightUrl);
    public Task<HueResponse<GroupedLight>> GetGroupedLight(Guid id) => HueGetRequest<GroupedLight>(ResourceIdUrl(GroupedLightUrl, id));
    public Task<HuePutResponse> UpdateGroupedLight(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(GroupedLightUrl, id), data);
    #endregion

    #region Device
    public Task<HueResponse<Device>> GetDevices() => HueGetRequest<Device>(DeviceUrl);
    public Task<HueResponse<Device>> GetDevice(Guid id) => HueGetRequest<Device>(ResourceIdUrl(DeviceUrl, id));
    public Task<HuePutResponse> UpdateDevice(Guid id, UpdateDevice data) => HuePutRequest(ResourceIdUrl(DeviceUrl, id), data);
    #endregion

    #region Bridge
    public Task<HueResponse<Bridge>> GetBridges() => HueGetRequest<Bridge>(BridgeUrl);
    public Task<HueResponse<Bridge>> GetBridge(Guid id) => HueGetRequest<Bridge>(ResourceIdUrl(BridgeUrl, id));
    public Task<HuePutResponse> UpdateBridge(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(BridgeUrl, id), data);
    #endregion

    #region DevicePower
    public Task<HueResponse<DevicePower>> GetDevicePowers() => HueGetRequest<DevicePower>(DevicePowerUrl);
    public Task<HueResponse<DevicePower>> GetDevicePower(Guid id) => HueGetRequest<DevicePower>(ResourceIdUrl(DevicePowerUrl, id));
    public Task<HuePutResponse> UpdateDevicePower(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(DevicePowerUrl, id), data);
    #endregion

    #region ZigbeeConnectivity
    public Task<HueResponse<ZigbeeConnectivity>> GetZigbeeConnectivity() => HueGetRequest<ZigbeeConnectivity>(ZigbeeConnectivityUrl);
    public Task<HueResponse<ZigbeeConnectivity>> GetZigbeeConnectivity(Guid id) => HueGetRequest<ZigbeeConnectivity>(ResourceIdUrl(ZigbeeConnectivityUrl, id));
    public Task<HuePutResponse> UpdateZigbeeConnectivity(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(ZigbeeConnectivityUrl, id), data);
    #endregion

    #region ZgpConnectivity
    public Task<HueResponse<ZgpConnectivity>> GetZgpConnectivity() => HueGetRequest<ZgpConnectivity>(ZgpConnectivityUrl);
    public Task<HueResponse<ZgpConnectivity>> GetZgpConnectivity(Guid id) => HueGetRequest<ZgpConnectivity>(ResourceIdUrl(ZgpConnectivityUrl, id));
    public Task<HuePutResponse> UpdateZgpConnectivity(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(ZgpConnectivityUrl, id), data);
    #endregion

    #region Motion
    public Task<HueResponse<Motion>> GetMotions() => HueGetRequest<Motion>(MotionUrl);
    public Task<HueResponse<Motion>> GetMotion(Guid id) => HueGetRequest<Motion>(ResourceIdUrl(MotionUrl, id));
    public Task<HuePutResponse> UpdateMotion(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(MotionUrl, id), data);
    #endregion

    #region Temperature
    public Task<HueResponse<Temperature>> GetTemperatures() => HueGetRequest<Temperature>(TemperatureUrl);
    public Task<HueResponse<Temperature>> GetTemperature(Guid id) => HueGetRequest<Temperature>(ResourceIdUrl(TemperatureUrl, id));
    public Task<HuePutResponse> UpdateTemperature(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(TemperatureUrl, id), data);
    #endregion

    #region LightLevel
    public Task<HueResponse<LightLevel>> GetLightLevels() => HueGetRequest<LightLevel>(LightLevelUrl);
    public Task<HueResponse<LightLevel>> GetLightLevel(Guid id) => HueGetRequest<LightLevel>(ResourceIdUrl(LightLevelUrl, id));
    public Task<HuePutResponse> UpdateLightLevel(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(LightLevelUrl, id), data);
    #endregion

    #region Button
    public Task<HueResponse<Button>> GetButtons() => HueGetRequest<Button>(ButtonUrl);
    public Task<HueResponse<Button>> GetButton(Guid id) => HueGetRequest<Button>(ResourceIdUrl(ButtonUrl, id));
    public Task<HuePutResponse> UpdateButton(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(ButtonUrl, id), data);
    #endregion

    #region BehaviorScript
    public Task<HueResponse<BehaviorScript>> GetBehaviorScripts() => HueGetRequest<BehaviorScript>(BehaviorScriptUrl);
    public Task<HueResponse<BehaviorScript>> GetBehaviorScript(Guid id) => HueGetRequest<BehaviorScript>(ResourceIdUrl(BehaviorScriptUrl, id));
    public Task<HuePutResponse> UpdateBehaviorScript(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(BehaviorScriptUrl, id), data);
    #endregion

    #region BehaviorInstance
    public Task<HueResponse<BehaviorInstance>> GetBehaviorInstances() => HueGetRequest<BehaviorInstance>(BehaviorInstanceUrl);
    public Task<HuePostResponse> CreateBehaviorInstance(BaseResourceRequest data) => HuePostRequest(BehaviorInstanceUrl, data);
    public Task<HueResponse<BehaviorInstance>> GetBehaviorInstance(Guid id) => HueGetRequest<BehaviorInstance>(ResourceIdUrl(BehaviorInstanceUrl, id));
    public Task<HuePutResponse> UpdateBehaviorInstance(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(BehaviorInstanceUrl, id), data);
    public Task<HueDeleteResponse> DeleteBehaviorInstance(Guid id) => HueDeleteRequest(ResourceIdUrl(BehaviorInstanceUrl, id));
    #endregion

    #region GeofenceClient
    public Task<HueResponse<GeofenceClient>> GetGeofenceClients() => HueGetRequest<GeofenceClient>(GeofenceClientUrl);
    public Task<HuePostResponse> CreateGeofenceClient(BaseResourceRequest data) => HuePostRequest(GeofenceClientUrl, data);
    public Task<HueResponse<GeofenceClient>> GetGeofenceClient(Guid id) => HueGetRequest<GeofenceClient>(ResourceIdUrl(GeofenceClientUrl, id));
    public Task<HuePutResponse> UpdateGeofenceClient(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(GeofenceClientUrl, id), data);
    public Task<HueDeleteResponse> DeleteGeofenceClient(Guid id) => HueDeleteRequest(ResourceIdUrl(GeofenceClientUrl, id));
    #endregion

    #region Geolocation
    public Task<HueResponse<Geolocation>> GetGeolocations() => HueGetRequest<Geolocation>(GeolocationUrl);
    public Task<HueResponse<Geolocation>> GetGeolocation(Guid id) => HueGetRequest<Geolocation>(ResourceIdUrl(GeolocationUrl, id));
    public Task<HuePutResponse> UpdateGeolocation(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(GeolocationUrl, id), data);
    #endregion

    #region EntertainmentConfiguration
    public Task<HueResponse<EntertainmentConfiguration>> GetEntertainmentConfigurations() => HueGetRequest<EntertainmentConfiguration>(EntertainmentConfigurationUrl);
    public Task<HuePostResponse> CreateEntertainmentConfiguration(BaseResourceRequest data) => HuePostRequest(EntertainmentConfigurationUrl, data);
    public Task<HueResponse<EntertainmentConfiguration>> GetEntertainmentConfiguration(Guid id) => HueGetRequest<EntertainmentConfiguration>(ResourceIdUrl(EntertainmentConfigurationUrl, id));
    public Task<HuePutResponse> UpdateEntertainmentConfiguration(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(EntertainmentConfigurationUrl, id), data);
    public Task<HueDeleteResponse> DeleteEntertainmentConfiguration(Guid id) => HueDeleteRequest(ResourceIdUrl(EntertainmentConfigurationUrl, id));
    #endregion

    #region Entertainment
    public Task<HueResponse<Entertainment>> GetEntertainmentServices() => HueGetRequest<Entertainment>(EntertainmentUrl);
    public Task<HueResponse<Entertainment>> GetEntertainmentService(Guid id) => HueGetRequest<Entertainment>(ResourceIdUrl(EntertainmentUrl, id));
    public Task<HuePutResponse> UpdateEntertainmentService(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(EntertainmentUrl, id), data);
    #endregion

    #region Homekit
    public Task<HueResponse<Homekit>> GetHomekits() => HueGetRequest<Homekit>(HomekitUrl);
    public Task<HueResponse<Homekit>> GetHomekit(Guid id) => HueGetRequest<Homekit>(ResourceIdUrl(HomekitUrl, id));
    public Task<HuePutResponse> UpdateHomekit(Guid id, BaseResourceRequest data) => HuePutRequest(ResourceIdUrl(HomekitUrl, id), data);
    #endregion

    #region Resource
    public Task<HueResponse<HueResource>> GetResources() => HueGetRequest<HueResource>(ResourceUrl);
    #endregion


    private readonly HttpClient client;

    public LocalHueClient(string ip, string? key, HttpClient? client = null)
    {
      if(client == null)
        client = new HttpClient();

      client.BaseAddress = new Uri($"https://{ip}/clip/v2");

      if(!string.IsNullOrEmpty(key))
        client.DefaultRequestHeaders.Add(KeyHeaderName, key);

      this.client = client;
    }

    protected async Task<HueResponse<T>> HueGetRequest<T>(string url)
    {
      var response = await client.GetAsync(url);

      return await ProcessResponse<HueResponse<T>>(response);
    }

    protected async Task<HueDeleteResponse> HueDeleteRequest(string url)
    {
      var response = await client.DeleteAsync(url);

      return await ProcessResponse<HueDeleteResponse>(response);
    }

    protected async Task<HuePutResponse> HuePutRequest<D>(string url, D data)
    {
      JsonSerializerOptions options = new()
      {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
      };

      var response = await client.PutAsJsonAsync(url, data, options);

      return await ProcessResponse<HuePutResponse>(response);
    }

    //protected async Task<HueResponse<T>> HuePutRequest<T, D>(string url, D data)
    //{
    //  var response = await client.PutAsJsonAsync(url, data);

    //  return await ProcessResponse<T>(response);
    //}

    protected async Task<HueResponse<T>> HueRegisterPostRequest<T, D>(string url, D data)
    {
      var response = await client.PostAsJsonAsync(url, data);

      return await ProcessResponse<HueResponse<T>>(response);
    }

    protected async Task<HuePostResponse> HuePostRequest<D>(string url, D data)
    {
      var response = await client.PostAsJsonAsync(url, data);

      return await ProcessResponse<HuePostResponse>(response);
    }


    protected async Task<T> ProcessResponse<T>(HttpResponseMessage? response) where T : HueErrorResponse, new()
    {
      if (response == null)
        return new T();

      if (response.IsSuccessStatusCode)
      {
        return (await response.Content.ReadFromJsonAsync<T>()) ?? new();
      }
      else
      {
        var errorResponse = await response.Content.ReadFromJsonAsync<HueErrorResponse>();

        var result = new T();
        if(errorResponse != null)
          result.Errors = errorResponse.Errors;

        return result;
      }
    }
  }
}
