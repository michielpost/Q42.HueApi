using System;
using System.Collections.Generic;
using System.Text;

namespace Q42.HueApi
{
  /// <summary>
  /// Indicates that a specific Hue API exception has occurred.
  /// </summary>
  [Serializable]
  public class HueException : Exception
  {
    public HueException() { }
    public HueException(string message) : base(message) { }
    public HueException(string message, Exception inner) : base(message, inner) { }
    protected HueException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
  }
}
