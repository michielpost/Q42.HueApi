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
    public static double? GetEffectStrengthMultiplier(this BaseEffect effect, EntertainmentLight light)
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
      return lightLocation.Distance(effect.X, effect.Y);
    }
  }
}
