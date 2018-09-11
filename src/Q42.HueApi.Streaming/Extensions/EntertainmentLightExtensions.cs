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
  public static class EntertainmentLightExtensions
  {
    /// <summary>
    /// Brightness between 0 and 1
    /// </summary>
    /// <param name="light"></param>
    /// <param name="brightness">between 0 and 1</param>
    /// <param name="timeSpan"></param>
    /// <param name="cancellationToken"></param>
    public static void SetBrightness(this EntertainmentLight light, CancellationToken cancellationToken, double brightness, TimeSpan timeSpan = default(TimeSpan))
    {
      light.SetState(cancellationToken, null, brightness, timeSpan);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="light"></param>
    /// <param name="rgb"></param>
    /// <param name="timeSpan"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static void SetColor(this EntertainmentLight light, CancellationToken cancellationToken, RGBColor rgb, TimeSpan timeSpan = default(TimeSpan))
    {
      light.SetState(cancellationToken, rgb, null, timeSpan);
    }

    /// <summary>
    /// Set state on a single light
    /// </summary>
    /// <param name="light"></param>
    /// <param name="rgb"></param>
    /// <param name="brightness"></param>
    /// <param name="timeSpan"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static void SetState(this EntertainmentLight light, CancellationToken cancellationToken, RGBColor? rgb = null, double? brightness = null, TimeSpan timeSpan = default(TimeSpan))
    {
      //Create a new transition for this light
      Transition transition = new Transition(rgb, brightness, timeSpan);

      light.Transition = transition;

      //Start the transition
      transition.Start(light.State.RGBColor, light.State.Brightness, cancellationToken);
    }
  }
}
