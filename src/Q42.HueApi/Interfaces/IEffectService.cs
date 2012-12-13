using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Interfaces
{
  public interface IEffectService
  {
    /// <summary>
    /// Set the next Hue color
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    Task SetNextHueColor(IEnumerable<string> lampList = null);

    /// <summary>
    /// Star the colorloop effect
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    Task Colorloop(IEnumerable<string> lampList = null);

    /// <summary>
    /// Stop the colorloop effect
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    Task ColorloopStop(IEnumerable<string> lampList = null);

    /// <summary>
    /// Send single alert
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    Task Alert(IEnumerable<string> lampList = null);

    /// <summary>
    /// Blinks multiple times (for 10 seconds)
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    Task AlertMultiple(IEnumerable<string> lampList = null);

    /// <summary>
    /// Stop blinking
    /// </summary>
    /// <param name="lampList"></param>
    /// <returns></returns>
    Task AlertStop(IEnumerable<string> lampList = null);



  }
}
