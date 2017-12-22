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
    public static Task QuickFlash(this IEnumerable<StreamingLight> group, RGBColor color, IteratorEffectMode mode = IteratorEffectMode.Cycle, TimeSpan ? timeSpan = null, CancellationToken cancellationToken = new CancellationToken())
    {
      if (timeSpan == null)
        timeSpan = TimeSpan.FromMilliseconds(50);
      return group.IteratorEffect(async (current, prev, t) => {
        current.SetState(color, 1, TimeSpan.FromMilliseconds(0));
        await Task.Delay(t.Value);
        current.SetBrightness(0, TimeSpan.FromMilliseconds(0));
      }, mode, timeSpan, cancellationToken);
    }

    public static Task KnightRider(this IEnumerable<StreamingLight> group, CancellationToken cancellationToken = new CancellationToken())
    {
      return group.IteratorEffect((current, prev, timeSpan) => {
        current.SetState(new Q42.HueApi.ColorConverters.RGBColor("FF0000"), 1, TimeSpan.FromMilliseconds(0));
        prev.SetBrightness(0, TimeSpan.FromMilliseconds(750));
      }, IteratorEffectMode.Bounce, TimeSpan.FromMilliseconds(225), cancellationToken);
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
      return group.IteratorEffect((current, prev, timeSpan) => {
        if (startGreen)
          current.SetState(new Q42.HueApi.ColorConverters.RGBColor("00FF00"), 1, TimeSpan.FromSeconds(0));
        else
          current.SetState(new Q42.HueApi.ColorConverters.RGBColor("FF0000"), 1, TimeSpan.FromSeconds(0));

        startGreen = !startGreen;
      }, IteratorEffectMode.Single, TimeSpan.FromSeconds(0), cancellationToken);
    }
  }
}
