using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class CreateUpdateMotionAreaConfig : BaseResourceRequest
  {
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("group")]
    public ResourceIdentifier? Group { get; set; }

    /// <summary>
    /// A list of motion area services used in this configuration. Supported types “MotionAreaCandidate”. Required on create.
    /// </summary>
    [JsonPropertyName("participants")]
    public List<MotionAreaParticipant>? Participants { get; set; }


    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

  }

}
