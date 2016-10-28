using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
	/// <summary>
	/// Can be used to create any type of schedule command
	/// Use a raw jsonString as input
	/// 
	/// dynamic dynamicCOmmand = new ExpandoObject();
	/// dynamicCOmmand.status = 1;
	/// var jsonString = JsonConvert.SerializeObject(dynamicCOmmand);
	/// var commandBody = new GenericScheduleCommand(jsonString);
	/// </summary>
	public class GenericScheduleCommand : ICommandBody
	{
		public string JsonString { get; set; }

		public GenericScheduleCommand(string jsonString)
		{
			JsonString = jsonString;
		}

		public bool IsSceneCommand()
		{
			JObject jObject = JObject.Parse(this.JsonString);

			return jObject["scene"] != null || jObject["Scene"] != null;
		}

		public SceneCommand AsSceneCommand()
		{
			return JsonConvert.DeserializeObject<SceneCommand>(this.JsonString);
		}

		public LightCommand AsLightCommand()
		{
			return JsonConvert.DeserializeObject<LightCommand>(this.JsonString);
		}

		public SensorState AsSensorCommand()
		{
			return JsonConvert.DeserializeObject<SensorState>(this.JsonString);
		}
	}
}
