using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.ColorConverters;

namespace Q42.HueApi.Streaming.Models
{
  /// <summary>
  /// Responsible for calculating all states between current state and target state
  /// </summary>
  public class Transition
  {
    private static int _updateFrequencyMs = 50; //Max update per ms

    public RGBColor? TargetRgb { get; internal set; }
    public double? TargetBri { get; internal set; }
    public TimeSpan TimeSpan { get; internal set; }

    public bool IsFinished { get; set; }

    /// <summary>
    /// Current state of the transition
    /// </summary>
    public StreamingState TransitionState { get; private set; } = new StreamingState();


    /// <summary>
    /// Start the transition
    /// </summary>
    /// <param name="startRgb"></param>
    /// <param name="startBrightness"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task Start(RGBColor startRgb, double startBrightness, CancellationToken cancellationToken)
    {
      //Update should happen fast, so dont move between values but just set them
        if (TimeSpan.TotalMilliseconds < (_updateFrequencyMs * 1.5))
      {
        SetFinalState(TargetRgb, TargetBri);

        IsFinished = true;
        return Task.CompletedTask;
      }

      TransitionState.SetRGBColor(startRgb);
      TransitionState.SetBrightnes(startBrightness);

      return Task.Run(async () =>
      {
        var updates = (TimeSpan.TotalMilliseconds / _updateFrequencyMs) * 0.75;

        if (TargetBri.HasValue
        && TargetRgb.HasValue
        && TargetBri.Value > 0.1
        && startBrightness <= 0.1)
        {
          //If going from OFF to light, and a color transition, set the color direct, to prevent weird color flashes
          TransitionState.SetRGBColor(TargetRgb.Value);

          //Set rgb to null to prevent any other updates
          TargetRgb = null;
          Debug.WriteLine("Quick set rgb");
        }

        double? stepSizeR = null;
        double? stepSizeG = null;
        double? stepSizeB = null;
        double? stepSizeBri = null;

        if (TargetRgb.HasValue)
        {
          stepSizeR = (TargetRgb.Value.R - startRgb.R) / updates;
          stepSizeG = (TargetRgb.Value.G - startRgb.G) / updates;
          stepSizeB = (TargetRgb.Value.B - startRgb.B) / updates;
        }

        if (TargetBri.HasValue)
        {
          stepSizeBri = (TargetBri - startBrightness) / updates;
        }

        Stopwatch sw = new Stopwatch();
        sw.Start();
        for (int i = 0; i < updates; i++)
        {
          if (cancellationToken.IsCancellationRequested)
            return;

          //Only update color if there are changes
          if (TargetRgb.HasValue
          && stepSizeR.HasValue && stepSizeG.HasValue && stepSizeB.HasValue)
          {
            TransitionState.SetRGBColor(new RGBColor(
              TransitionState.RGBColor.R + stepSizeR.Value,
              TransitionState.RGBColor.G + stepSizeG.Value,
              TransitionState.RGBColor.B + stepSizeB.Value
             ));
          }

          //Only update brightness if there are changes
          if (TargetBri.HasValue && stepSizeBri.HasValue)
          {
            TransitionState.SetBrightnes(TransitionState.Brightness + stepSizeBri.Value);
            Debug.WriteLine("Bri:" + TransitionState.Brightness);
          }

          await Task.Delay(_updateFrequencyMs).ConfigureAwait(false);

          if (sw.ElapsedMilliseconds > TimeSpan.TotalMilliseconds)
            break;
        }

        sw.Stop();

        SetFinalState(TargetRgb, TargetBri);

        IsFinished = true;

      }, cancellationToken);
    }

    private void SetFinalState(RGBColor? rgb, double? brightness)
    {
      if (rgb.HasValue)
        TransitionState.SetRGBColor(rgb.Value);
      if (brightness.HasValue)
        TransitionState.SetBrightnes(brightness.Value);

      Debug.WriteLine($"Finished: {rgb?.ToHex()}, bri: {brightness}");
    }
  }
}
