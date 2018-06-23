using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Q42.HueApi.Streaming.Models;

namespace Q42.HueApi.Streaming.Sample
{
  class Program
  {
    public static async Task Main(string[] args)
    {
      Console.WriteLine("Q42.HueApi Streaming Sample App");

      HueStreaming s = new HueStreaming();
      await s.Start();

      //HueLightDJ s = new HueLightDJ();
      //await s.Start();

      //BeatController b = new BeatController(null);
      //b.EffectFunction = Functiona;
      //b.StartAutoTimer(TimeSpan.FromSeconds(2));

      //Console.ReadLine();
      //b.ManualBeat(null);

      //Console.ReadLine();
      //b.ManualBeat(null);

      //Console.ReadLine();
      //b.ManualBeat(null);

      //Console.ReadLine();
      //b.ManualBeat(null);

      //Console.ReadLine();
      //b.ManualBeat(null);


      //Console.ReadLine();
      //b.AutoContinueManualBeat();
      Console.WriteLine("finished");

      Console.ReadLine();

      Console.ReadLine();

      Console.ReadLine();

      Console.ReadLine();
    }

    private static Task Functiona(IEnumerable<EntertainmentLight> current, TimeSpan? timeSpan)
    {
      Console.WriteLine("now");

      return Task.CompletedTask;
    }
  }
}
