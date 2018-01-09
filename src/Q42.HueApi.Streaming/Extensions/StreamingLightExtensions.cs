using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Extensions
{
  public static class StreamingLightExtensions
  {
    /// <summary>
    /// Brightness between 0 and 1
    /// </summary>
    /// <param name="light"></param>
    /// <param name="brightness">between 0 and 1</param>
    /// <param name="timeSpan"></param>
    /// <param name="cancellationToken"></param>
    public static Task SetBrightness(this StreamingLight light, double brightness, TimeSpan timeSpan = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
    {
      return light.SetState(null, brightness, timeSpan, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="light"></param>
    /// <param name="rgb"></param>
    /// <param name="timeSpan"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task SetColor(this StreamingLight light, RGBColor rgb, TimeSpan timeSpan = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
    {
      return light.SetState(rgb, null, timeSpan, cancellationToken);
    }

    public static Task SetState(this StreamingLight light, RGBColor? rgb = null, double? brightness = null, TimeSpan timeSpan = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
    {
      //Create a new transition for this light
      Transition transition = CreateTransition(rgb, brightness, timeSpan);

      light.Transitions.Add(transition);

      //Start the transition
      return transition.Start(light.State.RGBColor, light.State.Brightness, cancellationToken);
    }

    public static Transition CreateTransition(RGBColor? rgb, double? brightness, TimeSpan timeSpan)
    {
      Transition transition = new Transition();
      transition.TargetRgb = rgb;
      transition.TargetBri = brightness;
      transition.TimeSpan = timeSpan;
      return transition;
    }
  }
}
