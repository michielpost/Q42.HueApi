using Q42.HueApi.Models.Groups;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Q42.HueApi.Streaming.Extensions
{
  /// <summary>
  /// Extension methods for BaseEffect
  /// </summary>
  public static class BaseEffectExtensions
  {
    /// <summary>
    /// Get the multiplier for an effect on a light
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="light"></param>
    /// <returns></returns>
    public static double? GetEffectStrengthMultiplier(this BaseEffect effect, StreamingLight light)
    {
      var distance = Distance(effect, light.LightLocation);
      double? multiplier = effect.Radius - distance;

      multiplier = multiplier > 1 ? 1 : multiplier;
      multiplier = multiplier < 0 ? null : multiplier;

      return multiplier;
    }

    /// <summary>
    /// Calculate the distance between the effect and a light location
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="lightLocation"></param>
    /// <returns></returns>
    public static double Distance(this BaseEffect effect, LightLocation lightLocation)
    {
      var x1 = effect.X;
      var y1 = effect.Y;
      var x2 = lightLocation.X;
      var y2 = lightLocation.Y;

      return Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
    }
  }
}
