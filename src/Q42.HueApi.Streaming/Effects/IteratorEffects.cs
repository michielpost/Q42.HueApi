using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Effects
{
  public static class IteratorEffects
  {
    public static Task QuickFlash(this IEnumerable<StreamingLight> group, RGBColor color, IteratorEffectMode mode = IteratorEffectMode.Cycle, Ref<TimeSpan?> waitTime = null, TimeSpan? duration = null, CancellationToken cancellationToken = new CancellationToken())
    {
      if (waitTime == null)
        waitTime = TimeSpan.FromMilliseconds(50);
      return group.IteratorEffect(async (current, t) => {
        current.SetState(color, 1, TimeSpan.FromMilliseconds(0));
        Task.Run(async () => { 
          await Task.Delay(t.Value);
          current.SetBrightness(0, TimeSpan.FromMilliseconds(0));
        }, cancellationToken);
      }, mode, waitTime, duration, cancellationToken);
    }

    public static Task KnightRider(this IEnumerable<StreamingLight> group, TimeSpan? duration = null, CancellationToken cancellationToken = new CancellationToken())
    {
      return group.IteratorEffect((current, t) => {
        current.SetState(new Q42.HueApi.ColorConverters.RGBColor("FF0000"), 1, TimeSpan.FromMilliseconds(0));
        Task.Run(async () => {
          await Task.Delay(t.Value);
          current.SetBrightness(0, TimeSpan.FromMilliseconds(750));
        }, cancellationToken);
      }, IteratorEffectMode.Bounce, TimeSpan.FromMilliseconds(225), duration, cancellationToken);
    }

    public static async Task Christmas(this IEnumerable<StreamingLight> group, CancellationToken cancellationToken = new CancellationToken())
    {
      bool startGreen = false;
      while(!cancellationToken.IsCancellationRequested)
      {
        await group.ChristmasInit(startGreen, cancellationToken);
        await Task.Delay(TimeSpan.FromSeconds(5));
        startGreen = !startGreen;
      }
    }

    public static Task ChristmasInit(this IEnumerable<StreamingLight> group, bool startGreen = false, CancellationToken cancellationToken = new CancellationToken())
    {
      return group.IteratorEffect((current, timeSpan) => {
        if (startGreen)
          current.SetState(new Q42.HueApi.ColorConverters.RGBColor("00FF00"), 1, TimeSpan.FromSeconds(0));
        else
          current.SetState(new Q42.HueApi.ColorConverters.RGBColor("FF0000"), 1, TimeSpan.FromSeconds(0));

        startGreen = !startGreen;
      }, IteratorEffectMode.Single, TimeSpan.FromSeconds(0), cancellationToken: cancellationToken);
    }
  }
}
