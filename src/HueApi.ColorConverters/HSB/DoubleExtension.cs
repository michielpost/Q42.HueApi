namespace HueApi.ColorConverters.HSB
{
  /// <summary>
  /// Based on code contributed by https://github.com/CharlyTheKid
  /// </summary>
  internal static class DoubleExtension
  {
    /// <summary>
    /// Tests equality with a certain amount of precision.  Default to smallest possible double
    /// </summary>
    /// <param name="a">first value</param>
    /// <param name="b">second value</param> 
    /// <param name="precision">optional, smallest possible double value</param>
    /// <returns></returns>
    internal static bool AlmostEquals(this double a, double b, double precision = float.Epsilon)
    {
      return Math.Abs(a - b) <= precision;
    }
  }
}
