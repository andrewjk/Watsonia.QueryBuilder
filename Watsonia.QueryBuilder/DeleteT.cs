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
	/// The starting point for fluently creating delete statements.
	/// </summary>
	public static partial class Delete
	{
		/// <summary>
		/// Creates a delete statement from a type corresponding to the table that records should be deleted from.
		/// </summary>
		/// <typeparam name="T">The type corresponding to the table that records should be deleted from.</typeparam>
		/// <returns>The delete statement.</returns>
		public static DeleteStatement<T> From<T>()
		{
			return new DeleteStatement<T>() { Target = typeof(T) };
		}

		/// <summary>
		/// Sets the condition to delete all records from the table (be careful!).
		/// </summary>
		/// <typeparam name="T">The type corresponding to the table that records should be deleted from.</typeparam>
		/// <param name="delete">The delete statement.</param>
		/// <param name="all">if set to <c>true</c>, delete all records from the table.</param>
		/// <returns>The delete statement.</returns>
		public static DeleteStatement<T> Where<T>(this DeleteStatement<T> delete, bool all)
		{
			if (all)
			{
				delete.Conditions = (item) => true;
			}
			return delete;
		}

		/// <summary>
		/// Adds a condition to the delete statement.
		/// </summary>
		/// <typeparam name="T">The type corresponding to the table that records should be deleted from.</typeparam>
		/// <param name="delete">The delete statement.</param>
		/// <param name="condition">The condition.</param>
		/// <returns>The delete statement.</returns>
		public static DeleteStatement<T> Where<T>(this DeleteStatement<T> delete, Expression<Func<T, bool>> condition)
		{
			delete.Conditions = condition;
			return delete;
		}

		/// <summary>
		/// Adds an AND condition to the delete statement.
		/// </summary>
		/// <typeparam name="T">The type corresponding to the table that records should be deleted from.</typeparam>
		/// <param name="delete">The delete statement.</param>
		/// <param name="condition">The condition.</param>
		/// <returns>The delete statement.</returns>
		public static DeleteStatement<T> And<T>(this DeleteStatement<T> delete, Expression<Func<T, bool>> condition)
		{
			var combined = delete.Conditions.Body.AndAlso(condition.Body);
			combined = AnonymousParameterReplacer.Replace(combined, condition.Parameters);
			delete.Conditions = Expression.Lambda<Func<T, bool>>(combined, condition.Parameters);
			return delete;
		}

		/// <summary>
		/// Adds an OR condition to the delete statement.
		/// </summary>
		/// <typeparam name="T">The type corresponding to the table that records should be deleted from.</typeparam>
		/// <param name="delete">The delete statement.</param>
		/// <param name="condition">The condition.</param>
		/// <returns>The delete statement.</returns>
		public static DeleteStatement<T> Or<T>(this DeleteStatement<T> delete, Expression<Func<T, bool>> condition)
		{
			var combined = delete.Conditions.Body.OrElse(condition.Body);
			combined = AnonymousParameterReplacer.Replace(combined, condition.Parameters);
			delete.Conditions = Expression.Lambda<Func<T, bool>>(combined, condition.Parameters);
			return delete;
		}
	}
}
