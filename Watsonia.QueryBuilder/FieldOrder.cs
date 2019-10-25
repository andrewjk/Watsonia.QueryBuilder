using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// Represents a field and direction that is used for ordering a statement.
	/// </summary>
	public class FieldOrder
	{
		/// <summary>
		/// Gets or sets the field.
		/// </summary>
		/// <value>
		/// The field.
		/// </value>
		public PropertyInfo Field { get; private set; }

		/// <summary>
		/// Gets or sets the order direction (ascending or descending).
		/// </summary>
		/// <value>
		/// The direction.
		/// </value>
		public OrderDirection Direction { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldOrder"/> class.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <param name="direction">The order direction (ascending or descending).</param>
		public FieldOrder(PropertyInfo field, OrderDirection direction = OrderDirection.Ascending)
		{
			this.Field = field;
			this.Direction = direction;
		}
	}
}
