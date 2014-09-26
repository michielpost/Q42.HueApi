using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;
using Q42.HueApi.Models;
using System.Text.RegularExpressions;

namespace Q42.HueApi
{
  /// <summary>
  /// Custom DateTime converter for hue bridge
  /// </summary>
  internal class HueDateTimeConverter : IsoDateTimeConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      DateTime span;

      if (value == null)
      {
        return;
      }

      if (value is DateTime?)
        span = ((DateTime?)value).Value;
      else
        span = (DateTime)value;

      //moet anders
      writer.WriteValue(span.ToString("yyyy-MM-ddThh:mm:ss", CultureInfo.InvariantCulture));
    }

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			HueDateTime hueValueDate = new HueDateTime();
			if (reader.TokenType == JsonToken.Date)
			{
				hueValueDate.DateTime = (DateTime)reader.Value;				
				return hueValueDate;
			}

			string rawValue = reader.Value.ToString();

			//normal datetimes (optional randomtime)
			{
				var groups = Regex.Match(rawValue, @"(?<date>[0-9\-]+)T(?<time>[0-9:]+)(A(?<randomtime>[0-9:]+))?").Groups;
				if (groups.Count != 1)
				{
					hueValueDate.DateTime = DateTime.ParseExact(groups["date"].Value + "T" + groups["time"].Value, "yyyy-MM-ddTHH:mm:ss", (IFormatProvider)base.Culture, base.DateTimeStyles);
					if (groups["randomtime"].Success)
					{
						hueValueDate.RandomizedTime = TimeSpan.ParseExact(groups["randomtime"].Value, "hh\\:mm\\:ss", (IFormatProvider)base.Culture);
					}

					return hueValueDate;
				}
			}

			//days recurring (optional randomtime)
			if (rawValue.StartsWith("W"))
			{
				var groups = Regex.Match(rawValue, @"W(?<daysrecurring>\d{1,3})/T(?<time>[0-9:]+)(A(?<randomtime>[0-9:]+))?").Groups;
				if (groups.Count != 1)
				{
					hueValueDate.RecurringDay = (RecurringDay)Convert.ToInt32(groups["daysrecurring"].Value);
					hueValueDate.TimerTime = TimeSpan.ParseExact(groups["time"].Value, "hh\\:mm\\:ss", (IFormatProvider)base.Culture);

					if (groups["randomtime"].Success)
					{
						hueValueDate.RandomizedTime = TimeSpan.ParseExact(groups["randomtime"].Value, "hh\\:mm\\:ss", (IFormatProvider)base.Culture);
					}

					return hueValueDate;
				}
			}

			//timers (optional recurrences and randomtime)
			if (rawValue.StartsWith("R") || rawValue.Contains("PT"))
			{
				var groups = Regex.Match(rawValue, @"(R\[(?<recurrence>\d{2})\]/)?PT(?<timertime>[0-9:]+)(A(?<randomtime>[0-9:]+))?").Groups;
				hueValueDate.TimerTime = TimeSpan.ParseExact(groups["timertime"].Value, "hh\\:mm\\:ss", (IFormatProvider)base.Culture);

				if (groups["randomtime"].Success)
				{
					hueValueDate.RandomizedTime = TimeSpan.ParseExact(groups["randomtime"].Value, "hh\\:mm\\:ss", (IFormatProvider)base.Culture);
				}
				if (groups["recurrence"].Success)
				{
					hueValueDate.NumberOfRecurrences = Convert.ToInt32(groups["recurrence"].Value);
				}

				return hueValueDate;
			}

			return null;
		}
  }
}