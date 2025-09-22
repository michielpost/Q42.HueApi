using System.Net;
using System.Text.RegularExpressions;

namespace HueApi.BridgeLocator
{
  /// <summary>
  /// Base class for Bridge Locator sharing common behaviors
  /// </summary>
  public abstract class BridgeLocator : IBridgeLocator
  {
    private static readonly Regex xmlResponseCheckHueRegex = new Regex(@"Philips hue bridge", RegexOptions.IgnoreCase);
    private static readonly Regex xmlResponseSerialNumberRegex = new Regex(@"<serialnumber>(.+?)</serialnumber>", RegexOptions.IgnoreCase);

    private const string httpXmlDescriptorFileFormat = "http://{0}/description.xml";

    protected static readonly HttpClient _httpClient = new HttpClient();

    /// <summary>
    /// Event handler in case a bridge was found
    /// </summary>
    /// <param name="sender">The source of the event</param>
    /// <param name="locatedBridge">The bridge that was found</param>
    public delegate void BridgeFoundHandler(IBridgeLocator sender, LocatedBridge locatedBridge);

    /// <summary>
    /// Event that is called when a bridge is found
    /// </summary>
    public event BridgeFoundHandler BridgeFound = (sender, bridge) => { };

    /// <summary>
    /// Calls the event that a bridge is found
    /// </summary>
    /// <param name="locatedBridge">The bridge that was found</param>
    protected void OnBridgeFound(LocatedBridge locatedBridge)
    {
      BridgeFound(this, locatedBridge);
    }

    /// <summary>
    /// Locate bridges
    /// </summary>
    /// <param name="timeout">Timeout before stopping the search</param>
    /// <returns>List of bridge IPs found</returns>
    public async Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(TimeSpan timeout)
    {
      if (timeout <= TimeSpan.Zero)
      {
        throw new ArgumentException("Timeout value must be greater than zero.", nameof(timeout));
      }

      using (CancellationTokenSource cancelSource = new CancellationTokenSource(timeout))
      {
        try
        {
          return await LocateBridgesAsync(cancelSource.Token).ConfigureAwait(false);
        }
        catch
        {
          return Enumerable.Empty<LocatedBridge>();
        }
      }
    }

    /// <summary>
    /// Locate bridges
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the search</param>
    /// <returns>List of bridge IPs found</returns>
    public abstract Task<IEnumerable<LocatedBridge>> LocateBridgesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Check if a IP is a Hue Bridge by checking its descriptor file
    /// </summary>
    /// <param name="ip">IP to check</param>
    /// <param name="httpTimeout">Timeout for this specific check</param>
    /// <param name="cancellationToken">Token to cancel the check</param>
    /// <returns>The Serial Number, or empty if not a hue bridge</returns>
    public static async Task<string> CheckHueDescriptor(IPAddress ip, TimeSpan httpTimeout, CancellationToken? cancellationToken = null)
    {
      using (var httpTimeoutCts = new CancellationTokenSource(httpTimeout))
      using (var mergedCts = CancellationTokenSource.CreateLinkedTokenSource(
                                                    cancellationToken ?? CancellationToken.None, httpTimeoutCts.Token))
      {
        try
        {
          using (var response = await _httpClient.GetAsync(string.Format(httpXmlDescriptorFileFormat, ip), mergedCts.Token)
                                                 .ConfigureAwait(false))
          {
            if (response.IsSuccessStatusCode)
            {
              string xmlResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              if (xmlResponseCheckHueRegex.IsMatch(xmlResponse))
              {
                var serialNumberMatch = xmlResponseSerialNumberRegex.Match(xmlResponse);

                if (serialNumberMatch.Success)
                {
                  string bridgeId = serialNumberMatch.Groups[1].Value;
                  if (bridgeId.Length >= 6)
                    bridgeId = bridgeId.Insert(6, "fffe");

                  return bridgeId;
                }
                else
                {
                  // No S/N found
                }
              }
              else
              {
                // Not a Hue Bridge
              }
            }
            else
            {
              // No response, or cancellation requested
            }
          }
        }
        catch
        {
          // Something went wrong, ignore...
        }
      }

      return "";
    }
  }
}
