using Q42.HueApi.Streaming.Effects.BasEffects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Q42.HueApi.Streaming.Effects
{
    public class RedLightEffect : SinglePointEffect
  {

    public RedLightEffect()
    {
      Radius = 0.75;
    }

    public override void Start()
    {
      base.Start();

      var state = new Models.StreamingState();
      state.SetBrightness(1);
      state.SetRGBColor(new ColorConverters.RGBColor("FF0000"));

      this.State = state;

    }

  }
}
