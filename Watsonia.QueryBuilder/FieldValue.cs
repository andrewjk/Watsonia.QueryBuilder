using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// Represents a field and the value to set it to.
	/// </summary>
	public class FieldValue
	{
		/// <summary>
		/// Gets or sets the field.
		/// </summary>
		/// <value>
		/// The field.
		/// </value>
		public PropertyInfo Field { get; private set; }

		/// <summary>
		/// Gets or sets the value to set the field to.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public object Value { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldValue"/> class.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <param name="value">The value to set the field to.</param>
		public FieldValue(PropertyInfo field, object value)
		{
			this.Field = field;
			this.Value = value;
		}
	}
}
