using Q42.HueApi.ColorConverters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Q42.HueApi.Streaming.Models
{
  public class StreamingState
  {
    public RGBColor RGBColor { get; set; } = new RGBColor();

    /// <summary>
    /// Between 0 and 1
    /// </summary>
    public double Brightness { get; set; } = 1;

    public byte[] ToByteArray()
    {
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
