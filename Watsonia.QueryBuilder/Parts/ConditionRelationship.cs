using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// The logical relationship between a set of conditions.
	/// </summary>
	public enum ConditionRelationship
	{
		/// <summary>
		/// The set of conditions should return true if all conditions are true.
		/// </summary>
		And,
		/// <summary>
		/// The set of conditions should return true if any conditions are true.
		/// </summary>
		Or,
	}
}
