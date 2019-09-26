using Q42.HueApi.Models.Groups;
using Q42.HueApi.Streaming.Effects;
using Q42.HueApi.Streaming.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q42.HueApi.Streaming.Models
{
  /// <summary>
  /// Group of lights that we're streaming to
  /// </summary>
  public class StreamingGroup : List<StreamingLight>
  {
    /// <summary>
    /// Set to true if you're using the Hue simulator. Behaviour is slightly different then the real Hue Bridge
    /// </summary>
    public bool IsForSimulator { get; set; }

    /// <summary>
    /// 0 does not filter, 1 filters all brightness
    /// </summary>
    public double? BrightnessFilter { get; set; }

    public List<EntertainmentLayer> Layers { get; set; } = new List<EntertainmentLayer>();


    private static readonly List<byte> protocolName = Encoding.ASCII.GetBytes(new char[] { 'H', 'u', 'e', 'S', 't', 'r', 'e', 'a', 'm' }).ToList();

    /// <summary>
    /// Constructor without light locations
    /// </summary>
    /// <param name="lightIds"></param>
    public StreamingGroup(List<string> lightIds)
    {
      this.AddRange(lightIds.Select(x => new StreamingLight(x, new LightLocation())));
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="locations"></param>
    public StreamingGroup(Dictionary<string, LightLocation> locations)
    {
      this.AddRange(locations.Select(x => new StreamingLight(x.Key, x.Value)));
    }

    /// <summary>
    /// Creates a new layer
    /// </summary>
    /// <param name="isBaseLayer">Base layers keep updating their state to the light, even if there are no changes</param>
    /// <returns></returns>
    public EntertainmentLayer GetNewLayer(bool isBaseLayer = false)
    {
      var layer = new EntertainmentLayer(isBaseLayer);
      var all = this.Select(x => new EntertainmentLight(x.Id, x.LightLocation));
      layer.AddRange(all);

      this.Layers.Add(layer);

      return layer;
    }

    internal static List<byte[]>? GetCurrentStateAsByteArray(IEnumerable<IEnumerable<StreamingLight>> chunks)
    {
      //Nothing to update
      if (!chunks.Any())
        return null;

      List<byte[]> result = new List<byte[]>();

      foreach (var chunk in chunks)
      {
        List<byte> resultLightState = new List<byte>();

        List<byte> baseStateMsg = new List<byte> { //protocol
        0x01, 0x00, //version 1.0
        0x01, //sequence number 1
        0x00, 0x00, //reserved
        0x00, //color mode RGB
        0x00, //OR 0x01, //linear filter
        //0x00, 0x00, 0x01, //light 1
        //0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
        //0x00, 0x00, 0x02, //light 2
        //0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        //0x00, 0x00, 0x03, //light 3
        //0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    };

        //Add protocol in front of message
        resultLightState = protocolName.Concat(baseStateMsg).ToList();

        //Add state of lights to message
        foreach (var light in chunk)
        {
          resultLightState.AddRange(light.GetState());
        }

        result.Add(resultLightState.ToArray());
      }

      return result;
    }

    public IEnumerable<IEnumerable<StreamingLight>> GetChunksForUpdate(bool forceUpdate)
    {
      //All transitions should update their state
      //Use extra ToList to prevent exceptions about modified collections
      var transitions = this.Layers.SelectMany(x => x.Select(z => z.Transition).ToList()).ToList();
      transitions.Where(t => t?.IsStarted ?? false)
                            .ToList()
                            .Distinct()
                            .ToList()
                            .ForEach(x => x?.UpdateCurrentState());

      this.Layers.ForEach(x => x.ProcessTransitions());

      //Calculate state based on all layers
      this.ForEach(x => x.SetStateFor(x, this.Layers, this.BrightnessFilter));

      //A streaming message contains max 10 light updates
      var chunks = this.Where(x => x.State.IsDirty || forceUpdate).ChunkBy(IsForSimulator ? 100 : 10);
      return chunks;
    }

  }
}
