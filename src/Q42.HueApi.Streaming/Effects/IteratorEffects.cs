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
    public static Task KnightRider(this IEnumerable<StreamingLight> group, CancellationToken cancellationToken = new CancellationToken())
    {
      return group.IteratorEffect((current, prev) => {
        current.SetState(new Q42.HueApi.ColorConverters.RGBColor("FF0000"), 1, TimeSpan.FromMilliseconds(0));
        prev.SetBrightness(0, TimeSpan.FromMilliseconds(700));
      }, IteratorEffectMode.Cycle, TimeSpan.FromMilliseconds(450), cancellationToken);
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
      return group.IteratorEffect((current, prev) => {
        if (startGreen)
          current.SetState(new Q42.HueApi.ColorConverters.RGBColor("00FF00"), 1, TimeSpan.FromSeconds(0));
        else
          current.SetState(new Q42.HueApi.ColorConverters.RGBColor("FF0000"), 1, TimeSpan.FromSeconds(0));

        startGreen = !startGreen;
      }, IteratorEffectMode.Single, TimeSpan.FromSeconds(0), cancellationToken);
    }
  }
}
