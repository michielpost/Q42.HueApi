using Q42.HueApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueDiagnostics.Models
{
  public class DiagnosticsData
  {
    public string? BridgeIp { get; internal set; }
    public BridgeConfig? BridgeConfig { get; internal set; }
    public Bridge? Bridge { get; internal set; }
  }
}
