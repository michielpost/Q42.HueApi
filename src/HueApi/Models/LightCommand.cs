using HueApi.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
