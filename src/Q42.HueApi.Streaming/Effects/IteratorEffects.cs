using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Extensions;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Effects
{
  public static class IteratorEffects
  {
    public static IEnumerable<IEnumerable<EntertainmentLight>> To2DGroup(this IEnumerable<EntertainmentLight> group)
    {
      return group.Select(x => new List<EntertainmentLight> { x });
    }

    public static Task SetColor(this IEnumerable<IEnumerable<EntertainmentLight>> group, RGBColor color, IteratorEffectMode mode = IteratorEffectMode.Cycle, IteratorEffectMode secondaryIteratorMode = IteratorEffectMode.All, Ref<TimeSpan?> waitTime = null, Ref<TimeSpan?> transitionTime = null, TimeSpan? duration = null, CancellationToken cancellationToken = new CancellationToken())
    {
      var list = new List<RGBColor>() { color };
      return SetRandomColorFromList(group, list, mode, secondaryIteratorMode, waitTime, transitionTime, duration, cancellationToken);
    }


    public static Task SetRandomColorFromList(this IEnumerable<IEnumerable<EntertainmentLight>> group, List<RGBColor> colors, IteratorEffectMode mode = IteratorEffectMode.Cycle, IteratorEffectMode secondaryIteratorMode = IteratorEffectMode.All, Ref<TimeSpan?> waitTime = null, Ref<TimeSpan?> transitionTime = null, TimeSpan? duration = null, CancellationToken cancellationToken = new CancellationToken())
    {
      if (waitTime == null)
        waitTime = TimeSpan.FromMilliseconds(50);
      if (transitionTime == null)
        transitionTime = TimeSpan.FromMilliseconds(0);

      return group.IteratorEffect(async (current, t) =>
      {
        var color = colors.OrderBy(x => new Guid()).First();
        current.SetState(color, 1, transitionTime.Value.Value, cancellationToken: cancellationToken);
      }, mode, secondaryIteratorMode, waitTime, duration, cancellationToken: cancellationToken);
    }


    public static Task SetRandomColor(this IEnumerable<IEnumerable<EntertainmentLight>> group, IteratorEffectMode mode = IteratorEffectMode.Cycle, IteratorEffectMode secondaryIteratorMode = IteratorEffectMode.All, Ref<TimeSpan?> waitTime = null, Ref<TimeSpan?> transitionTime = null, TimeSpan? duration = null, CancellationToken cancellationToken = new CancellationToken())
    {
      if (waitTime == null)
        waitTime = TimeSpan.FromMilliseconds(50);
      if (transitionTime == null)
        transitionTime = TimeSpan.FromMilliseconds(0);

      return group.IteratorEffect(async (current, t) =>
      {
        var r = new Random();
        var color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());

        current.SetState(color, 1, transitionTime.Value.Value, cancellationToken: cancellationToken);
      }, mode, secondaryIteratorMode, waitTime, duration, cancellationToken: cancellationToken);
    }



    /// <summary>
    /// Does not wait for the previous flash to end
    /// NOTE: Can not be used with mode All or AllIndividual
    /// </summary>
    /// <param name="group"></param>
    /// <param name="color"></param>
    /// <param name="mode"></param>
    /// <param name="waitTime"></param>
    /// <param name="onTime"></param>
    /// <param name="transitionTimeOn"></param>
    /// <param name="transitionTimeOff"></param>
    /// <param name="duration"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task FlashQuick(this IEnumerable<IEnumerable<EntertainmentLight>> group, RGBColor? color, IteratorEffectMode mode = IteratorEffectMode.Cycle, IteratorEffectMode secondaryIteratorMode = IteratorEffectMode.All, Ref<TimeSpan?> waitTime = null, Ref<TimeSpan?> onTime = null, Ref<TimeSpan?> transitionTimeOn = null, Ref<TimeSpan?> transitionTimeOff = null, TimeSpan? duration = null, CancellationToken cancellationToken = new CancellationToken())
    {
      if (!color.HasValue)
      {
        var r = new Random();
        color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
      }

      if (mode == IteratorEffectMode.All || mode == IteratorEffectMode.AllIndividual)
        return group.Flash(color.Value, mode, secondaryIteratorMode, waitTime, onTime, transitionTimeOn, transitionTimeOff, true, duration, cancellationToken);
      else
      {
        if (waitTime == null)
          waitTime = TimeSpan.FromMilliseconds(50);
        if (onTime == null)
          onTime = waitTime;
        if (transitionTimeOn == null)
          transitionTimeOn = TimeSpan.FromMilliseconds(0);
        if (transitionTimeOff == null)
          transitionTimeOff = TimeSpan.FromMilliseconds(0);

        Ref<TimeSpan?> actualWaitTime = onTime.Value + transitionTimeOn.Value;

        return group.IteratorEffect(async (current, t) =>
        {
          actualWaitTime.Value = onTime.Value.Value + transitionTimeOn.Value.Value;

          current.SetState(color, 1, transitionTimeOn.Value.Value, cancellationToken: cancellationToken);
          Task.Run(async () =>
          {
            await Task.Delay(onTime.Value.Value + transitionTimeOn.Value.Value, cancellationToken);
            current.SetBrightness(0, transitionTimeOff.Value.Value, cancellationToken: cancellationToken);
          }, cancellationToken);
        }, mode, secondaryIteratorMode, actualWaitTime, duration, cancellationToken: cancellationToken);

      }
    }


    public static Task Flash(this IEnumerable<IEnumerable<EntertainmentLight>> group, RGBColor? color, IteratorEffectMode mode = IteratorEffectMode.Cycle, IteratorEffectMode secondaryIteratorMode = IteratorEffectMode.All, Ref<TimeSpan?> waitTime = null, Ref<TimeSpan?> onTime = null, Ref<TimeSpan?> transitionTimeOn = null, Ref<TimeSpan?> transitionTimeOff = null, bool waitTillFinished = true, TimeSpan? duration = null, CancellationToken cancellationToken = new CancellationToken())
    {
      if (!color.HasValue)
      {
        var r = new Random();
        color = new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
      }

      if (waitTime == null)
        waitTime = TimeSpan.FromMilliseconds(50);
      if (onTime == null)
        onTime = waitTime;
      if (transitionTimeOn == null)
        transitionTimeOn = TimeSpan.FromMilliseconds(0);
      if (transitionTimeOff == null)
        transitionTimeOff = TimeSpan.FromMilliseconds(0);

      Ref<TimeSpan?> actualWaitTime = waitTime.Value + onTime.Value + transitionTimeOn.Value + transitionTimeOff.Value;
      if (!waitTillFinished)
        actualWaitTime = waitTime.Value;

      return group.IteratorEffect(async (current, t) =>
      {
        if (!waitTillFinished)
          actualWaitTime.Value = waitTime.Value;
        else
          actualWaitTime.Value = waitTime.Value.Value + onTime.Value.Value + transitionTimeOn.Value.Value + transitionTimeOff.Value.Value;

        current.SetState(color, 1, transitionTimeOn.Value.Value, cancellationToken: cancellationToken);
        Task.Run(async () =>
        {
          await Task.Delay(onTime.Value.Value + transitionTimeOn.Value.Value);
          current.SetBrightness(0, transitionTimeOff.Value.Value, cancellationToken: cancellationToken);
        }, cancellationToken);
      }, mode, secondaryIteratorMode, actualWaitTime, duration, cancellationToken: cancellationToken);
    }


    public static Task KnightRider(this IEnumerable<IEnumerable<EntertainmentLight>> group, TimeSpan? duration = null, CancellationToken cancellationToken = new CancellationToken())
    {
      return group.IteratorEffect((current, t) =>
      {
        current.SetState(new Q42.HueApi.ColorConverters.RGBColor("FF0000"), 1, TimeSpan.FromMilliseconds(0), cancellationToken: cancellationToken);
        Task.Run(async () =>
        {
          await Task.Delay(t.Value, cancellationToken);
          current.SetBrightness(0, TimeSpan.FromMilliseconds(750));
        }, cancellationToken);
        return Task.CompletedTask;
      }, IteratorEffectMode.Bounce, IteratorEffectMode.All, TimeSpan.FromMilliseconds(225), duration, cancellationToken: cancellationToken);
    }

    public static async Task Christmas(this IEnumerable<IEnumerable<EntertainmentLight>> group, CancellationToken cancellationToken = new CancellationToken())
    {
      bool startGreen = false;
      while (!cancellationToken.IsCancellationRequested)
      {
        await group.ChristmasInit(startGreen, cancellationToken);
        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        startGreen = !startGreen;
      }
    }

    private static Task ChristmasInit(this IEnumerable<IEnumerable<EntertainmentLight>> group, bool startGreen = false, CancellationToken cancellationToken = new CancellationToken())
    {
      return group.IteratorEffect(async (current, timeSpan) =>
      {
        if (startGreen)
          current.SetState(new Q42.HueApi.ColorConverters.RGBColor("00FF00"), 1, TimeSpan.FromSeconds(0), cancellationToken: cancellationToken);
        else
          current.SetState(new Q42.HueApi.ColorConverters.RGBColor("FF0000"), 1, TimeSpan.FromSeconds(0), cancellationToken: cancellationToken);

        startGreen = !startGreen;
      }, IteratorEffectMode.Single, IteratorEffectMode.All, TimeSpan.FromSeconds(0), cancellationToken: cancellationToken);
    }
  }
}
