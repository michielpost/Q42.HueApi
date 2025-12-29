using HueApi.Models;

namespace HueDiagnostics.Models
{
  public class DiagnosticsData
  {
    public string? BridgeIp { get; internal set; }
    public Bridge? Bridge { get; internal set; }
    public Device? Device { get; internal set; }
  }
}
