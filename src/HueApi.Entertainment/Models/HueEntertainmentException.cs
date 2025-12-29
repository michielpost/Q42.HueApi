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

  }
}
