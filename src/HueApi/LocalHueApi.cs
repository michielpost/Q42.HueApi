using HueApi.Models;
using HueApi.Models.Requests;
using HueApi.Models.Responses;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HueApi
{
  public delegate void EventStreamMessage(List<EventStreamResponse> events);

  public class LocalHueApi : BaseHueApi
  {
    protected const string KeyHeaderName = "hue-application-key";

    public LocalHueApi(string ip, string? key, HttpClient? client = null)
    {
      if (client == null)
      {
        var handler = new HttpClientHandler()
        {
          ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        client = new HttpClient(handler);
      }

      client.BaseAddress = new Uri($"https://{ip}/");

      if(!string.IsNullOrEmpty(key))
        client.DefaultRequestHeaders.Add(KeyHeaderName, key);

      this.client = client;
    }
   
  }
}
