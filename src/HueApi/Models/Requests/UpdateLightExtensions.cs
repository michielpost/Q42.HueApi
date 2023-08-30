using HueApi.Models.Requests.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi.Models.Requests
{
  /// <summary>
  /// Extension methods to compose a light command
  /// </summary>
  public static class UpdateLightExtensions
  {
    /// <summary>
    /// Helper to set the color based on the light's built in XY color schema
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static T SetColor<T>(this T lightCommand, double x, double y) where T : IUpdateColor
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      if (lightCommand.Color == null)
        lightCommand.Color = new Color();

      lightCommand.Color.Xy = new XyPosition()
      {
        X = x,
        Y = y
      };

      return lightCommand;
    }

    /// <summary>
    /// Helper to set the color based on the light's built in CT color scheme
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static T SetColor<T>(this T lightCommand, int ct) where T : IUpdateColorTemperature
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      lightCommand.ColorTemperature = new ColorTemperature() { Mirek = ct };
      return lightCommand;
    }

    /// <summary>
    /// Helper to create turn on command
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <returns></returns>
    public static T TurnOn<T>(this T lightCommand) where T : IUpdateOn
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      lightCommand.On = new On() { IsOn = true };
      return lightCommand;
    }

    /// <summary>
    /// Helper to create turn off command
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <returns></returns>
    public static T TurnOff<T>(this T lightCommand) where T : IUpdateOn
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      lightCommand.On = new On() { IsOn = false };
      return lightCommand;
    }

    /// <summary>
    /// Helper to create DimmingDelta command
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <param name="action"></param>
    /// <param name="brightnessDelta"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static T SetBrightnessDelta<T>(this T lightCommand, DeltaAction action, double brightnessDelta) where T : IUpdateDimmingDelta
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      if (!(1 <= brightnessDelta && brightnessDelta <= 100))
        throw new ArgumentOutOfRangeException(nameof(brightnessDelta), "Value must be between 1 and 100");

      lightCommand.DimmingDelta = new DimmingDelta
      {
        Action = action,
        BrightnessDelta = brightnessDelta
      };
      return lightCommand;
    }

    /// <summary>
    /// Helper to create Dimming command
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lightCommand"></param>
    /// <param name="brightness"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static T SetBrightness<T>(this T lightCommand, double brightness) where T : IUpdateDimming
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      if (!(0 <= brightness && brightness <= 100))
        throw new ArgumentOutOfRangeException(nameof(brightness), "Value must be between 0 and 100");

      lightCommand.Dimming = new Dimming
      {
        Brightness = brightness
      };
      return lightCommand;
    }

    public static T SetDuration<T>(this T lightCommand, TimeSpan duration) where T : IUpdateDynamics
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      lightCommand.Dynamics = new Dynamics
      {
        Duration = (int)duration.TotalMilliseconds
      };
      return lightCommand;
    }

    public static T SetDuration<T>(this T lightCommand, int durationMs) where T : IUpdateDynamics
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      lightCommand.Dynamics = new Dynamics
      {
        Duration = durationMs
      };
      return lightCommand;
    }

    public static T SetSpeed<T>(this T lightCommand, double speed) where T : IUpdateDynamics
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      if (!(0 <= speed && speed <= 1))
        throw new ArgumentOutOfRangeException(nameof(speed), "Value must be between 0 and 1");

      lightCommand.Dynamics = new Dynamics
      {
        Speed = speed
      };
      return lightCommand;
    }
  }
}
