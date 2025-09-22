using HueApi.Entertainment.Effects.BasEffects;

namespace HueApi.Entertainment.Effects.Examples
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

      State = state;

    }

  }
}
