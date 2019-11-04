using System;
using System.Collections.Generic;
using System.Linq;
using Watsonia.QueryBuilder;

namespace Watsonia.QueryBuilder
{
	/// <summary>
	/// The starting point for fluently creating insert statements.
	/// </summary>
	public static partial class Insert
	{
		/// <summary>
		/// Creates an insert statement with the name of the table that records should be inserted into.
		/// </summary>
		/// <param name="tableName">Name of the table.</param>
		/// <returns>The insert statement.</returns>
		public static InsertStatement Into(string tableName)
		{
			return Insert.Into(new Table(tableName));
		}

		/// <summary>
		/// Creates an insert statement with the table that records should be inserted into.
		/// </summary>
		/// <param name="table">The table.</param>
		/// <returns>The insert statement.</returns>
		public static InsertStatement Into(Table table)
		{
			return new InsertStatement() { Target = table };
		}

		/// <summary>
		/// Adds a value to insert with the statement.
		/// </summary>
		/// <param name="insert">The insert statement.</param>
		/// <param name="columnName">Name of the column.</param>
		/// <param name="value">The value.</param>
		/// <returns>The insert statement.</returns>
		public static InsertStatement Value(this InsertStatement insert, string columnName, object value)
		{
			insert.SetValues.Add(new SetValue(columnName, value));
			return insert;
		}

		/// <summary>
		/// Adds a list of columns to insert with the statement (used in conjuction with Select).
		/// </summary>
		/// <param name="insert">The insert statement.</param>
		/// <param name="columnNames">The column names.</param>
		/// <returns>The insert statement.</returns>
		public static InsertStatement Columns(this InsertStatement insert, params string[] columnNames)
		{
			insert.TargetFields.AddRange(columnNames.Select(cn => new Column(cn)));
			return insert;
		}

		/// <summary>
		/// Adds a select statement to insert with to the statement (used in conjunction with Columns).
		/// </summary>
		/// <param name="insert">The insert statement.</param>
		/// <param name="statement">The statement.</param>
		/// <returns>The insert statement.</returns>
		public static InsertStatement Select(this InsertStatement insert, SelectStatement statement)
		{
			insert.Source = statement;
			return insert;
		}
	}
}
