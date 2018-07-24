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
    /// Between 0 and 1
    /// </summary>
    public double Brightness { get; private set; } = 0;

    public bool IsDirty { get; set; } = false;

    public void SetRGBColor(RGBColor color)
    {
      this.RGBColor = color;
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
