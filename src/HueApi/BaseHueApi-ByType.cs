using HueApi.Extensions;
using HueApi.Models;
using HueApi.Models.Sensors;
using System.Net.Http.Json;

namespace HueApi
{
  public abstract partial class BaseHueApi
  {
    protected List<HueResourceTypeData> _resourceTypes = new List<HueResourceTypeData>()
    {
        HueResourceTypeDataHelpers.AddHueResourceData<HueResource>("resource"),
        HueResourceTypeDataHelpers.AddHueResourceData<Light>("light"),
        HueResourceTypeDataHelpers.AddHueResourceData<Scene>("scene"),
        HueResourceTypeDataHelpers.AddHueResourceData<Room>("room"),
        HueResourceTypeDataHelpers.AddHueResourceData<Zone>("zone"),
        HueResourceTypeDataHelpers.AddHueResourceData<BridgeHome>("bridge_home"),
        HueResourceTypeDataHelpers.AddHueResourceData<GroupedLight>("grouped_light"),
        HueResourceTypeDataHelpers.AddHueResourceData<Device>("device"),
        HueResourceTypeDataHelpers.AddHueResourceData<Bridge>("bridge"),
        HueResourceTypeDataHelpers.AddHueResourceData<DeviceSoftwareUpdate>("device_software_update"),
        HueResourceTypeDataHelpers.AddHueResourceData<DevicePower>("device_power"),
        HueResourceTypeDataHelpers.AddHueResourceData<ZigbeeConnectivity>("zigbee_connectivity"),
        HueResourceTypeDataHelpers.AddHueResourceData<ZgpConnectivity>("zgp_connectivity"),
        HueResourceTypeDataHelpers.AddHueResourceData<ZigbeeDeviceDiscovery>("zigbee_device_discovery"),
        HueResourceTypeDataHelpers.AddHueResourceData<MotionResource>("motion"),
        HueResourceTypeDataHelpers.AddHueResourceData<ServiceGroupResource>("service_group"),
        HueResourceTypeDataHelpers.AddHueResourceData<GroupedMotionResource>("grouped_motion"),
        HueResourceTypeDataHelpers.AddHueResourceData<GroupedLightLevelResource>("grouped_light_level"),
        HueResourceTypeDataHelpers.AddHueResourceData<CameraMotionResource>("camera_motion"),
        HueResourceTypeDataHelpers.AddHueResourceData<TemperatureResource>("temperature"),
        HueResourceTypeDataHelpers.AddHueResourceData<LightLevelResource>("light_level"),
        HueResourceTypeDataHelpers.AddHueResourceData<ButtonResource>("button"),
        HueResourceTypeDataHelpers.AddHueResourceData<BellButtonResource>("bell_button"),
        HueResourceTypeDataHelpers.AddHueResourceData<RelativeRotaryResource>("relative_rotary"),
        HueResourceTypeDataHelpers.AddHueResourceData<BehaviorScript>("behavior_script"),
        HueResourceTypeDataHelpers.AddHueResourceData<BehaviorInstance>("behavior_instance"),
        HueResourceTypeDataHelpers.AddHueResourceData<GeofenceClient>("geofence_client"),
        HueResourceTypeDataHelpers.AddHueResourceData<Geolocation>("geolocation"),
        HueResourceTypeDataHelpers.AddHueResourceData<EntertainmentConfiguration>("entertainment_configuration"),
        HueResourceTypeDataHelpers.AddHueResourceData<Entertainment>("entertainment"),
        HueResourceTypeDataHelpers.AddHueResourceData<Homekit>("homekit"),
        HueResourceTypeDataHelpers.AddHueResourceData<MatterItem>("matter"),
        HueResourceTypeDataHelpers.AddHueResourceData<MatterFabric>("matter_fabric"),
        HueResourceTypeDataHelpers.AddHueResourceData<SmartScene>("smart_scene"),
        HueResourceTypeDataHelpers.AddHueResourceData<ContactSensor>("contact"),
        HueResourceTypeDataHelpers.AddHueResourceData<TamperSensor>("tamper"),
        HueResourceTypeDataHelpers.AddHueResourceData<MotionAreaConfigResource>("motion_area_configuration"),
        HueResourceTypeDataHelpers.AddHueResourceData<MotionAreaCandidateResource>("motion_area_candidate"),
        HueResourceTypeDataHelpers.AddHueResourceData<ConvenienceAreaMotionResource>("convenience_area_motion"),
        HueResourceTypeDataHelpers.AddHueResourceData<SecurityAreaMotionResource>("security_area_motion"),
        HueResourceTypeDataHelpers.AddHueResourceData<SpeakerResource>("speaker"),
        HueResourceTypeDataHelpers.AddHueResourceData<ClipResource>("clip"),
        HueResourceTypeDataHelpers.AddHueResourceData<WifiConnectivityResource>("wifi_connectivity"),
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

      var selectedType = _resourceTypes.Where(x => x.Key == rtype).SingleOrDefault();
      if (selectedType != null)
        type = selectedType.GetType;


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
