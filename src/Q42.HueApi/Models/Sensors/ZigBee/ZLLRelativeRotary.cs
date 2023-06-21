using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models.Sensors.ZigBee
{
  /// <summary>
  /// Hue Tap Dial Switch)
  /// </summary>
  public interface ZLLRelativeRotary : GeneralSensor
  {
    string ProductId { get; set; }

    string SwConfigId { get; set; }
  }

  public interface ZLLRelativeRotaryConfig : GeneralSensorConfig
  {
  }

  public interface ZLLRelativeRotaryState : GeneralSensorState
  {
    /// <summary>
    /// See: http://www.developers.meethue.com/documentation/supported-sensors
    /// </summary>
    int? RotaryEvent { get; set; }
    int? ExpectedRotation { get; set; }
    int? ExpectedEventDuration { get; set; }

  }
}
