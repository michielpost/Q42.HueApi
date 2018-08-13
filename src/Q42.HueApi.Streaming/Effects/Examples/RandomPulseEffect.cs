using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Effects.BasEffects;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
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

    public double MaxRadius { get; set; } = 3;

    public RandomPulseEffect(bool fadeToZero = true, Func<TimeSpan> waitTime = null)
    {
      _fadeToZero = fadeToZero;
      _waitTime = waitTime;

      if (_waitTime == null)
        _waitTime = () => TimeSpan.FromMilliseconds(50);

      Radius = 0;
    }

    public override void Start()
    {
      base.Start();

      var state = new Models.EntertainmentState();
      state.SetBrightness(1);
      state.SetRGBColor(RGBColor.Random());

      this.State = state;

      _cts = new CancellationTokenSource();

      Task.Run(async () =>
      {
        double step = 0.2;
        while (true && !_cts.IsCancellationRequested)
        {
          Radius += step;
          await Task.Delay(_waitTime(), _cts.Token).ConfigureAwait(false);
          if (Radius >= MaxRadius)
          {
            if (_fadeToZero)
            {
              step = -0.2;
            }
            else
            {
              Radius = 0;
              state.SetRGBColor(RGBColor.Random());
            }
          }
          if (Radius <= 0)
          {
            step = 0.2;
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
