using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// The starting point for fluently creating insert statements.
	/// </summary>
	public static partial class Insert
	{
		/// <summary>
		/// Creates an insert statement from a type corresponding to the table that records should be inserted into.
		/// </summary>
		/// <typeparam name="T">The type corresponding to the table that records should be inserted into.</typeparam>
		/// <returns>The insert statement.</returns>
		public static InsertStatement<T> Into<T>()
		{
			return new InsertStatement<T>();
		}

		/// <summary>
		/// Adds a value to insert with the statement.
		/// </summary>
		/// <typeparam name="T">The type corresponding to the table that records should be inserted into.</typeparam>
		/// <param name="insert">The insert statement.</param>
		/// <param name="property">The property.</param>
		/// <param name="value">The value.</param>
		/// <returns>The insert statement.</returns>
		public static InsertStatement<T> Value<T>(this InsertStatement<T> insert, Expression<Func<T, object>> property, object value)
		{
			insert.SetValues.Add(new FieldValue(FuncToPropertyInfo(property), value));
			return insert;
		}

		// TODO: This should go into a helper
		private static PropertyInfo FuncToPropertyInfo<T>(Expression<Func<T, object>> selector)
		{
			if (selector.Body is MemberExpression mex)
			{
				return (PropertyInfo)mex.Member;
			}
			else if (selector.Body is UnaryExpression uex) // Throw away Converts
			{
				if (uex.Operand is MemberExpression omex)
				{
					return (PropertyInfo)omex.Member;
				}
			}

			throw new InvalidOperationException();
		}
	}
}
