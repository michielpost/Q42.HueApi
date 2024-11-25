// See https://aka.ms/new-console-template for more information
using HueApi;
using HueApi.Models;
using HueApi.Models.Responses;
using Microsoft.Extensions.Configuration;

Console.WriteLine("HueApi Console Sample");

var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
var config = builder.Build();

string ip = config["ip"] ?? string.Empty;
string key = config["key"] ?? string.Empty;

Console.WriteLine($"Connecting to {ip} with key: {key}");


var localHueClient = new LocalHueApi(ip, key);

Console.WriteLine("Getting all resources...");

var resources = await localHueClient.GetResourcesAsync();
var roots = resources.Data.Where(x => x.Owner == null);

PrintChildren(resources, null, 0);

void PrintChildren(HueResponse<HueResource> resources, Guid? owner, int level)
{
  var children = resources.Data.Where(x => x.Owner?.Rid == owner);

  foreach (var child in children)
  {
    string spaces = new string(' ', level);
    Console.WriteLine(spaces + $"- {child.Type}");

    PrintChildren(resources, child.Id, level + 1);
  }
}


localHueClient.OnEventStreamMessage += EventStreamMessage;
localHueClient.StartEventStream();

Console.WriteLine("Waiting for Hue Bridge events...");

//await Task.Delay(TimeSpan.FromHours(1));

Console.ReadLine();
localHueClient.StopEventStream();

Console.WriteLine("Stoped listening for Hue Bridge events...");


Console.ReadLine();


void EventStreamMessage(string bridgeIp, List<EventStreamResponse> events)
{
  Console.WriteLine($"{DateTimeOffset.UtcNow} | {events.Count} new events");

  foreach(var hueEvent in events)
  {
    foreach(var data in hueEvent.Data)
    {
      Console.WriteLine($"Bridge IP: {bridgeIp} | Data: {data.Metadata?.Name} / {data.IdV1}");

      foreach(var jsonData in data.ExtensionData)
      {
        Console.WriteLine(jsonData);
      }
      Console.WriteLine();
    }
  }
}

