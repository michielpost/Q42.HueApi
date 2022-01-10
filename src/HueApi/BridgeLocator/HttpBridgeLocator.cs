using System.Net.Http.Json;

namespace HueApi.BridgeLocator
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
          var responseModel = await response.Content.ReadFromJsonAsync<List<DiscoveryResponse>>();

          if (responseModel != null)
          {
            var locatedBridges = responseModel.Select(x => new LocatedBridge(x.Id, x.InternalIpAddress, x.Port)).ToList();
            locatedBridges.ForEach(OnBridgeFound);
            return locatedBridges;
          }
          else
            return Enumerable.Empty<LocatedBridge>();
        }
        else
        {
          return new List<LocatedBridge>();
        }
      }
    }
  }
}
