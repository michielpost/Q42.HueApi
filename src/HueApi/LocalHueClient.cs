using HueApi.Models;
using System.Net.Http.Json;

namespace HueApi
{
  public class LocalHueClient
  {
    protected const string KeyHeaderName = "hue-application-key";

    protected const string ResourceUrl = "/resource";
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

    public Task<HueResponse<Bridge>> GetDevice() => HueGetRequest<Bridge>(DeviceUrl);
    public Task<HueResponse<Bridge>> GetDevice(Guid id) => HueGetRequest<Bridge>(ResourceIdUrl(DeviceUrl, id));
    public Task<HueResponse<Bridge>> UpdateDevice(Guid id, BridgeUpdateRequest data) => HuePutRequest<Bridge, BridgeUpdateRequest>(ResourceIdUrl(DeviceUrl, id), data);




    private readonly HttpClient client;

    public LocalHueClient(string ip, string key, HttpClient? client = null)
    {
      if(client == null)
        client = new HttpClient();

      client.BaseAddress = new Uri($"https://{ip}/clip/v2");
      client.DefaultRequestHeaders.Add(KeyHeaderName, key);

      this.client = client;
    }

    public async Task<HueResponse<T>> HueGetRequest<T>(string url)
    {
      var response = await client.GetAsync(url);

      return await ProcessResponse<T>(response);
    }

    public async Task<HueResponse<T>> HueDeleteRequest<T>(string url)
    {
      var response = await client.DeleteAsync(url);

      return await ProcessResponse<T>(response);
    }

    public async Task<HueResponse<T>> HuePutRequest<T, D>(string url, D data)
    {
      var response = await client.PutAsJsonAsync(url, data);

      return await ProcessResponse<T>(response);
    }

    public async Task<HueResponse<T>> HuePostRequest<T, D>(string url, D data)
    {
      var response = await client.PostAsJsonAsync(url, data);

      return await ProcessResponse<T>(response);
    }


    public async Task<HueResponse<T>> ProcessResponse<T>(HttpResponseMessage? response)
    {
      if (response == null)
        return new HueResponse<T>(null);

      if (response.IsSuccessStatusCode)
      {
        var result = await response.Content.ReadFromJsonAsync<T>();
        var errors = await response.Content.ReadFromJsonAsync<HueErrors>();
        return new HueResponse<T>(result, errors);
      }
      else
      {
        var errors = await response.Content.ReadFromJsonAsync<HueErrors>();
        return new HueResponse<T>(errors);
      }
    }
  }
}
