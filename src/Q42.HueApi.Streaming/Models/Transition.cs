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

    private RGBColor? _targetRgb;
    private double? _targetBri;
    private TimeSpan _timeSpan;

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
      _targetRgb = targetRgb;
      _targetBri = targetBri;
      _timeSpan = timeSpan;
    }

    public void Start(RGBColor startRgb, double startBrightness, CancellationToken cancellationToken)
    {
      //Update should happen fast, so dont move between values but just set them
      if (_timeSpan.TotalMilliseconds < (_updateFrequencyMs))
      {
        SetFinalState(_targetRgb, _targetBri);

        IsFinished = true;
        return;
      }

      this.cancellationToken = cancellationToken;
      _startRgb = startRgb;
      _startBri = startBrightness;

      _deltaBri = _targetBri.HasValue ? _targetBri - _startBri : null;
      if(_targetRgb.HasValue)
      {
        _deltaR = _targetRgb.Value.R - _startRgb.R;
        _deltaG= _targetRgb.Value.G - _startRgb.G;
        _deltaB = _targetRgb.Value.B - _startRgb.B;
      }

      TransitionState.SetRGBColor(startRgb);
      TransitionState.SetBrightness(startBrightness);
      TransitionState.IsDirty = false;

      sw.Start();
    }

    public void UpdateCurrentState()
    {
      if (cancellationToken.IsCancellationRequested)
        IsFinished = true;

      if (IsFinished)
        return;

      var progress = sw.Elapsed.TotalMilliseconds / this._timeSpan.TotalMilliseconds;

      if (progress > 1)
      {
        SetFinalState(_targetRgb, _targetBri);

        sw.Stop();
        IsFinished = true;
        return;
      }

      //TODO: Apply easing function to progress

      //Apply progress to Delta
      if (_deltaR.HasValue)
      {
        TransitionState.SetRGBColor(new RGBColor(
                _startRgb.R + (_deltaR.Value * progress),
                _startRgb.G + (_deltaG.Value * progress),
                _startRgb.B + (_deltaB.Value * progress)
               ));
      }

      if (_deltaBri.HasValue)
      {
        TransitionState.SetBrightness(_startBri + (_deltaBri.Value * progress));
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
