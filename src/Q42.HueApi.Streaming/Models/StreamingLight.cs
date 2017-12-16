using Q42.HueApi.ColorConverters;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Text;

namespace Q42.HueApi.Streaming.Models
{
  public class StreamingLight
  {
    private LightLocation _location = new LightLocation();

    public byte Id { get; set; }

    public StreamingState State { get; set; } = new StreamingState();


    public StreamingLight(string id, LightLocation location = null)
    {
      Id = byte.Parse(id);
      _location = location;
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
  }
}
