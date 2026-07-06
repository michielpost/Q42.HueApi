using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdateHomekit : BaseResourceRequest
  {
    /// <summary>
    /// Reset homekit, including removing all pairings and reset state and Bonjour service to factory settings. The Homekit will start functioning after approximately 10 seconds. Only allowed value: homekit_reset
    /// </summary>
    [JsonPropertyName("action")]
    public string? Action { get; set; }
  }
}
