namespace HueApi.Models.Exceptions
{
  public class HueException : Exception
  {
    public HueException() { }
    public HueException(string? message) : base(message) { }
    public HueException(string? message, Exception inner) : base(message, inner) { }
  }
}
