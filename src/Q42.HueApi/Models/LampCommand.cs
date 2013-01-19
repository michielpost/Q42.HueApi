using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{

  /// <summary>
  /// Compose a lamp command to send to a lamp
  /// </summary>
  public class LampCommand
  {
    public List<double> xy { get; set; }
    public int? bri { get; set; }
    public string hue { get; set; }
    public int? sat { get; set; }
    public int? ct { get; set; }
    public bool? on { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public Effects effect { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public Alerts alert { get; set; }

    /// <summary>
    /// In 1/10 of a second
    /// </summary>
    public int? transitiontime { get; set; }

  }

  public enum Alerts
  {
    /// <summary>
    /// Stop alert
    /// </summary>
    none,
    /// <summary>
    /// Alert once
    /// </summary>
    select,
    /// <summary>
    /// Alert multiple times
    /// </summary>
    lselect
  }

  public enum Effects
  {
    /// <summary>
    /// Stop current effect
    /// </summary>
    none,
    /// <summary>
    /// Colorloop
    /// </summary>
    colorloop
  }

  /// <summary>
  /// Extension methods to compose a lamp command
  /// </summary>
  public static class LampCommandExtensions
  {
    /// <summary>
    /// Helper to set the color based on a HEX value
    /// </summary>
    /// <param name="lampCommand"></param>
    /// <param name="hexColor"></param>
    /// <returns></returns>
    public static LampCommand SetColor(this LampCommand lampCommand, string hexColor)
    {
      if (lampCommand == null)
        throw new ArgumentNullException ("lampCommand");
      if (hexColor == null)
        throw new ArgumentNullException ("hexColor");

      int red = int.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier);
      int green = int.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier);
      int blue = int.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier);

      return lampCommand.SetColor(red, green, blue);
    }

    /// <summary>
    /// Helper to set the color based on RGB strings
    /// </summary>
    /// <param name="lampCommand"></param>
    /// <param name="red"></param>
    /// <param name="green"></param>
    /// <param name="blue"></param>
    /// <returns></returns>
    public static LampCommand SetColor(this LampCommand lampCommand, string red, string green, string blue)
    {
      if (lampCommand == null)
        throw new ArgumentNullException ("lampCommand");

      return lampCommand.SetColor(int.Parse(red), int.Parse(green), int.Parse(blue));
    }

    /// <summary>
    /// Helper to set the color based on RGB
    /// </summary>
    /// <param name="lampCommand"></param>
    /// <param name="red"></param>
    /// <param name="green"></param>
    /// <param name="blue"></param>
    /// <returns></returns>
    public static LampCommand SetColor(this LampCommand lampCommand, int red, int green, int blue)
    {
      if (lampCommand == null)
        throw new ArgumentNullException ("lampCommand");

      var point = HueColorConverter.XyFromColor(red, green, blue);
      return lampCommand.SetColor(point.x, point.y);
    }

    /// <summary>
    /// Helper to set the color based on the lamp's built in XY color schema
    /// </summary>
    /// <param name="lampCommand"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static LampCommand SetColor(this LampCommand lampCommand, double x, double y)
    {
      if (lampCommand == null)
        throw new ArgumentNullException ("lampCommand");

      lampCommand.xy = new List<double>();
      lampCommand.xy.Add(x);
      lampCommand.xy.Add(y);
      return lampCommand;
    }

    /// <summary>
    /// Helper to set the color based on the lamp's built in CT color scheme
    /// </summary>
    /// <param name="lampCommand"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static LampCommand SetColor(this LampCommand lampCommand, int ct)
    {
      if (lampCommand == null)
        throw new ArgumentNullException ("lampCommand");

      lampCommand.ct = ct;
      return lampCommand;
    }

    /// <summary>
    /// Helper to create turn on command
    /// </summary>
    /// <param name="lampCommand"></param>
    /// <returns></returns>
    public static LampCommand TurnOn(this LampCommand lampCommand)
    {
      if (lampCommand == null)
        throw new ArgumentNullException ("lampCommand");

      lampCommand.on = true;
      return lampCommand;
    }

    /// <summary>
    /// Helper to create turn off command
    /// </summary>
    /// <param name="lampCommand"></param>
    /// <returns></returns>
    public static LampCommand TurnOff(this LampCommand lampCommand)
    {
      if (lampCommand == null)
        throw new ArgumentNullException ("lampCommand");

      lampCommand.on = false;
      return lampCommand;
    }

  }
}
