using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;

namespace Q42.HueApi.Converters
{
  public class StringNullableEnumConverter : StringEnumConverter
  {
    public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      try
      {
        return base.ReadJson(reader, objectType, existingValue, serializer);
      }
      catch (JsonSerializationException)
      {
        if (IsNullableType(objectType))
          return null;
        else
          throw;
      }
    }

    private bool IsNullableType(Type t)
    {
      return (t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
    }

  }
}
