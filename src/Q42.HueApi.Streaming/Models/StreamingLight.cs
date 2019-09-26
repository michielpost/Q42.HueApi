using Q42.HueApi.ColorConverters;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Q42.HueApi.Streaming.Models
{
  /// <summary>
  /// Light that is included in a streaming group
  /// </summary>
  public class StreamingLight
  {
    public LightLocation LightLocation { get; private set; }

    public byte Id { get; set; }

    public StreamingState State { get; set; } = new StreamingState();

   // public List<Transition> Transitions { get; set; } = new List<Transition>();


    public StreamingLight(string id, LightLocation location)
    {
      Id = byte.Parse(id);
      LightLocation = location;
    }

    internal IEnumerable<byte> GetState()
    {
      List<byte> result = new List<byte>();

      byte deviceType = 0x00; //Type of device 0x00 = Light; 0x01 = Area
      var lightIdBytes = BitConverter.GetBytes(this.Id);

      result.Add(deviceType);
      result.Add(0x00);
      result.Add(this.Id);
      result.AddRange(this.State.ToByteArray());

      return result;
    }

    internal void SetStateFor(StreamingLight light, List<EntertainmentLayer> layers, double? brightnessFilter)
    {
      //Base state does not check IsDirty flag
      var baseState = layers.Where(x => x.IsBaseLayer).SelectMany(x => x).Where(l => l.Id == light.Id).Select(x => x.State).LastOrDefault();
      var lightState = layers.Where(x => !x.IsBaseLayer).SelectMany(x => x).Where(l => l.Id == light.Id && l.State.Brightness > 0).Select(x => x.State).LastOrDefault();

      var currentState = lightState ?? baseState;
      if(currentState != null)
      {
        var finalBri = currentState.Brightness;
        if(brightnessFilter.HasValue && brightnessFilter > 0)
        {
          //Filter brightness
          finalBri -= finalBri * brightnessFilter.Value;
        }

        if(light.State.RGBColor != currentState.RGBColor)
          light.State.SetRGBColor(currentState.RGBColor);

        if(light.State.Brightness != finalBri)
          light.State.SetBrightness(finalBri);
      }
    }
  }
}
