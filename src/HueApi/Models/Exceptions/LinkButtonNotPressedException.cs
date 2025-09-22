namespace HueApi.Models.Exceptions
{
  public class LinkButtonNotPressedException : HueException
  {
    public LinkButtonNotPressedException() { }
    public LinkButtonNotPressedException(string message) : base(message) { }
    public LinkButtonNotPressedException(string message, Exception inner) : base(message, inner) { }
  }
}
