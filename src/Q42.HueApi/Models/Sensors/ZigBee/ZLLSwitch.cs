namespace Q42.HueApi.Models.Sensors.ZigBee
{
	/// <summary>
	/// Hue Wireless Dimmer Switch)
	/// </summary>
	public interface ZLLSwitch : GeneralSensor
	{
		string ProductId { get; set; }

		string SwConfigId { get; set; }
	}

	public interface ZLLSwitchConfig : GeneralSensorConfig
	{
	}

	public interface ZLLSwitchState : GeneralSensorState
	{
		/// <summary>
		/// See: http://www.developers.meethue.com/documentation/supported-sensors
		/// </summary>
		int? ButtonEvent { get; set; }

	}
}
