using Q42.HueApi.ColorConverters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Q42.HueApi.Streaming.Models
{
  public class StreamingState
  {
    public RGBColor RGBColor { get; private set; } = new RGBColor();

    /// <summary>
    /// Between 0 and 1
    /// </summary>
    public double Brightness { get; private set; } = 1;

    public bool IsDirty { get; set; } = true;

    public void SetRGBColor(RGBColor color)
    {
      this.RGBColor = color;
      IsDirty = true;
    }

    public void SetBrightnes(double brightness)
    {
      this.Brightness = brightness;
      IsDirty = true;
    }

    public byte[] ToByteArray()
    {
      IsDirty = false;

      byte red = (byte)(RGBColor.R * Brightness * 255.99);
      byte green = (byte)(RGBColor.G * Brightness * 255.99);
      byte blue = (byte)(RGBColor.B * Brightness * 255.99);

      List<byte> result = new List<byte>();
      result.Add(red);
      result.Add(red);
      result.Add(green);
      result.Add(green);
      result.Add(blue);
      result.Add(blue);

      return result.ToArray();
    }
  }
}
