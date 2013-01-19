using System;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Q42.HueApi.Models
{

  /// <summary>
  /// Compose a lamp command to send to a lamp
  /// </summary>
  [DataContract]
  public class LampCommand
  {
    /// <summary>
    /// Gets or sets the colors based on CIE 1931 Color coordinates.
    /// </summary>
    [DataMember (Name = "xy")]
    public double[] ColorCoordinates { get; set; }

    /// <summary>
    /// Gets or sets the brightness 0-255.
    /// </summary>
    [DataMember (Name = "bri")]
    public byte? Brightness { get; set; }

    /// <summary>
    /// Gets or sets the hue for Hue and <see cref="Saturation"/> mode.
    /// </summary>
    [DataMember (Name = "hue")]
    public int? Hue { get; set; }

    /// <summary>
    /// Gets or sets the saturation for <see cref="Hue"/> and Saturation mode.
    /// </summary>
    [DataMember (Name = "sat")]
    public int? Saturation { get; set; }

    /// <summary>
    /// Gets or sets the Color Temperature
    /// </summary>
    [DataMember (Name = "ct")]
    public int? ColorTemperature { get; set; }

    /// <summary>
    /// Gets or sets whether the lamp is on.
    /// </summary>
    [DataMember (Name = "on")]
    public bool? On { get; set; }

    /// <summary>
    /// Gets or sets the current effect for the lamp.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [DataMember (Name = "effect")]
    public Effect Effect { get; set; }

    /// <summary>
    /// Gets or sets the current alert for the lamp.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [DataMember (Name = "alert")]
    public Alert Alert { get; set; }

    /// <summary>
    /// Gets or sets the transition time for the lamp.
    /// </summary>
    [DataMember (Name = "transitiontime")]
    [JsonConverter (typeof(TransitionTimeConverter))]
    public TimeSpan? TransitionTime { get; set; }

  }

  public enum Alert
  {
    /// <summary>
    /// Stop alert
    /// </summary>
    [EnumMember (Value = "none")]
    None,

    /// <summary>
    /// Alert once
    /// </summary>
    [EnumMember (Value = "select")]
    Once,

    /// <summary>
    /// Alert multiple times
    /// </summary>
    [EnumMember (Value = "lselect")]
    Multiple
  }

  public enum Effect
  {
    /// <summary>
    /// Stop current effect
    /// </summary>
    [EnumMember (Value = "none")]
    None,

    /// <summary>
    /// Color loop
    /// </summary>
    [EnumMember (Value = "colorloop")]
    ColorLoop
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

      lampCommand.ColorCoordinates = new[] { x, y };
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

      lampCommand.ColorTemperature = ct;
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

      lampCommand.On = true;
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

      lampCommand.On = false;
      return lampCommand;
    }

  }
}
