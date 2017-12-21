using Q42.HueApi.Models.Groups;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Q42.HueApi.Streaming.Extensions
{
    public static class BaseEffectExtensions
    {
    public static double? GetEffectStrengthMultiplier(this BaseEffect effect, StreamingLight light)
    {
      var distance = Distance(effect, light.LightLocation);
      double? multiplier = effect.Radius - distance;

      multiplier = multiplier > 1 ? 1 : multiplier;
      multiplier = multiplier < 0 ? null : multiplier;

      return multiplier;
    }

    public static double Distance(this BaseEffect effect, LightLocation l2)
    {
      var x1 = effect.X;
      var y1 = effect.Y;
      var x2 = l2.X;
      var y2 = l2.Y;

      return Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
    }
  }
}
