using Q42.HueApi.ColorConverters;
using Q42.HueApi.Models.Groups;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Models
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
      this.ForEach(x => x.ProcessTransitions());
    }

    /// <summary>
    /// Adds an effect to the effect list
    /// </summary>
    /// <param name="baseEffect"></param>
    public void PlaceEffect(BaseEffect baseEffect)
    {
      this.Effects.Add(baseEffect);
    }

    /// <summary>
    /// Used to auto update and apply effects that are added to the Effects list of the StreamingGroup
    /// </summary>
    /// <param name="entGroup"></param>
    /// <param name="cancellationToken"></param>
    public void AutoCalculateEffectUpdate(CancellationToken cancellationToken)
    {
      Task.Run(async () => {
        int waitTime = 50;

        while (!cancellationToken.IsCancellationRequested)
        {
          foreach (var light in this)
          {
            double? finalMultiplier = null;
            BaseEffect finalEffect = null;

            //Only activate effect with strongest effect multiplier
            foreach (var effect in this.Effects.Where(x => x.State != null).ToList())
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
              light.SetState(cancellationToken, finalEffect.State?.RGBColor, finalEffect.State?.Brightness * finalMultiplier.Value);
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
    public LightLocation LightLocation { get; private set; }

    public byte Id { get; set; }

    public EntertainmentState State { get; set; } = new EntertainmentState();

    public BlockingCollection<Transition> Transitions { get; set; } = new BlockingCollection<Transition>();


    public EntertainmentLight(byte id, LightLocation location = null)
    {
      Id = id;
      LightLocation = location;
    }

    /// <summary>
    /// Changes the state based on one or more transition
    /// </summary>
    internal void ProcessTransitions()
    {
      var finishedStates = Transitions.Where(x => x?.IsFinished ?? false).ToList();

      if (finishedStates.Any())
      {
        foreach (var finished in finishedStates)
        {
          this.State.SetBrightness(finished.TransitionState.Brightness);
          this.State.SetRGBColor(finished.TransitionState.RGBColor);

          //Transitions.Remove(finished);
        }

        //Cancel and remove all transitions, last finished state is important
        Transitions = new BlockingCollection<Transition>();
      }
      else
      {
        //Get active transition
        var activeTransition = Transitions.Where(x => x?.TransitionState.IsDirty ?? false).ToList().LastOrDefault();
        if (activeTransition != null)
        {
          this.State.SetBrightness(activeTransition.TransitionState.Brightness);
          this.State.SetRGBColor(activeTransition.TransitionState.RGBColor);
        }
      }
    }
  }
}
