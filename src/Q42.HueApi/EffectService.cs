using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
  /// <summary>
  /// Holds special effects for the lamps
  /// </summary>
  public class EffectService : IEffectService
  {
    public ILampService LampService { get; set; }

    public EffectService(ILampService lampService)
    {
      LampService = lampService;
    }


    /// <summary>
    /// Set the next Hue color
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    public Task SetNextHueColor(IEnumerable<string> lampList = null)
    {

      string command = "{\"hue\":+10000,\"sat\":255}";

      return LampService.SendCommand(command, lampList);

    }


    /// <summary>
    /// Starts a color loop
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    public Task Colorloop(IEnumerable<string> lampList = null)
    {
      LampCommand command = new LampCommand();
      command.effect = Effects.colorloop;

      return LampService.SendCommand(command, lampList);
    }

    /// <summary>
    /// Stops colorloop effect
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    public Task ColorloopStop(IEnumerable<string> lampList = null)
    {
      LampCommand command = new LampCommand();
      command.effect = Effects.none;

      return LampService.SendCommand(command, lampList);
    }

    /// <summary>
    /// Blink / alerts lamps once
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    public Task Alert(IEnumerable<string> lampList = null)
    {
      LampCommand command = new LampCommand();
      command.alert = Alerts.select;

      return LampService.SendCommand(command, lampList);
    }

    /// <summary>
    /// Blinks multiple times (for 10 seconds)
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    public Task AlertMultiple(IEnumerable<string> lampList = null)
    {
      LampCommand command = new LampCommand();
      command.alert = Alerts.lselect;

      return LampService.SendCommand(command, lampList);
    }

    /// <summary>
    /// Stops blinking
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    public Task AlertStop(IEnumerable<string> lampList = null)
    {
      LampCommand command = new LampCommand();
      command.alert = Alerts.none;

      return LampService.SendCommand(command, lampList);
    }

   
  }
}
