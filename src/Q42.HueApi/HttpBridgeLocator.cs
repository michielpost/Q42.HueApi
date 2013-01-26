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
  public class HttpBridgeLocator : IBridgeLocator
  {

    private readonly Uri NuPnPUrl = new Uri("http://www.meethue.com/api/nupnp");

    public async Task<IEnumerable<string>> LocateBridgesAsync(TimeSpan timeout)
    {
      HttpClient client = new HttpClient();
      client.Timeout = timeout;

      string response = await client.GetStringAsync(NuPnPUrl);

      NuPnPResponse[] responseModel = JsonConvert.DeserializeObject<NuPnPResponse[]>(response);

      return responseModel.Select(x => x.InternalIpAddress).ToList();

    }
  }
}
