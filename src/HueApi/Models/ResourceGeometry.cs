using System.Text.Json.Serialization;

namespace HueApi.Models
{
  /// <summary>
  /// Data describing geometry information of a device or room. Only positioning of light services is supported.
  /// </summary>
  public class ResourceGeometry
  {
    [JsonPropertyName("objects")]
    public List<GeometryObject>? Objects { get; set; }
  }

  public class GeometryObject
  {
    /// <summary>
    /// Reference to the positioned device service
    /// </summary>
    [JsonPropertyName("reference")]
    public ResourceIdentifier Reference { get; set; } = new();

    [JsonPropertyName("transform")]
    public GeometryTransform Transform { get; set; } = new();
  }

  public class GeometryTransform
  {
    /// <summary>
    /// A 3D coordinate in space in meters.
    /// </summary>
    [JsonPropertyName("position")]
    public HuePosition Position { get; set; } = new();

    /// <summary>
    /// A quaternion representing rotation in 3D space.
    /// </summary>
    [JsonPropertyName("rotation")]
    public GeometryRotation Rotation { get; set; } = new();
  }

  /// <summary>
  /// A quaternion representing rotation in 3D space. All values minimum -1, maximum 1.
  /// </summary>
  public class GeometryRotation
  {
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("z")]
    public double Z { get; set; }

    [JsonPropertyName("w")]
    public double W { get; set; }
  }
}
