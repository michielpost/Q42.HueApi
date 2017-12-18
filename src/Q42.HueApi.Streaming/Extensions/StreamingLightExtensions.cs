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
    private static int _updateFrequencyMs = 50; //Max update per ms

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
      //if (light.LightLocation.IsLeft)
      //  Console.WriteLine($"INPUT R: {light.State.RGBColor.R}, G: {light.State.RGBColor.G}, B: {light.State.RGBColor.B}");

      //Update should happen fast, so dont move between values but just set them
      if(timeSpan.TotalMilliseconds < (_updateFrequencyMs * 1.5))
      {
        SetFinalState(light, rgb, brightness);
        return Task.CompletedTask;
      }

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
          int red = (int)(light.State.RGBColor.R * 255);
          int green = (int)(light.State.RGBColor.G * 255);
          int blue = (int)(light.State.RGBColor.B * 255);

          int newred = (int)(rgb.Value.R * 255);
          int newgreen = (int)(rgb.Value.G * 255);
          int newblue = (int)(rgb.Value.B * 255);

          stepSizeR = (int)((newred - red) / updates);
          stepSizeG = (int)((newgreen - green) / updates);
          stepSizeB = (int)((newblue - blue) / updates);
        }

        if (brightness.HasValue)
        {
          stepSizeBri = (brightness - light.State.Brightness) / updates;
        }

        if (brightness.HasValue
        && rgb.HasValue
        && brightness.Value > 0.1
        && light.State.Brightness <= 0.1)
        {
          //If going from OFF to light, and a color transition, set the color direct, to prevent weird color flashes
          light.State.SetRGBColor(rgb.Value);

          //Set rgb to null to prevent any other updates
          rgb = null;
        }

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
            int red = (int)(light.State.RGBColor.R * 255);
            int green = (int)(light.State.RGBColor.G * 255);
            int blue = (int)(light.State.RGBColor.B * 255);

            light.State.SetRGBColor(new RGBColor(
             red + stepSizeR.Value,
             green + stepSizeG.Value,
             blue + stepSizeB.Value
             ));

            if (light.State.RGBColor.R < 0 || light.State.RGBColor.R > 1
            || light.State.RGBColor.G < 0 || light.State.RGBColor.G > 1
            || light.State.RGBColor.B < 0 || light.State.RGBColor.B > 1
            )
            {
              Debug.WriteLine("Invalid values!");
            }
          }

          //Only update brightness if there are changes
          if (brightness.HasValue && stepSizeBri.HasValue)
          {
            light.State.SetBrightnes(light.State.Brightness + stepSizeBri.Value);
            //Debug.WriteLine("Bri:" + light.State.Brightness);
          }

          await Task.Delay(_updateFrequencyMs).ConfigureAwait(false);

          if (sw.ElapsedMilliseconds > timeSpan.TotalMilliseconds)
            break;
        }

        sw.Stop();

        SetFinalState(light, rgb, brightness);

        //if (light.LightLocation.IsLeft)
        //  Console.WriteLine($"FINISHED R: {light.State.RGBColor.R}, G: {light.State.RGBColor.G}, B: {light.State.RGBColor.B}");

      }, cancellationToken);
    }

    private static void SetFinalState(StreamingLight light, RGBColor? rgb, double? brightness)
    {
      if (rgb.HasValue)
        light.State.SetRGBColor(rgb.Value);
      if (brightness.HasValue)
        light.State.SetBrightnes(brightness.Value);
    }
  }
}
