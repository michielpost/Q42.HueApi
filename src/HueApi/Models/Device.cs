using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HueApi.Models
{

  public class ProductData
  {
    [JsonPropertyName("certified")]
    public bool? Certified { get; set; }

    [JsonPropertyName("manufacturer_name")]
    public string? ManufacturerName { get; set; }

    [JsonPropertyName("model_id")]
    public string? ModelId { get; set; }

    [JsonPropertyName("product_archetype")]
    public string? ProductArchetype { get; set; }

    [JsonPropertyName("product_name")]
    public string? ProductName { get; set; } 

    [JsonPropertyName("software_version")]
    public string? SoftwareVersion { get; set; }

    [JsonPropertyName("hardware_platform_type")]
    public string? HardwarePlatformType { get; set; }

    [JsonPropertyName("function")]
    public string? Function { get; set; }
  }

  public class Device : HueResource
  {
    [JsonPropertyName("product_data")]
    public ProductData ProductData { get; set; } = new();


  }
}
