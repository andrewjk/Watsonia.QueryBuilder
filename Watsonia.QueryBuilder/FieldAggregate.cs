using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// Represents a field and aggregate (count, sum, etc) that is used with a select statement.
	/// </summary>
	public class FieldAggregate
	{
		/// <summary>
		/// Gets or sets the field.
		/// </summary>
		/// <value>
		/// The field.
		/// </value>
		public PropertyInfo Field { get; private set; }

		/// <summary>
		/// Gets or sets the aggregate (count, sum, etc).
		/// </summary>
		/// <value>
		/// The aggregate.
		/// </value>
		public AggregateType Aggregate { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldAggregate"/> class.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <param name="aggregate">The aggregate (count, sum, etc).</param>
		public FieldAggregate(PropertyInfo field, AggregateType aggregate)
		{
			this.Field = field;
			this.Aggregate = aggregate;
		}
	}
}
