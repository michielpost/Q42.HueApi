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
    private Ref<TimeSpan?> _waitTime;

    public RandomPulseEffect(bool fadeToZero = true, Ref<TimeSpan?> waitTime = null)
    {
      _fadeToZero = fadeToZero;
      _waitTime = waitTime;

      if (_waitTime == null)
        _waitTime = TimeSpan.FromMilliseconds(50);

      Radius = 0;
    }

    public override void Start()
    {
      base.Start();

      var state = new Models.EntertainmentState();
      state.SetBrightnes(1);
      state.SetRGBColor(GetRandomColor());

      this.State = state;

      _cts = new CancellationTokenSource();

      Task.Run(async () =>
      {
        double step = 0.2;
        while (true && !_cts.IsCancellationRequested)
        {
          Radius += step;
          await Task.Delay(_waitTime.Value.Value);
          if (Radius >= 2)
          {
            if (_fadeToZero)
            {
              step = -0.2;
            }
            else
            {
              Radius = 0;
              state.SetRGBColor(GetRandomColor());
            }
          }
          if (Radius <= 0)
          {
            step = 0.2;
            state.SetRGBColor(GetRandomColor());
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

    private RGBColor GetRandomColor()
    {
      var r = new Random();
      return new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
    }

  }
}
