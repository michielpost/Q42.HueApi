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
  public class HueLightDJ
  {
    public async Task Start()
    {
      StreamingGroup stream = await StreamingSetup.SetupAndReturnGroup();
      var entGroup = stream.GetNewLayer();

      //Order lights based on position in the room
      var orderedLeft = entGroup.GetLeft().OrderByDescending(x => x.LightLocation.Y).ThenBy(x => x.LightLocation.X);
      var orderedRight = entGroup.GetRight().OrderByDescending(x => x.LightLocation.Y).ThenByDescending(x => x.LightLocation.X);
      var allLightsOrdered = orderedLeft.Concat(orderedRight.Reverse()).ToArray();

      var allLightsReverse = allLightsOrdered.ToList();
      allLightsReverse.Reverse();

      CancellationTokenSource cst = new CancellationTokenSource();
      Ref<TimeSpan?> baseTimer = TimeSpan.FromMilliseconds(500); 
      Ref<TimeSpan?> quick = TimeSpan.FromMilliseconds(50); 

      Console.WriteLine("Start base colors on all lights");
      entGroup.SetRandomColor(IteratorEffectMode.AllIndividual, baseTimer, cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Same random color on all lights");
      entGroup.SetRandomColor(IteratorEffectMode.All, TimeSpan.FromMilliseconds(500), cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Start base colors on all lights");
      entGroup.SetRandomColor(IteratorEffectMode.AllIndividual, baseTimer, cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Flash lights (750ms), press enter to decrease by 200 ms");
      allLightsOrdered.FlashQuick(null, IteratorEffectMode.Cycle, waitTime: quick, cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Start base colors on all lights");
      entGroup.SetRandomColor(IteratorEffectMode.AllIndividual, baseTimer, cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Flash on random lights");
      allLightsOrdered.FlashQuick(null, IteratorEffectMode.Random, waitTime: quick, cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Start base colors on all lights");
      entGroup.SetRandomColor(IteratorEffectMode.AllIndividual, baseTimer, cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Flash on ALL lights");
      allLightsOrdered.Flash(new Q42.HueApi.ColorConverters.RGBColor("FFFFFF"), IteratorEffectMode.All, waitTime: quick, cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Start base colors on all lights");
      entGroup.SetRandomColor(IteratorEffectMode.AllIndividual, baseTimer, cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Flash effect with transition times");
      entGroup.GetLeft().Flash(new Q42.HueApi.ColorConverters.RGBColor("FF0000"), IteratorEffectMode.All, waitTime: TimeSpan.FromSeconds(1), transitionTimeOn: TimeSpan.FromMilliseconds(1000), transitionTimeOff: TimeSpan.FromMilliseconds(1000), cancellationToken: cst.Token);
      await Task.Delay(2000);
      entGroup.GetRight().Flash(new Q42.HueApi.ColorConverters.RGBColor("FF0000"), IteratorEffectMode.All, waitTime: TimeSpan.FromSeconds(1), transitionTimeOn: TimeSpan.FromMilliseconds(1000), transitionTimeOff: TimeSpan.FromMilliseconds(1000), cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

      Console.WriteLine("Start base colors on all lights");
      entGroup.SetRandomColor(IteratorEffectMode.AllIndividual, baseTimer, cancellationToken: cst.Token);
      cst = WaitCancelAndNext(cst);

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
