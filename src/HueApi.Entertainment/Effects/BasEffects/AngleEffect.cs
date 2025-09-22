using HueApi.Entertainment.Models;
using HueApi.Models;

namespace HueApi.Entertainment.Effects.BasEffects
{
  public abstract class AngleEffect : BaseEffect
  {
    /// <summary>
    /// Between -1 and 1
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Between -1 and 1
    /// </summary>
    public double Y { get; set; }

    public double CurrentAngle { get; set; }

    /// <summary>
    /// Between 0 and 1
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// Get the multiplier for an effect on a light
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="light"></param>
    /// <returns></returns>
    public override double? GetEffectStrengthMultiplier(EntertainmentLight light)
    {
      var angle = Angle(light.LightLocation);

      var dif1 = Math.Abs(CurrentAngle - angle);
      var dif2 = Math.Abs(CurrentAngle - angle - 360);

      var diff = Math.Min(dif1, dif2) / 60;

      double? multiplier = Width - diff;

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
    public double Angle(HuePosition lightLocation)
    {
      return lightLocation.Angle(X, Y);
    }

    public Task Rotate(CancellationToken cancellationToken, Func<int>? stepSize = null, Func<TimeSpan>? waitTime = null)
    {
      if (stepSize == null)
        stepSize = () => 20;

      if (waitTime == null)
        waitTime = () => TimeSpan.FromMilliseconds(50);

      return Task.Run(async () =>
      {
        while (!cancellationToken.IsCancellationRequested)
        {
          CurrentAngle += stepSize();
          await Task.Delay(waitTime(), cancellationToken).ConfigureAwait(false);
          if (CurrentAngle >= 360)
            CurrentAngle = 0;
          if (CurrentAngle < 0)
            CurrentAngle = 360;
        }
      }, cancellationToken);
    }
  }
}
