namespace HueApi.Entertainment.Models
{
  /// <summary>
  /// Helper to support passing structs as refs in async functions
  /// https://www.thomaslevesque.com/2014/11/04/passing-parameters-by-reference-to-an-asynchronous-method/
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class Ref<T>
  {
    public Ref() { }
    public Ref(T value) { Value = value; }
    public T? Value { get; set; }
    public override string ToString()
    {
      T? value = Value;
      if (value == null)
        return string.Empty;
      else
        return value.ToString()!;
    }
    public static implicit operator T(Ref<T> r) { return r.Value!; }
    public static implicit operator Ref<T>(T value) { return new Ref<T>(value); }
  }
}
