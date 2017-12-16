using Q42.HueApi.Models.Groups;
using Q42.HueApi.Streaming.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q42.HueApi.Streaming.Models
{
  public class StreamingGroup : List<StreamingLight>
  {
    public bool IsForSimulator { get; set; }

    private List<byte> _protocolName = Encoding.ASCII.GetBytes(new char[] { 'H', 'u', 'e', 'S', 't', 'r', 'e', 'a', 'm' }).ToList();

    public StreamingGroup(List<string> lightIds)
    {
      this.AddRange(lightIds.Select(x => new StreamingLight(x)));
    }

    public StreamingGroup(Dictionary<string, LightLocation> locations)
    {
      this.AddRange(locations.Select(x => new StreamingLight(x.Key, x.Value)));
    }

    public List<byte[]> GetCurrentState()
    {
      List<byte[]> result = new List<byte[]>();

      //A streaming message contains max 10 light updates
      var chunks = this.ChunkBy(IsForSimulator ? 100 : 10);

      foreach (var chunk in chunks)
      {
        List<byte> resultLightState = new List<byte>();

        List<byte> baseStateMsg = new List<byte> { //protocol
        0x01, 0x00, //version 1.0
        0x01, //sequence number 1
        0x00, 0x00, //reserved
        0x00, //color mode RGB
        0x01, //linear filter
        //0x00, 0x00, 0x01, //light 1
        //0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
        //0x00, 0x00, 0x02, //light 2
        //0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
        //0x00, 0x00, 0x03, //light 3
        //0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
    };

        //Add protocol in front of message
        resultLightState = _protocolName.Concat(baseStateMsg).ToList();

        //Add state of lights to message
        foreach (var light in chunk)
        {
          resultLightState.AddRange(light.GetState());
        }

        result.Add(resultLightState.ToArray());
      }

      return result;
    }
  }
}
