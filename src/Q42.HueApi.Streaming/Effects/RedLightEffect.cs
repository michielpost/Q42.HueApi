using System;
using System.Collections.Generic;
using System.Text;

namespace Q42.HueApi.Streaming.Effects
{
    public class RedLightEffect : BaseEffect
    {

    public RedLightEffect()
    {
      Radius = 0.75;
    }

    public override void Start()
    {
      base.Start();

      var state = new Models.StreamingState();
      state.SetBrightnes(1);
      state.SetRGBColor(new ColorConverters.RGBColor("FF0000"));

      this.State = state;

    }

  }
}
