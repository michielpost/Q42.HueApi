using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace Q42.HueApi.Converters
{
  /// <summary>
  /// Custom DateTime converter for hue bridge
  /// </summary>
  internal class DateTimeConverter : IsoDateTimeConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      DateTime span;

      if (value == null)
      {
          return;
      }

      if (value is DateTime?)
        span = ((DateTime?)value).Value;
      else
        span = (DateTime)value;

      writer.WriteValue(span.ToString("yyyy-MM-ddThh:mm:ss", CultureInfo.InvariantCulture));
    }
  }
}