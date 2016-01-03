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
	}
}
