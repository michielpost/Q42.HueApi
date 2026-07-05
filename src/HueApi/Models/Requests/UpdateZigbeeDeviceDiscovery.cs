using System.Text.Json.Serialization;

namespace HueApi.Models.Requests
{
  public class UpdateZigbeeDeviceDiscovery : BaseResourceRequest
  {
    /// <summary>
    /// Trigger a device search. Set action_type (and optionally search_codes / search_channels).
    /// </summary>
    [JsonPropertyName("action")]
    public ZigbeeDeviceDiscoveryAction? Action { get; set; }

    [JsonPropertyName("add_install_codes")]
    public AddInstallCodes? AddInstallCodes { get; set; }
  }

  public class AddInstallCodes
  {
    /// <summary>
    /// minItems: 1 – maxItems: 50
    /// </summary>
    [JsonPropertyName("install_codes")]
    public List<InstallCode>? InstallCodes { get; set; }
  }

  public class InstallCode
  {
    /// <summary>
    /// Pattern: ^([0-9a-fA-F]{2}:){7}[0-9a-fA-F]{2}$
    /// </summary>
    [JsonPropertyName("mac_address")]
    public string MacAddress { get; set; } = default!;

    /// <summary>
    /// Pattern: ^[A-F0-9]{36}$
    /// </summary>
    [JsonPropertyName("ic")]
    public string Ic { get; set; } = default!;
  }
}
