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

    private readonly TimeSpan _rgbTimeSpan;
    private readonly TimeSpan _briTimeSpan;

    /// <summary>
    /// Delta between TargetRgb and StartRgb
    /// </summary>
    private double? _deltaR;
    private double? _deltaG;
    private double? _deltaB;

    /// <summary>
    /// Delta between TargetBri and StartBri
    /// </summary>
    private double? _deltaBri;

    private RGBColor _startRgb;
    private double _startBri;

    public RGBColor? TargetRgb { get; }

    public double? TargetBri { get; }

    public TimeSpan ElapsedTime => sw.Elapsed;

    public TimeSpan RgbRemainingTime => _rgbTimeSpan - ElapsedTime;

    public TimeSpan BrightnessRemainingTime => _briTimeSpan - ElapsedTime;

    public bool IsBrightnessFinished { get; private set; }

    public bool IsRgbFinished { get; private set; }

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
    public EntertainmentState TransitionState { get; private set; } = new EntertainmentState();

    private Stopwatch sw = new Stopwatch();
    private CancellationToken cancellationToken;

    public Transition(RGBColor? targetRgb, double? targetBri, TimeSpan timeSpan)
    {
      TargetRgb = targetRgb;
      TargetBri = targetBri;
      _rgbTimeSpan = timeSpan;
      _briTimeSpan = timeSpan;

      IsRgbFinished = !targetRgb.HasValue;
      IsBrightnessFinished = !targetBri.HasValue;
    }

    public Transition(RGBColor targetRgb, double targetBri, TimeSpan rgbTimeSpan, TimeSpan briTimeSpan)
    {
      TargetRgb = targetRgb;
      TargetBri = targetBri;
      _rgbTimeSpan = rgbTimeSpan;
      _briTimeSpan = briTimeSpan;
    }

    public Transition(RGBColor targetRgb, TimeSpan rgbTimeSpan)
    {
      TargetRgb = targetRgb;
      TargetBri = null;
      _rgbTimeSpan = rgbTimeSpan;
      _briTimeSpan = TimeSpan.Zero;

      IsBrightnessFinished = true;
    }

    public Transition(double targetBri, TimeSpan briTimeSpan)
    {
      TargetRgb = null;
      TargetBri = targetBri;
      _rgbTimeSpan = TimeSpan.Zero;
      _briTimeSpan = briTimeSpan;

      IsRgbFinished = true;
    }

    public void Start(RGBColor startRgb, double startBrightness, CancellationToken cancellationToken)
    {
      //Update should happen fast, so dont move between values but just set them
      if (!TargetRgb.HasValue || _rgbTimeSpan.TotalMilliseconds < (_updateFrequencyMs))
      {
        SetFinalState(TargetRgb ?? startRgb, null);
        IsRgbFinished = true;
      }

      if (!TargetBri.HasValue || _briTimeSpan.TotalMilliseconds < (_updateFrequencyMs)) {
        SetFinalState(null, TargetBri ?? startBrightness);
        IsBrightnessFinished = true;
      }

      if (IsRgbFinished && IsBrightnessFinished) {
        IsFinished = true;
        return;
      }

      this.cancellationToken = cancellationToken;
      _startRgb = startRgb;
      _startBri = startBrightness;

      _deltaBri = TargetBri.HasValue ? TargetBri - _startBri : null;
      if(TargetRgb.HasValue)
      {
        _deltaR = TargetRgb.Value.R - _startRgb.R;
        _deltaG= TargetRgb.Value.G - _startRgb.G;
        _deltaB = TargetRgb.Value.B - _startRgb.B;
      }

      if (!IsRgbFinished) {
        TransitionState.SetRGBColor(startRgb);
        TransitionState.IsDirty = false;
      }
      if (!IsBrightnessFinished) {
        TransitionState.SetBrightness(startBrightness);
        TransitionState.IsDirty = false;
      }
      
      sw.Start();
    }

    public void UpdateCurrentState()
    {
      if (cancellationToken.IsCancellationRequested)
        IsFinished = true;

      if (IsFinished)
        return;

      var rgbProgress = sw.Elapsed.TotalMilliseconds / this._rgbTimeSpan.TotalMilliseconds;
      var briProgress = sw.Elapsed.TotalMilliseconds / this._briTimeSpan.TotalMilliseconds;

      if (!IsRgbFinished && rgbProgress > 1)
      {
        SetFinalState(TargetRgb, null);
        IsRgbFinished = true;
      }

      if (!IsBrightnessFinished && briProgress > 1) {
        SetFinalState(null, TargetBri);
        IsBrightnessFinished = true;
      }

      if (IsRgbFinished && IsBrightnessFinished) {
        sw.Stop();
        IsFinished = true;
        return;
      }

      //TODO: Apply easing function to progress

      //Apply progress to Delta
      if (_deltaR.HasValue && !IsRgbFinished)
      {
        TransitionState.SetRGBColor(new RGBColor(
                _startRgb.R + (_deltaR.Value * rgbProgress),
                _startRgb.G + (_deltaG.Value * rgbProgress),
                _startRgb.B + (_deltaB.Value * rgbProgress)
               ));
      }

      if (_deltaBri.HasValue && !IsBrightnessFinished)
      {
        TransitionState.SetBrightness(_startBri + (_deltaBri.Value * briProgress));
      }
    }
   
    private void SetFinalState(RGBColor? rgb, double? brightness)
    {
      if (rgb.HasValue)
        TransitionState.SetRGBColor(rgb.Value);
      if (brightness.HasValue)
        TransitionState.SetBrightness(brightness.Value);
    }
  }
}
