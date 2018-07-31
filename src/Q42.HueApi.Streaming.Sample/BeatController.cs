//using Q42.HueApi.Streaming.Extensions;
//using Q42.HueApi.Streaming.Models;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Text;
//using System.Timers;

//namespace Q42.HueApi.Streaming.Sample
//{
//  public class BeatController
//  {
//    public TimeSpan? LastManualBeat { get; set; }
//    public Func<TimeSpan> CurrentBeat { get; set; }

//    private Timer _timer = new Timer();
//    private IEnumerable<EntertainmentLight> _lights;
//    private Stopwatch sw = new Stopwatch();

//    public IteratorEffectFunc EffectFunction { get; set; }

//    public BeatController(IEnumerable<EntertainmentLight> lights)
//    {
//      _timer.Elapsed += _timer_Elapsed;
//      _lights = lights;
//    }

//    private void _timer_Elapsed(object sender, ElapsedEventArgs e)
//    {
//      EffectFunction?.Invoke(_lights, new System.Threading.CancellationToken(), CurrentBeat);
//    }

//    public void StartAutoTimer(TimeSpan interval)
//    {
//      CurrentBeat.Value = interval;
//      _timer.Interval = interval.TotalMilliseconds;
//      _timer.Start();
//    }

//    public void StopAutoTimer()
//    {
//      _timer.Stop();
//    }

//    public void ManualBeat(IteratorEffectFunc effectFunction = null)
//    {
//      if (effectFunction != null)
//        EffectFunction = effectFunction;

//      if (sw.IsRunning && sw.Elapsed < TimeSpan.FromSeconds(5))
//      {
//        LastManualBeat = sw.Elapsed;
//        sw.Reset();
//      }
//      sw.Start();

//      _timer.Stop();
//      if (LastManualBeat.HasValue)
//        StartAutoTimer(LastManualBeat.Value);

//      _timer_Elapsed(null, null);

//    }

//    public void AutoContinueManualBeat()
//    {
//      sw.Stop();
//      StartAutoTimer(LastManualBeat.Value);

//    }

//  }
//}
