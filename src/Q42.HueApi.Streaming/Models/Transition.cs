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

    /// <summary>
    /// Delta between TargetRgb and StartRgb
    /// </summary>
    public RGBColor? DeltaRgb { get; internal set; }

    /// <summary>
    /// Delta between TargetBri and StartBri
    /// </summary>
    public double? DeltaBri { get; internal set; }

    public RGBColor StartRgb { get; internal set; }
    public double StartBri { get; internal set; }

    public bool IsFinished { get; set; }

    public bool IsStarted
    {
      get
      {
        return sw.IsRunning;
      }
    }

    /// <summary>
    /// Current state of the transition
    /// </summary>
    public StreamingState TransitionState { get; private set; } = new StreamingState();

    private Stopwatch sw = new Stopwatch();
    private CancellationToken cancellationToken;

    public void Start(RGBColor startRgb, double startBrightness, CancellationToken cancellationToken)
    {
      //Update should happen fast, so dont move between values but just set them
      if (TimeSpan.TotalMilliseconds < (_updateFrequencyMs))
      {
        SetFinalState(TargetRgb, TargetBri);

        IsFinished = true;
        return;
      }

      this.cancellationToken = cancellationToken;
      this.StartRgb = startRgb;
      this.StartBri = startBrightness;

      this.DeltaBri = TargetBri.HasValue ? TargetBri - StartBri : null;
      this.DeltaRgb = TargetRgb.HasValue ? new RGBColor(TargetRgb.Value.R - StartRgb.R, TargetRgb.Value.G - StartRgb.G, TargetRgb.Value.B - StartRgb.B) : (RGBColor?)null;

      TransitionState.SetRGBColor(startRgb);
      TransitionState.SetBrightnes(startBrightness);

      sw.Start();
    }

    public void UpdateCurrentState()
    {
      if (cancellationToken.IsCancellationRequested)
        IsFinished = true;

      if (IsFinished)
        return;

      var progress = sw.Elapsed.TotalMilliseconds / this.TimeSpan.TotalMilliseconds;

      if (progress > 1)
      {
        SetFinalState(TargetRgb, TargetBri);

        sw.Stop();
        IsFinished = true;
        return;
      }

      //TODO: Apply easing function to progress

      //Apply progress to Delta
      if (DeltaRgb.HasValue)
      {
        TransitionState.SetRGBColor(new RGBColor(
                StartRgb.R + (DeltaRgb.Value.R * progress),
                StartRgb.G + (DeltaRgb.Value.G * progress),
                StartRgb.B + (DeltaRgb.Value.B * progress)
               ));
      }

      if (DeltaBri.HasValue)
      {
        TransitionState.SetBrightnes(StartBri + (DeltaBri.Value * progress));
      }
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
