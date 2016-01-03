using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Converters
{
  internal class CommandBodyConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return (objectType == typeof(ICommandBody));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.StartObject)
      {
        JObject jObject = JObject.Load(reader);

        //Check if it is a scene command
        if (jObject["scene"] != null || jObject["Scene"] != null)
        {
          var sceneTarget = new SceneCommand();
          // Populate the object properties
          serializer.Populate(jObject.CreateReader(), sceneTarget);
          return sceneTarget;
        }

        // Populate the object properties
        var target = new LightCommand();
        serializer.Populate(jObject.CreateReader(), target);
        return target;
      }
     
      return serializer.Deserialize<LightCommand>(reader);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
			if (value is GenericScheduleCommand)
			{
				var genericCommand = value as GenericScheduleCommand;
				writer.WriteRawValue(genericCommand.JsonString);
			}
			else
			{
				serializer.Serialize(writer, value);
			}
    }
  }
}
