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

    #region Device
    public Task<HueResponse<Device>> GetDevices() => HueGetRequest<Device>(DeviceUrl);
    public Task<HueResponse<Device>> GetDevice(Guid id) => HueGetRequest<Device>(ResourceIdUrl(DeviceUrl, id));
    public Task<HuePutResponse> UpdateDevice(Guid id, UpdateDevice data) => HuePutRequest(ResourceIdUrl(DeviceUrl, id), data);
    #endregion

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
