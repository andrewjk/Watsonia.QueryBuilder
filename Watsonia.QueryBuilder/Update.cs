using System;
using System.Collections.Generic;
using System.Linq;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// The starting point for fluently creating update statements.
	/// </summary>
	public static partial class Update
	{
		/// <summary>
		/// Creates an update statement with the name of the table that records should be updated in.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>The update statement.</returns>
		public static UpdateStatement Table(string tableName)
		{
			return Update.Table(new Table(tableName));
		}

		/// <summary>
		/// Creates an update statement with the table that records should be updated in.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns>The update statement.</returns>
		public static UpdateStatement Table(Table table)
		{
			return new UpdateStatement() { Target = table };
		}

		/// <summary>
		/// Adds a value to set with the statement.
		/// </summary>
		/// <param name="update">The update statement.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="value">The value.</param>
		/// <returns>The update statement.</returns>
		public static UpdateStatement Set(this UpdateStatement update, string columnName, object value)
		{
			update.SetValues.Add(new SetValue(columnName, value));
			return update;
		}

		/// <summary>
		/// Adds a condition to the update statement.
		/// </summary>
		/// <param name="update">The update statement.</param>
		/// <param name="all">if set to <c>true</c> [all].</param>
		/// <returns>The update statement.</returns>
		public static UpdateStatement Where(this UpdateStatement update, bool all)
		{
			if (all)
			{
				var newCondition = new Condition();
				newCondition.Field = new ConstantPart(true);
				newCondition.Value = new ConstantPart(true);
				update.Conditions.Add(newCondition);
			}
			return update;
		}

		/// <summary>
		/// Adds a condition to the update statement.
		/// </summary>
		/// <param name="update">The update statement.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="op">The op.</param>
		/// <param name="value">The value.</param>
		/// <returns>The update statement.</returns>
		public static UpdateStatement Where(this UpdateStatement update, string columnName, SqlOperator op, object value)
		{
			update.Conditions.Add(new Condition(columnName, op, value));
			return update;
		}

		/// <summary>
		/// Adds a NOT condition to the update statement.
		/// </summary>
		/// <param name="update">The update statement.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="op">The op.</param>
		/// <param name="value">The value.</param>
		/// <returns>The update statement.</returns>
		public static UpdateStatement WhereNot(this UpdateStatement update, string columnName, SqlOperator op, object value)
		{
			update.Conditions.Add(new Condition(columnName, op, value) { Not = true });
			return update;
		}

		/// <summary>
		/// Adds an AND condition to the update statement.
		/// </summary>
		/// <param name="update">The update statement.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="op">The op.</param>
		/// <param name="value">The value.</param>
		/// <returns>The update statement.</returns>
		public static UpdateStatement And(this UpdateStatement update, string columnName, SqlOperator op, object value)
		{
			update.Conditions.Add(new Condition(columnName, op, value) { Relationship = ConditionRelationship.And });
			return update;
		}

		/// <summary>
		/// Adds an OR condition to the update statement.
		/// </summary>
		/// <param name="update">The update statement.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="op">The op.</param>
		/// <param name="value">The value.</param>
		/// <returns>The update statement.</returns>
		public static UpdateStatement Or(this UpdateStatement update, string columnName, SqlOperator op, object value)
		{
			update.Conditions.Add(new Condition(columnName, op, value) { Relationship = ConditionRelationship.Or });
			return update;
		}
	}
}
