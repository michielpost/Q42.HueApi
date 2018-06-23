using Q42.HueApi.ColorConverters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Effects
{
  public class RandomPulseEffect : BaseEffect
  {
    private CancellationTokenSource _cts;
    private bool _fadeToZero;

    public RandomPulseEffect(bool fadeToZero = true)
    {
      _fadeToZero = fadeToZero;

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
        while (true)
        {
          Radius += step;
          await Task.Delay(50);
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
    }

    private RGBColor GetRandomColor()
    {
      var r = new Random();
      return new RGBColor(r.NextDouble(), r.NextDouble(), r.NextDouble());
    }

  }
}
