using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Q42.HueApi.Converters
{
  internal class HttpMethodConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof (HttpMethod);
    }

    public override bool CanRead
    {
      get { return true; }
    }

    public override bool CanWrite
    {
      get { return true; }
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return null;

      if (reader.TokenType == JsonToken.String)
      {
        string? value = (string?)reader.Value;
        if (string.IsNullOrEmpty(value))
          return null;

        return new HttpMethod(value);
      }

      return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      if (value == null)
      {
          writer.WriteNull();
          return;
      }

      writer.WriteValue(((HttpMethod)value).Method);
    }
  }
}
