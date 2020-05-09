using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors
{
	/// <summary>
	/// Represents Base General Sensor data structure
	/// http://www.developers.meethue.com/documentation/supported-sensors
	/// </summary>
	public interface GeneralSensor
	{
		string Id { get; set; }

		string Name { get; set; }

		string Type { get; set; }

		string ModelId { get; set; }

		string ManufacturerName { get; set; }

		string UniqueId { get; set; }

		string SwVersion { get; set; }
	}

	public interface GeneralSensorConfig
	{
		bool? On { get; set; }

		bool? Reachable { get; set; }

		int? Battery { get; set; }

		string Alert { get; set; }

		bool? Usertest { get; set; }
		string Url { get; set; }

		List<string> Pending { get; set; }

		bool? LedIndication { get; set; }
	}

	public interface GeneralSensorState
	{
		DateTime? Lastupdated { get; set; }
	}

	public interface GeneralSensorCapabilities
	{
		bool? Certified { get; set; }
		bool? Primary { get; set; }
		SensorInput[]? Inputs { get; set; }
	}
}
