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
	/// The starting point for fluently creating update statements.
	/// </summary>
	public static partial class Update
	{
		/// <summary>
		/// Creates an update statement from a type corresponding to the table that records should be updated in.
		/// </summary>
		/// <typeparam name="T">The type corresponding to the table that records should be updated in.</typeparam>
		/// <returns>The update statement.</returns>
		public static UpdateStatement<T> Table<T>()
		{
			return new UpdateStatement<T>() { Target = typeof(T) };
		}

		/// <summary>
		/// Adds a value to set with the statement.
		/// </summary>
		/// <typeparam name="T">The type corresponding to the table that records should be updated in.</typeparam>
		/// <param name="update">The update.</param>
		/// <param name="property">The property.</param>
		/// <param name="value">The value.</param>
		/// <returns>The update statement.</returns>
		public static UpdateStatement<T> Set<T>(this UpdateStatement<T> update, Expression<Func<T, object>> property, object value)
		{
			update.SetValues.Add(new FieldValue(FuncToPropertyInfo(property), value));
			return update;
		}

		/// <summary>
		/// Adds a condition to the update statement.
		/// </summary>
		/// <typeparam name="T">The type corresponding to the table that records should be updated in.</typeparam>
		/// <param name="update">The update.</param>
		/// <param name="condition">The condition.</param>
		/// <returns>The update statement.</returns>
		public static UpdateStatement<T> Where<T>(this UpdateStatement<T> update, Expression<Func<T, bool>> condition)
		{
			update.Conditions = condition;
			return update;
		}

		/// <summary>
		/// Adds an AND condition to the update statement.
		/// </summary>
		/// <typeparam name="T">The type corresponding to the table that records should be updated in.</typeparam>
		/// <param name="update">The update.</param>
		/// <param name="condition">The condition.</param>
		/// <returns>The update statement.</returns>
		public static UpdateStatement<T> And<T>(this UpdateStatement<T> update, Expression<Func<T, bool>> condition)
		{
			var combined = update.Conditions.Body.AndAlso(condition.Body);
			combined = AnonymousParameterReplacer.Replace(combined, condition.Parameters);
			update.Conditions = Expression.Lambda<Func<T, bool>>(combined, condition.Parameters);
			return update;
		}

		/// <summary>
		/// Adds an OR condition to the update statement.
		/// </summary>
		/// <typeparam name="T">The type corresponding to the table that records should be updated in.</typeparam>
		/// <param name="update">The update.</param>
		/// <param name="condition">The condition.</param>
		/// <returns>The update statement.</returns>
		public static UpdateStatement<T> Or<T>(this UpdateStatement<T> update, Expression<Func<T, bool>> condition)
		{
			var combined = update.Conditions.Body.OrElse(condition.Body);
			combined = AnonymousParameterReplacer.Replace(combined, condition.Parameters);
			update.Conditions = Expression.Lambda<Func<T, bool>>(combined, condition.Parameters);
			return update;
		}

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
