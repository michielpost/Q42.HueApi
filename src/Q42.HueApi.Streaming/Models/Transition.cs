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
    public double? DeltaR { get; internal set; }
    public double? DeltaG { get; internal set; }
    public double? DeltaB { get; internal set; }

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
      if(TargetRgb.HasValue)
      {
        DeltaR = TargetRgb.Value.R - StartRgb.R;
        DeltaG= TargetRgb.Value.G - StartRgb.G;
        DeltaB = TargetRgb.Value.B - StartRgb.B;
      }

      TransitionState.SetRGBColor(startRgb);
      TransitionState.SetBrightnes(startBrightness);
      TransitionState.IsDirty = false;

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
      if (DeltaR.HasValue)
      {
        TransitionState.SetRGBColor(new RGBColor(
                StartRgb.R + (DeltaR.Value * progress),
                StartRgb.G + (DeltaG.Value * progress),
                StartRgb.B + (DeltaB.Value * progress)
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
    }
  }
}
