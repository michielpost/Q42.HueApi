using HueApi.ColorConverters;
using HueApi.Entertainment.Effects;
using HueApi.Entertainment.Effects.Examples;
using HueApi.Entertainment.Extensions;
using HueApi.Entertainment.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueApi.Entertainment.ConsoleSample
{
  public class HueStreaming
  {
    public async Task Start()
    {
      StreamingGroup stream = await StreamingSetup.SetupAndReturnGroup();
      var baseEntLayer = stream.GetNewLayer(isBaseLayer: true);
      var effectLayer = stream.GetNewLayer();

      //Optional: calculated effects that are placed on this layer
      baseEntLayer.AutoCalculateEffectUpdate(new CancellationToken());
      effectLayer.AutoCalculateEffectUpdate(new CancellationToken());

      //Order lights based on position in the room
      var orderedLeft = baseEntLayer.GetLeft().OrderByDescending(x => x.LightLocation.Y).ThenBy(x => x.LightLocation.X).To2DGroup();
      var orderedRight = baseEntLayer.GetRight().OrderByDescending(x => x.LightLocation.Y).ThenByDescending(x => x.LightLocation.X);
      var allLightsOrdered = baseEntLayer.OrderBy(x => x.LightLocation.X).ThenBy(x => x.LightLocation.Y).ToList().To2DGroup();
      var allLightsOrderedFlat = baseEntLayer.OrderBy(x => x.LightLocation.X).ThenBy(x => x.LightLocation.Y).ToList();
      var orderedByDistance = baseEntLayer.OrderBy(x => x.LightLocation.Distance(0, 0, 0)).To2DGroup();
      var orderedByAngle = baseEntLayer.OrderBy(x => x.LightLocation.Angle(0, 0)).To2DGroup();
      var groupedByDevice = baseEntLayer.To2DDeviceGroup();

      var line1 = baseEntLayer.Where(x => x.LightLocation.X <= -0.6).ToList();
      var line2 = baseEntLayer.Where(x => x.LightLocation.X > -0.6 && x.LightLocation.X <= -0.1).ToList();
      var line3 = baseEntLayer.Where(x => x.LightLocation.X > -0.1 && x.LightLocation.X <= 0.1).ToList();
      var line4 = baseEntLayer.Where(x => x.LightLocation.X > 0.1 && x.LightLocation.X <= 0.6).ToList();
      var line5 = baseEntLayer.Where(x => x.LightLocation.X > 0.6).ToList();

      var allLightsReverse = allLightsOrdered.ToList();
      allLightsReverse.Reverse();


      CancellationTokenSource cst = new CancellationTokenSource();

      if(groupedByDevice.Where(x => x.Count() > 5).Any())
      {
        Console.WriteLine("Knight Rider on Gradient Play Lightstrips");
        foreach(var group in groupedByDevice.Where(x => x.Count() > 5))
        {
          group.To2DGroup().KnightRider(cst.Token);
        }
       
        //allLightsOrdered.KnightRider(cst.Token);
        cst = WaitCancelAndNext(cst);

      }

      //Console.WriteLine("Blue line on 90 degree angle");
      //var blueLineEffect = new HorizontalScanLineEffect();
      //baseEntLayer.PlaceEffect(blueLineEffect);
      //blueLineEffect.Start();
      //cst = WaitCancelAndNext(cst);
      //blueLineEffect.Stop();

      //Ref<int?> stepSize = 20;
      //blueLineEffect.Rotate(stepSize);

      //Console.ReadLine();
      //stepSize.Value -= 5;
      //Console.ReadLine();
      //stepSize.Value -= 5;
      //Console.ReadLine();
      //stepSize.Value -= 5;
      //Console.ReadLine();
      //stepSize.Value -= 5;
      //Console.ReadLine();
      //stepSize.Value -= 5;
      //Console.ReadLine();
      //stepSize.Value -= 5;
      //Console.ReadLine();
      //stepSize.Value -= 5;
      //Console.ReadLine();
      //stepSize.Value -= 5;
      //Console.ReadLine();
      //stepSize.Value -= 5;
      //Console.ReadLine();
      //stepSize.Value -= 5;

      //Console.WriteLine("Finished");

      //cst = WaitCancelAndNext(cst);
      //blueLineEffect.Stop();

      var quarter = new[] { baseEntLayer.GetLeft().GetFront(), baseEntLayer.GetLeft().GetBack(), baseEntLayer.GetRight().GetBack(), baseEntLayer.GetRight().GetFront() }.ToList();

      baseEntLayer.SetState(cst.Token, new RGBColor("FFFFFF"), 1);
      cst = WaitCancelAndNext(cst);
      Console.WriteLine("Transition to red in 10 seconds");
      baseEntLayer.SetState(cst.Token, new RGBColor("FF0000"), TimeSpan.FromSeconds(10));
      Console.ReadLine();
      Console.WriteLine("Transition to bri 0.25");
      baseEntLayer.SetState(cst.Token, null, default, 0.25, TimeSpan.FromSeconds(1), true);
      Console.ReadLine();
      Console.WriteLine("Transition to bri 1");
      baseEntLayer.SetState(cst.Token, new RGBColor("0000FF"), TimeSpan.FromSeconds(5), 1, TimeSpan.FromSeconds(1), false);
      Console.ReadLine();

      cst = WaitCancelAndNext(cst);
      Console.WriteLine("Random color All / All");
      quarter.SetRandomColor(cst.Token, IteratorEffectMode.All, IteratorEffectMode.All, waitTime: () => TimeSpan.FromMilliseconds(500));
      cst = WaitCancelAndNext(cst);


      Console.WriteLine("Flash on lights Cycle / Random");
      quarter.FlashQuick(cst.Token, new HueApi.ColorConverters.RGBColor("FFFFFF"), IteratorEffectMode.Cycle, IteratorEffectMode.Random, waitTime: () => TimeSpan.FromMilliseconds(50));
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("SetColor white Single / Single");
      quarter.SetColor(cst.Token, new RGBColor("FFFFFF"), IteratorEffectMode.Single, IteratorEffectMode.Single, waitTime: () => TimeSpan.FromMilliseconds(200));
      cst = WaitCancelAndNext(cst);


      Console.WriteLine("Flash on lights Cycle / All");
      quarter.FlashQuick(cst.Token, new HueApi.ColorConverters.RGBColor("FFFFFF"), IteratorEffectMode.Cycle, IteratorEffectMode.All, waitTime: () => TimeSpan.FromMilliseconds(50));
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Flash on lights Cycle / Single");
      quarter.FlashQuick(cst.Token, new HueApi.ColorConverters.RGBColor("FFFFFF"), IteratorEffectMode.Cycle, IteratorEffectMode.Single, waitTime: () => TimeSpan.FromMilliseconds(50));
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Random color Cycle / All");
      quarter.SetRandomColor(cst.Token, IteratorEffectMode.Cycle, IteratorEffectMode.All, waitTime: () => TimeSpan.FromMilliseconds(500));
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Random color Cycle / AllIndividual");
      quarter.SetRandomColor(cst.Token, IteratorEffectMode.Cycle, IteratorEffectMode.AllIndividual, waitTime: () => TimeSpan.FromMilliseconds(500));
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Random color Cycle / Single");
      quarter.SetRandomColor(cst.Token, IteratorEffectMode.Cycle, IteratorEffectMode.Single, waitTime: () => TimeSpan.FromMilliseconds(500));
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Random color Cycle / Random");
      quarter.SetRandomColor(cst.Token, IteratorEffectMode.Cycle, IteratorEffectMode.Random, waitTime: () => TimeSpan.FromMilliseconds(500));
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Random color Cycle / Bounce");
      quarter.SetRandomColor(cst.Token, IteratorEffectMode.Cycle, IteratorEffectMode.Bounce, waitTime: () => TimeSpan.FromMilliseconds(500));
      cst = WaitCancelAndNext(cst);


      Console.WriteLine("Random color on all lights");
      baseEntLayer.To2DGroup().SetRandomColor(cst.Token, IteratorEffectMode.All, waitTime: () => TimeSpan.FromMilliseconds(500));
      cst = WaitCancelAndNext(cst);

      //Uncomment for demo using a secondary layer
      //var secondGroup = stream.GetNewLayer();
      //secondGroup.FlashQuick(new Q42.HueApi.ColorConverters.RGBColor("FFFFFF"), IteratorEffectMode.Cycle, waitTime: TimeSpan.FromMilliseconds(500));

      cst = WaitCancelAndNext(cst);

      //Group demo
      //Console.WriteLine("Group demo");
      ////var groups = new List<IEnumerable<EntertainmentLight>>() { line1, line2, line3, line4, line5 };
      //var groups = allLightsOrderedFlat.ChunkBy(5);
      //var groupstest = allLightsOrderedFlat.ChunkByGroupNumber(4);
      //groups.IteratorEffect(async (current, duration) => {
      //  //var r = new Random();
      //  //var color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
      //  //current.SetState(color, 1);

      //  current.SetRandomColor(IteratorEffectMode.All, TimeSpan.FromMilliseconds(5000), duration: duration);

      //}, IteratorEffectMode.All, TimeSpan.FromMilliseconds(500));
      //cst = WaitCancelAndNext(cst);


      //Random color from center
      Console.WriteLine("Fill white color from center");
      await orderedByDistance.SetColor(cst.Token, new RGBColor("FFFFFF"), IteratorEffectMode.Single, waitTime: () => TimeSpan.FromMilliseconds(50));
      cst = WaitCancelAndNext(cst);

      //Random color from center
      Console.WriteLine("Fill red color order by angle from center");
      await orderedByAngle.SetColor(cst.Token, new RGBColor("FF0000"), IteratorEffectMode.Single, waitTime: () => TimeSpan.FromMilliseconds(50));
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("A pulse of random color is placed on an XY grid, matching your entertainment setup");
      var randomPulseEffect = new RandomPulseEffect();
      baseEntLayer.PlaceEffect(randomPulseEffect);
      randomPulseEffect.Start();

      cst = WaitCancelAndNext(cst);
      randomPulseEffect.Stop();

      Console.WriteLine("A pulse of random color is placed on an XY grid, matching your entertainment setup");
      var randomPulseEffectNoFade = new RandomPulseEffect(false);
      baseEntLayer.PlaceEffect(randomPulseEffectNoFade);
      randomPulseEffectNoFade.Start();

      cst = WaitCancelAndNext(cst);
      randomPulseEffectNoFade.Stop();


      Console.WriteLine("Different random colors on all lights");
      baseEntLayer.To2DGroup().SetRandomColor(cst.Token, IteratorEffectMode.AllIndividual, waitTime: () => TimeSpan.FromMilliseconds(500));
      cst = WaitCancelAndNext(cst);


      Console.WriteLine("Trailing light effect with transition times");
      allLightsOrdered.Flash(cst.Token, new HueApi.ColorConverters.RGBColor("FF0000"), IteratorEffectMode.Cycle, waitTime: () => TimeSpan.FromMilliseconds(500), transitionTimeOn: () => TimeSpan.FromMilliseconds(1000), transitionTimeOff: () => TimeSpan.FromMilliseconds(1000), waitTillFinished: false);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Knight rider (works best with 6+ lights)");
      allLightsOrdered.KnightRider(cst.Token);
      cst = WaitCancelAndNext(cst);



      Ref<TimeSpan> waitTime = TimeSpan.FromMilliseconds(750);

      Console.WriteLine("Flash lights (750ms), press enter to decrease by 200 ms");
      allLightsOrdered.FlashQuick(cst.Token, new HueApi.ColorConverters.RGBColor("FFFFFF"), IteratorEffectMode.Cycle, waitTime: () => waitTime);
      Console.ReadLine();

      waitTime.Value -= TimeSpan.FromMilliseconds(200);
      Console.WriteLine($"Flash ({waitTime.Value.TotalMilliseconds})");
      Console.ReadLine();

      waitTime.Value -= TimeSpan.FromMilliseconds(200);
      Console.WriteLine($"Flash ({waitTime.Value.TotalMilliseconds})");
      Console.ReadLine();

      waitTime.Value -= TimeSpan.FromMilliseconds(200);
      Console.WriteLine($"Flash ({waitTime.Value.TotalMilliseconds})");
      Console.ReadLine();

      waitTime.Value -= TimeSpan.FromMilliseconds(100);
      Console.WriteLine($"Flash ({waitTime.Value.TotalMilliseconds})");
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Flash on random lights");
      allLightsOrdered.FlashQuick(cst.Token, new HueApi.ColorConverters.RGBColor("FFFFFF"), IteratorEffectMode.Random, waitTime: () => waitTime);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Flash on ALL lights");
      waitTime.Value = TimeSpan.FromMilliseconds(150);
      allLightsOrdered.Flash(cst.Token, new HueApi.ColorConverters.RGBColor("FFFFFF"), IteratorEffectMode.All, waitTime: () => waitTime);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Flash effect with transition times");
      baseEntLayer.GetLeft().To2DGroup().Flash(cst.Token, new HueApi.ColorConverters.RGBColor("FF0000"), IteratorEffectMode.All, waitTime: () => TimeSpan.FromSeconds(1), transitionTimeOn: () => TimeSpan.FromMilliseconds(1000), transitionTimeOff: () => TimeSpan.FromMilliseconds(1000));
      await Task.Delay(2000);
      baseEntLayer.GetRight().To2DGroup().Flash(cst.Token, new HueApi.ColorConverters.RGBColor("FF0000"), IteratorEffectMode.All, waitTime: () => TimeSpan.FromSeconds(1), transitionTimeOn: () => TimeSpan.FromMilliseconds(1000), transitionTimeOff: () => TimeSpan.FromMilliseconds(1000));
      cst = WaitCancelAndNext(cst);


      Console.WriteLine("A red light that is moving in vertical direction and is placed on an XY grid, matching your entertainment setup");
      var redLightEffect = new RedLightEffect();
      redLightEffect.Radius = 0.7;
      redLightEffect.Y = -0.8;
      redLightEffect.X = -0.8;
      baseEntLayer.PlaceEffect(redLightEffect);
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
      await allLightsOrdered.Christmas(cst.Token);
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
