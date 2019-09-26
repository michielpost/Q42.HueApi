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

    public static Task SetColor(this IEnumerable<IEnumerable<EntertainmentLight>> group, CancellationToken cancellationToken, RGBColor color, IteratorEffectMode mode = IteratorEffectMode.Cycle, IteratorEffectMode secondaryIteratorMode = IteratorEffectMode.All, Func<TimeSpan>? waitTime = null, Func<TimeSpan>? transitionTime = null, TimeSpan? duration = null)
    {
      var list = new List<RGBColor>() { color };
      return SetRandomColorFromList(group, cancellationToken, list, mode, secondaryIteratorMode, waitTime, transitionTime, duration);
    }


    public static Task SetRandomColorFromList(this IEnumerable<IEnumerable<EntertainmentLight>> group, CancellationToken cancellationToken, List<RGBColor> colors, IteratorEffectMode mode = IteratorEffectMode.Cycle, IteratorEffectMode secondaryIteratorMode = IteratorEffectMode.All, Func<TimeSpan>? waitTime = null, Func<TimeSpan>? transitionTime = null, TimeSpan? duration = null)
    {
      if (waitTime == null)
        waitTime = () => TimeSpan.FromMilliseconds(50);
      if (transitionTime == null)
        transitionTime = () => TimeSpan.FromMilliseconds(0);

      return group.IteratorEffect(cancellationToken, async(current, ct, t) =>
      {
        var color = colors.OrderBy(x => new Guid()).First();
        current.SetState(ct, color, 1, transitionTime());
      }, mode, secondaryIteratorMode, waitTime, duration);
    }


    public static Task SetRandomColor(this IEnumerable<IEnumerable<EntertainmentLight>> group, CancellationToken cancellationToken, IteratorEffectMode mode = IteratorEffectMode.Cycle, IteratorEffectMode secondaryIteratorMode = IteratorEffectMode.All, Func<TimeSpan>? waitTime = null, Func<TimeSpan>? transitionTime = null, TimeSpan? duration = null)
    {
      if (waitTime == null)
        waitTime = () => TimeSpan.FromMilliseconds(50);
      if (transitionTime == null)
        transitionTime = () => TimeSpan.FromMilliseconds(0);

      return group.IteratorEffect(cancellationToken, async(current, ct, t) =>
      {
        var color = RGBColor.Random();

        current.SetState(ct, color, 1, transitionTime());
      }, mode, secondaryIteratorMode, waitTime, duration);
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
    public static Task FlashQuick(this IEnumerable<IEnumerable<EntertainmentLight>> group, CancellationToken cancellationToken, RGBColor? color, IteratorEffectMode mode = IteratorEffectMode.Cycle, IteratorEffectMode secondaryIteratorMode = IteratorEffectMode.All, Func<TimeSpan>? waitTime = null, Func<TimeSpan>? onTime = null, Func<TimeSpan>? transitionTimeOn = null, Func<TimeSpan>? transitionTimeOff = null, TimeSpan? duration = null)
    {
      if (!color.HasValue)
      {
        color = RGBColor.Random();
      }

      if (mode == IteratorEffectMode.All || mode == IteratorEffectMode.AllIndividual)
        return group.Flash(cancellationToken, color.Value, mode, secondaryIteratorMode, waitTime, onTime, transitionTimeOn, transitionTimeOff, true, duration);
      else
      {
        if (waitTime == null)
          waitTime = () => TimeSpan.FromMilliseconds(50);
        if (onTime == null)
          onTime = waitTime;
        if (transitionTimeOn == null)
          transitionTimeOn = () => TimeSpan.FromMilliseconds(0);
        if (transitionTimeOff == null)
          transitionTimeOff = () => TimeSpan.FromMilliseconds(0);

        Func<TimeSpan> actualWaitTime = () => onTime() + transitionTimeOn();

        return group.IteratorEffect(cancellationToken, async(current, ct, t) =>
        {
          current.SetState(ct, color, 1, transitionTimeOn());
          Task.Run(async () =>
          {
            await Task.Delay(onTime() + transitionTimeOn(), ct).ConfigureAwait(false);
            current.SetBrightness(ct, 0, transitionTimeOff());
          }, ct);
        }, mode, secondaryIteratorMode, actualWaitTime, duration);

      }
    }


    public static Task Flash(this IEnumerable<IEnumerable<EntertainmentLight>> group, CancellationToken cancellationToken, RGBColor? color, IteratorEffectMode mode = IteratorEffectMode.Cycle, IteratorEffectMode secondaryIteratorMode = IteratorEffectMode.All, Func<TimeSpan>? waitTime = null, Func<TimeSpan>? onTime = null, Func<TimeSpan>? transitionTimeOn = null, Func<TimeSpan>? transitionTimeOff = null, bool waitTillFinished = true, TimeSpan? duration = null)
    {
      if (!color.HasValue)
      {
        color = RGBColor.Random();
      }

      if (waitTime == null)
        waitTime = () => TimeSpan.FromMilliseconds(50);
      if (onTime == null)
        onTime = waitTime;
      if (transitionTimeOn == null)
        transitionTimeOn = () => TimeSpan.FromMilliseconds(0);
      if (transitionTimeOff == null)
        transitionTimeOff = () => TimeSpan.FromMilliseconds(0);

      Func<TimeSpan> actualWaitTime = () => waitTime() + onTime() + transitionTimeOn() + transitionTimeOff();
      if (!waitTillFinished)
        actualWaitTime = waitTime;

      return group.IteratorEffect(cancellationToken, async(current, ct, t) =>
      {
        if (!waitTillFinished)
          actualWaitTime = waitTime;
        else
          actualWaitTime = () => waitTime() + onTime() + transitionTimeOn() + transitionTimeOff();

        current.SetState(ct, color, 1, transitionTimeOn());
        Task.Run(async () =>
        {
          await Task.Delay(onTime() + transitionTimeOn(), ct).ConfigureAwait(false);
          current.SetBrightness(ct, 0, transitionTimeOff());
        }, ct);
      }, mode, secondaryIteratorMode, actualWaitTime, duration);
    }


    public static Task KnightRider(this IEnumerable<IEnumerable<EntertainmentLight>> group, CancellationToken cancellationToken, TimeSpan? duration = null)
    {
      return group.IteratorEffect(cancellationToken, (current, ct, t) =>
      {
        current.SetState(ct, new Q42.HueApi.ColorConverters.RGBColor("FF0000"), 1, TimeSpan.FromMilliseconds(0));
        Task.Run(async () =>
        {
          await Task.Delay(t, ct).ConfigureAwait(false);
          current.SetBrightness(ct, 0, TimeSpan.FromMilliseconds(750));
        }, ct);
        return Task.CompletedTask;
      }, IteratorEffectMode.Bounce, IteratorEffectMode.All, () => TimeSpan.FromMilliseconds(225), duration);
    }

    public static async Task Christmas(this IEnumerable<IEnumerable<EntertainmentLight>> group, CancellationToken cancellationToken)
    {
      bool startGreen = false;
      while (!cancellationToken.IsCancellationRequested)
      {
        await group.ChristmasInit(cancellationToken, startGreen).ConfigureAwait(false);
        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken).ConfigureAwait(false);
        startGreen = !startGreen;
      }
    }

    private static Task ChristmasInit(this IEnumerable<IEnumerable<EntertainmentLight>> group, CancellationToken cancellationToken, bool startGreen = false)
    {
      return group.IteratorEffect(cancellationToken, async(current, ct, timeSpan) =>
      {
        if (startGreen)
          current.SetState(ct, new Q42.HueApi.ColorConverters.RGBColor("00FF00"), 1, TimeSpan.FromSeconds(0));
        else
          current.SetState(ct, new Q42.HueApi.ColorConverters.RGBColor("FF0000"), 1, TimeSpan.FromSeconds(0));

        startGreen = !startGreen;
      }, IteratorEffectMode.Single, IteratorEffectMode.All, () => TimeSpan.FromSeconds(0));
    }
  }
}
