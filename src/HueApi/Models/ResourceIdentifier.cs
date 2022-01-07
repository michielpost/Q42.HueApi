using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class ResourceIdentifier
  {
    [JsonPropertyName("rid")]
    public Guid Rid { get; set; }

    [JsonPropertyName("rtype")]
    public string Rtype { get; set; } = default!;
  }
}
