using System.Text.Json.Serialization;

namespace HueApi.Models
{
  public class ResourceIdentifier
  {
    [JsonPropertyName("rid")]
    public string Rid { get; set; } = default!;

    [JsonPropertyName("rtype")]
    public string Rtype { get; set; } = default!;
  }
}
