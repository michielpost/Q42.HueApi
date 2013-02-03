using System;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Q42.HueApi
{

  /// <summary>
  /// Compose a light command to send to a light
  /// </summary>
  [DataContract]
  public class LightCommand
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
    /// Gets or sets whether the light is on.
    /// </summary>
    [DataMember (Name = "on")]
    public bool? On { get; set; }

    /// <summary>
    /// Gets or sets the current effect for the light.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [DataMember (Name = "effect")]
    public Effect Effect { get; set; }

    /// <summary>
    /// Gets or sets the current alert for the light.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [DataMember (Name = "alert")]
    public Alert Alert { get; set; }

    /// <summary>
    /// Gets or sets the transition time for the light.
    /// </summary>
    [DataMember (Name = "transitiontime")]
    [JsonConverter (typeof(TransitionTimeConverter))]
    public TimeSpan? TransitionTime { get; set; }

  }

  /// <summary>
  /// Possible light alerts
  /// </summary>
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

  /// <summary>
  /// Possible light effects
  /// </summary>
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
  /// Extension methods to compose a light command
  /// </summary>
  public static class lightCommandExtensions
  {
    /// <summary>
    /// Helper to set the color based on a HEX value
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <param name="hexColor"></param>
    /// <returns></returns>
    public static LightCommand SetColor(this LightCommand lightCommand, string hexColor)
    {
      if (lightCommand == null)
        throw new ArgumentNullException ("lightCommand");
      if (hexColor == null)
        throw new ArgumentNullException ("hexColor");

      int red = int.Parse(hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier);
      int green = int.Parse(hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier);
      int blue = int.Parse(hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier);

      return lightCommand.SetColor(red, green, blue);
    }

    /// <summary>
    /// Helper to set the color based on RGB strings
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <param name="red"></param>
    /// <param name="green"></param>
    /// <param name="blue"></param>
    /// <returns></returns>
    public static LightCommand SetColor(this LightCommand lightCommand, string red, string green, string blue)
    {
      if (lightCommand == null)
        throw new ArgumentNullException ("lightCommand");

      return lightCommand.SetColor(int.Parse(red), int.Parse(green), int.Parse(blue));
    }

    /// <summary>
    /// Helper to set the color based on RGB
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <param name="red"></param>
    /// <param name="green"></param>
    /// <param name="blue"></param>
    /// <returns></returns>
    public static LightCommand SetColor(this LightCommand lightCommand, int red, int green, int blue)
    {
      if (lightCommand == null)
        throw new ArgumentNullException ("lightCommand");

      var point = HueColorConverter.XyFromColor(red, green, blue);
      return lightCommand.SetColor(point.x, point.y);
    }

    /// <summary>
    /// Helper to set the color based on the light's built in XY color schema
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static LightCommand SetColor(this LightCommand lightCommand, double x, double y)
    {
      if (lightCommand == null)
        throw new ArgumentNullException ("lightCommand");

      lightCommand.ColorCoordinates = new[] { x, y };
      return lightCommand;
    }

    /// <summary>
    /// Helper to set the color based on the light's built in CT color scheme
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static LightCommand SetColor(this LightCommand lightCommand, int ct)
    {
      if (lightCommand == null)
        throw new ArgumentNullException ("lightCommand");

      lightCommand.ColorTemperature = ct;
      return lightCommand;
    }

    /// <summary>
    /// Helper to create turn on command
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <returns></returns>
    public static LightCommand TurnOn(this LightCommand lightCommand)
    {
      if (lightCommand == null)
        throw new ArgumentNullException ("lightCommand");

      lightCommand.On = true;
      return lightCommand;
    }

    /// <summary>
    /// Helper to create turn off command
    /// </summary>
    /// <param name="lightCommand"></param>
    /// <returns></returns>
    public static LightCommand TurnOff(this LightCommand lightCommand)
    {
      if (lightCommand == null)
        throw new ArgumentNullException ("lightCommand");

      lightCommand.On = false;
      return lightCommand;
    }

  }
}
