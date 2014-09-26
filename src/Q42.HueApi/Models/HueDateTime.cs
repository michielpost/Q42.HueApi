using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi.Models
{
  public class HueDateTime
  {
		// http://www.developers.meethue.com/documentation/datatypes-and-time-patterns

		/// <summary>
		/// Absolute time
		/// </summary>
    public DateTime? DateTime { get; set; }

		/// <summary>
		/// Timers and timeparts for recurring times
		/// </summary>
		public TimeSpan? TimerTime{ get; set; }

		/// <summary>
		/// Randomized time
		/// </summary>
    public TimeSpan? RandomizedTime { get; set; }

		/// <summary>
		/// Recurring days
		/// </summary>
    public RecurringDay RecurringDay { get; set; }

		/// <summary>
		/// Number of recurrences (0=repeat forever)
		/// </summary>
		public int? NumberOfRecurrences { get; set; }
  }

  [Flags]
  public enum RecurringDay
  {
    RecurringNone = 0,
    RecurringSunday = 1,
    RecurringSaturday = 2,
    RecurringFriday = 4,
    RecurringThursday = 8,
    RecurringWednesday = 16,
    RecurringTuesday = 32,
    RecurringMonday = 64,
    RecurringWeekdays = 124,
    RecurringWeekend = 3,
    RecurringAlldays = 127,
  }
}
