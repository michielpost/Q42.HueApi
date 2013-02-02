using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Extensions
{
  /// <summary>
  /// IEnumerable Helpers
  /// </summary>
  internal static class IEnumerableExtensions
  {

    /// <summary>
    /// http://blogs.msdn.com/b/pfxteam/archive/2012/03/05/10278165.aspx
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="dop"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public static Task ForEachAsync<T>(this IEnumerable<T> source, int dop, Func<T, Task> body)
    {
      return Task.WhenAll(
          from partition in Partitioner.Create(source).GetPartitions(dop)
          select Task.Run(async delegate
          {
            using (partition)
              while (partition.MoveNext())
                await body(partition.Current);
          }));
    }
  }
}
