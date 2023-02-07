using System.Diagnostics;
using System.Text.Json.Serialization;

namespace HueApi.Models
{
  [DebuggerDisplay("{X}, {Y}, {Z}")]
  public record HuePosition
  {
    public HuePosition()
    {

    }

    public HuePosition(double x, double y, double z)
    {
      X = x;
      Y = y;
      Z = z;
    }

    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("z")]
    public double Z { get; set; }

    [JsonIgnore]
    public bool IsLeft => X <= 0; //Include 0 with left

    [JsonIgnore]
    public bool IsRight => X > 0;

    [JsonIgnore]
    public bool IsFront => Y >= 0; //Include 0 with front

    [JsonIgnore]
    public bool IsBack => Y < 0;

    [JsonIgnore]
    public bool IsTop => Z >= 0; //Include 0 with top

    [JsonIgnore]
    public bool IsBottom => Z < 0;

    /// <summary>
    /// X > -0.1 && X < 0.1
    /// </summary>
    [JsonIgnore]
    public bool IsCenter => X > -0.1 && X < 0.1;

    public double Distance(double x, double y, double z)
    {
      var x2 = this.X;
      var y2 = this.Y;
      var z2 = this.Z;

      return Math.Sqrt(Math.Pow(x - x2, 2) + Math.Pow(y - y2, 2) + Math.Pow(z - z2, 2));
    }

    public double Angle(double x, double y)
    {
      var lengthX = this.X - x;
      var lengthY = this.Y - y;

      return (Math.Atan2(lengthY, lengthX) * (180 / Math.PI)) + 180;
    }
  }
}
