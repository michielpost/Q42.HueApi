using HueApi.Entertainment.Effects.BasEffects;
using HueApi.Entertainment.Extensions;
using HueApi.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HueApi.Entertainment.Models
{
  public class EntertainmentLayer : List<EntertainmentLight>
  {

    public bool IsBaseLayer { get; set; }

    /// <summary>
    /// List of effects applied to this streaminggroup
    /// </summary>
    public List<BaseEffect> Effects { get; set; } = new List<BaseEffect>();

    public EntertainmentLayer(bool isBaseLayer = false)
    {
      IsBaseLayer = isBaseLayer;
    }

    internal void ProcessTransitions()
    {
      ForEach(x => x.ProcessTransitions());
    }

    /// <summary>
    /// Adds an effect to the effect list
    /// </summary>
    /// <param name="baseEffect"></param>
    public void PlaceEffect(BaseEffect baseEffect)
    {
      Effects.Add(baseEffect);
    }

    /// <summary>
    /// Used to auto update and apply effects that are added to the Effects list of the StreamingGroup
    /// </summary>
    /// <param name="entGroup"></param>
    /// <param name="cancellationToken"></param>
    public void AutoCalculateEffectUpdate(CancellationToken cancellationToken)
    {
      Task.Run(async () =>
      {
        int waitTime = 50;

        while (!cancellationToken.IsCancellationRequested)
        {
          foreach (var light in this)
          {
            double? finalMultiplier = null;
            BaseEffect? finalEffect = null;

            //Only activate effect with strongest effect multiplier
            foreach (var effect in Effects.Where(x => x.State != null).ToList())
            {
              var effectMultiplier = effect.GetEffectStrengthMultiplier(light);
              if (effectMultiplier > finalMultiplier || !finalMultiplier.HasValue)
              {
                finalMultiplier = effectMultiplier;
                finalEffect = effect;
              }
            }

            if (finalMultiplier.HasValue)
            {
              light.SetState(cancellationToken, finalEffect?.State?.RGBColor, finalEffect?.State?.Brightness * finalMultiplier.Value);
            }
          }

          await Task.Delay(waitTime, cancellationToken).ConfigureAwait(false);
        }

      }, cancellationToken);
    }
  }

  /// <summary>
  /// Light that is included in a entertainment group
  /// </summary>
  public class EntertainmentLight
  {
    private readonly object transitionLock = new object();

    public HuePosition LightLocation { get; private set; }
    public List<Guid> DeviceIds { get; }
    public byte Id { get; set; }

    public EntertainmentState State { get; set; } = new EntertainmentState();

    private Transition? _transition;
    public Transition? Transition
    {
      get
      {
        return _transition;
      }
      set
      {
        lock (transitionLock)
        {
          _transition = value;
        }
      }
    }


    public EntertainmentLight(byte id, HuePosition location, List<Guid> deviceIds)
    {
      Id = id;
      LightLocation = location;
      DeviceIds = deviceIds;
    }

    /// <summary>
    /// Changes the state based on one or more transition
    /// </summary>
    internal void ProcessTransitions()
    {
      lock (transitionLock)
      {
        if (Transition == null)
          return;

        State.SetBrightness(Transition.TransitionState.Brightness);
        State.SetRGBColor(Transition.TransitionState.RGBColor);

        if (Transition.IsFinished)
          Transition = null;
      }
    }
  }
}
