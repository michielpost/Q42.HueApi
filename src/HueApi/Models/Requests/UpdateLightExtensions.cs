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

  }
}
