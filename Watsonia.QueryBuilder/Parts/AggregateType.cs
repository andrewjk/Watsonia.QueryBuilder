using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// An aggregate that may be applied to a field such as sum or count.
	/// </summary>
	public enum AggregateType
	{
		/// <summary>
		/// No aggregate.
		/// </summary>
		None,
		/// <summary>
		/// Counts the number of items.
		/// </summary>
		Count,
		/// <summary>
		/// Counts the number of items and returns a large integer.
		/// </summary>
		BigCount,
		/// <summary>
		/// Adds the values contained in the field together.
		/// </summary>
		Sum,
		/// <summary>
		/// Returns the minimum value contained in the field.
		/// </summary>
		Min,
		/// <summary>
		/// Returns the maximum value contained in the field.
		/// </summary>
		Max,
		/// <summary>
		/// Returns the average value contained in the field.
		/// </summary>
		Average,
	}
}
