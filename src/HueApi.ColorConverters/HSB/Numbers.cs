namespace HueApi.ColorConverters.HSB
{
  /// <summary>
  /// Based on code contributed by https://github.com/CharlyTheKid
  /// </summary>
  internal static class Numbers
  {
    internal static double Max(params double[] numbers)
    {
      return numbers.Max();
    }

    internal static double Min(params double[] numbers)
    {
      return numbers.Min();
    }
  }
}
