using HueApi.Entertainment.Models;

namespace HueApi.Entertainment.ConsoleSample
{
  public class StreamingSetup
  {
    public static async Task<StreamingGroup> SetupAndReturnGroup()
    {
      string ip = "192.168.1.57";
      string key = "YLRlMeKdb6Fedj7VaJdKIYVErs-STLlNbCmf4rwb";
      string entertainmentKey = "9EF44FFF43C59CC3C51AEBF70AF4BBDD";
      var useSimulator = false;

      //string ip = "127.0.0.1";
      //string key = "aSimulatedUser";
      //string entertainmentKey = "01234567890123456789012345678901";
      //var useSimulator = true;


      //Initialize streaming client
      StreamingHueClient client = new StreamingHueClient(ip, key, entertainmentKey);

      //Get the entertainment group
      var all = await client.LocalHueApi.EntertainmentConfiguration.GetAllAsync();
      var group = all.Data.Skip(1).FirstOrDefault();

      if (group == null)
        throw new HueEntertainmentException("No Entertainment Group found. Create one using the Q42.HueApi.UniversalWindows.Sample");
      else
        Console.WriteLine($"Using Entertainment Group {group.Id}");

      //Create a streaming group
      var stream = new StreamingGroup(group.Channels);
      stream.IsForSimulator = useSimulator;


      //Connect to the streaming group
      await client.ConnectAsync(group.Id, simulator: useSimulator);

      //Start auto updating this entertainment group
      client.AutoUpdateAsync(stream, new CancellationToken(), 50, onlySendDirtyStates: false);

      //Optional: Check if streaming is currently active
      var entArea = await client.LocalHueApi.EntertainmentConfiguration.GetByIdAsync(group.Id);
      Console.WriteLine(entArea.Data.First().Status == HueApi.Models.EntertainmentConfigurationStatus.active ? "Streaming is active" : "Streaming is not active");
      return stream;
    }
  }
}
