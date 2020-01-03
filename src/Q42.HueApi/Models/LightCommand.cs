using System;
using System.Globalization;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Converters;

namespace Q42.HueApi
{

  /// <summary>
  /// Compose a light command to send to a light
  /// </summary>
  [DataContract]
  public class LightCommand : ICommandBody
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
    public Effect? Effect { get; set; }

    /// <summary>
    /// Gets or sets the current alert for the light.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    [DataMember (Name = "alert")]
    public Alert? Alert { get; set; }

    /// <summary>
    /// Gets or sets the transition time for the light.
    /// </summary>
    [DataMember (Name = "transitiontime")]
    [JsonConverter (typeof(TransitionTimeConverter))]
    public TimeSpan? TransitionTime { get; set; }

    /// <summary>
    /// -254 to 254
    /// As of 1.7. Increments or decrements the value of the brightness.  bri_inc is ignored if the bri attribute is provided. Any ongoing bri transition is stopped. Setting a value of 0 also stops any ongoing transition. The bridge will return the bri value after the increment is performed.
    /// </summary>
    [DataMember(Name = "bri_inc")]
    public int? BrightnessIncrement { get; set; }

    /// <summary>
    /// -254 to 254
    /// As of 1.7. Increments or decrements the value of the sat.  sat_inc is ignored if the sat attribute is provided. Any ongoing sat transition is stopped. Setting a value of 0 also stops any ongoing transition. The bridge will return the sat value after the increment is performed.
    /// </summary>
    [DataMember(Name = "sat_inc")]
    public int? SaturationIncrement { get; set; }

    /// <summary>
    /// -65534 to 65534
    /// As of 1.7. Increments or decrements the value of the hue.   hue_inc is ignored if the hue attribute is provided. Any ongoing color transition is stopped. Setting a value of 0 also stops any ongoing transition. The bridge will return the hue value after the increment is performed.
    /// </summary>
    [DataMember(Name = "hue_inc")]
    public int? HueIncrement { get; set; }

    /// <summary>
    /// -65534 to 65534
    /// As of 1.7. Increments or decrements the value of the ct. ct_inc is ignored if the ct attribute is provided. Any ongoing color transition is stopped. Setting a value of 0 also stops any ongoing transition. The bridge will return the ct value after the increment is performed.
    /// </summary>
    [DataMember(Name = "ct_inc")]
    public int? ColorTemperatureIncrement { get; set; }

    /// <summary>
    /// -0.5 to 0.5
    /// As of 1.7. Increments or decrements the value of the xy.  xy_inc is ignored if the xy attribute is provided. Any ongoing color transition is stopped.  Will stop at it's gamut boundaries. Setting a value of 0 also stops any ongoing transition.  The bridge will return the xy value after the increment is performed.
    /// </summary>
    [DataMember(Name = "xy_inc")]
    public double[]? ColorCoordinatesIncrement { get; set; }

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
	/// Helper to set the color based on the light's built in XY color schema
	/// </summary>
	/// <param name="lightCommand"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static LightCommand SetColor(this LightCommand lightCommand, double x, double y)
    {
      if (lightCommand == null)
        throw new ArgumentNullException (nameof(lightCommand));

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
        throw new ArgumentNullException (nameof(lightCommand));

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
        throw new ArgumentNullException (nameof(lightCommand));

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
        throw new ArgumentNullException (nameof(lightCommand));

      lightCommand.On = false;
      return lightCommand;
    }

  }
}
