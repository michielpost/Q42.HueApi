using HueApi.Models;

namespace HueApi.Extensions
{
  static class HueResourceTypeDataHelpers
  {
    // Core implementation: accepts Type? for optional types
    public static HueResourceTypeData AddHueResourceData<TGet>(
        string input,
        Type? postType = null,
        Type? putType = null,
        Type? deleteType = null) where TGet : HueResource
    {
      return new HueResourceTypeData
      {
        Key = input,
        GetType = typeof(HueResponse<TGet>),
        PostType = postType,
        PutType = putType,
        DeleteType = deleteType,
      };
    }
  }

}
