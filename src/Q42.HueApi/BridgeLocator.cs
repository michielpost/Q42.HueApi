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
  /// Uses the special nupnp url from meethue.com to find registered bridges based on your external IP
  /// </summary>
  public abstract class BridgeLocator : IBridgeLocator
  {
    private readonly Regex xmlResponseCheckHueRegex = new Regex(@"Philips hue bridge", RegexOptions.IgnoreCase);
    private readonly Regex xmlResponseSerialNumberRegex = new Regex(@"<serialnumber>(.+?)</serialnumber>", RegexOptions.IgnoreCase);

    private const string httpXmlDescriptorFileFormat = "http://{0}/description.xml";

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
    /// <param name="cancellationToken">Token to cancel the check</param>
    /// <returns>The Serial Number, or empty if not a hue bridge</returns>
    public async Task<string> CheckHueDescriptor(IPAddress ip, CancellationToken? cancellationToken = null)
    {
      try
      {
        using (var client = new HttpClient())
        {
          var response = await client.GetAsync(
            string.Format(httpXmlDescriptorFileFormat, ip),
            cancellationToken ?? CancellationToken.None).ConfigureAwait(false);

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

      return "";
    }
  }
}
