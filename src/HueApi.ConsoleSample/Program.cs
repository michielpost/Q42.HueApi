using HueApi;
using HueApi.Models;
using HueApi.Models.Requests;
using HueApi.Models.Responses;
using Microsoft.Extensions.Configuration;
using HueApi.ColorConverters.Original.Extensions;

Console.WriteLine("HueApi Console Sample");

var builder = new ConfigurationBuilder().AddUserSecrets<Program>();
var config = builder.Build();

string ip = config["ip"] ?? string.Empty;
string key = config["key"] ?? string.Empty;

Console.WriteLine($"Connecting to {ip} with key: {key}");


var localHueClient = new LocalHueApi(ip, key);

//Console.WriteLine("Getting all resources...");

//var resources = await localHueClient.GetResourcesAsync();
//var roots = resources.Data.Where(x => x.Owner == null);

//PrintChildren(resources, null, 0);

var allLights = await localHueClient.GetLightsAsync();
var firstLightId = allLights.Data.First().Id; //First

var allGroups = await localHueClient.GetGroupedLightsAsync();
var allGroupId = allGroups.Data.OrderBy(x=> x.IdV1).First().Id; //All

localHueClient.OnEventStreamMessage += EventStreamMessage;
localHueClient.StartEventStream();

Console.WriteLine("Waiting for Hue Bridge events...");


Console.WriteLine("Press enter to change the lights to red..");
Console.ReadLine();
await ChangeColorForGroup(allGroupId, "FF0000");
//await ChangeColorForLight(firstLightId, "FF0000");

Console.WriteLine("Press enter to change the lights to green..");
Console.ReadLine();
await ChangeColorForGroup(allGroupId, "00FF00");
//await ChangeColorForLight(firstLightId, "00FF00");


Console.WriteLine("Press enter to stop listening for events...");
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



Task ChangeColorForGroup(Guid id, string hex)
{
  var req = new UpdateGroupedLight()
  .TurnOn()
  .SetColor(new HueApi.ColorConverters.RGBColor(hex));

  return localHueClient.UpdateGroupedLightAsync(id, req);
}

Task ChangeColorForLight(Guid id, string hex)
{
  var req = new UpdateLight()
  .TurnOn()
  .SetColor(new HueApi.ColorConverters.RGBColor(hex));

  return localHueClient.UpdateLightAsync(id, req);
}

