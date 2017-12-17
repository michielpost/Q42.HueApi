using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Extensions
{
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
      foreach(var light in group)
      {
        light.SetBrightness(brightness, timeSpan, cancellationToken);
      }

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

      foreach (var light in group)
      {
        light.SetColor(rgb, timeSpan, cancellationToken);
      }

      //var p = Parallel.ForEach(group, (light) => light.SetColor(rgb, timeSpan, cancellationToken));
      
      return group;
    }
  }
}
