using HueApi.Models;

namespace HueApi.Extensions.cs
{
  public static class HueResourceExtensions
  {
    public static ResourceIdentifier ToResourceIdentifier(this HueResource hueResource)
    {
      return new ResourceIdentifier
      {
        Rid = hueResource.Id,
        Rtype = hueResource.Type
      };
    }
  }
}
