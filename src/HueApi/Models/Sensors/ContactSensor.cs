using System.Text.Json.Serialization;

namespace HueApi.Models.Sensors
{
  public class ContactSensor : HueResource
  {
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = default!;

    [JsonPropertyName("contact_report")]
    public ContactReport? ContactReport { get; set; }
  }

  public class ContactReport
  {
    [JsonPropertyName("changed")]
    public DateTimeOffset Changed { get; set; }

    [JsonPropertyName("state")]
    public ContactState State { get; set; }

  }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public enum ContactState
  {
    contact, no_contact
  }
}
