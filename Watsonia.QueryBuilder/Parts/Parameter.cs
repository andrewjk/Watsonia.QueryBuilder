using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// A parameter for passing to a stored procedure or function.
	/// </summary>
	public class Parameter
	{
		/// <summary>
		/// Gets the name of the parameter.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the value of the parameter.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public object Value { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Parameter"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="value">The value.</param>
		public Parameter(string name, object value)
		{
			this.Name = name;
			this.Value = value;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this.Name;
		}
	}
}
