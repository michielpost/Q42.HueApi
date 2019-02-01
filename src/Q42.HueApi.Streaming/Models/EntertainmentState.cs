using Q42.HueApi.ColorConverters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Q42.HueApi.Streaming.Models
{
  /// <summary>
  /// Contains state info for a single entertainment light in a layer
  /// </summary>
  public class EntertainmentState
  {
    public RGBColor RGBColor { get; private set; } = new RGBColor();

    /// <summary>
    /// Gets or sets the colors based on CIE 1931 Color coordinates.
    /// </summary>
    public double[] ColorCoordinates { get; private set; } = new double[2];

    /// <summary>
    /// Between 0 and 1
    /// </summary>
    public double Brightness { get; private set; } = 0;

    public bool IsDirty { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="color"></param>
    public void SetRGBColor(RGBColor color)
    {
      this.RGBColor = color;
      IsDirty = true;
    }

    public void SetXY(int x, int y)
    {
      if (x > 1)
        x = 1;
      if (y > 1)
        y = 1;

      if (x < 0)
        x = 0;
      if (y < 0)
        y = 0;

      ColorCoordinates[0] = x;
      ColorCoordinates[1] = y;
      IsDirty = true;
    }

    public void SetBrightness(double brightness)
    {
      brightness = brightness < 0 ? 0 : brightness;
      brightness = brightness > 1 ? 1 : brightness;

      this.Brightness = brightness;
      IsDirty = true;
    }

  }
}
