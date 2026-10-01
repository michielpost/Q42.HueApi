using System.Text.Json;
using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class CreateBehaviorScriptFormula : BaseResourceRequest
  {
    /// <summary>
    /// Short description of script.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// JSON schema object used for validating ScriptInstance.configuration property.
    /// </summary>
    [JsonPropertyName("configuration_schema")]
    public JsonElement? ConfigurationSchema { get; set; }

    /// <summary>
    /// JSON schema object used for validating ScriptInstance.trigger property.
    /// </summary>
    [JsonPropertyName("trigger_schema")]
    public JsonElement? TriggerSchema { get; set; }

    /// <summary>
    /// JSON schema of ScriptInstance.state property.
    /// </summary>
    [JsonPropertyName("state_schema")]
    public JsonElement? StateSchema { get; set; }

    /// <summary>
    /// Version of script.
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    /// Features that the script supports.
    /// </summary>
    [JsonPropertyName("supported_features")]
    public List<string>? SupportedFeatures { get; set; }

    /// <summary>
    /// Language of the formula content. Always "hsl".
    /// </summary>
    [JsonPropertyName("language")]
    public string? Language { get; set; }

    /// <summary>
    /// Formula source.
    /// </summary>
    [JsonPropertyName("content")]
    public JsonElement? Content { get; set; }
  }
}
