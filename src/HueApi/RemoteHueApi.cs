using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HueApi
{
  public class RemoteHueApi : BaseHueApi
  {
    protected const string KeyHeaderName = "hue-application-key";

    public RemoteHueApi(string appKey, string remoteAccessToken, HttpClient? client = null)
    {
      if (client == null)
      {
        client = new HttpClient();
      }

      client.BaseAddress = new Uri("https://api.meethue.com/route/");

      client.DefaultRequestHeaders.Add(KeyHeaderName, appKey);
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", remoteAccessToken);

      this.client = client;
    }
  }
}
