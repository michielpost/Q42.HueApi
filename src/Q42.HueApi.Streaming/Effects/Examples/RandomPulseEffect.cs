using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects.BasEffects;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Effects
{
  public class RandomPulseEffect : SinglePointEffect
  {
    private CancellationTokenSource _cts;
    private bool _fadeToZero;
    private Func<TimeSpan> _waitTime;
    private double _maxRadius = 2.5;

    public double StepSize { get; set; } = 0.2;
    public bool AutoRepeat { get; set; } = true;

    public RandomPulseEffect(bool fadeToZero = true, Func<TimeSpan>? waitTime = null)
    {
      _fadeToZero = fadeToZero;

      if(waitTime != null)
        _waitTime = waitTime;
      else if (_waitTime == null)
        _waitTime = () => TimeSpan.FromMilliseconds(50);

      Radius = 0;
    }

    public void CalculateMaxRadius()
    {
      var dis1 = Distance(1, 1);
      var dis2 = Distance(1, -1);
      var dis3 = Distance(-1, 1);
      var dis4 = Distance(-1, -1);

      var maxDistance = (new List<double>() { dis1, dis2, dis3, dis4 }).OrderBy(x => x).Last();
      this._maxRadius = maxDistance * 1.3;
    }

    private double Distance(double x, double y)
    {
      var x2 = this.X;
      var y2 = this.Y;

      return Math.Sqrt((Math.Pow(x - x2, 2) + Math.Pow(y - y2, 2)));
    }

    public override void Start()
    {
      base.Start();

      CalculateMaxRadius();

      var state = new Models.EntertainmentState();
      state.SetBrightness(1);
      state.SetRGBColor(RGBColor.Random());

      this.State = state;

      _cts = new CancellationTokenSource();

      Task.Run(async () =>
      {
        double step = StepSize;
        while (true && !_cts.IsCancellationRequested)
        {
          Radius += step;
          await Task.Delay(_waitTime(), _cts.Token).ConfigureAwait(false);
          if (Radius >= _maxRadius)
          {
            if (_fadeToZero)
            {
              step = -1 * Math.Abs(StepSize);
            }
            else
            {
              Radius = 0;

              if (!AutoRepeat)
                break;

              state.SetRGBColor(RGBColor.Random());
            }
          }
          if (Radius <= 0)
          {
            if (!AutoRepeat)
              break;

            step = Math.Abs(StepSize);
            state.SetRGBColor(RGBColor.Random());
          }
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
