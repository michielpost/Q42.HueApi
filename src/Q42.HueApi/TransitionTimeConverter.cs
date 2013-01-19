using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
  internal class TransitionTimeConverter
    : Newtonsoft.Json.Converters.CustomCreationConverter<TimeSpan?>
  {
    public override TimeSpan? Create (Type objectType)
    {
      return new TimeSpan();
    }

    public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
    {
      int? value = reader.ReadAsInt32();
      if (value == null)
        return null;

      return (TimeSpan?)TimeSpan.FromMilliseconds (value.Value * 100);
    }

    public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
    {
      TimeSpan? ts = (TimeSpan?)value;
      if (ts == null)
        writer.WriteValue ((int?)null);
      else
        writer.WriteValue((int?)(ts.Value.TotalSeconds * 10));
    }
  }
}