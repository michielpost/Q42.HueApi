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
    public byte[] ToByteArray(StreamingColorMode colorMode)
    {
      IsDirty = false;
      List<byte> result = new List<byte>();

      if (colorMode == StreamingColorMode.RGB)
      {

        byte red = (byte)(RGBColor.R * Brightness * 255);
        byte green = (byte)(RGBColor.G * Brightness * 255);
        byte blue = (byte)(RGBColor.B * Brightness * 255);

        result.Add(red);
        result.Add(red);
        result.Add(green);
        result.Add(green);
        result.Add(blue);
        result.Add(blue);

      }
      else if(colorMode == StreamingColorMode.XY)
      {
        double maxValue = 65535;
        var x = BitConverter.GetBytes(this.ColorCoordinates[0] * maxValue‭‬);
        var y= BitConverter.GetBytes(this.ColorCoordinates[1] * maxValue‭‬);
        var bri = BitConverter.GetBytes(this.Brightness * maxValue‭‬);

        result.AddRange(x);
        result.AddRange(y);
        result.AddRange(bri);

      }

      return result.ToArray();

    }
  }
}
