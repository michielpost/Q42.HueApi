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
    public bool Certified { get; set; }

    [JsonPropertyName("manufacturer_name")]
    public string ManufacturerName { get; set; } = default!;

    [JsonPropertyName("model_id")]
    public string ModelId { get; set; } = default!;

    [JsonPropertyName("product_archetype")]
    public string ProductArchetype { get; set; } = default!;

    [JsonPropertyName("product_name")]
    public string ProductName { get; set; } = default!;

    [JsonPropertyName("software_version")]
    public string SoftwareVersion { get; set; } = default!;
  }

  public class Device : HueResource
  {
    [JsonPropertyName("product_data")]
    public ProductData ProductData { get; set; } = new();

    [JsonPropertyName("services")]
    public List<ResourceIdentifier> Services { get; set; } = new();

  }
}
