using HueApi.Models;

namespace HueApi.Extensions
{
  public static class HueResourceTypeDataExtensions
  {
    public static HueResourceTypeData WithPost<TPost>(this HueResourceTypeData input)
    {
      input.PostType = typeof(HueResponse<TPost>);
      return input;
    }

    public static HueResourceTypeData WithPut<TPut>(this HueResourceTypeData input)
    {
      input.PutType = typeof(HueResponse<TPut>);
      return input;
    }

    public static HueResourceTypeData WithDelete<TDelete>(this HueResourceTypeData input)
    {
      input.DeleteType = typeof(HueResponse<TDelete>);
      return input;
    }
  }

}
