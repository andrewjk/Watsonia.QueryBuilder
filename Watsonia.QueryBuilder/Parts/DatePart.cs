using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// A date part.
	/// </summary>
	public enum DatePart
	{
		/// <summary>
		/// The millisecond component of the date's time.
		/// </summary>
		Millisecond,
		/// <summary>
		/// The second component of the date's time.
		/// </summary>
		Second,
		/// <summary>
		/// The minute component of the date's time.
		/// </summary>
		Minute,
		/// <summary>
		/// The hour component of the date's time.
		/// </summary>
		Hour,
		/// <summary>
		/// The day component of the date.
		/// </summary>
		Day,
		/// <summary>
		/// The day of the week component of the date.
		/// </summary>
		DayOfWeek,
		/// <summary>
		/// The day of the year component of the date.
		/// </summary>
		DayOfYear,
		/// <summary>
		/// The month component of the date.
		/// </summary>
		Month,
		/// <summary>
		/// The year component of the date.
		/// </summary>
		Year,
		/// <summary>
		/// The date component of the date.
		/// </summary>
		Date,
	}
}
