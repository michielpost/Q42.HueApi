using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;

namespace Q42.HueApi
{
  /// <summary>
  /// Base class for Bridge Locator sharing common behaviors
  /// </summary>
  public abstract class BridgeLocator : IBridgeLocator
  {
    private static readonly Regex xmlResponseCheckHueRegex = new Regex(@"Philips hue bridge", RegexOptions.IgnoreCase);
    private static readonly Regex xmlResponseSerialNumberRegex = new Regex(@"<serialnumber>(.+?)</serialnumber>", RegexOptions.IgnoreCase);

    private const string httpXmlDescriptorFileFormat = "http://{0}/description.xml";

    protected readonly static HttpClient _httpClient = new HttpClient();

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
        return await LocateBridgesAsync(cancelSource.Token);
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
    protected static async Task<string> CheckHueDescriptor(IPAddress ip, TimeSpan httpTimeout, CancellationToken? cancellationToken = null)
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
              string xmlResponse = await response.Content.ReadAsStringAsync();
              if (xmlResponseCheckHueRegex.IsMatch(xmlResponse))
              {
                var serialNumberMatch = xmlResponseSerialNumberRegex.Match(xmlResponse);

                if (serialNumberMatch.Success)
                {
                  return serialNumberMatch.Groups[1].Value;
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
