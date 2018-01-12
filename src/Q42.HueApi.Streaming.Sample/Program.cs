using System;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Sample
{
  class Program
  {
    public static async Task Main(string[] args)
    {
      Console.WriteLine("Q42.HueApi Streaming Sample App");

      HueStreaming s = new HueStreaming();
      await s.Start();

      Console.ReadLine();
    }
  }
}
