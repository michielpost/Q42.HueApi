using Newtonsoft.Json;

namespace Q42.HueApi
{
  public class PortalState
  {
    [JsonProperty("signedon")]
    public bool SignedOn { get; set; }

    [JsonProperty("incoming")]
    public bool Incoming { get; set; }

    [JsonProperty("outgoing")]
    public bool Outgoing { get; set; }

    [JsonProperty("communication")]
    public string Communication { get; set; }
  }
}
