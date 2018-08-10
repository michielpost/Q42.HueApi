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
  public class HorizontalScanLineEffect : XAxisLineEffect
  {
   
    private CancellationTokenSource _cts;
    private Func<TimeSpan> _waitTime;
    private RGBColor _color;

    public HorizontalScanLineEffect(Func<TimeSpan> waitTime = null, RGBColor? color = null)
    {
      _waitTime = waitTime;

      if (_waitTime == null)
        _waitTime = () => TimeSpan.FromMilliseconds(50);

      _color = color ?? RGBColor.Random();

      Radius = 0.6;
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
        double step = 0.1;
        while (true && !_cts.IsCancellationRequested)
        {
          await Task.Delay(_waitTime(), _cts.Token).ConfigureAwait(false);
          if (this.X >= 1.5)
          {
            step = -0.1;
          }
          else if (this.X <= -1.5)
          {
            step = 0.1;
          }

          this.X += step;

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
