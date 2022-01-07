// See https://aka.ms/new-console-template for more information
using HueApi;
using HueApi.Models.Responses;

Console.WriteLine("HueApi Console Sample");

string ip = "192.168.0.4";
string key = "PUT_YOUR_KEY_HERE";

Console.WriteLine($"Connecting to {ip} with key: {key}");


var localHueClient = new LocalHueClient(ip, key);

localHueClient.OnEventStreamMessage += EventStreamMessage;



localHueClient.StartEventStream();

Console.WriteLine("Waiting for Hue Bridge events...");

Console.ReadLine();


void EventStreamMessage(List<EventStreamResponse> events)
{
  Console.WriteLine($"{events.Count} new events");
}

