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
      StreamingGroup stream = await StreamingSetup.SetupAndReturnGroup();
      var entGroup = stream.GetNewLayer(isBaseLayer: true);

      //Optional: calculated effects that are placed on this layer
      entGroup.AutoCalculateEffectUpdate();

      //Order lights based on position in the room
      var orderedLeft = entGroup.GetLeft().OrderByDescending(x => x.LightLocation.Y).ThenBy(x => x.LightLocation.X);
      var orderedRight = entGroup.GetRight().OrderByDescending(x => x.LightLocation.Y).ThenByDescending(x => x.LightLocation.X);
      var allLightsOrdered = entGroup.OrderBy(x => x.LightLocation.X).ThenBy(x => x.LightLocation.Y).ToList();
      var orderedByDistance = entGroup.OrderBy(x => x.LightLocation.Distance(0, 0));
      var orderedByAngle = entGroup.OrderBy(x => x.LightLocation.Angle(0, 0));
      var line1 = entGroup.Where(x => x.LightLocation.X <= -0.6).ToList();
      var line2 = entGroup.Where(x => x.LightLocation.X > -0.6 && x.LightLocation.X <= -0.1).ToList();
      var line3 = entGroup.Where(x => x.LightLocation.X > -0.1 && x.LightLocation.X <= 0.1).ToList();
      var line4 = entGroup.Where(x => x.LightLocation.X > 0.1 && x.LightLocation.X  <= 0.6).ToList();
      var line5 = entGroup.Where(x => x.LightLocation.X > 0.6).ToList();

      var allLightsReverse = allLightsOrdered.ToList();
      allLightsReverse.Reverse();


      CancellationTokenSource cst = new CancellationTokenSource();

      Console.WriteLine("Random color on all lights");
      entGroup.SetRandomColor(IteratorEffectMode.All, TimeSpan.FromMilliseconds(500), cancellationToken: cst.Token);

      //Uncomment for demo using a secondary layer
      //var secondGroup = stream.GetNewLayer();
      //secondGroup.FlashQuick(new Q42.HueApi.ColorConverters.RGBColor("FFFFFF"), IteratorEffectMode.Cycle, waitTime: TimeSpan.FromMilliseconds(500));

      cst = WaitCancelAndNext(cst);

      //Group demo
      Console.WriteLine("Group demo");
      //var groups = new List<IEnumerable<EntertainmentLight>>() { line1, line2, line3, line4, line5 };
      var groups = orderedByAngle.ChunkBy(4);
      var groupstest = orderedByAngle.ChunkByGroupNumber(4);
      groups.IteratorEffect(async (current, duration) => {
        //var r = new Random();
        //var color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
        //current.SetState(color, 1);

        current.SetRandomColor(IteratorEffectMode.All, TimeSpan.FromMilliseconds(500), duration: duration, cancellationToken: cst.Token);

      }, IteratorEffectMode.Cycle, TimeSpan.FromMilliseconds(100), cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);


      //Random color from center
      Console.WriteLine("Fill white color from center");
      await orderedByDistance.SetColor(new RGBColor("FFFFFF"), IteratorEffectMode.Single, TimeSpan.FromMilliseconds(50), cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      //Random color from center
      Console.WriteLine("Fill red color order by angle from center");
      await orderedByAngle.SetColor(new RGBColor("FF0000"), IteratorEffectMode.Single, TimeSpan.FromMilliseconds(50), cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("A pulse of random color is placed on an XY grid, matching your entertainment setup");
      var randomPulseEffect = new RandomPulseEffect();
      entGroup.PlaceEffect(randomPulseEffect);
      randomPulseEffect.Start();

      cst = WaitCancelAndNext(cst);
      randomPulseEffect.Stop();

      Console.WriteLine("A pulse of random color is placed on an XY grid, matching your entertainment setup");
      var randomPulseEffectNoFade = new RandomPulseEffect(false);
      entGroup.PlaceEffect(randomPulseEffectNoFade);
      randomPulseEffectNoFade.Start();

      cst = WaitCancelAndNext(cst);
      randomPulseEffectNoFade.Stop();


      Console.WriteLine("Different random colors on all lights");
      entGroup.SetRandomColor(IteratorEffectMode.AllIndividual, TimeSpan.FromMilliseconds(500), cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);


      Console.WriteLine("Trailing light effect with transition times");
      allLightsOrdered.Flash(new Q42.HueApi.ColorConverters.RGBColor("FF0000"), IteratorEffectMode.Cycle, waitTime: TimeSpan.FromMilliseconds(500), transitionTimeOn: TimeSpan.FromMilliseconds(1000), transitionTimeOff: TimeSpan.FromMilliseconds(1000), waitTillFinished: false, cancellationToken: cst.Token);
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


      Console.WriteLine("A red light that is moving in vertical direction and is placed on an XY grid, matching your entertainment setup");
      var redLightEffect = new RedLightEffect();
      redLightEffect.Radius = 0.7;
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
