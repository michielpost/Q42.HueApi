using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Converters;
using Q42.HueApi.Interfaces;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Q42.HueApi.Models
{
  public class InternalBridgeCommand
  {
    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonConverter(typeof(HttpMethodConverter))]
    [JsonProperty("method")]
    public HttpMethod Method { get; set; }

    [JsonConverter(typeof(CommandBodyConverter))]
    [JsonProperty("body")]
    public ICommandBody Body { get; set; }

	[OnDeserialized]
	internal void OnDeserializedMethod(StreamingContext context)
	{
		if(Body != null)
		{
			if(Body is GenericScheduleCommand genericCommand)
			{
				var invariantAddress = Address.ToLowerInvariant();

				//Check if it is a scene command
				if (genericCommand.IsSceneCommand())
				{
					Body = genericCommand.AsSceneCommand();
				}
				//If it is going to a lights or groups URL, it's probably a LightCommand
				else if (invariantAddress.Contains("/lights") || invariantAddress.Contains("/groups"))
				{
					Body = genericCommand.AsLightCommand();
				}
				//If it is going to a sensor url, it's probably a SensorCommand
				else if (invariantAddress.Contains("/sensors"))
				{
					Body = genericCommand.AsSensorCommand();
				}
			}
		}
			
	}
  }
}
