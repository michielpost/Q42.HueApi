using Q42.HueApi.ColorConverters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Q42.HueApi.Streaming.Models
{
  /// <summary>
  /// Contains state info for a single light
  /// </summary>
  public class StreamingState : EntertainmentState
  {
    public byte[] ToByteArray()
    {
      IsDirty = false;

      byte red = (byte)(RGBColor.R * Brightness * 255);
      byte green = (byte)(RGBColor.G * Brightness * 255);
      byte blue = (byte)(RGBColor.B * Brightness * 255);

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
