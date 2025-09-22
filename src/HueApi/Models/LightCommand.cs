using HueApi.Models.Requests;

namespace HueApi
{
  /// <summary>
  /// For easy migration from Q42.HueApi
  /// </summary>
  [Obsolete("Replace with: UpdateLight")]
  public class LightCommand : UpdateLight
  {
  }
}
