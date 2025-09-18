using HueApi.Models;

namespace HueApi.Extensions
{
  public static class HueResponseExtensions
  {
    public static HueResponse<HueResource> ConvertToBase<TDerived>(this HueResponse<TDerived> derivedResponse)
        where TDerived : HueResource
    {
      if (derivedResponse == null)
        throw new ArgumentNullException(nameof(derivedResponse));

      return new HueResponse<HueResource>
      {
        Data = derivedResponse.Data.Cast<HueResource>().ToList(),
        Errors = derivedResponse.Errors
      };
    }
  }
}
