using Newtonsoft.Json;
using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
	/// <summary>
	/// Uses the special nupnp url from meethue.com to find registered bridges based on your external IP
	/// </summary>
  public class HttpBridgeLocator : IBridgeLocator
  {
    private readonly Uri NuPnPUrl = new Uri("http://www.meethue.com/api/nupnp");


		/// <summary>
		/// Locate bridges
		/// </summary>
		/// <param name="timeout"></param>
		/// <returns></returns>
    public async Task<IEnumerable<string>> LocateBridgesAsync(TimeSpan timeout)
    {
      // since this specifies timeout (and probably isn't called much), don't use shared client
      HttpClient client = new HttpClient();
      client.Timeout = timeout;

      string response = await client.GetStringAsync(NuPnPUrl);

      NuPnPResponse[] responseModel = JsonConvert.DeserializeObject<NuPnPResponse[]>(response);

      return responseModel.Select(x => x.InternalIpAddress).ToList();

    }
  }
}
