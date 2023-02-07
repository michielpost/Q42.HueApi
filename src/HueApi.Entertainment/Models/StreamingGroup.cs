using HueApi.Entertainment.Extensions;
using HueApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HueApi.Entertainment.Models
{
  /// <summary>
  /// Group of channels/lights that we're streaming to
  /// </summary>
  public class StreamingGroup : List<StreamingChannel>
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
    public StreamingGroup(List<int> channelIds)
    {
      this.AddRange(channelIds.Select(x => new StreamingChannel(x, new HuePosition(), new())));
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="channels"></param>
    public StreamingGroup(List<EntertainmentChannel> channels)
    {
      AddRange(channels.Select(x => new StreamingChannel(x.ChannelId, x.Position, x.Members.Select(x => x.Service?.Rid).Where(x => x.HasValue).Select(x => x!.Value).ToList())));
    }

    public StreamingGroup(Dictionary<int, Tuple<HuePosition, List<Guid>>> locations)
    {
      AddRange(locations.Select(x => new StreamingChannel(x.Key, x.Value.Item1, x.Value.Item2)));
    }

    /// <summary>
    /// Creates a new layer
    /// </summary>
    /// <param name="isBaseLayer">Base layers keep updating their state to the light, even if there are no changes</param>
    /// <returns></returns>
    public EntertainmentLayer GetNewLayer(bool isBaseLayer = false)
    {
      var layer = new EntertainmentLayer(isBaseLayer);
      var all = this.Select(x => new EntertainmentLight(x.Id, x.ChannelLocation, x.DeviceIds));
      layer.AddRange(all);

      Layers.Add(layer);

      return layer;
    }

    internal static List<byte[]>? GetCurrentStateAsByteArray(byte[] entConfigId, IEnumerable<IEnumerable<StreamingChannel>> chunks)
    {
      //Nothing to update
      if (!chunks.Any())
        return null;

      List<byte[]> result = new List<byte[]>();

      foreach (var chunk in chunks)
      {
        List<byte> resultLightState = new List<byte>();

        List<byte> baseStateMsg = new List<byte> { //protocol
        0x02, 0x00, //version 2.0
        0x01, //sequence number 1
        0x00, 0x00, //reserved
        0x00, //color mode RGB
        0x00, //OR 0x01, //linear filter
        //entertainment configuration id
        //0x00, 0x00, 0x01, //light 1
        //0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
        //0x00, 0x00, 0x02, //light 2
        //0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        //0x00, 0x00, 0x03, //light 3
        //0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    };

        //Add protocol in front of message
        resultLightState = protocolName.Concat(baseStateMsg).Concat(entConfigId).ToList();

        //Add state of lights to message
        foreach (var light in chunk)
        {
          resultLightState.AddRange(light.GetState());
        }

        result.Add(resultLightState.ToArray());
      }

      return result;
    }

    public IEnumerable<IEnumerable<StreamingChannel>> GetChunksForUpdate(bool forceUpdate)
    {
      //All transitions should update their state
      //Use extra ToList to prevent exceptions about modified collections
      var transitions = Layers.SelectMany(x => x.Select(z => z.Transition).ToList()).ToList();
      transitions.Where(t => t?.IsStarted ?? false)
                            .ToList()
                            .Distinct()
                            .ToList()
                            .ForEach(x => x?.UpdateCurrentState());

      Layers.ForEach(x => x.ProcessTransitions());

      //Calculate state based on all layers
      ForEach(x => x.SetStateFor(x, Layers, BrightnessFilter));

      //A streaming message contains max 20 light updates
      var chunks = this.Where(x => x.State.IsDirty || forceUpdate).ChunkBy(IsForSimulator ? 100 : 20);
      return chunks;
    }

  }
}
