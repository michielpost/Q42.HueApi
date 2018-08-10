using Q42.HueApi.Streaming.Effects.BasEffects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Q42.HueApi.Streaming.Effects
{
  public class BlueLineEffect : AngleEffect
  {
    public BlueLineEffect()
    {
      Width = 1;
      X = 0;
      Y = 0;
    }

    public override void Start()
    {
      base.Start();

      CurrentAngle = 90;

      var state = new Models.StreamingState();
      state.SetBrightness(1);
      state.SetRGBColor(new ColorConverters.RGBColor("0000FF"));

      this.State = state;
    }
  }
}
