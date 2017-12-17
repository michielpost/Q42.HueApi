using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Extensions
{
  public static class StreamingLightExtensions
  {
    private static int _updateFrequencyMs = 50; //Max 50ms update frequency

    /// <summary>
    /// Brightness between 0 and 1
    /// </summary>
    /// <param name="light"></param>
    /// <param name="brightness">between 0 and 1</param>
    /// <param name="timeSpan"></param>
    /// <param name="cancellationToken"></param>
    public static Task SetBrightness(this StreamingLight light, double brightness, TimeSpan timeSpan = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
    {
      return light.SetState(null, brightness, timeSpan, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="light"></param>
    /// <param name="rgb"></param>
    /// <param name="timeSpan"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task SetColor(this StreamingLight light, RGBColor rgb, TimeSpan timeSpan = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
    {
      return light.SetState(rgb, null, timeSpan, cancellationToken);
    }

    public static Task SetState(this StreamingLight light, RGBColor? rgb = null, double? brightness = null, TimeSpan timeSpan = default(TimeSpan), CancellationToken cancellationToken = default(CancellationToken))
    {
      if (light.LightLocation.IsLeft)
        Console.WriteLine($"INPUT R: {light.State.RGBColor.R}, G: {light.State.RGBColor.G}, B: {light.State.RGBColor.B}");

      return Task.Run(async () =>
      {
        //if (light.LightLocation.IsLeft)
        //  Console.WriteLine($"START R: {light.State.RGBColor.R}, G: {light.State.RGBColor.G}, B: {light.State.RGBColor.B}");

        var updates = (timeSpan.TotalMilliseconds / _updateFrequencyMs) * 0.75;
        int? stepSizeR = null;
        int? stepSizeG = null;
        int? stepSizeB = null;
        double? stepSizeBri = null;

        if (rgb.HasValue)
        {
          int red = (int)(light.State.RGBColor.R * 255.99);
          int green = (int)(light.State.RGBColor.G * 255.99);
          int blue = (int)(light.State.RGBColor.B * 255.99);

          int newred = (int)(rgb.Value.R * 255.99);
          int newgreen = (int)(rgb.Value.G * 255.99);
          int newblue = (int)(rgb.Value.B * 255.99);

          stepSizeR = (int)((newred - red) / updates);
          stepSizeG = (int)((newgreen - green) / updates);
          stepSizeB = (int)((newblue - blue) / updates);
        }

        if (brightness.HasValue)
        {
          stepSizeBri = (brightness - light.State.Brightness) / updates;
        }

        //var stepSizeR = (rgb.R - light.State.RGBColor.R) / updates;
        //var stepSizeG = (rgb.G - light.State.RGBColor.G) / updates;
        //var stepSizeB = (rgb.B - light.State.RGBColor.B) / updates;
        //Console.WriteLine($"Updates: {updates}");

        Stopwatch sw = new Stopwatch();
        sw.Start();
        for (int i = 0; i < updates; i++)
        {
          if (cancellationToken.IsCancellationRequested)
            return;



          //Only update color if there are changes
          if (rgb.HasValue
          && stepSizeR.HasValue && stepSizeG.HasValue && stepSizeB.HasValue)
          {
            int red = (int)(light.State.RGBColor.R * 255.99);
            int green = (int)(light.State.RGBColor.G * 255.99);
            int blue = (int)(light.State.RGBColor.B * 255.99);

            light.State.RGBColor = new RGBColor(
             red + stepSizeR.Value,
             green + stepSizeG.Value,
             blue + stepSizeB.Value
             );
          }

          //Only update brightness if there are changes
          if (brightness.HasValue && stepSizeBri.HasValue)
            light.State.Brightness += stepSizeBri.Value;

          //if (light.LightLocation.IsLeft)
          //  Console.WriteLine($"{i} + R: {light.State.RGBColor.R}, G: {light.State.RGBColor.G}, B: {light.State.RGBColor.B}");

          await Task.Delay(_updateFrequencyMs).ConfigureAwait(false);

          if (sw.ElapsedMilliseconds > timeSpan.TotalMilliseconds)
            break;

          //var updatesLeft = (timeSpan.TotalMilliseconds - sw.ElapsedMilliseconds) / _updateFrequencyMs;

          //if (light.LightLocation.IsLeft)
          //  Console.WriteLine("Up: " + updates + "  StepR: " + stepSizeR);

          //  updates = i + updatesLeft;

          //stepSizeR = (int)((newred - red) / updatesLeft);
          //stepSizeG = (int)((newgreen - green) / updatesLeft);
          //stepSizeB = (int)((newblue - blue) / updatesLeft);

          //if (light.LightLocation.IsLeft)
          //  Console.WriteLine("NewUp: " + updates + "  StepR: " + stepSizeR);

        }

        sw.Stop();

        if (rgb.HasValue)
          light.State.RGBColor = rgb.Value;
        if (brightness.HasValue)
          light.State.Brightness = brightness.Value;

        //if (light.LightLocation.IsLeft)
        //  Console.WriteLine($"FINISHED R: {light.State.RGBColor.R}, G: {light.State.RGBColor.G}, B: {light.State.RGBColor.B}");

      }, cancellationToken);
    }
  }
}
