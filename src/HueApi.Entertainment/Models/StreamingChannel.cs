using HueApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace HueApi.Entertainment.Models
{
  /// <summary>
  /// Channel that is included in a streaming group
  /// </summary>
  public class StreamingChannel
  {
    public HuePosition ChannelLocation { get; private set; }
    public List<Guid> DeviceIds { get; }
    public byte Id { get; set; }

    public StreamingState State { get; set; } = new StreamingState();

    // public List<Transition> Transitions { get; set; } = new List<Transition>();


    /// <summary>
    /// StreamingChannel
    /// </summary>
    /// <param name="id"></param>
    /// <param name="location"></param>
    /// <param name="deviceIds">Device IDs targeted with this channel</param>
    public StreamingChannel(int id, HuePosition location, List<Guid> deviceIds)
    {
      Id = Convert.ToByte(id);
      ChannelLocation = location;
      DeviceIds = deviceIds;
    }

    internal IEnumerable<byte> GetState()
    {
      List<byte> result = new List<byte>();

      //byte deviceType = 0x00; //Type of device 0x00 = Light; 0x01 = Area
      //var lightIdBytes = BitConverter.GetBytes(Id);

      //result.Add(deviceType);
      //result.Add(0x00);
      result.Add(Id);
      result.AddRange(State.ToByteArray());

      return result;
    }

    internal void SetStateFor(StreamingChannel light, List<EntertainmentLayer> layers, double? brightnessFilter)
    {
      //Base state does not check IsDirty flag
      var baseState = layers.Where(x => x.IsBaseLayer).SelectMany(x => x).Where(l => l.Id == light.Id).Select(x => x.State).LastOrDefault();
      var lightState = layers.Where(x => !x.IsBaseLayer).SelectMany(x => x).Where(l => l.Id == light.Id && l.State.Brightness > 0).Select(x => x.State).LastOrDefault();

      var currentState = lightState ?? baseState;
      if (currentState != null)
      {
        var finalBri = currentState.Brightness;
        if (brightnessFilter.HasValue && brightnessFilter > 0)
        {
          //Filter brightness
          finalBri -= finalBri * brightnessFilter.Value;
        }

        if (light.State.RGBColor != currentState.RGBColor)
          light.State.SetRGBColor(currentState.RGBColor);

        if (light.State.Brightness != finalBri)
          light.State.SetBrightness(finalBri);
      }
    }
  }
}
