using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.ColorConverters.Gamut;
using Q42.HueApi.Models.Gamut;

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
    public static void SetBrightness(this EntertainmentLight light, CancellationToken cancellationToken, double brightness, TimeSpan timeSpan = default)
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
    public static void SetColor(this EntertainmentLight light, CancellationToken cancellationToken, RGBColor rgb, TimeSpan timeSpan = default)
    {
      light.SetState(cancellationToken, rgb, null, timeSpan);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="light"></param>
    /// <param name="xy"></param>
    /// <param name="gamut">The gamut to use</param>
    /// <param name="timeSpan"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static void SetColor(this EntertainmentLight light, CancellationToken cancellationToken, CIE1931Point xy, CIE1931Gamut gamut, TimeSpan timeSpan = default) {
      var rgb = HueColorConverter.XYToRgb(xy, gamut);
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

    /// <summary>
    /// Set state on a single light
    /// </summary>
    /// <param name="light"></param>
    /// <param name="xy"></param>
    /// <param name="gamut"></param>
    /// <param name="brightness"></param>
    /// <param name="timeSpan"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static void SetState(this EntertainmentLight light, CancellationToken cancellationToken, CIE1931Point xy, CIE1931Gamut gamut, double? brightness = null, TimeSpan timeSpan = default(TimeSpan))
    {
      var rgb = HueColorConverter.XYToRgb(xy, gamut);

      //Create a new transition for this light
      Transition transition = new Transition(rgb, brightness, timeSpan);

      light.Transition = transition;

      //Start the transition
      transition.Start(light.State.RGBColor, light.State.Brightness, cancellationToken);
    }

    //Source: https://github.com/Q42/Q42.HueApi/pull/174
    public static void SetState(this EntertainmentLight light, CancellationToken cancellationToken, RGBColor? rgb = null, TimeSpan rgbTimeSpan = default, double? brightness = null, TimeSpan briTimeSpan = default, bool overwriteExistingTransition = true)
    {
      if (light == null) throw new ArgumentNullException(nameof(light));

      // No change in state required.
      if (!rgb.HasValue && !brightness.HasValue) return;

      var currentTransition = light.Transition;
      bool hasExistingTransition = currentTransition != null && !currentTransition.IsFinished;
      bool canCombineTransitions = hasExistingTransition &&
                                   (!rgb.HasValue || !brightness.HasValue) &&
                                   (!rgb.HasValue || !currentTransition!.IsBrightnessFinished) &&
                                   (!brightness.HasValue || !currentTransition!.IsRgbFinished);

      Transition transition;

      // If we can't combine the existing transition (if there is one) with the new transition, start a fresh transition.
      if (!canCombineTransitions || overwriteExistingTransition)
      {
        if (rgb.HasValue && brightness.HasValue)
        {
          transition = new Transition(rgb.Value, brightness.Value, rgbTimeSpan, briTimeSpan);
        }
        else if (rgb.HasValue)
        {
          transition = new Transition(rgb.Value, rgbTimeSpan);
        }
        else
        {
          transition = new Transition(brightness.Value, briTimeSpan);
        }
      }
      else
      {
        // A transition is currently in progress, which we need to combine with the colour or brightness that was specified.
        if (rgb.HasValue)
        {
          // Combine new colour transition with existing brightness transition.
          transition = new Transition(rgb.Value, currentTransition!.TargetBri.Value, rgbTimeSpan, currentTransition.BrightnessRemainingTime);
        }
        else
        {
          // Combine new brightness transition with existing colour transition.
          transition = new Transition(currentTransition!.TargetRgb.Value, brightness.Value, currentTransition.RgbRemainingTime, briTimeSpan);
        }
      }

      light.Transition = transition;
      transition.Start(light.State.RGBColor, light.State.Brightness, cancellationToken);
    }
  }
}
