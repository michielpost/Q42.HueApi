using Q42.HueApi.ColorConverters;
using Q42.HueApi.Streaming.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Q42.HueApi.Streaming.Extensions
{
  public enum IteratorEffectMode
  {
    /// <summary>
    /// Loops the lights one by one
    /// </summary>
    Cycle,
    /// <summary>
    /// Each light one by one, bounces back on the end of the list
    /// </summary>
    Bounce,
    /// <summary>
    /// Only Once
    /// </summary>
    Single,
    /// <summary>
    /// Will select a random light each iteration
    /// </summary>
    Random,
    /// <summary>
    /// Order lights random and do each light once
    /// </summary>
    RandomOrdered,
    /// <summary>
    /// Apply the effect on all lights at the same time, ignoring different start states.
    /// Best for syncing all lights
    /// </summary>
    All,
    /// <summary>
    /// Apply the effect on all lights individually.
    /// Best used for example random colors to all lights
    /// </summary>
    AllIndividual
  }

  /// <summary>
  /// Function to apply light effects.
  /// </summary>
  /// <param name="current">Will contain 1 light, only contains multiple lights when IteratorEffectMode.All is used</param>
  /// <param name="timeSpan"></param>
  public delegate Task IteratorEffectFunc(IEnumerable<EntertainmentLight> current, CancellationToken cancellationToken, TimeSpan timeSpan);

  public static class EntertainmentGroupExtensions
  {
    public static IEnumerable<EntertainmentLight> GetLeft(this IEnumerable<EntertainmentLight> group) => group.Where(x => x.LightLocation.IsLeft);

    public static IEnumerable<EntertainmentLight> GetRight(this IEnumerable<EntertainmentLight> group) => group.Where(x => x.LightLocation.IsRight);

    public static IEnumerable<EntertainmentLight> GetFront(this IEnumerable<EntertainmentLight> group) => group.Where(x => x.LightLocation.IsFront);

    public static IEnumerable<EntertainmentLight> GetBack(this IEnumerable<EntertainmentLight> group) => group.Where(x => x.LightLocation.IsBack);
    public static IEnumerable<EntertainmentLight> GetTop(this IEnumerable<EntertainmentLight> group) => group.Where(x => x.LightLocation.IsTop);
    public static IEnumerable<EntertainmentLight> GetBottom(this IEnumerable<EntertainmentLight> group) => group.Where(x => x.LightLocation.IsBottom);

    /// <summary>
    /// X > -0.1 && X < 0.1
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    public static IEnumerable<EntertainmentLight> GetCenter(this IEnumerable<EntertainmentLight> group) => group.Where(x => x.LightLocation.IsCenter);

    /// <summary>
    /// Apply the effectFunction repeatedly to a group of lights
    /// </summary>
    /// <param name="group"></param>
    /// <param name="effectFunction"></param>
    /// <param name="mode"></param>
    /// <param name="waitTime"></param>
    /// <param name="duration"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task IteratorEffect(this IEnumerable<EntertainmentLight> group, CancellationToken cancellationToken, IteratorEffectFunc effectFunction, IteratorEffectMode mode, Func<TimeSpan> waitTime, TimeSpan? duration = null, int maxIterations = int.MaxValue)
    {
      if (waitTime == null)
        waitTime = () => TimeSpan.FromSeconds(1);
      if (duration == null)
        duration = TimeSpan.MaxValue;

      bool keepGoing = true;
      var lights = group.ToList();
      bool reverse = false;

      Stopwatch sw = new Stopwatch();
      sw.Start();

      int i = 0;
      while (keepGoing && !cancellationToken.IsCancellationRequested && !(sw.Elapsed > duration) && i < maxIterations)
      {
        //Apply to whole group if mode is all
        if(mode == IteratorEffectMode.All)
        {
          effectFunction(group, cancellationToken, waitTime());

          await Task.Delay(waitTime(), cancellationToken).ConfigureAwait(false);

          i++;
          continue;
        }

        if (reverse)
          lights.Reverse();
        if (mode == IteratorEffectMode.Random || mode == IteratorEffectMode.RandomOrdered)
          lights = lights.OrderBy(x => Guid.NewGuid()).ToList();

        foreach(var light in lights.Skip(reverse ? 1 : 0))
        {
          if (!cancellationToken.IsCancellationRequested)
          {
            effectFunction(new List<EntertainmentLight>() { light }, cancellationToken, waitTime());

            if (mode != IteratorEffectMode.AllIndividual)
              await Task.Delay(waitTime(), cancellationToken).ConfigureAwait(false);
          }
        }

        if(mode == IteratorEffectMode.AllIndividual)
          await Task.Delay(waitTime(), cancellationToken).ConfigureAwait(false);

        keepGoing = mode == IteratorEffectMode.Single ? false : true;
        if (mode == IteratorEffectMode.Bounce)
          reverse = true;

        i++;
      }
    }

    /// <summary>
    /// Apply the groupFunction repeatedly to a list of groups of lights
    /// </summary>
    /// <param name="list"></param>
    /// <param name="groupFunction"></param>
    /// <param name="mode"></param>
    /// <param name="waitTime"></param>
    /// <param name="duration"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task IteratorEffect(this IEnumerable<IEnumerable<EntertainmentLight>> list, CancellationToken cancellationToken, IteratorEffectFunc groupFunction, IteratorEffectMode mode, IteratorEffectMode secondaryMode, Func<TimeSpan> waitTime, TimeSpan? duration = null, int maxIterations = int.MaxValue)
    {
      if (waitTime == null)
        waitTime = () => TimeSpan.FromSeconds(1);
      if (duration == null)
        duration = TimeSpan.MaxValue;

      int secondaryMaxIterations = 1;
      //Normalize secondary iterator mode
      switch (secondaryMode)
      {
        case IteratorEffectMode.Bounce:
          secondaryMaxIterations = 2;
          break;
        case IteratorEffectMode.Cycle:
        case IteratorEffectMode.Single:
          secondaryMode = IteratorEffectMode.Single;
          break;
        case IteratorEffectMode.Random:
        case IteratorEffectMode.RandomOrdered:
          secondaryMode = IteratorEffectMode.RandomOrdered;
          break;
        case IteratorEffectMode.All:
        case IteratorEffectMode.AllIndividual:
        default:
          break;
      }

      bool keepGoing = true;
      var groups = list.ToList();
      bool reverse = false;

      if(mode == IteratorEffectMode.RandomOrdered)
        groups = groups.OrderBy(x => Guid.NewGuid()).ToList();

      Stopwatch sw = new Stopwatch();
      sw.Start();

      int i = 0;
      while (keepGoing && !cancellationToken.IsCancellationRequested && !(sw.Elapsed > duration) && i < maxIterations)
      {
        //Apply to all groups if mode is all
        if (mode == IteratorEffectMode.All)
        {
          var flatGroup = list.SelectMany(x => x);
          if (!cancellationToken.IsCancellationRequested)
            groupFunction(flatGroup, cancellationToken, waitTime());

          //foreach (var group in list)
          //{
          //  if (!cancellationToken.IsCancellationRequested)
          //    await groupFunction(group, waitTime);
          //}

          await Task.Delay(waitTime(), cancellationToken).ConfigureAwait(false);
          i++;
          continue;
        }

        if (reverse)
          groups.Reverse();
        if (mode == IteratorEffectMode.Random)
          groups = groups.OrderBy(x => Guid.NewGuid()).ToList();

        if (mode == IteratorEffectMode.AllIndividual)
        {
          List<Task> allIndividualTasks = new List<Task>();
          foreach (var group in groups.Skip(reverse ? 1 : 0).Where(x => x.Any()))
          {
            //Do not await, AllIndividual runs them all at the same time
            var t = group.IteratorEffect(cancellationToken, groupFunction, secondaryMode, waitTime, maxIterations: secondaryMaxIterations);
            allIndividualTasks.Add(t);
          }

          await Task.WhenAll(allIndividualTasks).ConfigureAwait(false);
        }
        else
        {
          foreach (var group in groups.Skip(reverse ? 1 : 0).Where(x => x.Any()))
          {
            await group.IteratorEffect(cancellationToken, groupFunction, secondaryMode, waitTime, maxIterations: secondaryMaxIterations).ConfigureAwait(false);
          }
        }

        keepGoing = mode == IteratorEffectMode.Single || mode == IteratorEffectMode.RandomOrdered ? false : true;
        if (mode == IteratorEffectMode.Bounce)
          reverse = true;

        i++;
      }
    }



    /// <summary>
    /// Brightness between 0 and 1
    /// </summary>
    /// <param name="group"></param>
    /// <param name="brightness">Between 0 and 1</param>
    /// <param name="transitionTime"></param>
    /// <param name="inSync">Syncs the transition over all lights. Set to false if each light has a different starting rgb/bri for the transition</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static IEnumerable<EntertainmentLight> SetBrightness(this IEnumerable<EntertainmentLight> group,
      CancellationToken cancellationToken,
      double brightness, TimeSpan transitionTime = default(TimeSpan), bool inSync = true)
    {
      group.SetState(cancellationToken, null, brightness, transitionTime, inSync);

      return group;
    }

    /// <summary>
    /// Transition to new RGB Color
    /// </summary>
    /// <param name="group"></param>
    /// <param name="rgb"></param>
    /// <param name="transitionTime"></param>
    /// <param name="inSync">Syncs the transition over all lights. Set to false if each light has a different starting rgb/bri for the transition</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static IEnumerable<EntertainmentLight> SetColor(this IEnumerable<EntertainmentLight> group,
      CancellationToken cancellationToken,
      RGBColor rgb, TimeSpan transitionTime = default(TimeSpan), bool inSync = true)
    {
      group.SetState(cancellationToken, rgb, null, transitionTime, inSync);

      return group;
    }

   

    /// <summary>
    /// 
    /// </summary>
    /// <param name="group"></param>
    /// <param name="rgb"></param>
    /// <param name="brightness"></param>
    /// <param name="transitionTime"></param>
    /// <param name="inSync">Syncs the transition over all lights. Set to false if each light has a different starting rgb/bri for the transition</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static IEnumerable<EntertainmentLight> SetState(this IEnumerable<EntertainmentLight> group,
      CancellationToken cancellationToken,
      RGBColor? rgb = null, double? brightness = null, TimeSpan transitionTime = default, bool inSync = true)
    {
      if (!group.Any())
        return group;

      //Re-use the same transition for all lights so transition is in sync. The transition will use the start rgb/bri from the first light in the group.
      if (inSync)
      {
        //Create a new transition
        Transition transition = new Transition(rgb, brightness, transitionTime);

        //Add the same transition to all lights in this group
        foreach (var light in group)
        {
          if(!cancellationToken.IsCancellationRequested)
            light.Transition = transition;
        }

        //Start the transition
        var firstLight = group.First();
        transition.Start(firstLight.State.RGBColor, firstLight.State.Brightness, cancellationToken);
      }
      else
      {
        foreach (var light in group)
        {
          if (!cancellationToken.IsCancellationRequested)
            light.SetState(cancellationToken, rgb, brightness, transitionTime);
        }
      }

      return group;
    }

    public static IEnumerable<EntertainmentLight> SetState(this IEnumerable<EntertainmentLight> group, CancellationToken cancellationToken, RGBColor? rgb = null, TimeSpan rgbTimeSpan = default, double? brightness = null, TimeSpan briTimeSpan = default, bool overwriteExistingTransition = true)
    {
      if (!group.Any())
        return group;

      foreach (var light in group)
      {
        if (!cancellationToken.IsCancellationRequested)
          light.SetState(cancellationToken, rgb, rgbTimeSpan, brightness, briTimeSpan, overwriteExistingTransition);
      }

      return group;
    }
  }
}
