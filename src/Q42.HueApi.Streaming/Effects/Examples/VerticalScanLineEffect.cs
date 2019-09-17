using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects.BasEffects;
using Q42.HueApi.Streaming.Models;

namespace Q42.HueApi.Streaming.Effects.Examples
{
  public class VerticalScanLineEffect : YAxisLineEffect
  {
    private CancellationTokenSource _cts;
    private Func<TimeSpan> _waitTime;
    private RGBColor _color;
    public double StepSize { get; set; } = 0.15;
    public bool AutoRepeat { get; set; } = true;
    public double TurningPoint { get; set; } = 1.5;

    public VerticalScanLineEffect(Func<TimeSpan>? waitTime = null, RGBColor? color = null)
    {
      if(waitTime != null)
        _waitTime = waitTime;
      else if (_waitTime == null)
        _waitTime = () => TimeSpan.FromMilliseconds(50);

      _color = color ?? RGBColor.Random();

      Radius = 1;
    }

    public override void Start()
    {
      base.Start();

      var state = new Models.EntertainmentState();
      state.SetBrightness(1);
      state.SetRGBColor(_color);

      this.State = state;

      _cts = new CancellationTokenSource();

      Task.Run(async () =>
      {
        double step = StepSize;
        if (this.Y > 0)
          step = -1 * StepSize;

        while (true && !_cts.IsCancellationRequested)
        {
          await Task.Delay(_waitTime(), _cts.Token).ConfigureAwait(false);
          if (this.Y >= TurningPoint)
          {
            if (!AutoRepeat)
              break;

            step = -1 * Math.Abs(step);
          }
          else if (this.Y <= (-1 * TurningPoint))
          {
            if (!AutoRepeat)
              break;

            step = Math.Abs(step);
          }

          this.Y += step;

        }
      }, _cts.Token);

    }

    public override void Stop()
    {
      base.Stop();

      _cts.Cancel();
      Radius = 0;
    }
  }
}
