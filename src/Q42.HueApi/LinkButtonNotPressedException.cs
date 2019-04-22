using System;
using System.Collections.Generic;
using System.Text;

namespace Q42.HueApi
{
  /// <summary>
  /// Indicates that the Hue bridge push button was excepted to have been pressed.
  /// </summary>
  [Serializable]
  public class LinkButtonNotPressedException : HueException
  {
    public LinkButtonNotPressedException() { }
    public LinkButtonNotPressedException(string message) : base(message) { }
    public LinkButtonNotPressedException(string message, Exception inner) : base(message, inner) { }
    protected LinkButtonNotPressedException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
  }
}
