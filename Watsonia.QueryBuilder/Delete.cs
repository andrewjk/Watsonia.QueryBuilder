using System.Collections.Generic;
using Watsonia.QueryBuilder;
using System;
using System.Linq;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// The starting point for fluently creating delete statements.
	/// </summary>
	public static partial class Delete
	{
		/// <summary>
		/// Creates a delete statement with the name of the table that records should be deleted from.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>The delete statement.</returns>
		public static DeleteStatement From(string tableName)
		{
			return Delete.From(new Table(tableName));
		}

		/// <summary>
		/// Creates a delete statement with the table that records should be deleted from.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns>The delete statement.</returns>
		public static DeleteStatement From(Table table)
		{
			return new DeleteStatement() { Target = table };
		}

		/// <summary>
		/// Sets the condition to delete all records from the table (be careful!).
		/// </summary>
		/// <param name="delete">The delete statement.</param>
		/// <param name="all">if set to <c>true</c>, delete all records from the table.</param>
		/// <returns>The delete statement.</returns>
		public static DeleteStatement Where(this DeleteStatement delete, bool all)
		{
			if (all)
			{
				var newCondition = new Condition();
				newCondition.Field = new ConstantPart(true);
				newCondition.Value = new ConstantPart(true);
				delete.Conditions.Add(newCondition);
			}
			return delete;
		}

		/// <summary>
		/// Adds a condition to the delete statement.
		/// </summary>
		/// <param name="delete">The delete statement.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="op">The operator.</param>
		/// <param name="value">The value.</param>
		/// <returns>The delete statement.</returns>
		public static DeleteStatement Where(this DeleteStatement delete, string columnName, SqlOperator op, object value)
		{
			delete.Conditions.Add(new Condition(columnName, op, value));
			return delete;
		}

		/// <summary>
		/// Adds a NOT condition to the delete statement.
		/// </summary>
		/// <param name="delete">The delete statement.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="op">The operator.</param>
		/// <param name="value">The value.</param>
		/// <returns>The delete statement.</returns>
		public static DeleteStatement WhereNot(this DeleteStatement delete, string columnName, SqlOperator op, object value)
		{
			delete.Conditions.Add(new Condition(columnName, op, value) { Not = true });
			return delete;
		}

		/// <summary>
		/// Adds an AND condition to the delete statement.
		/// </summary>
		/// <param name="delete">The delete statement.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="op">The operator.</param>
		/// <param name="value">The value.</param>
		/// <returns>The delete statement.</returns>
		public static DeleteStatement And(this DeleteStatement delete, string columnName, SqlOperator op, object value)
		{
			delete.Conditions.Add(new Condition(columnName, op, value) { Relationship = ConditionRelationship.And });
			return delete;
		}

		/// <summary>
		/// Adds an OR condition to the delete statement.
		/// </summary>
		/// <param name="delete">The delete statement.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="op">The operator.</param>
		/// <param name="value">The value.</param>
		/// <returns>The delete statement.</returns>
		public static DeleteStatement Or(this DeleteStatement delete, string columnName, SqlOperator op, object value)
		{
			delete.Conditions.Add(new Condition(columnName, op, value) { Relationship = ConditionRelationship.Or });
			return delete;
		}
	}
}
