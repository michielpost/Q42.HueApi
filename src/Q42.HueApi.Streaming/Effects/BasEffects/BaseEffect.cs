using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Effects
{
  /// <summary>
  /// https://developers.meethue.com/documentation/hue-edk-effect-creation
  /// 5.3 LightSourceEffect will map a virtual light source to actual lights such that lights close to the virtual light source are more strongly influenced than lights further away from the virual light source. LightSourceEffects have next to an animation for color also an animation for position and radius. Note the difference with the previous effect types: in both AreaEffect and MultiChannelEffect, a light either belongs to an area or channel or not, whereas for the LightSourceEffect it’s a more gradual relation: the closer to the virtual source the more strongly a light is influenced by the source.
  /// </summary>
  public abstract class BaseEffect
  {
    public EntertainmentState? State { get; set; }

    public virtual void Start()
    {
    }

    public virtual void Stop()
    {
      State?.SetBrightness(0);

      //Wait, because we want to make sure we set the state on the lights before we set the state to null
      Task.Run(async () => {
        await Task.Delay(TimeSpan.FromMilliseconds(200)).ConfigureAwait(false);
        State = null;
      });
    }

    public abstract double? GetEffectStrengthMultiplier(EntertainmentLight light);
  }
}
