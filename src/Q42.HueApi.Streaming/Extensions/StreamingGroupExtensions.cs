using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Extensions
{
  public enum IteratorEffectMode
  {
    Cycle,
    Bounce,
    Single,
    Random
  }

  public delegate void IteratorEffectFunc(StreamingLight current, StreamingLight previous, TimeSpan? timeSpan = null);

  public static class StreamingGroupExtensions
  {
    public static IEnumerable<StreamingLight> GetLeft(this IEnumerable<StreamingLight> group)
    {
      return group.Where(x => x.LightLocation.IsLeft);
    }

    public static IEnumerable<StreamingLight> GetRight(this IEnumerable<StreamingLight> group)
    {
      return group.Where(x => x.LightLocation.IsRight);
    }

    public static IEnumerable<StreamingLight> GetFront(this IEnumerable<StreamingLight> group)
    {
      return group.Where(x => x.LightLocation.IsFront);
    }

    public static IEnumerable<StreamingLight> GetBack(this IEnumerable<StreamingLight> group)
    {
      return group.Where(x => x.LightLocation.IsBack);
    }

    public static IEnumerable<StreamingLight> GetCenter(this IEnumerable<StreamingLight> group)
    {
      return group.Where(x => x.LightLocation.IsCenter);
    }

    public static async Task IteratorEffect(this IEnumerable<StreamingLight> group, IteratorEffectFunc effectFunction, IteratorEffectMode mode, TimeSpan? timeSpan, CancellationToken cancellationToken = new CancellationToken())
    {
      if (timeSpan == null)
        timeSpan = TimeSpan.FromSeconds(1);

      bool keepGoing = true;
      var lights = group.ToList();
      bool reverse = false;

      while (keepGoing && !cancellationToken.IsCancellationRequested)
      {
        if (reverse)
          lights.Reverse();
        if (mode == IteratorEffectMode.Random)
          lights = lights.OrderBy(x => Guid.NewGuid()).ToList();

        for (int i = 0; i < lights.Count; i++)
        {
          //Default for Single and Cycle
          int prevIndex = i == 0 ? lights.Count - 1 : i - 1;
          if (mode == IteratorEffectMode.Bounce)
          {
            if (i == 0)
              prevIndex = lights.Count > 1 ? 1 : 0;

            //Always skip last light in bounce mode, it will be the first light in reverse
            if (i + 1 == lights.Count)
              continue;
          }

          Debug.WriteLine($"{i} and {prevIndex}");
          effectFunction(lights[i], lights[prevIndex], timeSpan);
          await Task.Delay(timeSpan.Value);
        }

        keepGoing = mode == IteratorEffectMode.Single ? false : true;
        if (mode == IteratorEffectMode.Bounce)
          reverse = true;
      }
    }


    /// <summary>
    /// Brightness between 0 and 1
    /// </summary>
    /// <param name="group"></param>
    /// <param name="brightness">Between 0 and 1</param>
    /// <param name="timeSpan"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static IEnumerable<StreamingLight> SetBrightness(this IEnumerable<StreamingLight> group,
      double brightness, TimeSpan timeSpan = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
    {
      group.SetState(null, brightness, timeSpan, cancellationToken);

      return group;
    }

    /// <summary>
    /// Transition to new RGB Color
    /// </summary>
    /// <param name="group"></param>
    /// <param name="rgb"></param>
    /// <param name="timeSpan"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static IEnumerable<StreamingLight> SetColor(this IEnumerable<StreamingLight> group,
      RGBColor rgb, TimeSpan timeSpan = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
    {

      group.SetState(rgb, null, timeSpan, cancellationToken);

      //var p = Parallel.ForEach(group, (light) => light.SetColor(rgb, timeSpan, cancellationToken));

      return group;
    }

    public static IEnumerable<StreamingLight> SetState(this IEnumerable<StreamingLight> group,
      RGBColor? rgb = null, double? brightness = null, TimeSpan timeSpan = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
    {
      foreach (var light in group)
      {
        light.SetState(rgb, brightness, timeSpan, cancellationToken);
      }

      return group;
    }
  }
}
