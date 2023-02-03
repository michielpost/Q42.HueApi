using HueApi.Entertainment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi.Entertainment.ConsoleSample
{
  public class StreamingSetup
  {
    public static async Task<StreamingGroup> SetupAndReturnGroup()
    {
      string ip = "192.168.0.4";
      string key = "im5PBqU--4CJq2N2t8xMVNvZ2qxOtgzLcfVTkwzP";
      string entertainmentKey = "32C1FEB5439F313891C44369FF71388C";
      var useSimulator = false;

      //string ip = "127.0.0.1";
      //string key = "aSimulatedUser";
      //string entertainmentKey = "01234567890123456789012345678901";
      //var useSimulator = true;


      //Initialize streaming client
      StreamingHueClient client = new StreamingHueClient(ip, key, entertainmentKey);

      //Get the entertainment group
      var all = await client.LocalHueApi.GetEntertainmentConfigurationsAsync();
      var group = all.Data.LastOrDefault();

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
      var entArea = await client.LocalHueApi.GetEntertainmentConfigurationAsync(group.Id);
      Console.WriteLine(entArea.Data.First().Status == HueApi.Models.EntertainmentConfigurationStatus.active ? "Streaming is active" : "Streaming is not active");
      return stream;
    }
  }
}
