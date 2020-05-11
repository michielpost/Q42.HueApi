using System;
using System.Collections.Generic;
using System.Text;

namespace Q42.HueApi.Models.Bridge
{
  public class RegisterEntertainmentResult
  {
    public string Ip { get; set; }
    public string Username { get; set; }
    public string? StreamingClientKey { get; set; }
  }
}
