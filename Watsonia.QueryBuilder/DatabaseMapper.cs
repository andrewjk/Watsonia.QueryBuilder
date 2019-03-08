using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Watsonia.QueryBuilder
{
	public class DatabaseMapper
	{
		/// <summary>
		/// Gets the name of the table for the supplied type.
		/// </summary>
		/// <remarks>
		/// For a Book item, this would return "Book" by default but might be overridden to return "Books" or something different.
		/// </remarks>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public virtual string GetTableName(Type type)
		{
			return type.Name;
		}

		/// <summary>
		/// Gets the name of the column for the supplied property.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		public virtual string GetColumnName(PropertyInfo property)
		{
			return property.Name;
		}
	}
}
