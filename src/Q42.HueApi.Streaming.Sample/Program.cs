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
      Console.WriteLine("Edit your bridge keys in StreamingSetup.cs");

      HueStreaming s = new HueStreaming();
      await s.Start();

      Console.WriteLine("finished");

      Console.ReadLine();

      Console.ReadLine();
    }
  }
}
