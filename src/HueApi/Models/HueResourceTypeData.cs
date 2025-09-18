namespace HueApi.Models
{
  public class HueResourceTypeData
  {
    public required string Key { get; set; }

    public required Type GetType { get; set; }
    public required Type? PostType { get; set; }
    public required Type? PutType { get; set; }
    public required Type? DeleteType { get; set; }
  }
}
