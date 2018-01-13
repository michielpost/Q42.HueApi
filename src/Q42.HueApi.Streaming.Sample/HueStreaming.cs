using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Sample
{
  public class HueStreaming
  {
    public async Task Start()
    {

      //string ip = "192.168.0.4";
      //string key = "8JwWAj5J1tSsKLxyUOdAkWmcCQFcNc51AKRhxdH9";
      //string entertainmentKey = "AFFD322C34C993C19503D369481869FD";
      //var useSimulator = false;


      //string ip = "10.42.39.194";
      //string key = "tocjq6GmPJ8KX5DyLDKXQreZE6txQVQ5oBqbYDFn";
      //string entertainmentKey = "DB088F63639524B5A8CDC8AEEAC9C322";
      //var useSimulator = false;

      string ip = "127.0.0.1";
      string key = "aSimulatedUser";
      string entertainmentKey = "01234567890123456789012345678901";
      var useSimulator = true;


      //Initialize streaming client
      StreamingHueClient client = new StreamingHueClient(ip, key, entertainmentKey);

      //Get the entertainment group
      var all = await client.LocalHueClient.GetEntertainmentGroups();
      var group = all.FirstOrDefault();

      if (group == null)
        throw new Exception("No Entertainment Group found. Create one using the Q42.HueApi.UniversalWindows.Sample");
      else
        Console.WriteLine($"Using Entertainment Group {group.Id}");

      //Create a streaming group
      var entGroup = new StreamingGroup(group.Locations);
      entGroup.IsForSimulator = useSimulator;

      //Connect to the streaming group
      await client.Connect(group.Id, simulator: useSimulator);

      //Start auto updating this entertainment group
      client.AutoUpdate(entGroup, 50);

      //Optional: calculated effects that are placed in the room
      client.AutoCalculateEffectUpdate(entGroup);

      //Optional: Check if streaming is currently active
      var bridgeInfo = await client.LocalHueClient.GetBridgeAsync();
      Console.WriteLine(bridgeInfo.IsStreamingActive ? "Streaming is active" : "Streaming is not active");

      //Order lights based on position in the room
      var orderedLeft = entGroup.GetLeft().OrderByDescending(x => x.LightLocation.Y).ThenBy(x => x.LightLocation.X);
      var orderedRight = entGroup.GetRight().OrderByDescending(x => x.LightLocation.Y).ThenByDescending(x => x.LightLocation.X);
      var allLightsOrdered = orderedLeft.Concat(orderedRight.Reverse()).ToArray();

      var allLightsReverse = allLightsOrdered.ToList();
      allLightsReverse.Reverse();


      CancellationTokenSource cst = new CancellationTokenSource();

      Console.WriteLine("Random color on all lights");
      entGroup.SetRandomColor(IteratorEffectMode.All, TimeSpan.FromMilliseconds(250), cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Different random colors on all lights");
      entGroup.SetRandomColor(IteratorEffectMode.AllIndividual, TimeSpan.FromMilliseconds(250), cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Knight rider (works best with 6+ lights)");
      allLightsOrdered.KnightRider(cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Ref<TimeSpan?> waitTime = TimeSpan.FromMilliseconds(750);

      Console.WriteLine("Flash lights (750ms), press enter to decrease by 200 ms");
      allLightsOrdered.FlashQuick(new Q42.HueApi.ColorConverters.RGBColor("FFFFFF"), IteratorEffectMode.Cycle, waitTime: waitTime, cancellationToken: cst.Token);
      Console.ReadLine();

      waitTime.Value -= TimeSpan.FromMilliseconds(200);
      Console.WriteLine($"Flash ({waitTime.Value.Value.TotalMilliseconds})");
      Console.ReadLine();

      waitTime.Value -= TimeSpan.FromMilliseconds(200);
      Console.WriteLine($"Flash ({waitTime.Value.Value.TotalMilliseconds})");
      Console.ReadLine();

      waitTime.Value -= TimeSpan.FromMilliseconds(200);
      Console.WriteLine($"Flash ({waitTime.Value.Value.TotalMilliseconds})");
      Console.ReadLine();

      waitTime.Value -= TimeSpan.FromMilliseconds(100);
      Console.WriteLine($"Flash ({waitTime.Value.Value.TotalMilliseconds})");
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Flash on random lights");
      allLightsOrdered.FlashQuick(new Q42.HueApi.ColorConverters.RGBColor("FFFFFF"), IteratorEffectMode.Random, waitTime: waitTime, cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Flash on ALL lights");
      waitTime.Value = TimeSpan.FromMilliseconds(150);
      allLightsOrdered.Flash(new Q42.HueApi.ColorConverters.RGBColor("FFFFFF"), IteratorEffectMode.All, waitTime: waitTime, cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Flash effect with transition times");
      entGroup.GetLeft().Flash(new Q42.HueApi.ColorConverters.RGBColor("FF0000"), IteratorEffectMode.All, waitTime: TimeSpan.FromSeconds(1), transitionTimeOn: TimeSpan.FromMilliseconds(1000), transitionTimeOff: TimeSpan.FromMilliseconds(1000), cancellationToken: cst.Token);
      await Task.Delay(2000);
      entGroup.GetRight().Flash(new Q42.HueApi.ColorConverters.RGBColor("FF0000"), IteratorEffectMode.All, waitTime: TimeSpan.FromSeconds(1), transitionTimeOn: TimeSpan.FromMilliseconds(1000), transitionTimeOff: TimeSpan.FromMilliseconds(1000), cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      //Console.WriteLine("Or build your own effects");
      //Task.Run(async () =>
      //{
      //  while (true && !cst.Token.IsCancellationRequested)
      //  {
      //    entGroup.SetState(new RGBColor("0000FF"), 1, TimeSpan.FromSeconds(4), cancellationToken: cst.Token);
      //    await Task.Delay(TimeSpan.FromSeconds(5));
      //    entGroup.SetState(new RGBColor("FF0000"), 0.6, TimeSpan.FromSeconds(4), cancellationToken: cst.Token);
      //    await Task.Delay(TimeSpan.FromSeconds(5));
      //  }
      //}, cst.Token);
      //cst = WaitCancelAndNext(cst);

      Console.WriteLine("A red light that is moving in horizontal direction and is placed on an XY grid, matching your entertainment setup");
      var redLightEffect = new RedLightEffect();
      redLightEffect.Radius = 0.3;
      redLightEffect.Y = -0.8;
      redLightEffect.X = -0.8;
      entGroup.PlaceEffect(redLightEffect);
      redLightEffect.Start();

      Task.Run(async () =>
      {
        double step = 0.2;
        while (true)
        {
          redLightEffect.Y += step;
          await Task.Delay(100);
          if (redLightEffect.Y >= 2)
            step = -0.1;
          if (redLightEffect.Y <= -2)
            step = +0.1;
        }
      }, cst.Token);


      cst = WaitCancelAndNext(cst);
      redLightEffect.Stop();


      Console.WriteLine("Thank you for using Q42.Hue.Streaming. This library was developed during Christmas 2017.");
      await allLightsOrdered.Christmas(cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Press Enter to Exit");
      Console.ReadLine();

    }

    private static CancellationTokenSource WaitCancelAndNext(CancellationTokenSource cst)
    {
      Console.WriteLine("Press Enter for next sample");
      Console.ReadLine();
      cst.Cancel();
      cst = new CancellationTokenSource();
      return cst;
    }
  }
}
