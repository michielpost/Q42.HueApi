using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Models.Gamut;

namespace Q42.HueApi.Converters
{
  internal class GamutConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(CIE1931Gamut?) || objectType == typeof(CIE1931Gamut);
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
      if (reader.TokenType == JsonToken.StartArray)
      {

        JArray array = JArray.Load(reader);
        var gammutValues = array.ToObject<IList<IList<double>>>();

        if (gammutValues != null && gammutValues.Count == 3)
        {
          var red = new CIE1931Point(gammutValues[0][0], gammutValues[0][1]);
          var green = new CIE1931Point(gammutValues[1][0], gammutValues[1][1]);
          var blue = new CIE1931Point(gammutValues[2][0], gammutValues[2][1]);

          return new CIE1931Gamut(red, green, blue);
        }
      }

      return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      CIE1931Gamut gammut;

      if (value == null)
      {
        writer.WriteNull();
        return;
      }

      if (value is CIE1931Gamut?)
        gammut = ((CIE1931Gamut?)value).Value;
      else
        gammut = (CIE1931Gamut)value;

      var red = new List<double>() { gammut.Red.x, gammut.Red.y };
      var green = new List<double>() { gammut.Green.x, gammut.Green.y };
      var blue = new List<double>() { gammut.Blue.x, gammut.Blue.y };

      var result = new List<List<double>>() { red, green, blue };

      serializer.Serialize(writer, result);
    }
  }
}
