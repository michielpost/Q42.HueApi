using HueApi.Entertainment.Effects.BasEffects;
using HueApi.Entertainment.Extensions;
using HueApi.Models;
using System.Collections;

namespace HueApi.Entertainment.Models
{
  public class EntertainmentLayer : IReadOnlyList<EntertainmentLight>
  {
    //Guards both lights and effects. Enumeration hands out snapshots so callers
    //can mutate the layer while AutoCalculateEffectUpdate is reading on another thread.
    private readonly object syncLock = new object();
    private readonly List<EntertainmentLight> lights = new List<EntertainmentLight>();
    private readonly List<BaseEffect> effects = new List<BaseEffect>();

    public bool IsBaseLayer { get; set; }

    /// <summary>
    /// Snapshot of the effects applied to this layer. Use PlaceEffect/RemoveEffect/ClearEffects to modify
    /// </summary>
    public IReadOnlyList<BaseEffect> Effects
    {
      get
      {
        lock (syncLock)
          return effects.ToList();
      }
    }

    public int Count
    {
      get
      {
        lock (syncLock)
          return lights.Count;
      }
    }

    public EntertainmentLight this[int index]
    {
      get
      {
        lock (syncLock)
          return lights[index];
      }
    }

    public EntertainmentLayer(bool isBaseLayer = false)
    {
      IsBaseLayer = isBaseLayer;
    }

    public void Add(EntertainmentLight light)
    {
      lock (syncLock)
        lights.Add(light);
    }

    public void AddRange(IEnumerable<EntertainmentLight> newLights)
    {
      lock (syncLock)
        lights.AddRange(newLights);
    }

    public bool Remove(EntertainmentLight light)
    {
      lock (syncLock)
        return lights.Remove(light);
    }

    public void Clear()
    {
      lock (syncLock)
        lights.Clear();
    }

    /// <summary>
    /// Enumerates a snapshot of the lights in this layer
    /// </summary>
    public IEnumerator<EntertainmentLight> GetEnumerator()
    {
      List<EntertainmentLight> snapshot;
      lock (syncLock)
        snapshot = lights.ToList();

      return snapshot.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    internal void ProcessTransitions()
    {
      foreach (var light in this)
        light.ProcessTransitions();
    }

    /// <summary>
    /// Adds an effect to the effect list
    /// </summary>
    /// <param name="baseEffect"></param>
    public void PlaceEffect(BaseEffect baseEffect)
    {
      lock (syncLock)
        effects.Add(baseEffect);
    }

    /// <summary>
    /// Removes an effect from the effect list
    /// </summary>
    /// <param name="baseEffect"></param>
    public bool RemoveEffect(BaseEffect baseEffect)
    {
      lock (syncLock)
        return effects.Remove(baseEffect);
    }

    /// <summary>
    /// Removes all effects from the effect list
    /// </summary>
    public void ClearEffects()
    {
      lock (syncLock)
        effects.Clear();
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
          var activeEffects = Effects.Where(x => x.State != null).ToList();

          foreach (var light in this)
          {
            double? finalMultiplier = null;
            BaseEffect? finalEffect = null;

            //Only activate effect with strongest effect multiplier
            foreach (var effect in activeEffects)
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
