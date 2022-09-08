using HueApi.Models;
using HueApi.Models.Requests;
using HueApi.Models.Responses;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HueApi
{
  public class LocalHueApi : BaseHueApi
  {
    protected const string KeyHeaderName = "hue-application-key";

    public event EventStreamMessage? OnEventStreamMessage;
    private CancellationTokenSource? eventStreamCancellationTokenSource;

    protected const string EventStreamUrl = "eventstream/clip/v2";

    public LocalHueApi(string ip, string? key, HttpClient? client = null)
    {
      if (client == null)
      {
        var handler = new HttpClientHandler()
        {
#if NET461
          ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
#else
          ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
#endif
        };

        client = new HttpClient(handler);
      }

      client.BaseAddress = new Uri($"https://{ip}/");

      if(!string.IsNullOrEmpty(key))
        client.DefaultRequestHeaders.Add(KeyHeaderName, key);

      this.client = client;
    }


    public async void StartEventStream(CancellationToken? cancellationToken = null)
    {
      this.eventStreamCancellationTokenSource?.Cancel();

      if (cancellationToken.HasValue)
        this.eventStreamCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken.Value);
      else
        this.eventStreamCancellationTokenSource = new CancellationTokenSource();

      var cancelToken = this.eventStreamCancellationTokenSource.Token;

      try
      {
        while (!cancelToken.IsCancellationRequested) //Auto retry on stop
        {
#if NET461
          using (var streamReader = new StreamReader(await client.GetStreamAsync(EventStreamUrl)))
#else
          using (var streamReader = new StreamReader(await client.GetStreamAsync(EventStreamUrl, cancelToken)))
#endif
          {
            while (!streamReader.EndOfStream)
            {
              var jsonMsg = await streamReader.ReadLineAsync();
              //Console.WriteLine($"Received message: {message}");

              if (jsonMsg != null)
              {
                var data = System.Text.Json.JsonSerializer.Deserialize<List<EventStreamResponse>>(jsonMsg);

                if (data != null && data.Any())
                {
                  OnEventStreamMessage?.Invoke(data);
                }
              }
            }
          }
        }
      }
      catch (TaskCanceledException)
      {
        //Ignore task canceled
      }
    }

    public void StopEventStream()
    {
      this.eventStreamCancellationTokenSource?.Cancel();
    }

  }
}
