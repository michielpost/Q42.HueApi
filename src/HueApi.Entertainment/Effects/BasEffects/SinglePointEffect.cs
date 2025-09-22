using HueApi.Entertainment.Models;
using HueApi.Models;

namespace HueApi.Entertainment.Effects.BasEffects
{
  public abstract class SinglePointEffect : BaseEffect
  {

    /// <summary>
    /// Between -1 and 1
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Between -1 and 1
    /// </summary>
    public double Y { get; set; }

    /// <summary>
    /// Between -1 and 1
    /// </summary>
    public double Z { get; set; }

    /// <summary>
    /// Between 0 and 1
    /// </summary>
    public double Radius { get; set; }

    /// <summary>
    /// Get the multiplier for an effect on a light
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="light"></param>
    /// <returns></returns>
    public override double? GetEffectStrengthMultiplier(EntertainmentLight light)
    {
      var distance = Distance(light.LightLocation);
      double? multiplier = Radius - distance;

      multiplier = multiplier > 1 ? 1 : multiplier;
      multiplier = multiplier < 0 ? 0 : multiplier;

      return multiplier;
    }

    /// <summary>
    /// Calculate the distance between the effect and a light location
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="lightLocation"></param>
    /// <returns></returns>
    public double Distance(HuePosition lightLocation)
    {
      return lightLocation.Distance(X, Y, Z);
    }
  }
}
