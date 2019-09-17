using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Q42.HueApi.Converters;
using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  [DataContract]
  public class InternalBridgeCommand
  {
    [DataMember(Name = "address")]
    public string Address { get; set; }

    [JsonConverter(typeof(HttpMethodConverter))]
    [DataMember(Name = "method")]
    public HttpMethod Method { get; set; }

    [JsonConverter(typeof(CommandBodyConverter))]
    [DataMember(Name = "body")]
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
