using HueApi.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi.Extensions
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
    public static UpdateLight SetColor(this UpdateLight lightCommand, double x, double y)
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      lightCommand.Color = new Models.Color()
      {
        Xy = new Models.XyPosition()
        {
          X = x,
          Y = y
        }
      };

      return lightCommand;
    }

    /// <summary>
    /// Helper to set the color based on the light's built in CT color scheme
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static UpdateLight SetColor(this UpdateLight lightCommand, int ct)
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      lightCommand.ColorTemperature = new Models.ColorTemperature() {  Mirek = ct};
      return lightCommand;
    }

    /// <summary>
    /// Helper to create turn on command
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <returns></returns>
    public static UpdateLight TurnOn(this UpdateLight lightCommand)
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      lightCommand.On = new Models.On() { IsOn = true };
      return lightCommand;
    }

    /// <summary>
    /// Helper to create turn off command
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <returns></returns>
    public static UpdateLight TurnOff(this UpdateLight lightCommand)
    {
      if (lightCommand == null)
        throw new ArgumentNullException(nameof(lightCommand));

      lightCommand.On = new Models.On() { IsOn = false };
      return lightCommand;
    }

  }
}
