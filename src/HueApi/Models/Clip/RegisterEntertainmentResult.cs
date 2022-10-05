using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueApi.Models.Clip
{
  public class RegisterEntertainmentResult
  {
    /// <summary>
    /// Hue Bridge IP
    /// </summary>
    public string? Ip { get; set; }

    /// <summary>
    /// Hue Bridge Username / Key
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Only filled when generateClientKey is set to true on the request to register an app
    /// </summary>
    public string? StreamingClientKey { get; set; }
  }
}
