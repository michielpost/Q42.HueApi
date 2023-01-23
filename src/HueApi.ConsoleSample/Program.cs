// See https://aka.ms/new-console-template for more information
using HueApi;
using HueApi.Models.Responses;
using Microsoft.Extensions.Configuration;

Console.WriteLine("HueApi Console Sample");

var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
var config = builder.Build();

string ip = config["ip"] ?? string.Empty;
string key = config["key"] ?? string.Empty;

Console.WriteLine($"Connecting to {ip} with key: {key}");


var localHueClient = new LocalHueApi(ip, key);

localHueClient.OnEventStreamMessage += EventStreamMessage;
localHueClient.StartEventStream();

Console.WriteLine("Waiting for Hue Bridge events...");

//await Task.Delay(TimeSpan.FromHours(1));

Console.ReadLine();
localHueClient.StopEventStream();

Console.WriteLine("Stoped listening for Hue Bridge events...");


Console.ReadLine();


void EventStreamMessage(List<EventStreamResponse> events)
{
  Console.WriteLine($"{DateTimeOffset.UtcNow} | {events.Count} new events");

  foreach(var hueEvent in events)
  {
    foreach(var data in hueEvent.Data)
    {
      Console.WriteLine($"Data: {data.Metadata?.Name} / {data.IdV1}");
    }
  }
}

