using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi
{
  public class RemoteHueApi : BaseHueApi
  {
    protected const string KeyHeaderName = "hue-application-key";

    public RemoteHueApi(string remoteAccessToken, HttpClient? client = null)
    {
      if (client == null)
      {
        client = new HttpClient();
      }

      client.BaseAddress = new Uri("https://api.meethue.com/route/");

      client.DefaultRequestHeaders.Add(KeyHeaderName, remoteAccessToken);

      this.client = client;
    }
  }
}
