using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi.Entertainment.Models
{
  /// <summary>
  /// Indicates that a specific Hue Entertainment exception has occurred.
  /// </summary>
  [Serializable]
  public class HueEntertainmentException : Exception
  {
    public HueEntertainmentException() { }
    public HueEntertainmentException(string message) : base(message) { }
    public HueEntertainmentException(string message, Exception inner) : base(message, inner) { }
    protected HueEntertainmentException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
  }
}
