using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class HuePosition
  {
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("z")]
    public double Z { get; set; }


    public bool IsLeft => X <= 0; //Include 0 with left
    public bool IsRight => X > 0;
    public bool IsFront => Y >= 0; //Include 0 with front
    public bool IsBack => Y < 0;
    public bool IsTop => Z >= 0; //Include 0 with top
    public bool IsBottom => Z < 0;


    /// <summary>
    /// X > -0.1 && X < 0.1
    /// </summary>
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
