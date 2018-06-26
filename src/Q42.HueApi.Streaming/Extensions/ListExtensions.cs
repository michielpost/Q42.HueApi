using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q42.HueApi.Streaming.Extensions
{
  /// <summary>
  /// Helper methods for lists.
  /// https://stackoverflow.com/questions/11463734/split-a-list-into-smaller-lists-of-n-size
  /// </summary>
  public static class ListExtensions
  {
    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
    {
      return source
          .Select((x, i) => new { Index = i, Value = x })
          .GroupBy(x => x.Index / chunkSize)
          .Select(x => x.Select(v => v.Value).ToList())
          .ToList();
    }

    public static IEnumerable<IEnumerable<T>> ChunkByGroupNumber<T>(this IEnumerable<T> source, int groups)
    {
      return source
          .Select((x, i) => new { Index = i, Value = x })
          .GroupBy(x => x.Index % groups)
          .Select(x => x.Select(v => v.Value).ToList())
          .ToList();
    }
  }
}
