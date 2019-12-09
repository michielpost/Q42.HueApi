using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Q42.HueApi.Models.Bridge;

namespace Q42.HueApi
{
  /// <summary>
  /// Uses the special nupnp url from meethue.com to find registered bridges based on your external IP
  /// </summary>
  public class HttpBridgeLocator : BridgeLocator
  {
    private readonly Uri NuPnPUrl = new Uri("https://discovery.meethue.com");

    /// <summary>
    /// Locate bridges
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the search</param>
    /// <returns>List of bridge IPs found</returns>
    public override async Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(CancellationToken cancellationToken)
    {
      using (var response = await _httpClient.GetAsync(NuPnPUrl, cancellationToken).ConfigureAwait(false))
      {
        if (response.IsSuccessStatusCode && !cancellationToken.IsCancellationRequested)
        {
          string content = await response.Content.ReadAsStringAsync();

          NuPnPResponse[] responseModel = JsonConvert.DeserializeObject<NuPnPResponse[]>(content);

          return responseModel.Select(x => new LocatedBridge() { BridgeId = x.Id, IpAddress = x.InternalIpAddress }).ToList();
        }
        else
        {
          return new List<LocatedBridge>();
        }
      }
    }
  }
}
